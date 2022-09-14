using System;
using UnityEngine;

namespace RishUI.Deprecated
{
    [Serializable]
    public class Prototype
    {
        [SerializeField]
        private UnityComponent component;
        public UnityComponent Component => component;
        
        [SerializeField]
        private int initialCount;
        public int InitialCount => Mathf.Max(1, initialCount);
    }
}