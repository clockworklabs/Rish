using System;
using System.Collections.Generic;
using RishUI.Deprecated.Input;
using UnityEngine;

namespace RishUI.Deprecated.Components
{
    public class Form : RishComponent<FormProps>, ICustomComponent, IFocusedKeyboardListener
    {
        private List<IFormElement> Elements { get; } = new List<IFormElement>();

        void ICustomComponent.Restart()
        {
            Elements.Clear();
        }

        // TODO: Improve performance
        public void Add(IFormElement element) {
            if (element == null || Elements.Contains(element))
            {
                return;
            }
            
            Elements.Add(element);
        }

        // TODO: Improve performance
        public void Remove(IFormElement element)
        {
            if (element == null || !Elements.Contains(element))
            {
                return;
            }
            
            Elements.Remove(element);
        }

        protected override RishElement Render()
        {
            return Props.content;
        }

        void IFocusedKeyboardListener.OnKeyboardFocus(bool focus) { }
        bool IFocusedKeyboardListener.OnKeyTyped(KeyboardInfo info)
        {
            if (info.keyCode != KeyCode.Return)
            {
                return false;
            }
            
            Submit();
            return true;
        }

        private void Submit() => Props.action?.Invoke();
    }

    public struct FormProps
    {
        public RishElement content;
        public Action action;

        [Comparer]
        public static bool Equals(FormProps a, FormProps b) => RishUtils.Compare<RishElement>(a.content, b.content);
    }
}