using System.Collections.Generic;

namespace Dawncraft.NekoDialogue {

/// <summary>
/// 选择指令
/// </summary>
public class ChoiceInstruction: Instruction {
    public sealed class Choice {
        public string text;
        public string label;
        public string file;
    }

    private readonly List<Choice> choices = new List<Choice>();

    public void Add(string text, string label, string file) {
        choices.Add(new Choice { text = text, label = label, file = file });
    }

    public Choice this[int index] { get => choices[index]; }
    public int Count { get => choices.Count; }

    public override Result Execute(DialogueManager dialogueManager) {
        dialogueManager.DialogueUI.SetChoices(choices);
        dialogueManager.DialogueController.WaitForMakeChoice(Count);
        return Result.AWAIT;
    }
}

}
