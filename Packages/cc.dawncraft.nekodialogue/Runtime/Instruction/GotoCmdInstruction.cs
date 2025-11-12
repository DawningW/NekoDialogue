using System.Text.RegularExpressions;

/// <summary>
/// 跳转命令指令
/// </summary>
public class GotoCmdInstruction: CmdInstruction {
    public const string COMMAND = "GOTO";

    public override string Command => COMMAND;

    public string label;
    public string file;
    private static readonly Regex gotoCmdPattern = new Regex(DialogueParser.gotoPattern, RegexOptions.Compiled);

    public override Result Execute(DialogueManager dialogueManager) {
        if (string.IsNullOrEmpty(file)) {
            dialogueManager.GotoLabel(label);
            return Result.END;
        } else {
            Dialogue dialogue = DialogueManager.GetDialogue(file);
            dialogueManager.StartDialogue(dialogue, label);
            return Result.AWAIT;
        }
    }

    public static GotoCmdInstruction Parse(string dialogue, string args) {
        Match match = gotoCmdPattern.Match(args);
        if (match.Success) {
            return new GotoCmdInstruction { label = match.Groups["label"].Value, file = match.Groups["file"].Value };
        }
        return null;
    }
}
