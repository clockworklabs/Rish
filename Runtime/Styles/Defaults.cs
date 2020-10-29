using System;
using System.Collections.Generic;

namespace RishUI.RDS
{
    public class Defaults
    {
        private Dictionary<uint, IStyleSheet> StyleSheets { get; } = new Dictionary<uint, IStyleSheet>();

        public Defaults(params (uint, IStyleSheet)[] styleSheets)
        {
            if (styleSheets == null) return;
            
            foreach (var (style, styleSheet) in styleSheets)
            {
                if(StyleSheets.ContainsKey(style)) continue;

                StyleSheets[style] = styleSheet;
            }
        }
        
        public void Get<T>(uint style, out T result) where T : struct, IEquatable<T>
        {
            result = default;

            if (StyleSheets.TryGetValue(style, out var stylesheet))
            {
                stylesheet.Get(ref result);
            }
        }
    }
}