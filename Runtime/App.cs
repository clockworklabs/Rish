using System;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using UnityEngine;

namespace RishUI
{
    public interface IApp
    {
        Element GetRoot();
    }
    
    [PoolSize(1)]
    public class App : RishElement<AppProps>, IPropsListener
    {
#if UNITY_EDITOR && RISH_HOT_RELOAD_READY
        // private HotReloader HotReloader { get; set; }
#endif

        private IApp UserApp { get; set; }
        
        void IPropsListener.PropsDidChange()
        {
// #if UNITY_EDITOR && RISH_HOT_RELOAD_READY
//             HotReloader?.Dispose();
//             
//             HotReloader = new HotReloader(Props.assemblyDefinition);
//             HotReloader.OnSuccessfulCompilation += SetAppComponent;
// #else
            var asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(asm => asm.GetType(Props.rootClassName.Value) != null);
            SetApp(asm);
// #endif
        }
        
        void IPropsListener.PropsWillChange()
        {
// #if UNITY_EDITOR && RISH_HOT_RELOAD_READY
//             HotReloader?.Dispose();
//             HotReloader = null;
// #endif
        }

        protected override Element Render() => UserApp?.GetRoot() ?? Element.Null;

        private void SetApp(Assembly assembly)
        {
            var type = assembly.GetType(Props.rootClassName.Value);
            if (type == null || Activator.CreateInstance(type) is not IApp app)
            {
                throw new UnityException("No app found");
            }

            UserApp = app;
            
            Dirty();
        }
    }

    public struct AppProps
    {
        public FixedString64Bytes rootClassName;
    }
}