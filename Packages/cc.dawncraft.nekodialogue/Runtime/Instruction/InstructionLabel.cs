public class InstructionLabel: Instruction {
    public string label;

    public override Result Execute(DialogueManager dialogueManager) {
        return Result.END;
    }
}
