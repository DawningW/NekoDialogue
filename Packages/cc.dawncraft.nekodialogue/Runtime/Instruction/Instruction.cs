namespace Dawncraft.NekoDialogue {

/// <summary>
/// 剧本中的每一行表示一条指令
/// </summary>
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

    /// <summary>
    /// 执行指令
    /// </summary>
    /// <param name="dialogueManager">对话管理器的实例</param>
    /// <returns>指令的执行结果</returns>
    public abstract Result Execute(DialogueManager dialogueManager);
}

}
