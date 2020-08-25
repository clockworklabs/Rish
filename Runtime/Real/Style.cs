using System;
using System.Collections.Generic;
using UnityEngine;

namespace RishUI
{
    [CreateAssetMenu(fileName = "Style", menuName = "Rish/Style")]
    public class Style : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<Prototype> prototypes = new List<Prototype>();
        private List<Prototype> Prototypes => prototypes;

        private Dictionary<Type, int> Dictionary { get; } = new Dictionary<Type, int>();

        public int PrototypesCount => Prototypes.Count;

        public Prototype GetPrototype(int index) => Prototypes[index];

        public Prototype GetPrototype(Type type)
        {
            Dictionary.TryGetValue(type, out var index);
            
            return index <= 0 ? null : Prototypes[index - 1];
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            for (int i = 0, n = PrototypesCount; i < n; i++)
            {
                var prototype = Prototypes[i];
                var element = prototype.Element;
                if (element == null)
                {
                    continue;
                }
                var type = element.GetType();
                Dictionary[type] = i + 1;
            }
        }
    }
}