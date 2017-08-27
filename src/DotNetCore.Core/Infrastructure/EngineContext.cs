using DotNetCore.Core.Interface;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

namespace DotNetCore.Core.Infrastructure
{
    public class EngineContext
    {
        #region Properties

        public static IEngine Current
        {
            get
            {
                return Singleton<IEngine>.Instance;
            }
        }

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(IServiceCollection serviceCollection,bool forceRecreate)
        {
            if (Singleton<IEngine>.Instance == null || forceRecreate)
            {
                Singleton<IEngine>.Instance = new WebEngine();
                Singleton<IEngine>.Instance.Initialize(serviceCollection);
            }
            return Singleton<IEngine>.Instance;
        }

        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        #endregion

    }
}
