namespace Dawncraft.NekoDialogue {

/// <summary>
/// 自定义事件命令指令
/// </summary>
public class TriggerCmdInstruction: CmdInstruction {
    public const string COMMAND = "TRIGGER";

    public override string Command => COMMAND;

    public string dialogue;
    public string argument;

    public override Result Execute(DialogueManager dialogueManager) {
        DialogueManager.Instance.triggerDelegates.Invoke(dialogue, argument);
        return Result.AWAIT;
    }

    public static TriggerCmdInstruction Parse(string dialogue, string args) {
        return new TriggerCmdInstruction() { dialogue = dialogue, argument = args };
    }
}

}
