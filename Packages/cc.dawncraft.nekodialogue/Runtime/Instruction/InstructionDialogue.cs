using UnityEngine;

public class InstructionDialogue: Instruction {
    public Overlay.FigureLocation? location;
    public string characterId;
    public string name;
    public string image;
    public string text;

    public override Result Execute(DialogueManager dialogueManager) {
        // 名字为空, 只能是清空立绘或者旁白
        if (string.IsNullOrEmpty(characterId)) {
            if (location != null) {
                // 清空某位置的立绘
                dialogueManager.OverlayUI.SetFigure(location.Value, null);
                return Result.END;
            } else {
                // 显示旁白说的话
                dialogueManager.OverlayUI.FocusOnFigure(null);
                dialogueManager.OverlayUI.SetName(string.Empty);
                dialogueManager.OverlayUI.SetMessage(text);
                dialogueManager.OverlayUI.PlayTypingAnimation();
                Game.Instance.PlayerController.WaitForNextDialogue();
                return Result.AWAIT;
            }
        }
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
        Character character = characterId == Player.id ? (Character) Player.current : NPC.GetNPC(characterId);
        if (character == null) {
            Debug.LogError("找不到id为" + characterId + "的人物");
        }
        // 此人物第一次出现且未指定立绘则使用该人物在物品栏中展示的立绘
        Sprite sprite;
        if (string.IsNullOrEmpty(image)) {
            sprite = character.standingFigure;
        } else {
            sprite = character.standingFigureList.Find(e => e.name == image);
        }
        dialogueManager.OverlayUI.SetFigure(location.Value, sprite);
        // 要说的话为空, 视为设置人物立绘指令
        if (string.IsNullOrEmpty(text)) {
            return Result.END;
        }
        // 显示人物说的话并聚焦镜头到人物身上
        string displayName = string.IsNullOrEmpty(name) ? characterId : name;
        dialogueManager.OverlayUI.FocusOnFigure(location);
        dialogueManager.OverlayUI.SetName(displayName);
        dialogueManager.OverlayUI.SetMessage(text);
        dialogueManager.OverlayUI.PlayTypingAnimation();
        Game.Instance.PlayerController.WaitForNextDialogue();
        return Result.AWAIT;
    }
}
