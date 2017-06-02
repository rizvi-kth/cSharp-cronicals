using Autofac;

namespace AutoFac_Scope
{
    public interface IServiceProvider
    {
        IContainer AppContainer { get; set; }
        T GetService<T>();
    }
}