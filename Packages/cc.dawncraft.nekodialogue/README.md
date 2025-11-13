# NekoDialogue

简体中文 | [English](README_en.md)

NekoDialogue 是一个用于开发游戏对话系统的框架，适用于 Unity 2020.3 及更高版本。

和那些包罗万象的游戏框架不同，NekoDialogue 专注于剧本的解析和对话的执行，不包含 UI 组件和复杂的功能扩展（类似于前端中 Headless UI 的概念），旨在为开发者提供一个灵活且易于集成的对话系统解决方案。

特色：

- 简单轻量但性能一般（~~全是正则表达式当然性能一般了~~）
- 使用一种经过精心设计的符合直觉的剧本语言编写对话
- 支持从文件/直接使用代码创建对话，便于 Mod 的开发
- 支持代码/可视化编程方式交互
- 不绑定游戏对象，而是通过适配层接入，对原有游戏架构的侵入性极小

NekoDialogue 的代码简洁易懂，因此特别适用于教学用途，可用于学习对话系统的设计，进而实现自己的对话系统。同时也适用于旮旯给木、文字冒险、视觉小说、角色扮演等各类具有对话系统的游戏，但**需要注意的是**，NekoDialogue 本体仅包含对话系统的核心功能，若从头开发游戏，可考虑使用示例作为模板进行开发；若在现有游戏中集成 NekoDialogue，则需参考示例自行实现适配层。

> [!IMPORTANT]
>
> 由于 NekoDialogue 仅包含对话系统的核心功能，因此本框架推荐的用法是先学习并理解其工作原理，然后根据自己的需求修改代码甚至是再造一个适合于您游戏的对话框架。如果想要开箱即用的完整解决方案，更推荐使用 [NodeCanvas](https://assetstore.unity.com/packages/tools/visual-scripting/nodecanvas-14914) 和 [Dialogue System for Unity](https://assetstore.unity.com/packages/tools/behavior-ai/dialogue-system-for-unity-11672) 等插件。

## 安装

### 通过 [OpenUPM](https://openupm.com/)（推荐）

由于 OpenUPM 支持依赖解析、升级和降级等特性，推荐使用 [OpenUPM 命令行工具](https://openupm.com/) 安装 NekoDialogue。

如果还没有[安装 OpenUPM](https://openupm.com/docs/getting-started.html#installing-openupm-cli)，您可以使用 Node.js 的 `npm` 包管理器全局安装 OpenUPM 命令行工具：

```shell
npm install -g openupm-cli
```

然后运行以下命令将 NekoDialogue 添加到您的 Unity 项目中：

```shell
openupm add cc.dawncraft.nekodialogue
```

### 通过 [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

Unity 自带的包管理器也支持[通过 Git 仓库导入包](https://docs.unity3d.com/Manual/upm-ui-giturl.html)，具体步骤如下：

1. 点击 Unity 包管理器左上角的 + 号按钮，选择 `Add package from git URL...` 选项
2. 在弹出的对话框中输入本仓库的 Git URL：https://github.com/DawningW/NekoDialogue.git?path=Packages/cc.dawncraft.nekodialogue

这种方式需要您手动处理依赖，好在 NekoDialogue 目前仅依赖于 Unity 引擎本身和可视化编程插件。

### 直接下载源码

如果该项目无法满足您的需求，您可以直接将源码拷贝到您的项目中并进行修改。但要注意的是随着差异的增加，后续升级可能会很困难，因此建议将改进贡献回本仓库。

## 快速开始

1. 将 NekoDialogue 导入到您的 Unity 项目中
2. 实现 `DialogueUI` 和 `DialogueController`，并将其挂载到对话界面和玩家控制器上
3. 创建 `DialogueManager` 的子类，让 `DialogueUI` 和 `DialogueController` 返回对应的实例
4. 把 `DialogueManager` 的子类挂载到一个不会被销毁的游戏对象上，然后就可以用它来管理对话了
   > 注意：`DialogueManager` 是单例模式，在整个游戏中只能存在一个
5. 在 `Assets/Resources/Dialogues` 目录下编写对话剧本，并使用 `DialogueManager.Instance` 加载和运行对话

## 文档

请前往 [Documentation](Packages/cc.dawncraft.nekodialogue/Documentation~/index.md) 目录查看详细的使用说明和 API 文档。

## 示例

TODO

## 贡献

NekoDialogue 以 MIT 协议开源，你可以随意在自己的项目中使用并修改它。如果做出了某些改进或修复了某些 Bug，也欢迎提交 Pull Request。

## 特别致谢

对话框架的原始代码最初来源于 Dawncraft 工作室 2021 年的[战争冒险企划](https://store.steampowered.com/app/2134540)，但受限于本人当时的开发水平，该项目最终未能完成从 RPG Maker MV 向 Unity 的迁移，只留下了最有价值的对话系统和一些零散的系统（输入管理系统、屏幕特效系统、UI 管理系统、音频管理系统、配置管理系统、本地化系统、存档管理系统、物品系统、角色系统等），将对话系统和游戏内容分离后作为框架在本仓库中开源，其余系统在分离游戏内容后会作为示例供大家参考。

对话框架的代码主要受到了 Minecraft 和 RPG Maker MV 的启发，剧本语法参考了 [inkle's narrative scripting language](https://github.com/inkle/inky) 和 [Librian](https://github.com/RimoChan/Librian)，在此对以上项目表示感谢。

- 最初版本类、方法、枚举和变量的命名很像 Minecraft 的代码（MCP 反混淆），因为我是从写 Minecraft Mod 入门的游戏开发，所以受 Minecraft 代码风格影响很深
- `TRIGGER` 命令的灵感来自于 Minecraft 的 `/trigger` 命令，后者可以修改游戏内的记分板数值，从而通过轮询检测到记分项变化，进而触发事件
- 对话系统的设计参考了 RPG Maker MV，因为当时战争冒险企划已经在 RPG Maker MV 上做了大量的原型设计工作，我只需要移植到 Unity 上就好了
