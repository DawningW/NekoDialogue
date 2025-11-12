using UnityEngine;
using UnityEngine.Events;

namespace Dawncraft.NekoDialogue {

/// <summary>
/// 对话中由TRIGGER命令触发的事件
/// </summary>
public class DialogueEventListener: MonoBehaviour {
    public UnityEvent<string, string> OnTrigger;

    private void OnEnable() {
        DialogueManager.Instance.triggerDelegates += Trigger;
    }

    private void OnDisable() {
        DialogueManager.Instance.triggerDelegates -= Trigger;
    }

    public void Trigger(string dialogue, string argument) {
        OnTrigger.Invoke(dialogue, argument);
    }
}

}
