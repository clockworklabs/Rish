using System;
using System.Collections.Generic;
using UnityEngine;

namespace RishUI
{
    [CreateAssetMenu(fileName = "Style", menuName = "Rish/Style")]
    public class Style : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        private DOMElement[] prototypes;
        private DOMElement[] Prototypes => prototypes;

        private Dictionary<Type, DOMElement> Dictionary { get; } = new Dictionary<Type, DOMElement>();

        public int PrototypesCount => Prototypes.Length;

        public DOMElement GetPrototype(int index) => Prototypes[index];

        public T GetPrototype<T>() where T : DOMElement => GetPrototype(typeof(T)) as T;
        
        public DOMElement GetPrototype(Type type)
        {
            Dictionary.TryGetValue(type, out var prototype);

            return prototype;
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            foreach (var prototype in Prototypes)
            {
                var type = prototype.GetType();
                Dictionary[type] = prototype;
            }
        }
    }
}