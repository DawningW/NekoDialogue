public class InstructionCmdTrigger: Instruction {
    public string dialogue;
    public string argument;

    public override Result Execute(DialogueManager dialogueManager) {
        DialogueEvent.Trigger(dialogue, argument);
        return Result.AWAIT;
    }
}
