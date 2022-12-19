using System;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using UnityEngine;
#if UNITY_EDITOR
using RishUI.Elements;
#endif

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
        
#if UNITY_EDITOR
        private bool Ready { get; set; }
#endif
        
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

        protected override Element Render()
        {
            // Without this monstrosity Unity can't compute Text layout and preferred size properly. Thank you, Unity. 
            // Oh, it's just necessary in the Editor because in the builds (at least on Windows), of course it works...
#if UNITY_EDITOR
            if (!Ready)
            {
                Ready = true;
                Dirty();
                return Rish.Create<Label, LabelProps>();
            }
#endif
            
            return UserApp?.GetRoot() ?? Element.Null;
        }
        
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