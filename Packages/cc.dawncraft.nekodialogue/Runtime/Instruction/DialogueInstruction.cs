namespace Dawncraft.NekoDialogue {

/// <summary>
/// 对话指令
/// </summary>
public class DialogueInstruction: Instruction {
    public DialogueUI.FigureLocation? location;
    public string characterId;
    public string name;
    public string image;
    public string text;

    public override Result Execute(DialogueManager dialogueManager) {
        // 名字为空, 只能是清空立绘或者旁白
        if (string.IsNullOrEmpty(characterId)) {
            if (location != null) {
                // 清空某位置的立绘
                dialogueManager.DialogueUI.SetFigure(location.Value, null, null);
                return Result.END;
            } else {
                // 显示旁白说的话
                dialogueManager.DialogueUI.FocusOnFigure(null);
                dialogueManager.DialogueUI.SetName(string.Empty);
                dialogueManager.DialogueUI.SetMessage(text);
                dialogueManager.DialogueUI.PlayTypingAnimation();
                dialogueManager.DialogueController.WaitForNextDialogue();
                return Result.AWAIT;
            }
        }
        // 若省略立绘位置或立绘图片则尝试从缓存中获取
        if (location == null) {
            dialogueManager.figureLocations.TryGetValue(characterId, out location);
        } else {
            dialogueManager.figureLocations[characterId] = location;
        }
        if (string.IsNullOrEmpty(image)) {
            dialogueManager.figureSprites.TryGetValue(characterId, out image);
        } else {
            dialogueManager.figureSprites[characterId] = image;
        }
        dialogueManager.DialogueUI.SetFigure(location.Value, characterId, image);
        // 要说的话为空, 视为设置人物立绘指令
        if (string.IsNullOrEmpty(text)) {
            return Result.END;
        }
        // 显示人物说的话并聚焦镜头到人物身上
        string displayName = string.IsNullOrEmpty(name) ? characterId : name;
        dialogueManager.DialogueUI.FocusOnFigure(location);
        dialogueManager.DialogueUI.SetName(displayName);
        dialogueManager.DialogueUI.SetMessage(text);
        dialogueManager.DialogueUI.PlayTypingAnimation();
        dialogueManager.DialogueController.WaitForNextDialogue();
        return Result.AWAIT;
    }
}

}
