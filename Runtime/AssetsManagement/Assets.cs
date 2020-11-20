using System.Collections.Generic;
using RishUI.Styling;
using UnityEngine;

namespace RishUI.AssetsManagement
{
    public class Assets : MonoBehaviour
    {
        private Dictionary<string, object> Objects { get; } = new Dictionary<string, object>();
        private Dictionary<uint, IStyleSheet> StyleSheets { get; } = new Dictionary<uint, IStyleSheet>();

        public void RemoveAll()
        {
            RemoveAllAssets();
            RemoveAllStyleSheets();
        }
        
        // Assets
        
        public T Get<T>(string address)
        {
            if (!Objects.TryGetValue(address, out var asset) || !(asset is T tAsset)) return default;

            return tAsset;
        }

        public void Import(params (string, object)[] assets)
        {
            if (assets == null) return;
            
            foreach (var (style, styleSheet) in assets)
            {
                Import(style, styleSheet);
            }
        }

        public bool Import(string address, object asset)
        {
            if(Objects.ContainsKey(address)) return false;
            
            Objects[address] = asset;

            return true;
        }

        public void Remove(string address) => Objects.Remove(address);

        public void RemoveAllAssets() => Objects.Clear();
        
        // Styling
        
        public void Get<T>(uint style, out T result) where T : struct, IRishData<T>
        {
            result = default;
            result.Default();

            if (style > 0)
            {
                Override(0, ref result);
            }
            Override(style, ref result);
        }

        private void Override<T>(uint style, ref T result) where T : struct, IRishData<T>
        {
            if (!StyleSheets.TryGetValue(style, out var styleSheet)) return;

            if (!(styleSheet is IOverride<T> tStyleSheet)) return;
            
            tStyleSheet.Get(ref result);
        }

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
            if(StyleSheets.ContainsKey(style)) return false;
            
            StyleSheets[style] = styleSheet;

            return true;
        }

        public void Remove(uint style) => StyleSheets.Remove(style);

        public void RemoveAllStyleSheets() => StyleSheets.Clear();
    }
}