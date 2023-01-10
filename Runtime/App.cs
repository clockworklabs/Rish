using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RishUI.Events;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Label = RishUI.Elements.Label;
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
    internal class App : RishElement<AppProps>, IPropsListener
    {
#if UNITY_EDITOR && RISH_HOT_RELOAD_READY
        // private HotReloader HotReloader { get; set; }
#endif

        private Dictionary<int, int> HoveredPointers { get; } = new();
        private Dictionary<int, int> PressedPointers { get; } = new();

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

        internal void OnPointerEnter(int pointerId)
        {
            if (HoveredPointers.TryGetValue(pointerId, out var count))
            {
                count += 1;
            }
            else
            {
                count = 1;
            }

            HoveredPointers[pointerId] = count;
        }

        internal void OnPointerExit(int pointerId)
        {
            if (!HoveredPointers.TryGetValue(pointerId, out var count)) return;
            count -= 1;
            if (count > 0)
            {
                HoveredPointers[pointerId] = count;
            }
            else
            {
                HoveredPointers.Remove(pointerId);
            }
        }

        internal void OnPointerDown(int pointerId)
        {
            if (PressedPointers.TryGetValue(pointerId, out var count))
            {
                count += 1;
            }
            else
            {
                count = 1;
            }

            PressedPointers[pointerId] = count;
        }

        internal void OnPointerUp(int pointerId)
        {
            if (!PressedPointers.TryGetValue(pointerId, out var count)) return;
            count -= 1;
            if (count > 0)
            {
                PressedPointers[pointerId] = count;
            }
            else
            {
                PressedPointers.Remove(pointerId);
            }
        }

        internal bool HasAnyPointerOver() => HoveredPointers.Count > 0;
        internal bool HasAnyPointerDown() => PressedPointers.Count > 0;

        internal bool HasPointerOver(int pointerId) => HoveredPointers.ContainsKey(pointerId);
        internal bool HasPointerDown(int pointerId) => PressedPointers.ContainsKey(pointerId);
        
#if UNITY_EDITOR
        internal int PointerOverCount => HoveredPointers.Count;
        internal int PointerDownCount => PressedPointers.Count;
#endif
    }

    public struct AppProps
    {
        public FixedString64Bytes rootClassName;
    }
}