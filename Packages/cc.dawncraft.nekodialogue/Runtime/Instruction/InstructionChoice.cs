using System.Collections.Generic;

public class InstructionChoice: Instruction {
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
        dialogueManager.OverlayUI.SetChoices(choices);
        Game.Instance.PlayerController.WaitForMakeChoice(Count);
        return Result.AWAIT;
    }
}
