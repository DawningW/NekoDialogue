/// <summary>
/// 剧本结束命令指令
/// </summary>
public class EndCmdInstruction: CmdInstruction {
    public const string COMMAND = "END";

    public override string Command => COMMAND;

    public override Result Execute(DialogueManager dialogueManager) {
        dialogueManager.EndDialogue();
        return Result.AWAIT; // 这是因为用AWAIT会让剧本的执行陷入等待, 如果用END就跑去执行下一条了
    }

    public static EndCmdInstruction Parse(string dialogue, string args) {
        return new EndCmdInstruction();
    }
}
