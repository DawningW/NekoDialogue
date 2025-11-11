using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;

// 剧本语言部分参考了https://github.com/librian-center/liber-language/blob/master/liber/core.py, 感谢
public class DialogueParser {
    private const string gotoPattern = @"(?<label>[^@\s]*)?(?:@(?<file>.*))?";
    private static readonly Regex commentPattern = new Regex(@"^(.*?)#.*$", RegexOptions.Compiled);
    private static readonly Regex dialoguePattern = new Regex(@"^(?:\[(?<position>[CLR])\])?(?:(?<name>[^?*>:\(]+)(?:\((?<image>.*)\))?)?(?::\s*(?<text>.*))?", RegexOptions.Compiled);
    private static readonly Regex choicePattern = new Regex(@"^\?\s*(?<text>.*?)\s*=>\s*" + gotoPattern, RegexOptions.Compiled);
    private static readonly Regex labelPattern = new Regex(@"^\*\s*(?<label>\S*)", RegexOptions.Compiled);
    private static readonly Regex commandPattern = new Regex(@"^>\s*(?<name>\S*)\s*(?<arguments>.*)", RegexOptions.Compiled);
    private static readonly Regex cmdGotoPattern = new Regex(gotoPattern, RegexOptions.Compiled);
    // private static readonly Dictionary<Type, string> patterns = new Dictionary<Type, string>() {};

    private readonly string id;
    private readonly TextReader textReader;
    private int lineCount = 0;

    private DialogueParser(string id, TextReader textReader) {
        this.id = id;
        this.textReader = textReader;
    }

    private string ReadLine() {
        string line = textReader.ReadLine();
        if (line != null) {
            lineCount++;
        }
        return line;
    }

    private string RemoveComment(string line) {
        Match match = commentPattern.Match(line);
        if (match.Success) {
            line = match.Groups[1].Value;
        }
        return line.Trim();
    }

    private Instruction ParseCommand(string name, string args) {
        if (name.Equals("GOTO")) {
            Match match = cmdGotoPattern.Match(args);
            if (match.Success) {
                return new InstructionCmdGoto { label = match.Groups["label"].Value, file = match.Groups["file"].Value };
            }
        } else if (name.Equals("END")) {
            return new InstructionCmdEnd();
        } else if (name.Equals("TRIGGER")) {
            return new InstructionCmdTrigger() { dialogue = id, argument = args };
        }
        Debug.LogWarning(string.Format("无法识别剧本 {0} 在第 {1} 行的命令: {2}, 参数: {3}", id, lineCount, name, args));
        return null;
    }

    public Dialogue Parse() {
        Dialogue dialogue = new Dialogue {
            ID = id
        };
        string line = string.Empty;
        bool hasReadNext = false;
        while (hasReadNext || (line = ReadLine()) != null) {
            hasReadNext = false;
            line = RemoveComment(line);
            if (line.Length == 0) {
                continue;
            }
            Match match = dialoguePattern.Match(line);
            // 这个正则表达式中的三个部分都是可选的, 因此会匹配到空字符串
            if (match.Success && match.Value != string.Empty) {
                string[] name = match.Groups["name"].Value.Split('|');
                dialogue.Add(new InstructionDialogue {
                    location = match.Groups["position"].Value switch {
                        "C" => Overlay.FigureLocation.Center,
                        "L" => Overlay.FigureLocation.Left,
                        "R" => Overlay.FigureLocation.Right,
                        _ => null
                    },
                    characterId = name[0],
                    name = name.Length > 1 ? name[1] : string.Empty,
                    image = match.Groups["image"].Value,
                    text = match.Groups["text"].Value
                });
                continue;
            }
            match = choicePattern.Match(line);
            if (match.Success) {
                InstructionChoice instruction = new InstructionChoice();
                instruction.Add(match.Groups["text"].Value, match.Groups["label"].Value, match.Groups["file"].Value);
                while ((line = ReadLine()) != null) {
                    line = RemoveComment(line);
                    if (line.Length > 0) {
                        match = choicePattern.Match(line);
                        if (match.Success) {
                            instruction.Add(match.Groups["text"].Value, match.Groups["label"].Value, match.Groups["file"].Value);
                        } else {
                            hasReadNext = true;
                            break;
                        }
                    }
                }
                dialogue.Add(instruction);
                continue;
            }
            match = labelPattern.Match(line);
            if (match.Success) {
                dialogue.Add(new InstructionLabel { label = match.Groups["label"].Value });
                continue;
            }
            match = commandPattern.Match(line);
            if (match.Success) {
                Instruction instruction = ParseCommand(match.Groups["name"].Value, match.Groups["arguments"].Value);
                if (instruction != null) {
                    dialogue.Add(instruction);
                }
                continue;
            }
            Debug.LogWarning(string.Format("无法解析剧本 {0} 的第 {1} 行: {2}", id, lineCount, line));
        }
        textReader.Close();
        return dialogue;
    }

    public static DialogueParser fromPath(string path) {
        string fileName = Path.GetFileNameWithoutExtension(path);
        return new DialogueParser(fileName, new StreamReader(path));
    }

    public static DialogueParser fromResource(string location) {
        TextAsset textAsset = Resources.Load<TextAsset>("Dialogues/" + location);
        // string assetName = textAsset.name;
        StringReader stringReader = new StringReader(textAsset.text);
        Resources.UnloadAsset(textAsset);
        return new DialogueParser(location, stringReader);
    }
}
