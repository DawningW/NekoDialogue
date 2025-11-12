using System.Collections.Generic;

namespace Dawncraft.NekoDialogue {

/// <summary>
/// 对话界面适配层, 需要用户自行实现并注册进DialogueManager
/// </summary>
public interface DialogueUI {
    /// <summary>
    /// 要控制的立绘位置
    /// </summary>
    public enum FigureLocation {
        /// <summary>
        /// 中间的立绘
        /// </summary>
        Center,
        /// <summary>
        /// 左边的立绘
        /// </summary>
        Left,
        /// <summary>
        /// 右边的立绘
        /// </summary>
        Right
    }

    /// <summary>
    /// 对话界面是否显示
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// 设置指定位置的立绘图片
    /// </summary>
    /// <param name="location">立绘位置</param>
    /// <param name="characterId">立绘角色ID</param>
    /// <param name="image">立绘图片ID</param>
    public void SetFigure(FigureLocation location, string characterId, string image);

    /// <summary>
    /// 聚焦镜头到指定位置的立绘上
    /// </summary>
    /// <param name="location">立绘位置, 传入null表示都不聚焦, 一般用于旁白</param>
    public void FocusOnFigure(FigureLocation? location);

    /// <summary>
    /// 设置对话界面的人物名称
    /// </summary>
    /// <param name="name">人物名称</param>
    public void SetName(string name);

    /// <summary>
    /// 设置对话界面的消息文本
    /// </summary>
    /// <param name="message">消息文本</param>
    public void SetMessage(string message);

    /// <summary>
    /// 开始播放打字动画, 你可以使用任意动画库实现任意动画效果
    /// </summary>
    public void PlayTypingAnimation();

    /// <summary>
    /// 停止播放打字动画
    /// </summary>
    public void StopTypingAnimation();

    /// <summary>
    /// 设置对话界面中要选择的选项
    /// </summary>
    /// <param name="choices">选项</param>
    public void SetChoices(List<ChoiceInstruction.Choice> choices);

    /// <summary>
    /// 隐藏对话界面中要选择的选项
    /// </summary>
    public void HideChoices();
}

}
