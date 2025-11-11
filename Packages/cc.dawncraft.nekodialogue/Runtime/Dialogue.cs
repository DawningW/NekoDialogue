using System.Collections.Generic;

public class Dialogue: List<Instruction> {
    // 以不含后缀的文件名作为ID
    public string ID;
    // 标题
    public string Title;
    // 创作时间
    public string Date;
    // 作者
    public string Author;

    public int FindLabel(string label) {
        return FindIndex((e) => {
            if (e is InstructionLabel instructionLabel) {
                return instructionLabel.label == label;
            }
            return false;
        });
    }
}
