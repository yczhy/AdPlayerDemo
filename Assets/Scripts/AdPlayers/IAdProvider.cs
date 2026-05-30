namespace Duskvern
{
    public interface IAdProvider
    {
        AdType AdType { get; }
        string AdUnitId { get; }

        void Init(AdType adType, string adUnitId);

        void LoadAd();
        bool IsReady { get; }
    }
}