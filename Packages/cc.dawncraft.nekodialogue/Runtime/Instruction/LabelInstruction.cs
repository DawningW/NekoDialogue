namespace Dawncraft.NekoDialogue {

/// <summary>
/// 标签指令
/// </summary>
public class LabelInstruction: Instruction {
    public string label;

    public override Result Execute(DialogueManager dialogueManager) {
        return Result.END;
    }
}

}
