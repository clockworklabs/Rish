using System;
using UnityEngine;

namespace RishUI
{
    [CreateAssetMenu(fileName = "Provider", menuName = "Rish/Provider")]
    public class ElementsProvider : ScriptableObject
    {
        [SerializeField]
        private Style[] styles;
        private Style[] Styles => styles;

        public int StylesCount => Styles.Length;

        public Style GetDefaultStyle() => Styles[0];
        
        public Prototype GetPrototype(Type type, uint styleIndex)
        {
            var style = Styles[styleIndex];

            return style.GetPrototype(type);
        }
    }
}