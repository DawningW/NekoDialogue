namespace Dawncraft.NekoDialogue {

/// <summary>
/// 自定义命令指令
/// </summary>
public abstract class CmdInstruction: Instruction {
    public virtual string Command { get; }
}

}
