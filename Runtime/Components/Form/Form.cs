using System;
using System.Collections.Generic;
using RishUI.Input;
using UnityEngine;

namespace RishUI.Components
{
    public class Form : RishComponent<FormProps>, IDestroyListener, IFocusedKeyboardListener
    {
        private List<IFormElement> Elements { get; } = new List<IFormElement>();

        void IDestroyListener.ComponentWillDestroy()
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
        bool IFocusedKeyboardListener.OnKeyDown(KeyboardInfo info)
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

    public struct FormProps : IRishData<FormProps>
    {
        public RishElement content;
        public Action action;
        
        public void Default() { }

        public bool Equals(FormProps other) => content.Equals(other.content);
    }
}