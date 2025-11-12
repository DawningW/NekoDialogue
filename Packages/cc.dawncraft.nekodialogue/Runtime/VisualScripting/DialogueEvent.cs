using UnityEngine;
using Ludiq;
using Bolt;

namespace Dawncraft.NekoDialogue {

/// <summary>
/// 对话中由TRIGGER命令触发的事件
/// </summary>
[UnitTitle("对话触发事件")]
[UnitCategory("Events")]
public class DialogueEvent: GlobalEventUnit<DialogueEvent.Value> {
    public const string EventName = "DialogueEvent";
    protected override string hookName => EventName;

    [DoNotSerialize]
    [PortLabel("对话ID")]
    public ValueOutput dialogue { get; private set; }
    [DoNotSerialize]
    [PortLabel("触发参数")]
    public ValueOutput argument { get; private set; }

    protected override void Definition() {
        base.Definition();
        dialogue = ValueOutput<string>(nameof(dialogue));
        argument = ValueOutput<string>(nameof(argument));
    }

    protected override void AssignArguments(Flow flow, Value data) {
        flow.SetValue(dialogue, data.dialogue);
        flow.SetValue(argument, data.argument);
    }

    public static void Trigger(string dialogue, string argument) {
        EventBus.Trigger(EventName, new Value {
            dialogue = dialogue,
            argument = argument
        });
    }

    public sealed class Value {
        public string dialogue;
        public string argument;
    }
}

}
