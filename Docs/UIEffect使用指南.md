# UIEffect 流程式使用教程

这份文档按“跟着做”的方式整理，只讲使用流程，不讲 shader、材质仓库、网格修改等底层实现。目标是：你可以从一个空白 UI 练习场景开始，逐步学会用 `UIEffect` 做灰度、改色、模糊、扫光、溶解、描边、渐变、预设复用、批量复用和代码控制。

建议学习方式：

- 第一遍：从第 2 章开始按顺序做，不需要理解所有参数。
- 第二遍：每做完一个效果，回到第 15 章参数速查，对照理解参数。
- 第三遍：把第 14 章和项目里的 UI 打开/关闭流程结合起来。

## 1. 插件是什么

`UIEffect` 是 `Coffee.UIEffects` 命名空间下的 UGUI 特效组件。

它可以挂在这些 UI 组件所在的 GameObject 上：

- `Image`
- `RawImage`
- `Text`
- `TextMeshProUGUI`

常见用途：

- 灰度、复古、反相、海报化
- 改色、叠色、HSV 调整、对比度
- 模糊、像素化、RGB 偏移
- 溶解、扫光、遮罩、燃烧、图案转场
- 阴影、描边、镜像
- 渐变、边缘高亮、细节纹理

当前项目已经安装：

- 包名：`com.coffee.ui-effect`
- 版本：`5.11.3`
- 示例场景位置：`Assets/Samples/UI Effect/5.11.3/Demo`
- 项目设置：`Assets/ProjectSettings/UIEffectProjectSettings.asset`

## 2. 教学路线总览

这份教程包含“练习前准备 + 10 个实操练习 + 项目接入”。每个实操练习都包含“目标、适用场景、操作步骤、观察点、完成标准、常见问题”。

推荐顺序：

1. 准备练习场景：搭一个专门试效果的 UI 面板。
2. 灰度按钮：认识 `UIEffect` 最基础的使用方式。
3. 颜色高亮：学习 `Color Filter`。
4. 模糊和像素化：学习 `Sampling Filter`。
5. 循环扫光：学习 `Transition Filter + UIEffectTweener`。
6. 溶解打开/关闭：学习用 `Transition Rate` 做 UI 出入场。
7. 文字描边：学习 `Shadow Mode`。
8. 渐变标题：学习 `Gradation Mode`。
9. 保存和复用预设：学习 `Load`、`Append`、`Save As New`。
10. 批量复用效果：学习 `UIEffectReplica`。
11. 代码控制：在脚本里添加、修改、播放效果。
12. 接入项目 UI 流程：理解怎么和 `UITransitionAnimator` 搭配。

## 3. 练习前准备

### 3.1 创建一个练习场景

建议不要直接在业务场景里试。先创建一个干净的练习场景，方便大胆调参数。

操作：

1. 在 Unity 打开项目。
2. 在 `Assets/Scenes` 下创建一个新场景，比如 `UIEffect_Lab.unity`。
3. 打开这个场景。
4. 创建一个 `Canvas`。
5. Canvas 下创建一个空物体，命名为 `UIEffect_LabRoot`。
6. 在 `UIEffect_LabRoot` 下创建几个练习对象。

推荐层级：

```text
Canvas
  UIEffect_LabRoot
    Button_Gray
    Button_Shine
    Image_Dissolve
    Text_Outline
    Text_Gradation
    Icon_Replica_Source
    Icon_Replica_Copy_01
    Icon_Replica_Copy_02
```

### 3.2 准备几个基础 UI 元素

建议准备这些对象：

- 一个普通按钮：用来练灰度、改色、扫光。
- 一张图片：用来练溶解、模糊、像素化。
- 一段 TMP 文本：用来练描边、渐变。
- 三个图标：用来练 `UIEffectReplica`。

如果只是学习插件，不需要做精致 UI。能看清颜色和形状即可。

### 3.3 打开官方 Demo 作为对照

项目已经导入官方 Demo。学习时可以一边练，一边对照 Demo。

推荐打开顺序：

1. `Assets/Samples/UI Effect/5.11.3/Demo/UIEffect_Showcase.unity`
2. `Assets/Samples/UI Effect/5.11.3/Demo/UIEffect_AvailableFilters.unity`
3. `Assets/Samples/UI Effect/5.11.3/Demo/UIEffect_Tweener.unity`
4. `Assets/Samples/UI Effect/5.11.3/Demo/UIEffect_PatternAndEdge.unity`
5. `Assets/Samples/UI Effect/5.11.3/Demo/UIEffect_TextMeshProSupport.unity`

建议做法：

- 先打开 Demo 看效果。
- 再回自己的练习场景复现。
- 不要一开始照着 Demo 复制所有参数，先做出核心效果。

## 4. 练习一：灰度按钮

### 4.1 目标

做出一个“按钮不可用”的灰度效果。

这是最适合入门的效果，因为它只需要一个组件和两个参数。

### 4.2 适用场景

- 按钮未解锁。
- 功能暂不可用。
- 物品不可购买。
- 技能冷却中。

### 4.3 操作步骤

1. 选中 `Button_Gray`。
2. 找到按钮上的主 `Image` 对象。如果按钮层级里有 Background/Image，选真正显示图像的那个。
3. Inspector 点击 `Add Component`。
4. 搜索 `UIEffect`。
5. 添加 `UIEffect`。
6. 找到 `Tone Filter`。
7. 设置 `Tone Filter = Grayscale`。
8. 设置 `Tone Intensity = 1`。

### 4.4 观察点

你应该看到按钮颜色变成灰度。

如果 `Tone Intensity` 从 `0` 拖到 `1`：

- `0`：完全没有灰度。
- `0.5`：一半灰度。
- `1`：完全灰度。

### 4.5 完成标准

- 按钮可以从彩色变灰。
- 调整 `Tone Intensity` 时效果平滑变化。
- 你知道这个效果由 `Tone Filter` 控制。

### 4.6 常见问题

如果没有变化，检查：

- `UIEffect` 是否挂在有 `Image` 的同一个 GameObject 上。
- `Tone Filter` 是否仍是 `None`。
- `Tone Intensity` 是否为 `0`。
- UI 元素本身是不是透明或被其他对象挡住。

### 4.7 进阶练习

做一个“半禁用”状态：

- `Tone Filter = Grayscale`
- `Tone Intensity = 0.6`
- `Color Filter = Multiply`
- `Color` 选择偏暗颜色
- `Color Intensity = 0.3`

## 5. 练习二：颜色高亮

### 5.1 目标

做出按钮选中、高亮、受击、可领取等奖励提示效果。

### 5.2 适用场景

- 当前选中按钮。
- 奖励可领取。
- 商店物品高亮。
- 任务目标提示。

### 5.3 操作步骤

1. 选中一个按钮或图标。
2. 添加 `UIEffect`。
3. 找到 `Color Filter`。
4. 设置 `Color Filter = Additive`。
5. 设置 `Color` 为偏亮的黄色或白色。
6. 设置 `Color Intensity = 0.3`。
7. 慢慢把 `Color Intensity` 拖到 `1`，观察变化。

### 5.4 参数理解

`Color Filter` 决定颜色如何叠到原 UI 上。

常用选择：

- `Multiply`：压暗、染色。
- `Additive`：提亮、发光感。
- `Replace`：替换颜色。
- `Hsv Modifier`：改色相、饱和度、明度。
- `Contrast`：改对比度。

初学建议：

- 做高亮先试 `Additive`。
- 做暗淡先试 `Multiply`。
- 做换色先试 `Hsv Modifier`。

### 5.5 完成标准

- 你能让一个按钮明显变亮。
- 你能通过 `Color Intensity` 控制高亮强弱。
- 你知道高亮适合从 `Additive` 开始调。

### 5.6 进阶练习：冷却暗淡

设置：

- `Color Filter = Multiply`
- `Color` 选择灰色或偏蓝灰色
- `Color Intensity = 0.5`

观察：

- UI 会被压暗。
- 很适合做不可点击、冷却、锁定状态。

## 6. 练习三：模糊、像素化、RGB 错位

### 6.1 目标

学习 `Sampling Filter`，做出模糊、像素化、故障感。

### 6.2 适用场景

- 背景 UI 虚化。
- 弹窗背后图片模糊。
- 像素风转场。
- 网络异常、受干扰、故障闪烁。

### 6.3 操作步骤：模糊

1. 选中一张图片，比如 `Image_Dissolve`。
2. 添加 `UIEffect`。
3. 设置 `Sampling Filter = Blur Fast`。
4. 设置 `Sampling Intensity = 0.4`。
5. 调整 `Sampling Width`，观察模糊范围。

### 6.4 操作步骤：像素化

1. 保持同一个 UI 元素。
2. 设置 `Sampling Filter = Pixelation`。
3. 从 `Sampling Intensity = 0.2` 开始。
4. 慢慢调高到 `1`。

### 6.5 操作步骤：RGB 错位

1. 设置 `Sampling Filter = Rgb Shift`。
2. 设置 `Sampling Intensity = 0.2`。
3. 设置 `Sampling Width = 1` 到 `3`。

### 6.6 参数理解

- `Sampling Intensity`：效果强度。
- `Sampling Width`：采样宽度，通常影响偏移或模糊范围。
- `Sampling Scale`：采样参考比例，高分辨率图片可以调大。

### 6.7 完成标准

- 你能做出轻微模糊。
- 你能做出像素化效果。
- 你知道 `Sampling` 类效果通常比简单变色更重，移动端要谨慎用。

### 6.8 注意事项

TextMeshPro 上使用时：

- `Blur Medium` 和 `Blur Detail` 不推荐。
- 插件会因为性能原因回退到 `Blur Fast`。

## 7. 练习四：循环扫光

### 7.1 目标

让按钮或图标上循环划过一道光。

这是 UIEffect 最常用、最有“动效感”的练习之一。

### 7.2 适用场景

- 主按钮吸引点击。
- 奖励可领取。
- 新手引导强调目标。
- 稀有物品发光。

### 7.3 组件组合

这个效果需要两个组件：

- `UIEffect`：负责扫光视觉。
- `UIEffectTweener`：负责让扫光动起来。

### 7.4 操作步骤：先做出静态扫光

1. 选中 `Button_Shine` 的主 Image。
2. 添加 `UIEffect`。
3. 设置 `Transition Filter = Shiny`。
4. 设置 `Transition Rate = 0.5`。
5. 设置 `Transition Width = 0.2`。
6. 设置 `Transition Softness = 0.2`。
7. 设置 `Transition Color` 为白色或淡黄色。

观察：

- 你应该能看到一条光带。
- 调整 `Transition Rate` 时，光带位置会变化。

### 7.5 操作步骤：让扫光动起来

1. 在同一个 GameObject 上添加 `UIEffectTweener`。
2. 设置 `Culling Mask = Transition`。
3. 设置 `Direction = Forward`。
4. 设置 `Duration = 1.2`。
5. 设置 `Wrap Mode = Loop`。
6. 设置 `Play On Enable = Forward`。
7. 进入 Play Mode 观察。

### 7.6 参数理解

`UIEffectTweener` 会把某些效果参数从 `0` 播到 `1`。

这里 `Culling Mask = Transition`，表示它会驱动 `Transition Rate`。

常用 `Wrap Mode`：

- `Once`：播放一次。
- `Loop`：循环播放。
- `PingPongOnce`：正向一次，再反向一次。
- `PingPongLoop`：来回循环。

### 7.7 完成标准

- 按钮上有一条扫光。
- 扫光可以循环移动。
- 你知道动起来的关键是 `UIEffectTweener.Culling Mask = Transition`。

### 7.8 常见问题

扫光不动：

- 检查是否添加了 `UIEffectTweener`。
- 检查 `Culling Mask` 是否勾选 `Transition`。
- 检查 `Play On Enable` 是否为 `Forward`。
- 检查是否进入 Play Mode。

扫光太宽：

- 降低 `Transition Width`。

扫光边缘太硬：

- 提高 `Transition Softness`。

扫光方向不对：

- 改 `Direction`。
- 或勾选 `Transition Reverse`。

## 8. 练习五：溶解打开和关闭

### 8.1 目标

做一个 UI 出现/消失的溶解效果。

### 8.2 适用场景

- 弹窗打开。
- 奖励卡片出现。
- 图片揭示。
- 技能图标解锁。

### 8.3 操作步骤：做出溶解状态

1. 选中 `Image_Dissolve`。
2. 添加 `UIEffect`。
3. 设置 `Transition Filter = Dissolve`。
4. 设置 `Transition Rate = 0`。
5. 慢慢把 `Transition Rate` 拖到 `1`。
6. 观察图片显示和隐藏的变化。

### 8.4 找到显示方向

不同设置下，`Transition Rate = 0` 和 `Transition Rate = 1` 哪个是显示、哪个是隐藏，需要你先观察。

练习时记录：

```text
我的 Dissolve：
Transition Rate = 0 时：显示 / 隐藏
Transition Rate = 1 时：显示 / 隐藏
```

如果方向和你想要的相反：

- 勾选 `Transition Reverse`。
- 或在 Tweener 里用 `PlayReverse`。

### 8.5 加入 Tweener

1. 添加 `UIEffectTweener`。
2. 设置 `Culling Mask = Transition`。
3. 设置 `Wrap Mode = Once`。
4. 设置 `Duration = 0.35` 到 `0.8`。
5. 设置 `Play On Enable = Forward`。
6. 进入 Play Mode。

### 8.6 做关闭动画

如果要关闭时反向播放：

- Inspector 中可以把 `Direction = Reverse`。
- 代码里可以调用 `tweener.PlayReverse(true)`。

### 8.7 完成标准

- 你能手动拖 `Transition Rate` 控制溶解进度。
- 你能用 `UIEffectTweener` 播放一次溶解。
- 你知道如何反转方向。

### 8.8 常见问题

效果不明显：

- 调整 `Transition Softness`。
- 调整 `Transition Width`。
- 给 `Transition Color` 一个亮色。

想要特定图案溶解：

- 给 `Transition Tex` 指定一张黑白/带 Alpha 的纹理。
- 纹理要看 Alpha 通道。

## 9. 练习六：文字描边

### 9.1 目标

给文字加描边，让它在复杂背景上更清晰。

### 9.2 适用场景

- 标题文字。
- 战斗数值。
- 按钮文字。
- 浮动提示。

### 9.3 操作步骤

1. 创建或选中一个 `TextMeshProUGUI`。
2. 添加 `UIEffect`。
3. 设置 `Shadow Mode = Outline`。
4. 设置 `Shadow Color` 为黑色或深色。
5. 设置 `Shadow Distance = (1, -1)` 左右。
6. 如果描边不够明显，改成 `Outline8`。
7. 调整 `Shadow Fade`。

### 9.4 参数理解

- `Shadow Mode = Shadow`：更像投影。
- `Shadow Mode = Outline`：基础描边。
- `Shadow Mode = Outline8`：八方向描边，更完整。
- `Shadow Distance`：描边或阴影距离。
- `Shadow Fade`：透明度。

### 9.5 完成标准

- 文字有清晰描边。
- 你能区分 `Shadow` 和 `Outline` 的用途。

### 9.6 常见问题

描边太糊：

- 降低 `Shadow Blur Intensity`。
- 降低 `Shadow Distance`。

描边太粗：

- 降低 `Shadow Distance`。
- 从 `Outline8` 换回 `Outline`。

## 10. 练习七：渐变标题

### 10.1 目标

做一个带颜色渐变的标题或稀有度文字。

### 10.2 适用场景

- 金色奖励标题。
- 紫色史诗品质。
- 活动 Banner 文字。
- 排行榜名次文字。

### 10.3 操作步骤

1. 选中 `Text_Gradation`。
2. 添加 `UIEffect`。
3. 设置 `Gradation Mode = HorizontalGradient`。
4. 设置 `Gradation Intensity = 1`。
5. 编辑 `Gradation Gradient`。
6. 设置左侧颜色为深金色。
7. 设置右侧颜色为浅黄色或白色。

### 10.4 调整方向

如果要竖向渐变：

- 设置 `Gradation Mode = VerticalGradient`。

如果要斜向渐变：

- 设置 `Gradation Mode = AngleGradient`。
- 调整 `Gradation Rotation`。

### 10.5 做流动渐变

1. 添加 `UIEffectTweener`。
2. 设置 `Culling Mask = GradiationOffset`。
3. 设置 `Wrap Mode = Loop`。
4. 设置 `Duration = 2`。
5. 设置 `Play On Enable = Forward`。

注意：

- 插件里的枚举拼写是 `GradiationOffset`，不是 `GradationOffset`。Inspector 里照着选即可。

### 10.6 完成标准

- 文字有明显渐变。
- 你能调整横向、竖向、斜向。
- 你能让渐变动起来。

## 11. 练习八：保存和复用预设

### 11.1 目标

把调好的效果保存起来，之后一键复用。

### 11.2 为什么要学预设

如果每个按钮都手动调参数，会很快失控。预设适合保存这些常用效果：

- 禁用灰度。
- 按钮扫光。
- 金色标题。
- 图标高亮描边。
- 溶解出现。

### 11.3 保存预设

1. 选中已经调好效果的 UI 元素。
2. 找到 `UIEffect` Inspector 顶部的菜单。
3. 点击 `Save As New`。
4. 给预设起名，比如 `Button_Disabled_Gray`。

### 11.4 加载预设

1. 选中另一个 UI 元素。
2. 添加 `UIEffect`。
3. 点击 `Load`。
4. 选择刚保存的预设。

### 11.5 追加预设

`Append` 和 `Load` 不一样：

- `Load`：覆盖当前全部设置。
- `Append`：只追加预设里启用的效果模块。

练习：

1. 先做一个灰度效果。
2. 再 `Append` 一个描边效果。
3. 观察两个效果是否叠加。

### 11.6 运行时预设

如果要代码里调用：

```csharp
effect.LoadPreset("Button_Disabled_Gray");
```

需要先注册运行时预设：

1. 打开 `Edit > Project Settings > UI > UIEffect`。
2. 找到 `Runtime Presets`。
3. 把预设加进去。

当前项目里 `Runtime Presets` 为空，所以直接用代码加载前要先注册。

### 11.7 完成标准

- 你能保存一个预设。
- 你能把预设加载到另一个 UI 元素。
- 你知道 `Load` 和 `Append` 的区别。

## 12. 练习九：UIEffectReplica 批量复用

### 12.1 目标

让多个 UI 元素共享同一套效果。

### 12.2 适用场景

- 一组按钮同时变灰。
- 一组奖励图标都使用同样描边。
- 多个元素跟随同一个扫光或溶解参数。

### 12.3 准备对象

准备三个图标：

```text
Icon_Replica_Source
Icon_Replica_Copy_01
Icon_Replica_Copy_02
```

### 12.4 操作步骤

1. 选中 `Icon_Replica_Source`。
2. 添加 `UIEffect`。
3. 设置一个明显效果，比如：
   - `Edge Mode = Shiny`
   - `Edge Color = Yellow`
   - `Edge Width = 0.3`
4. 选中 `Icon_Replica_Copy_01`。
5. 添加 `UIEffectReplica`。
6. 把 `Icon_Replica_Source` 上的 `UIEffect` 拖到 `Target`。
7. 对 `Icon_Replica_Copy_02` 重复步骤 4 到 6。

### 12.5 观察点

现在你修改 Source 上的 `UIEffect`，两个 Copy 会跟着变化。

### 12.6 参数理解

- `Target`：要复制的 `UIEffect`。
- `Use Target Transform`：某些依赖空间位置的效果会使用目标的 Transform。
- `Custom Root`：指定自定义参考 Transform。
- `Sampling Scale`：复制时覆盖采样比例。
- `Allow To Modify Mesh Shape`：允许修改网格形状。

### 12.7 完成标准

- 你能用一个源效果控制多个 UI。
- 你知道批量统一效果优先考虑 `UIEffectReplica` 或预设。

## 13. 练习十：代码控制

### 13.1 目标

在脚本里添加和控制 UIEffect。

### 13.2 命名空间

```csharp
using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.UI;
```

### 13.3 代码添加灰度

```csharp
public class UIEffectGrayExample : MonoBehaviour
{
    [SerializeField] private Image targetImage;

    public void SetGray(bool gray)
    {
        var effect = targetImage.GetComponent<UIEffect>();
        if (effect == null)
        {
            effect = targetImage.gameObject.AddComponent<UIEffect>();
        }

        effect.toneFilter = ToneFilter.Grayscale;
        effect.toneIntensity = gray ? 1f : 0f;
    }
}
```

### 13.4 代码添加扫光

```csharp
public class UIEffectShinyExample : MonoBehaviour
{
    [SerializeField] private Image targetImage;

    public void PlayShinyLoop()
    {
        var effect = targetImage.GetComponent<UIEffect>();
        if (effect == null)
        {
            effect = targetImage.gameObject.AddComponent<UIEffect>();
        }

        effect.transitionFilter = TransitionFilter.Shiny;
        effect.transitionWidth = 0.2f;
        effect.transitionColor = Color.white;

        var tweener = targetImage.GetComponent<UIEffectTweener>();
        if (tweener == null)
        {
            tweener = targetImage.gameObject.AddComponent<UIEffectTweener>();
        }

        tweener.cullingMask = UIEffectTweener.CullingMask.Transition;
        tweener.wrapMode = UIEffectTweener.WrapMode.Loop;
        tweener.duration = 1.2f;
        tweener.playOnEnable = UIEffectTweener.PlayOnEnable.Forward;
        tweener.PlayForward(true);
    }
}
```

### 13.5 代码播放一次溶解

```csharp
public class UIEffectDissolveExample : MonoBehaviour
{
    [SerializeField] private UIEffect effect;
    [SerializeField] private UIEffectTweener tweener;

    public void PlayOpen()
    {
        effect.transitionFilter = TransitionFilter.Dissolve;
        tweener.cullingMask = UIEffectTweener.CullingMask.Transition;
        tweener.wrapMode = UIEffectTweener.WrapMode.Once;
        tweener.duration = 0.5f;
        tweener.PlayForward(true);
    }

    public void PlayClose()
    {
        tweener.PlayReverse(true);
    }
}
```

### 13.6 暂停时仍播放

暂停时如果 `Time.timeScale = 0`，普通 Tweener 不会动。设置成 Unscaled：

```csharp
tweener.updateMode = UIEffectTweener.UpdateMode.Unscaled;
```

### 13.7 完成标准

- 你能用代码拿到或添加 `UIEffect`。
- 你能修改灰度、扫光、溶解参数。
- 你能通过 `UIEffectTweener` 播放动画。

## 14. 接入当前项目 UI 流程

### 14.1 先区分两个“Transition”

当前项目里有：

```text
Assets/Scripts/UIModule/UITransitionAnimator.cs
```

它是项目自己的 UI 打开/关闭流程脚本。

UIEffect 里也有：

```text
Transition Filter
```

它是视觉转场效果，比如溶解、扫光、燃烧。

两者不是同一个东西，但可以配合。

### 14.2 当前项目 UITransitionAnimator 做了什么

当前脚本的逻辑很简单：

- `OpenUI(Action openAction)`：执行打开回调。
- `CloseUI(Action closeAction)`：执行关闭回调。

也就是说，它现在还没有真正播放动画。

### 14.3 推荐接入思路

如果要让某个面板打开时播放 UIEffect：

1. 在面板根节点或主背景图上添加 `UIEffect`。
2. 设置 `Transition Filter = Dissolve` 或 `Fade`。
3. 添加 `UIEffectTweener`。
4. 设置 `Culling Mask = Transition`。
5. 设置 `Wrap Mode = Once`。
6. 打开 UI 时调用 `PlayForward(true)`。
7. 关闭 UI 时调用 `PlayReverse(true)`，动画结束后再真正关闭。

### 14.4 教学版接入代码思路

下面是思路示例，不建议直接覆盖项目代码。你可以先单独写一个测试脚本验证。

```csharp
using System;
using Coffee.UIEffects;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UIEffectPanelTransition : MonoBehaviour
{
    [SerializeField] private UIEffectTweener tweener;

    public async UniTask PlayOpen(Action openAction)
    {
        openAction?.Invoke();

        if (tweener == null)
        {
            return;
        }

        tweener.PlayForward(true);
        await UniTask.Delay(TimeSpan.FromSeconds(tweener.totalTime));
    }

    public async UniTask PlayClose(Action closeAction)
    {
        if (tweener != null)
        {
            tweener.PlayReverse(true);
            await UniTask.Delay(TimeSpan.FromSeconds(tweener.totalTime));
        }

        closeAction?.Invoke();
    }
}
```

### 14.5 接入时的判断

如果面板是整体一张大背景：

- 把 `UIEffect` 挂在大背景上，最简单。

如果面板由很多子 UI 组成：

- 可以给每个关键子物体挂 `UIEffect`。
- 或使用 `UIEffectReplica` 统一效果。
- 或只给根背景做视觉转场，子 UI 用普通缩放/透明动画。

如果要做真正完整的 UI 入场动画：

- UIEffect 负责视觉效果。
- DOTween 或 Animator 负责位移、缩放、透明度。
- `UITransitionAnimator` 负责统一调度打开/关闭。

## 15. 参数速查：UIEffect 面板怎么读

可以把 Inspector 里的设置理解成几组效果模块。通常只开你需要的 1 到 3 组，不要一开始全开。

### Tone Filter

色调滤镜。

常用选项：

- `Grayscale`：变灰，常用于按钮禁用态。
- `Sepia`：复古棕黄色。
- `Negative`：反相。
- `Retro`：复古色。
- `Posterize`：色阶压缩，偏海报风格。

主要参数：

- `Tone Intensity`：强度，`0` 无效果，`1` 完全应用。

推荐用法：

- 禁用按钮：`Tone Filter = Grayscale`，`Tone Intensity = 1`。
- 受伤/异常状态：短时间把 `Negative` 或 `Retro` 播一下。

### Color Filter

颜色处理。

常用选项：

- `Multiply`：乘色，适合压暗或染色。
- `Additive`：加色，适合发亮、高亮。
- `Replace`：替换颜色。
- `Hsv Modifier`：改色相、饱和度、明度。
- `Contrast`：调对比度。

主要参数：

- `Color`
- `Color Intensity`
- `Color Glow`

推荐用法：

- 选中态：`Color Filter = Additive`，调一个亮色。
- 冷却中：`Color Filter = Multiply`，颜色偏暗。
- 换皮预览：`Color Filter = Hsv Modifier`。

### Sampling Filter

采样类效果，会读取周围像素，常用于模糊和像素化。

常用选项：

- `Blur Fast`：轻量模糊。
- `Blur Medium`：中等模糊。
- `Blur Detail`：更细致的模糊。
- `Pixelation`：像素化。
- `Rgb Shift`：RGB 错位。
- `Edge Luminance` / `Edge Alpha`：边缘检测。

主要参数：

- `Sampling Intensity`
- `Sampling Width`
- `Sampling Scale`

推荐用法：

- 背景 UI 虚化：`Blur Fast`，低强度开始调。
- 故障感：`Rgb Shift`，配合 `UIEffectTweener` 做短动画。
- 像素风过渡：`Pixelation`。

注意：

- TextMeshPro 使用时，`Blur Medium` 和 `Blur Detail` 会因为性能限制回退到 `Blur Fast`。

### Transition Filter

转场类效果。它最适合做“出现/消失/扫过”的动态效果。

常用选项：

- `Fade`：淡入淡出。
- `Cutoff`：硬切。
- `Dissolve`：溶解。
- `Shiny`：扫光。
- `Mask`：遮罩。
- `Melt`：融化。
- `Burn`：燃烧边缘。
- `Blaze`：火焰/光焰式过渡。
- `Pattern`：按图案过渡。

主要参数：

- `Transition Rate`：进度，通常配合动画从 `0` 到 `1`。
- `Transition Reverse`：反转方向。
- `Transition Tex`：用于溶解、图案、遮罩等效果的纹理。
- `Transition Width`：边缘宽度。
- `Transition Softness`：边缘柔和度。
- `Transition Color`：过渡边缘颜色。
- `Transition Auto Play Speed`：自动播放速度。

推荐用法：

- 图片扫光：`Transition Filter = Shiny`，再加 `UIEffectTweener` 循环播放 `Transition`。
- 弹窗溶解出现：`Transition Filter = Dissolve`，用 `Transition Rate` 做开关动画。
- 奖励图标燃烧高亮：`Transition Filter = Burn`，调边缘颜色。

注意：

- `Transition Tex` 主要看纹理 Alpha 通道。
- 如果用纹理的 `Scale`、`Offset`、`Speed`，纹理导入设置里的 `Wrap Mode` 通常要设为 `Repeat`。
- `Transition Auto Play Speed` 受 `Time.timeScale` 影响。暂停时还要动，用 `UIEffectTweener` 的 `Update Mode = Unscaled`。

### Target Mode

限制效果应用范围。

常用选项：

- `Hue`：按色相筛选。
- `Luminance`：按亮度筛选。

主要参数：

- `Target Color`
- `Target Range`
- `Target Softness`

推荐用法：

- 只让图片里某种颜色变亮。
- 只对暗部或亮部加效果。

### Blend Type

控制效果材质的混合方式。

常用选项：

- `Alpha Blend`：普通透明混合。
- `Multiply`：正片叠底感。
- `Additive`：发光叠加。
- `Soft Additive`：柔和加色。
- `Multiply Additive`：乘色加亮。

一般建议：

- 普通 UI 保持 `Alpha Blend`。
- 发光、扫光、高亮试 `Additive` 或 `Soft Additive`。

### Shadow Mode

阴影和描边。

常用选项：

- `Shadow`：普通阴影。
- `Shadow3`：更强的阴影。
- `Outline`：描边。
- `Outline8`：八方向描边。
- `Mirror`：镜像倒影。

主要参数：

- `Shadow Distance`
- `Shadow Iteration`
- `Shadow Color`
- `Shadow Fade`
- `Shadow Blur Intensity`

推荐用法：

- 文字描边：`Shadow Mode = Outline` 或 `Outline8`。
- 图标投影：`Shadow Mode = Shadow`。
- 倒影展示：`Shadow Mode = Mirror`。

### Gradation Mode

渐变。

常用选项：

- `Horizontal`
- `Vertical`
- `Radial`
- `Diagonal`
- `Angle`
- 带 `Gradient` 后缀的模式可以使用渐变条。

主要参数：

- `Gradation Color 1`
- `Gradation Color 2`
- `Gradation Gradient`
- `Gradation Intensity`
- `Gradation Offset`
- `Gradation Scale`
- `Gradation Rotation`

推荐用法：

- 彩色标题文字。
- 稀有度边框颜色。
- 进度条或徽章质感。

### Edge Mode

边缘效果。

常用选项：

- `Plain`：普通边缘。
- `Shiny`：边缘高光。

主要参数：

- `Edge Width`
- `Edge Color`
- `Edge Shiny Width`
- `Edge Shiny Auto Play Speed`

推荐用法：

- 可点击对象边缘高亮。
- 稀有物品图标边缘闪光。

### Detail Filter

叠加细节纹理或遮罩。

常用选项：

- `Masking`
- `Multiply`
- `Additive`
- `Subtractive`
- `Replace`
- `MultiplyAdditive`

主要参数：

- `Detail Tex`
- `Detail Color`
- `Detail Intensity`
- `Detail Threshold`
- `Scale`
- `Offset`
- `Speed`

推荐用法：

- 给 UI 加纹理质感。
- 做全息、扫描线、噪声感。
- 用细节贴图局部遮罩效果。

## 16. 常用效果配方速查

### 禁用按钮变灰

目标：按钮不可点击时变灰。

设置：

- 添加 `UIEffect`
- `Tone Filter = Grayscale`
- `Tone Intensity = 1`

可选：

- 再加 `Color Filter = Multiply`
- 颜色调暗一点

### 按钮循环扫光

目标：按钮表面循环划过一道光。

设置：

- 添加 `UIEffect`
- `Transition Filter = Shiny`
- 调整 `Transition Width`
- 调整 `Transition Color`
- 添加 `UIEffectTweener`
- `Culling Mask = Transition`
- `Wrap Mode = Loop`
- `Play On Enable = Forward`
- `Duration = 1` 到 `2`

如果方向不对：

- 勾选 `Transition Reverse`
- 或把 `UIEffectTweener.Direction` 改成 `Reverse`

### 弹窗溶解打开

目标：弹窗出现时从溶解状态恢复。

设置：

- 弹窗根节点或主要图片上添加 `UIEffect`
- `Transition Filter = Dissolve`
- 调整 `Transition Softness`
- 调整 `Transition Color`
- 添加 `UIEffectTweener`
- `Culling Mask = Transition`
- `Wrap Mode = Once`
- `Play On Enable = Forward`
- 通过 `Transition Reverse` 或 `PlayReverse` 调整出现方向

建议：

- 如果多个子物体都要一起溶解，先在一个父级或统一底图上验证效果。
- 如果必须多个元素统一变化，可以考虑 `UIEffectReplica`。

### 文字描边

目标：让文字更清晰。

设置：

- 文字对象添加 `UIEffect`
- `Shadow Mode = Outline` 或 `Outline8`
- 调整 `Shadow Color`
- 调整 `Shadow Distance`
- 必要时调 `Shadow Iteration`

### 图标高亮描边

目标：选中某个图标时边缘发亮。

设置：

- 添加 `UIEffect`
- `Edge Mode = Shiny`
- 调整 `Edge Width`
- 调整 `Edge Color`
- 调整 `Edge Shiny Width`
- 需要动起来时添加 `UIEffectTweener`
- `Culling Mask` 勾 `EdgeShiny`

### 稀有度渐变标题

目标：标题文字有金色、紫色等渐变。

设置：

- 文字对象添加 `UIEffect`
- `Gradation Mode = HorizontalGradient` 或 `AngleGradient`
- 调整 `Gradation Gradient`
- `Gradation Intensity = 1`
- 需要方向变化时调 `Gradation Rotation`

### 故障闪烁效果

目标：短时间出现 RGB 错位或像素抖动。

设置：

- 添加 `UIEffect`
- `Sampling Filter = Rgb Shift` 或 `Pixelation`
- 调低 `Sampling Intensity`
- 添加 `UIEffectTweener`
- `Culling Mask = Sampling`
- `Wrap Mode = Once`
- 用代码或事件触发 `PlayForward(true)`

## 17. 预设使用速查

### Inspector 里使用预设

`UIEffect` Inspector 顶部有预设菜单：

- `Load`：加载预设，会覆盖当前设置。
- `Append`：追加预设，只覆盖预设里启用的部分。
- `Save As New`：把当前效果保存成新预设。
- `Clear`：清空当前效果。

推荐习惯：

- 从 `Load` 找接近的效果。
- 用 `Append` 组合两个效果。
- 调满意后 `Save As New` 保存为自己的预设。

### 运行时使用预设

如果要在代码里 `LoadPreset("xxx")`：

1. 先创建或保存预设。
2. 打开 `Edit > Project Settings > UI > UIEffect`。
3. 把预设注册到 `Runtime Presets`。
4. 代码里调用 `effect.LoadPreset("预设名")`。

当前项目的 `Runtime Presets` 还是空的，所以直接写 `LoadPreset("Dissolve")` 这类代码前，要先注册运行时预设。

## 18. 代码调用速查

命名空间：

```csharp
using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.UI;
```

给一个 `Image` 添加灰度：

```csharp
var image = GetComponent<Image>();
var effect = image.gameObject.AddComponent<UIEffect>();

effect.toneFilter = ToneFilter.Grayscale;
effect.toneIntensity = 1f;
```

做一个扫光并循环播放：

```csharp
var image = GetComponent<Image>();
var effect = image.gameObject.AddComponent<UIEffect>();

effect.transitionFilter = TransitionFilter.Shiny;
effect.transitionWidth = 0.2f;
effect.transitionColor = Color.white;

var tweener = image.gameObject.AddComponent<UIEffectTweener>();
tweener.cullingMask = UIEffectTweener.CullingMask.Transition;
tweener.wrapMode = UIEffectTweener.WrapMode.Loop;
tweener.duration = 1.2f;
tweener.playOnEnable = UIEffectTweener.PlayOnEnable.Forward;
tweener.PlayForward(true);
```

暂停期间仍然播放：

```csharp
var tweener = GetComponent<UIEffectTweener>();
tweener.updateMode = UIEffectTweener.UpdateMode.Unscaled;
```

手动控制转场进度：

```csharp
var effect = GetComponent<UIEffect>();
effect.transitionFilter = TransitionFilter.Dissolve;
effect.transitionRate = 0.5f;
```

运行时加载已注册预设：

```csharp
var effect = GetComponent<UIEffect>();
effect.LoadPreset("MyDissolve");
```

## 19. 和项目 UI 代码的关系速查

当前项目里有一个 `Duskvern.UITransitionAnimator`，位置是：

`Assets/Scripts/UIModule/UITransitionAnimator.cs`

它目前只是在打开/关闭 UI 时调用回调，本身没有接入 `UIEffect`。

所以不要把这两个概念混在一起：

- `Duskvern.UITransitionAnimator`：项目自己的 UI 打开/关闭流程。
- `Coffee.UIEffects.UIEffect`：插件的视觉特效组件。
- `Transition Filter`：UIEffect 里的视觉转场效果。

如果以后要把 UIEffect 接入项目打开/关闭流程，可以在 `UITransitionAnimator.OpenUI` 和 `CloseUI` 里控制 `UIEffectTweener` 播放。

## 20. 推荐学习路线速查

### 第一步：只学 UIEffect

目标：会手动调静态效果。

练习：

- 灰度按钮
- 彩色标题
- 图标描边
- 模糊图片

### 第二步：学 UIEffectTweener

目标：会让效果动起来。

练习：

- 按钮扫光
- 溶解出现
- RGB 故障闪一下
- 边缘高光循环

### 第三步：学预设

目标：把调好的效果复用。

练习：

- 保存一个“禁用灰度”预设
- 保存一个“按钮扫光”预设
- 保存一个“稀有金色标题”预设
- 用 `Append` 组合效果

### 第四步：学 UIEffectReplica

目标：一套效果控制多个 UI。

练习：

- 一组按钮共用同一个灰度效果
- 一组奖励图标共用同一个边缘高光
- 调主效果，观察副本同步变化

## 21. 示例场景速查

项目已经导入了官方 Demo：

- `Assets/Samples/UI Effect/5.11.3/Demo/UIEffect_Showcase.unity`
- `Assets/Samples/UI Effect/5.11.3/Demo/UIEffect_AvailableFilters.unity`
- `Assets/Samples/UI Effect/5.11.3/Demo/UIEffect_Tweener.unity`
- `Assets/Samples/UI Effect/5.11.3/Demo/UIEffect_PatternAndEdge.unity`
- `Assets/Samples/UI Effect/5.11.3/Demo/UIEffect_TextMeshProSupport.unity`

推荐打开顺序：

1. `UIEffect_Showcase.unity`：先看整体效果。
2. `UIEffect_AvailableFilters.unity`：看每种 Filter 长什么样。
3. `UIEffect_Tweener.unity`：学习动画怎么播。
4. `UIEffect_PatternAndEdge.unity`：学习图案转场和边缘效果。
5. `UIEffect_TextMeshProSupport.unity`：学习 TMP 文本效果。

## 22. 常见问题

### 加了组件但没效果

检查：

- 是否挂在 `Image`、`RawImage`、`Text`、`TextMeshProUGUI` 同一个 GameObject 上。
- 对应 Filter 是否不是 `None`。
- 强度是否大于 `0`。
- UI 元素本身是否可见、Alpha 是否为 0。

### 动画不播放

检查：

- 是否添加了 `UIEffectTweener`。
- `Culling Mask` 是否勾了你想动画的模块。
- `Play On Enable` 是否不是 `None`。
- 或者是否在代码里调用了 `PlayForward(true)`。

### 暂停时效果不动

原因：

- 默认使用 `Time.deltaTime`。

解决：

- `UIEffectTweener.Update Mode = Unscaled`
- 或代码设置 `tweener.updateMode = UIEffectTweener.UpdateMode.Unscaled`

### 打包后某些效果丢失

可能原因：

- 运行时用到的 shader variant 没注册。

检查位置：

- `Edit > Project Settings > UI > UIEffect`
- 查看 `Registered Variants` / `Unregistered Variants`

### 多个 UI 想统一效果

做法：

- 一个物体上调好 `UIEffect`。
- 其他物体用 `UIEffectReplica` 指向它。
- 或保存为预设后批量加载。

## 23. 实战建议

- 初学时一次只开一个 Filter，确认效果后再叠第二个。
- 优先使用 Inspector 调参，满意后再考虑代码控制。
- 动画优先用 `UIEffectTweener`，比自己写 Update 改参数更直观。
- 常用效果保存成预设，避免每个按钮重复调。
- 模糊、描边、阴影、采样类效果更可能影响性能，移动端先少量使用。
- `Transition Filter` 做显示/隐藏时，先用一个单独 Image 验证 `0` 和 `1` 哪个是显示、哪个是隐藏，再接入 UI 流程。
