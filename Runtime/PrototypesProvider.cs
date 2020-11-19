using System;
using System.Collections.Generic;
using UnityEngine;

namespace RishUI
{
    [CreateAssetMenu(fileName = "PrototypesProvider", menuName = "Rish/Prototypes Provider")]
    public class PrototypesProvider : ScriptableObject
    {
        [SerializeField]
        private List<Prototype> prototypes;
        private List<Prototype> Prototypes => prototypes;
        
        public int Count => Prototypes.Count;
        public Prototype this[int index] => Prototypes[index];

        public Prototype Find(Predicate<Prototype> predicate) => Prototypes.Find(predicate);
    }
}