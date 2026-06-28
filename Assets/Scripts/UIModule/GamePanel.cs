using Duskvern;
using UnityEngine;

public class GamePanel : IUIPanel<GameOpenParams>
{
    protected override void OnAwake()
    {
        
    }

    protected override void OnClose()
    {
        
    }

    protected override IUIPanelBase OnOpen(GameOpenParams openUIParams)
    {
        return this;
    }
}

public sealed class GameOpenParams : IOpenUIParam
{
    public int TabIndex;
    public string GoodsId;
}


