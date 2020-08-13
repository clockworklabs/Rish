using System;
using UnityEngine;

namespace Rish
{
    [CreateAssetMenu(fileName = "Provider", menuName = "Rish/Provider")]
    public class ElementsProvider : ScriptableObject
    {
        [SerializeField]
        private Style[] styles;
        private Style[] Styles => styles;

        public int StylesCount => Styles.Length;

        public Style GetDefaultStyle() => GetStyle(0);
        
        public Style GetStyle(int index) => Styles[index];

        public T GetDefaultPrototype<T>() where T : DOMElement => GetPrototype<T>(0);

        public T GetPrototype<T>(uint styleIndex) where T : DOMElement => GetPrototype(typeof(T), styleIndex) as T;
        
        public DOMElement GetPrototype(Type type, uint styleIndex)
        {
            var style = Styles[styleIndex];

            return style.GetPrototype(type);
        }
    }
}