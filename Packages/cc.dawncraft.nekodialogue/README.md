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
> NekoDialogue 目前正在重构中，以便与原来的游戏解耦，当前可能无法编译或正常使用，请耐心等待更新。
>
> 由于 NekoDialogue 仅包含对话系统的核心功能，因此本框架推荐的用法是先学习并理解其工作原理，然后根据自己的需求修改代码甚至是再造一个适合于您游戏的对话框架。如果想要开箱即用的完整解决方案，更推荐使用 [NodeCanvas](https://assetstore.unity.com/packages/tools/visual-scripting/nodecanvas-14914) 和 [Dialogue System for Unity](https://assetstore.unity.com/packages/tools/behavior-ai/dialogue-system-for-unity-11672) 等插件。

## 快速开始

## 剧本语法

## API

## 示例

## 贡献

NekoDialogue 以 MIT 协议开源，你可以随意在自己的项目中使用并修改它。如果做出了某些改进或修复了某些 Bug，也欢迎提交 Pull Request。

## 特别致谢

对话框架的原始代码最初来源于 Dawncraft 工作室 2021 年的[战争冒险企划](https://store.steampowered.com/app/2134540)，但受限于本人当时的开发水平，该项目最终未能完成从 RPG Maker MV 向 Unity 的迁移，只留下了最有价值的对话系统和一些零散的系统（输入管理系统、屏幕特效系统、UI 管理系统、音频管理系统、配置管理系统、本地化系统、存档管理系统、物品系统、角色系统等），将对话系统和游戏内容分离后作为框架在本仓库中开源，其余系统在分离游戏内容后会作为示例供大家参考。

对话框架的代码主要受到了 Minecraft 和 RPG Maker MV 的启发，剧本语法参考了 [inkle's narrative scripting language](https://github.com/inkle/inky) 和 [Librian](https://github.com/RimoChan/Librian)，在此对以上项目表示感谢。

- 最初版本类、方法、枚举和变量的命名很像 Minecraft 的代码（MCP 反混淆），因为我是从写 Minecraft Mod 入门的游戏开发，所以受 Minecraft 代码风格影响很深
- 对话系统的设计参考了 RPG Maker MV，因为当时战争冒险企划已经在 RPG Maker MV 上做了大量的原型设计工作，我只需要移植到 Unity 上就好了
