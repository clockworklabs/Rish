using System.Collections.Generic;
using RishUI.Styling;
using UnityEngine;

namespace RishUI.Styling
{
    public class RCSS
    {
        private Dictionary<uint, IStyleSheet> StyleSheets { get; } = new Dictionary<uint, IStyleSheet>();

        public bool Import(uint style, IStyleSheet styleSheet)
        {
            if (styleSheet == null)
            {
                return false;
            }
            
            if (style == 0)
            {
                throw new UnityException("The style 0 is reserved for 'no style'");
            }
            
            if(StyleSheets.ContainsKey(style)) return false;
            
            StyleSheets[style] = styleSheet;

            return true;
        }

        internal void Override<T>(uint style, ref T result) where T : struct, IRishData<T>
        {
            if (!StyleSheets.TryGetValue(style, out var styleSheet)) return;

            if (!(styleSheet is IOverride<T> tStyleSheet)) return;
            
            tStyleSheet.Override(ref result);
        }
    }
}