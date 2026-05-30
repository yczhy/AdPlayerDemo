using UnityEngine;
using UnityEngine.UI;

public class TestManager : MonoBehaviour
{
    public Button rewardBtn;
    public Button InterstitialBtn;

    private void Start()
    {
        rewardBtn.onClick.AddListener(OnClickRewardAd);
        InterstitialBtn.onClick.AddListener(OnClickInterstitialAd);
    }
 
    private void OnClickRewardAd()
    {
        
    }

    private void OnClickInterstitialAd()
    {
        
    }
}
