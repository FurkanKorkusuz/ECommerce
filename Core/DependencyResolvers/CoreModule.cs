using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.MemoryChache;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Core.DependencyResolvers
{
    /// <summary>
    /// Artık Startup kısmında servise ekleyeceğim modulleri buradan merkezi olarak ekleyebilirim.
    /// services.AddMemoryCache(); yazdığımda services den sonraki referanslara ulaşamadım bu yüzden CoreModule kullanımından şimdilik vazgeçtim.
    /// services.AddMemoryCache(); kodumu Startup a yazacağım.
    /// </summary>
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddMemoryCache();

            // dependency resolver...
            services.AddSingleton<ICacheService, MemoryCacheManager>();
        }
    }
}
