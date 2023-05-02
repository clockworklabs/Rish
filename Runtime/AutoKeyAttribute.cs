using System;
using UnityEngine.Scripting;

namespace RishUI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AutoKeyAttribute : PreserveAttribute { 
        public readonly ulong offset;

        public AutoKeyAttribute() : this(1) { }
        
        public AutoKeyAttribute(ulong offset)
        {
            this.offset = offset;
        }
    }
}