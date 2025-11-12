using System.Collections.Generic;

namespace Dawncraft.NekoDialogue {

/// <summary>
/// 对话剧本数据类
/// </summary>
public class Dialogue: List<Instruction> {
    /// <summary>
    /// 以不含后缀的文件名作为ID
    /// </summary>
    public string ID;
    /// <summary>
    /// 标题
    /// </summary>
    public string Title;
    /// <summary>
    /// 创作时间
    /// </summary>
    public string Date;
    /// <summary>
    /// 作者
    /// </summary>
    public string Author;

    /// <summary>
    /// 查找对话剧本中指定标签所在的位置
    /// </summary>
    /// <param name="label">要查找的标签</param>
    /// <returns>标签所在的位置, 未找到则返回-1</returns>
    public int FindLabel(string label) {
        return FindIndex((e) => {
            if (e is LabelInstruction instructionLabel) {
                return instructionLabel.label == label;
            }
            return false;
        });
    }
}

}
