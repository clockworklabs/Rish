using System;
using System.Collections.Generic;

namespace RishUI.Components
{
    public class Form : RishComponent<FormProps>
    {
        private Dictionary<string, Func<object>> Fields { get; } = new Dictionary<string, Func<object>>();
        
        public void Add(string name, Func<object> getter)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }
            Fields[name] = getter;
        }
        
        public void Remove(string name) => Fields.Remove(name);

        protected override RishElement Render()
        {
            return Props.content;
        }
    }

    public struct FormProps : IRishData<FormProps>
    {
        public RishElement content;
        
        public void Default() { }

        public bool Equals(FormProps other) => content.Equals(other.content);
    }
}