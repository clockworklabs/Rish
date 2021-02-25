using System;

namespace RishUI.Components
{
    public class Form : RishComponent<FormProps>
    {
        protected override RishElement Render()
        {
            return RishElement.Null;
        }
    }

    public struct FormProps : IRishData<FormProps>
    {
        public Action onSubmit;
        
        public void Default() { }

        public bool Equals(FormProps other)
        {
            if (onSubmit != null || other.onSubmit != null)
            {
                return false;
            }
            
            return true;
        }
    }
}