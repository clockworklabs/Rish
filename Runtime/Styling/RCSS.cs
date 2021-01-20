using System.Collections.Generic;
using RishUI.Styling;
using UnityEngine;

namespace RishUI.Styling
{
    public class RCSS
    {
        private Dictionary<uint, IStyleSheet> StyleSheets { get; } = new Dictionary<uint, IStyleSheet>();

        public void Import(params (uint, IStyleSheet)[] styleSheets)
        {
            if (styleSheets == null) return;
            
            foreach (var (style, styleSheet) in styleSheets)
            {
                Import(style, styleSheet);
            }
        }

        public bool Import(uint style, IStyleSheet styleSheet)
        {
            if (style == 0)
            {
                throw new UnityException("The style 0 is reserved for 'no style'");
            }

            if (styleSheet == null)
            {
                return false;
            }
            
            if(StyleSheets.ContainsKey(style)) return false;
            
            StyleSheets[style] = styleSheet;

            return true;
        }

        public void Override<T>(uint style, ref T result) where T : struct, IRishData<T>
        {
            if (!StyleSheets.TryGetValue(style, out var styleSheet)) return;

            if (!(styleSheet is IOverride<T> tStyleSheet)) return;
            
            tStyleSheet.Override(ref result);
        }
    }
}