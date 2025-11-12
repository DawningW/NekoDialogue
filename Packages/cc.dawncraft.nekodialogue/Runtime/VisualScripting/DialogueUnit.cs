using UnityEngine;
using Ludiq;
using Bolt;

namespace Dawncraft.NekoDialogue {

[UnitTitle("开始对话")]
[UnitCategory("游戏/对话")]
public class StartDialogueUnit: Unit {
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlInput enter { get; private set; }
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlOutput exit { get; private set; }
    [DoNotSerialize]
    [PortLabel("对话ID")]
    public ValueInput dialogueId { get; private set; }
    [DoNotSerialize]
    [PortLabel("要跳转的标签")]
    public ValueInput label { get; private set; }

    protected override void Definition() {
        enter = ControlInput(nameof(enter), Run);
        exit = ControlOutput(nameof(exit));
        Succession(enter, exit);
        dialogueId = ValueInput<string>(nameof(dialogueId), string.Empty);
        label = ValueInput<string>(nameof(label), string.Empty);
        Requirement(dialogueId, enter);
        Requirement(label, enter);
    }

    protected ControlOutput Run(Flow flow) {
        string idValue = flow.GetValue<string>(dialogueId);
        string labelValue = flow.GetValue<string>(label);
        Dialogue dialogue = DialogueManager.GetDialogue(idValue);
        DialogueManager.Instance.StartDialogue(dialogue, labelValue);
        return exit;
    }
}

[UnitTitle("继续对话")]
[UnitCategory("游戏/对话")]
public class ContinueDialogueUnit: Unit {
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlInput enter { get; private set; }
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlOutput exit { get; private set; }

    protected override void Definition() {
        enter = ControlInput(nameof(enter), Run);
        exit = ControlOutput(nameof(exit));
        Succession(enter, exit);
    }

    protected ControlOutput Run(Flow flow) {
        // 用ContinueDialogue会无限递归
        DialogueManager.Instance.NextDialogue();
        return exit;
    }
}

[UnitTitle("结束对话")]
[UnitCategory("游戏/对话")]
public class EndDialogueUnit: Unit {
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlInput enter { get; private set; }
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlOutput exit { get; private set; }

    protected override void Definition() {
        enter = ControlInput(nameof(enter), Run);
        exit = ControlOutput(nameof(exit));
        Succession(enter, exit);
    }

    protected ControlOutput Run(Flow flow) {
        DialogueManager.Instance.EndDialogue();
        return exit;
    }
}

}
