using Duskvern;
using TMPro;
using UnityEngine;

public class GamePanel : IUIPanel<GameOpenParams>
{
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI descTxt;

    protected override void OnAwake()
    {
        
    }

    protected override void OnClose()
    {
        
    }

    protected override IUIPanelBase OnOpen(GameOpenParams openUIParams)
    {
        levelTxt.text = openUIParams.TabIndex.ToString();
        descTxt.text = openUIParams.GoodsId;
        return this;
    }
}

public sealed class GameOpenParams : IOpenUIParam, IPoolable
{
    public int TabIndex;
    public string GoodsId;

    public void OnDeSpawn()
    {
        
    }

    public void OnSpawn()
    {
        
    }
}


