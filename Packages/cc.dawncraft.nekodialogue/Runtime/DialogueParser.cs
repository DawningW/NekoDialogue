using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// 由正则表达式和访问者模式驱动的剧本语言解析器
/// </summary>
public class DialogueParser {
    internal const string gotoPattern = @"(?<label>[^@\s]*)?(?:@(?<file>.*))?";
    private static readonly Regex commentPattern = new Regex(@"^(.*?)#.*$", RegexOptions.Compiled);
    private static readonly Regex dialoguePattern = new Regex(@"^(?:\[(?<position>[CLR])\])?(?:(?<name>[^?*>:\(]+)(?:\((?<image>.*)\))?)?(?::\s*(?<text>.*))?", RegexOptions.Compiled);
    private static readonly Regex choicePattern = new Regex(@"^\?\s*(?<text>.*?)\s*=>\s*" + gotoPattern, RegexOptions.Compiled);
    private static readonly Regex labelPattern = new Regex(@"^\*\s*(?<label>\S*)", RegexOptions.Compiled);
    private static readonly Regex commandPattern = new Regex(@"^>\s*(?<name>\S*)\s*(?<arguments>.*)", RegexOptions.Compiled);

    public delegate CmdInstruction CommandParser(string dialogue, string args);
    private static readonly Dictionary<string, CommandParser> cmdParsers = new Dictionary<string, CommandParser>() {
        { GotoCmdInstruction.COMMAND,    GotoCmdInstruction.Parse    },
        { EndCmdInstruction.COMMAND,     EndCmdInstruction.Parse     },
        { TriggerCmdInstruction.COMMAND, TriggerCmdInstruction.Parse },
    };

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

    private CmdInstruction ParseCommand(string name, string args) {
        foreach (KeyValuePair<string, CommandParser> cmdParser in cmdParsers) {
            if (name.Equals(cmdParser.Key)) {
                return cmdParser.Value.Invoke(id, args);
            }
        }
        Debug.LogWarningFormat("无法识别剧本 {0} 在第 {1} 行的命令: {2}, 参数: {3}", id, lineCount, name, args);
        return null;
    }

    /// <summary>
    /// 解析对话剧本
    /// </summary>
    /// <returns>对话剧本</returns>
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
                dialogue.Add(new DialogueInstruction {
                    location = match.Groups["position"].Value switch {
                        "C" => DialogueUI.FigureLocation.Center,
                        "L" => DialogueUI.FigureLocation.Left,
                        "R" => DialogueUI.FigureLocation.Right,
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
                ChoiceInstruction instruction = new ChoiceInstruction();
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
                dialogue.Add(new LabelInstruction { label = match.Groups["label"].Value });
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
            Debug.LogWarningFormat("无法解析剧本 {0} 的第 {1} 行: {2}", id, lineCount, line);
        }
        textReader.Close();
        return dialogue;
    }

    /// <summary>
    /// 注册剧本命令, 形如 > COMMAND ARGS...
    /// </summary>
    /// <param name="cmd">剧本命令的名称</param>
    /// <param name="parser">剧本命令的解析器</param>
    public static void RegisterCommand(string cmd, CommandParser parser) {
        if (cmdParsers.ContainsKey(cmd)) {
            Debug.LogWarningFormat("重复注册剧本命令 {}, 跳过", cmd);
            return;
        }
        cmdParsers.Add(cmd, parser);
    }

    /// <summary>
    /// 从文本中解析剧本
    /// </summary>
    /// <param name="id"></param>
    /// <param name="text"></param>
    /// <returns>用于从该文本中解析剧本的解析器实例</returns>
    public static DialogueParser fromText(string id, string text) {
        return new DialogueParser(id, new StringReader(text));
    }

    /// <summary>
    /// 从文件中解析剧本
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static DialogueParser fromPath(string path) {
        string fileName = Path.GetFileNameWithoutExtension(path);
        return new DialogueParser(fileName, new StreamReader(path));
    }

    /// <summary>
    /// 从Unity资源中解析剧本, 剧本文件必须放在Assets/Resources/Dialogues目录中
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public static DialogueParser fromResource(string location) {
        TextAsset textAsset = Resources.Load<TextAsset>("Dialogues/" + location);
        // string assetName = textAsset.name;
        DialogueParser parser = fromText(location, textAsset.text);
        Resources.UnloadAsset(textAsset);
        return parser;
    }
}
