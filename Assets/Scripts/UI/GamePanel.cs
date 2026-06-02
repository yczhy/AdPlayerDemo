using Duskvern;
using UnityEngine;

public class GamePanel : IUIPanel<ShopOpenParams>
{
    public override string uiName => UIPanelName.GamePanel;

    public override void OnClose()
    {

    }

    protected override void OnOpen(ShopOpenParams openUIParams)
    {

    }
}

public sealed class ShopOpenParams : IOpenUIParams
{
    public int TabIndex;
    public string GoodsId;
}

public static partial class UIPanelName
{
    public const string GamePanel = "GamePanel";
}

