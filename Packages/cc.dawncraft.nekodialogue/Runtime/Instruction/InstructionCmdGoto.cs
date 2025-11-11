public class InstructionCmdGoto: Instruction {
    public string label;
    public string file;

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
}
