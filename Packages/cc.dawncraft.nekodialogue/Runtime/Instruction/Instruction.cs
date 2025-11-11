public abstract class Instruction {
    /// <summary>
    /// 指令的执行结果
    /// </summary>
    public enum Result {
        /// <summary>
        /// 没执行完或者交由玩家控制
        /// </summary>
        AWAIT,
        /// <summary>
        /// 执行完毕
        /// </summary>
        END
    }

    public abstract Result Execute(DialogueManager dialogueManager);
}
