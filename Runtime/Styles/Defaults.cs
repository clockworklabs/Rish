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
        
        public void Get<T>(uint style, out T result) where T : struct, IProps<T>
        {
            result = default;
            result.Default();

            if (style > 0)
            {
                Override(0, ref result);
            }
            Override(style, ref result);
        }

        private void Override<T>(uint style, ref T result) where T : struct, IProps<T>
        {
            if (!StyleSheets.TryGetValue(style, out var styleSheet)) return;

            if (!(styleSheet is IStyleSheet<T> tStyleSheet)) return;
            
            tStyleSheet.Get(ref result);
        }
    }
}