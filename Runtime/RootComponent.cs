using System;
using System.Linq;
using System.Reflection;
using RishUI.Components;
using UnityEditorInternal;
using UnityEngine;

namespace RishUI
{
    internal class RootComponent : RishComponent<RootComponentProps>, IPropsListener
    {
#if UNITY_EDITOR && RISH_HOT_RELOAD_READY
        private HotReloader HotReloader { get; set; }
#endif
        
        private AppComponent App { get; set; }
        internal AssetsManager AssetsManager { get; set; }
        
        void IPropsListener.PropsDidChange()
        {
#if UNITY_EDITOR && RISH_HOT_RELOAD_READY
            HotReloader?.Dispose();
            
            HotReloader = new HotReloader(Props.assemblyDefinition);
            HotReloader.OnSuccessfulCompilation += SetAppComponent;
#else
            var asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(asm => asm.GetType(Props.rootClassname) != null);
            SetAppComponent(asm);
#endif
        }
        
        void IPropsListener.PropsWillChange()
        {
#if UNITY_EDITOR && RISH_HOT_RELOAD_READY
            HotReloader?.Dispose();
            HotReloader = null;
#endif
        }

        protected override RishElement Render()
        {
            if (App == null)
            {
                return RishElement.Null;
            }
            
            return Rish.Create<Div, DivProps>(new DivProps
            {
                children = App.Run()
            });
        }

        private void SetAppComponent(Assembly assembly)
        {
            var type = assembly.GetType(Props.rootClassname);
            if (type == null || Activator.CreateInstance(type) is not AppComponent app)
            {
                throw new UnityException("No app found");
            }

            App = app;
            AssetsManager = new AssetsManager(app);
            
            ForceRender(true);
        }
    }

    internal struct RootComponentProps
    {
        public AssemblyDefinitionAsset assemblyDefinition;
        public string rootClassname;

        [Comparer]
        private bool Equals(RootComponentProps a, RootComponentProps b) => a.assemblyDefinition == b.assemblyDefinition && a.rootClassname == b.rootClassname;
    }
}