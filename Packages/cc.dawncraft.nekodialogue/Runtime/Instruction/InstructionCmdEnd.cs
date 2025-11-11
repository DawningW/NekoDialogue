public class InstructionCmdEnd: Instruction {
    public override Result Execute(DialogueManager dialogueManager) {
        dialogueManager.EndDialogue();
        return Result.AWAIT; // 这是因为用AWAIT会让剧本的执行陷入等待, 如果用END就跑去执行下一条了
    }
}
