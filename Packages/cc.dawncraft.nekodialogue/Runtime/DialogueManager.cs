using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager {
    private static readonly Dictionary<string, Dialogue> dialogueMap = new Dictionary<string, Dialogue>();

    public Dialogue current { get; private set; }
    public Dictionary<string, Overlay.FigureLocation?> figureLocations = new Dictionary<string, Overlay.FigureLocation?>();
    public Dictionary<string, string> figureSprites = new Dictionary<string, string>();
    public Overlay OverlayUI => Game.Instance.UIManager.overlay;
    // 程序计数器(bushi
    private int counter;

    internal void NextDialogue() {
        counter++;
        RunLoop();
    }

    internal void MakeChoice(int num) {
        if (!(current[counter] is InstructionChoice instruction)) {
            Debug.LogError("当前执行的对话指令并不是选项");
            return;
        }
        OverlayUI.HideChoices();
        InstructionChoice.Choice choice = instruction[num];
        if (string.IsNullOrEmpty(choice.file)) {
            GotoLabel(choice.label);
            RunLoop();
        } else {
            Dialogue dialogue = GetDialogue(choice.file);
            StartDialogue(dialogue, choice.label);
        }
    }

    internal void GotoLabel(string label) {
        if (string.IsNullOrEmpty(label)) {
            counter = 0;
            return;
        }
        counter = current.FindLabel(label);
        if (counter < 0) {
            counter = 0;
            Debug.LogWarning(string.Format("在剧本 {0} 中未找到标签 {1}, 将从头开始执行", current.ID, label));
        }
    }

    internal void RunLoop() {
        while (counter < current.Count) {
            Instruction.Result result = current[counter].Execute(this);
            if (result == Instruction.Result.AWAIT) {
                break;
            }
            counter++;
        }
    }

    public void StartDialogue(Dialogue dialogue, string label) {
        if (dialogue == null) {
            Debug.LogError("要进行的对话不能为空");
            return;
        }
        current = dialogue;
        GotoLabel(label);
        ContinueDialogue();
    }

    public void ContinueDialogue() {
        RunLoop();
        OverlayUI.DialogueActive = true;
    }

    public void EndDialogue() {
        OverlayUI.DialogueActive = false;
    }

    public static void LoadDialogue(string location) {
        DialogueParser parser = DialogueParser.fromResource(location);
        Dialogue dialogue = parser.Parse();
        dialogueMap.Add(dialogue.ID, dialogue);
    }

    public static Dialogue GetDialogue(string id) {
        if (!dialogueMap.ContainsKey(id)) {
            // UNDONE 应该在加载场景时统一加载对话
            Debug.LogWarning("未预加载id为" + id + "的对话剧本, 可能会造成性能问题");
            LoadDialogue(id);
            if (!dialogueMap.ContainsKey(id)) {
                Debug.LogError("无法找到id为" + id + "的对话剧本, 请检查拼写!");
                return null;
            }
        }
        return dialogueMap[id];
    }
}
