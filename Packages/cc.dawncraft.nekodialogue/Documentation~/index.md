# NekoDialogue

**NekoDialogue** 是一个用于开发游戏对话系统的框架。

## 剧本语法

若采用默认方式加载剧本文件，则剧本文件应放置于 `Assets/Resources/Dialogues` 目录下。

一个简单的剧本示例如下：

```text
# @Title: 剧本示例
# @Date: 2022年2月25日13:33:14
# @Author: QingChenW

# 井号开头的是注释, 会被忽略掉, 也可以加在每行的末尾
* Unused # 就像这样

# 星号开头的是标签, 选项和跳转可以跳到这里
# 开始对话的时候也可以从某标签开始, 如果不填标签则会从头开始
* Start
# 下面两行代表清空相应位置的立绘
[L]
[R]
# 下面两行是展示相应位置的立绘, 位置和图片分别是可省的
# 省略位置代表使用之前声明过的, 如果没声明默认主角在左, 其他角色全部在右
# 省略图片代表使用之前声明过的, 如果没声明过则使用该人物在物品栏中展示的立绘
[L]flora(Flora0_0)
[R]lekanta(Lekanta_0)
# 声明立绘位置和图片后, 如果后续的声明与之前的相同则可以省略不写
# 说话的同时也会让相应的人物变亮, 其他人变暗
flora|我: 怎么了
# 竖线右面是显示在屏幕上的名称, 如果不写则使用左边的角色id作为名称
lekanta|母亲: 该起床了我的孩子
# 不写名字就是旁白
# 旁白说话时所有人都会变暗
: (电视机) 现在播报早间新闻....
flora|我: 没有战争的日子真好
lekanta|母亲: 好啦, 快去洗漱, 然后去吃早饭
# 选项, 每组选项必须写在一起, 就像下面这样
? 嗯 => Choice1
? ... => Choice2

* Choice1
flora: 这就去
> GOTO End

* Choice2
flora: ......
> GOTO End

* End
# 触发参数为"1"的蓝图事件, 并且会暂停对话但不会关闭对话窗口
# 如果想结束对话或者继续对话需要在蓝图中调用相应的图块
> TRIGGER 1
# 结束对话并关闭对话窗口
> END

# 如果写错了不要担心, 无法解析的剧本会在Unity的日志窗口里报错的
# 例如下面这条
? ahdaok 这什么玩意我瞎写的
# 或者这条
> 不存在的命令 不存在的参数
```

## 可视化编程

NekoDialogue 支持 Unity 的可视化编程工具 Bolt（Visual Scripting），这是通过反射自动生成 Reflection Units 来实现的。

Bolt 现已被 Unity 官方收购，在 2020 版本前，您需要购买并安装 Bolt 插件，在 2021 版本及以后，Bolt 已经更名为 Visual Scripting 并集成在 Unity 中，您只需在 Package Manager 中安装 Visual Scripting 即可。

接下来需要将 NekoDialogue 加入到扫描程序集中：

- 对于 Bolt 来说，您需要打开 `Tools -> Bolt -> Unit Options Wizard`，在 `Assemblies` 页面中添加 `Dawncraft.NekoDialogue` 程序集，再点击生成即可。

- 对于 Visual Scripting 来说，您需要打开 `Edit -> Project Settings...`，点击 `Visual Scripting`，然后在 `Node Library` 中添加 `Dawncraft.NekoDialogue` 程序集，再点击 `Regenerate Nodes` 即可。

若想获得更好的可视化编程体验，您也可以手动实现调用了 DialogueManager 相关接口的 Unit，如对话事件、开始对话、继续对话、结束对话等，具体请参考 Visual Scripting 的官方文档。

## API

请见代码中的文档注释
