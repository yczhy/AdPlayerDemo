using Cysharp.Threading.Tasks;
using Duskvern;
using UnityEngine;
using UnityEngine.UI;

public class TestManager : MonoBehaviour
{
    public Button rewardBtn;
    public Button InterstitialBtn;

    private void Start()
    {
        // rewardBtn.onClick.AddListener(OnClickRewardAd);
        // InterstitialBtn.onClick.AddListener(OnClickInterstitialAd);
        AssetUtils.Initialize(AssetLoadType.Addressable);
        PoolUtil.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var _param = ClassPool<GameOpenParams>.Pop();
            UIModule.Instance.OpenPanel(UIPanelType.GamePanel, _param).Forget();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            UIModule.Instance.ClosePanel(UIPanelType.GamePanel).Forget();
        }
    }
 
    private void OnClickRewardAd()
    {
        
    }

    private void OnClickInterstitialAd()
    {
        
    }
}
