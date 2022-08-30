using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public class RishRoot : MonoBehaviour
    {
        [SerializeField]
        private StyleSheet[] _styleSheets;
        private StyleSheet[] StyleSheets => _styleSheets;
        
        [SerializeField]
        private string _rootClassName;
        private string RootClassName => _rootClassName;
        
        private Dom Dom { get; set; }
        
        private void Start()
        {
            var document = gameObject.GetComponent<UIDocument>();
            if (document == null)
            {
                document = gameObject.AddComponent<UIDocument>();
            }

            foreach (var styleSheet in StyleSheets)
            {
                document.rootVisualElement.styleSheets.Add(styleSheet);
            }
            
            Dom = new Dom(document, RootClassName);
        }

        private void OnDestroy()
        {
            Dom.Dispose();
        }

        private void LateUpdate()
        {
            Dom.Update();
        }
    }
}