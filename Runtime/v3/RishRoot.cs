using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.v3
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

        private Node CurrentNode { get; set; }
        private int CurrentDepth => CurrentNode?.Depth ?? -1;
        
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

        private void LateUpdate()
        {
            // Input?.OnLateUpdate();
                
            Dom.Update();
        }
    }
}