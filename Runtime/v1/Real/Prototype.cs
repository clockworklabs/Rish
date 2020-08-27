using System;
using UnityEngine;

namespace RishUI
{
    [Serializable]
    public class Prototype
    {
        [SerializeField]
        private UnityComponent component;
        public UnityComponent Component => component;
        
        [SerializeField]
        private int initialCount = 1;
        public int InitialCount => Mathf.Max(1, initialCount);
    }
}