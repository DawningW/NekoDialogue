using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dawncraft.NekoDialogue {

/// <summary>
/// 对话管理器, 用户需要创建其子类并实现DialogueUI和DialogueController
/// </summary>
[DisallowMultipleComponent]
public abstract class DialogueManager: MonoBehaviour {
    private static readonly Dictionary<string, Dialogue> dialogueMap = new Dictionary<string, Dialogue>();

    public Dialogue current { get; private set; }
    public Dictionary<string, DialogueUI.FigureLocation?> figureLocations = new Dictionary<string, DialogueUI.FigureLocation?>();
    public Dictionary<string, string> figureSprites = new Dictionary<string, string>();
    // 程序计数器(bushi
    protected int counter;

    /// <summary>
    /// 由TRIGGER命令触发的事件会调用此委托
    /// <br />
    /// 建议使用DialogueEvent组件, 而非直接注册此委托
    /// </summary>
    public Action<string, string> triggerDelegates;

    /// <summary>
    /// 对话界面, 需要用户自行实现
    /// </summary>
    public virtual DialogueUI DialogueUI { get; }
    /// <summary>
    /// 对话逻辑, 需要用户自行实现
    /// </summary>
    public virtual DialogueController DialogueController { get; }

    /// <summary>
    /// 对话管理器单例, 用于可视化编程访问对话系统
    /// </summary>
    public static DialogueManager Instance { get; private set; }

    public DialogueManager() {
        Instance = this;
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

    /// <summary>
    /// 从下一条指令开始继续执行对话
    /// </summary>
    public void NextDialogue() {
        counter++;
        RunLoop();
    }

    /// <summary>
    /// 选择指定的选项
    /// </summary>
    /// <param name="num">第几个选项</param>
    public void MakeChoice(int num) {
        if (!(current[counter] is ChoiceInstruction instruction)) {
            Debug.LogError("当前执行的对话指令不是选项");
            return;
        }
        DialogueUI.HideChoices();
        ChoiceInstruction.Choice choice = instruction[num];
        if (string.IsNullOrEmpty(choice.file)) {
            GotoLabel(choice.label);
            RunLoop();
        } else {
            Dialogue dialogue = GetDialogue(choice.file);
            StartDialogue(dialogue, choice.label);
        }
    }

    /// <summary>
    /// 跳转到当前对话中指定的标签
    /// </summary>
    /// <param name="label">要跳转的标签, 空字符串表示从头开始</param>
    public void GotoLabel(string label) {
        if (string.IsNullOrEmpty(label)) {
            counter = 0;
            return;
        }
        counter = current.FindLabel(label);
        if (counter < 0) {
            counter = 0;
            Debug.LogWarningFormat("在剧本 {0} 中未找到标签 {1}, 将从头开始执行", current.ID, label);
        }
    }

    /// <summary>
    /// 开始新的对话
    /// </summary>
    /// <param name="dialogue">要开始的对话剧本</param>
    /// <param name="label">指定从哪个标签开始, 空字符串表示从头开始</param>
    public void StartDialogue(Dialogue dialogue, string label) {
        if (dialogue == null) {
            Debug.LogError("要进行的对话不能为空");
            return;
        }
        current = dialogue;
        GotoLabel(label);
        ContinueDialogue();
    }

    /// <summary>
    /// 开始新的对话
    /// </summary>
    /// <param name="id">要开始的对话剧本ID</param>
    /// <param name="label">指定从哪个标签开始, 空字符串表示从头开始</param>
    public void StartDialogue(string id, string label) =>
        StartDialogue(GetDialogue(id), label);

    /// <summary>
    /// 从当前指令开始继续对话
    /// </summary>
    public void ContinueDialogue() {
        RunLoop();
        DialogueUI.Active = true;
    }

    /// <summary>
    /// 结束对话
    /// </summary>
    public void EndDialogue() {
        DialogueUI.Active = false;
    }

    /// <summary>
    /// 从Unity资源中预加载对话剧本, 建议在游戏加载阶段调用, 从而避免性能问题
    /// </summary>
    /// <param name="locations">对话剧本ID列表</param>
    public static void PreloadDialogues(IEnumerable<string> locations) {
        foreach (string location in locations) {
            LoadDialogue(location);
        }
    }

    /// <summary>
    /// 从Unity资源中加载对话剧本
    /// <br />
    /// 如果需要从其他位置加载请直接使用DialogueParser和PutDialogue
    /// </summary>
    /// <param name="location">对话剧本ID</param>
    public static void LoadDialogue(string location) {
        DialogueParser parser = DialogueParser.fromResource(location);
        Dialogue dialogue = parser.Parse();
        dialogueMap.Add(dialogue.ID, dialogue);
    }

    /// <summary>
    /// 卸载指定的对话剧本, 以免内存占用过高
    /// </summary>
    /// <param name="location">对话剧本ID</param>
    public static void UnloadDialogue(string location) {
        if (!dialogueMap.Remove(location)) {
            Debug.LogWarningFormat("id为 {0} 的对话剧本尚未加载, 跳过卸载流程", location);
        }
    }

    /// <summary>
    /// 卸载指定的对话剧本, 以免内存占用过高
    /// </summary>
    /// <param name="locations">对话剧本ID列表</param>
    public static void UnloadDialogues(IEnumerable<string> locations) {
        foreach (string location in locations) {
            UnloadDialogue(location);
        }
    }

    /// <summary>
    /// 直接注册对话剧本, 适用于通过其他方式解析或直接使用代码创建的剧本
    /// </summary>
    /// <param name="dialogue">对话剧本</param>
    public static void PutDialogue(Dialogue dialogue) {
        if (dialogueMap.ContainsKey(dialogue.ID)) {
            Debug.LogErrorFormat("id为 {0} 的对话剧本已被注册!", dialogue.ID);
            return;
        }
        dialogueMap.Add(dialogue.ID, dialogue);
    }

    /// <summary>
    /// 通过id获取对话剧本, 若未加载则自动从Unity资源中加载
    /// <br />
    /// 推荐在游戏加载时调用PreloadDialogues提前预加载剧本, 避免性能问题
    /// </summary>
    /// <param name="id">对话剧本ID</param>
    /// <returns>对话剧本</returns>
    public static Dialogue GetDialogue(string id) {
        if (!dialogueMap.ContainsKey(id)) {
            Debug.LogWarningFormat("未预加载id为 {0} 的对话剧本, 可能会造成性能问题", id);
            LoadDialogue(id);
            if (!dialogueMap.ContainsKey(id)) {
                Debug.LogErrorFormat("无法找到id为 {0} 的对话剧本, 请检查拼写!", id);
                return null;
            }
        }
        return dialogueMap[id];
    }
}

}
