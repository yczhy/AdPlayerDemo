using Cysharp.Threading.Tasks;

namespace Duskvern
{
    public interface IAdProvider
    {
        AdType AdType { get; }
        string AdUnitId { get; }
        bool IsReady { get; }
        AdPlatform ADPlatform { get; }
        double ECPM { get; }

        void Init(AdProviderParameters adProviderParameters);
        void Dispose();

        void LoadAd();
        void ShowAd();
    }
}