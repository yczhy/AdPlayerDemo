namespace Duskvern
{
    /// <summary>
    /// 单例接口，约束实现类为单例
    /// </summary>
    public interface ISingleton
    {
        void Init(); // 单例初始化方法
        void Dispose();    // 单例销毁方法
    }
}
