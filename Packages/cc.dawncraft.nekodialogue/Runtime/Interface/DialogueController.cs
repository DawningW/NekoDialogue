namespace Dawncraft.NekoDialogue {

/// <summary>
/// 对话逻辑适配层, 需要用户自行实现并注册进DialogueManager
/// </summary>
public interface DialogueController {
    /// <summary>
    /// 等待玩家输入以执行下一句对话时调用此方法
    /// </summary>
    public void WaitForNextDialogue();

    /// <summary>
    /// 等待玩家输入以做出选择时调用此方法
    /// </summary>
    /// <param name="count">选项数量</param>
    public void WaitForMakeChoice(int count);
}

}
