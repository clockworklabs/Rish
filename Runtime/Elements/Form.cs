using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Elements
{
    public class Form : RishBaseElement<FormProps>, IMountingListener
    {
        private Form ParentForm { get; set; }
        private uint Index { get; set; }
        
        private uint ElementsCount { get; set; }
        private uint FormsCount { get; set; }
        
        public Form()
        {
            RegisterCallback<KeyDownEvent>(OnKeyDown);
        }

        void IMountingListener.ComponentDidMount()
        {
            ParentForm = GetFirstAncestorOfType<Form>();

            Index = ParentForm?.RegisterForm() ?? 0;
        }

        void IMountingListener.ComponentWillUnmount()
        {
            ParentForm?.UnregisterForm();
            
            Index = 0;
            ElementsCount = 0;
            FormsCount = 0;
        }

        protected override Element Render()
        {
            return Props.content;
        }

        private void OnKeyDown(KeyDownEvent evt)
        {
            if (evt.keyCode != KeyCode.Return)
            {
                return;
            }
            
            evt.StopPropagation();
            
            Submit();
        }

        internal void Submit() => Props.submitAction?.Invoke();

        internal uint RegisterElement() => Index + ++ElementsCount;
        internal void UnregisterElement() => ElementsCount--;

        private uint RegisterForm()
        {
            if (ParentForm != null)
            {
                return ParentForm.RegisterForm();
            }

            return ++FormsCount * 10000;
        }
        private void UnregisterForm()
        {
            if (ParentForm != null)
            {
                ParentForm.UnregisterForm();
                return;
            }

            FormsCount--;
        }
    }

    public struct FormProps
    {
        public Element content;
        public Action submitAction;

        [Comparer]
        private static bool Equals(FormProps a, FormProps b) => RishUtils.Compare<Element>(a.content, b.content);

        [ReferencesGetter]
        private static References GetReferences(FormProps owner) => owner.content;
    }
}