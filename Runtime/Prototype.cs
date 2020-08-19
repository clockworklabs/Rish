using System;
using UnityEngine;

namespace RishUI
{
    [Serializable]
    public class Prototype
    {
        [SerializeField]
        private DOMElement element;
        public DOMElement Element => element;
        
        [SerializeField]
        private int initialCount;
        public int InitialCount => Mathf.Max(1, initialCount);
    }
}