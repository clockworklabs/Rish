using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    [RequireComponent(typeof(UIDocument))]
    public class RishRoot : MonoBehaviour
    {
        [SerializeField]
        private StyleSheet[] _styleSheets;
        private StyleSheet[] StyleSheets => _styleSheets;
        
        [SerializeField]
        private string _rootClassName;
        private string RootClassName => _rootClassName;
        
        private Tree Tree { get; set; }
        
        private void Start()
        {
            var document = gameObject.GetComponent<UIDocument>();
            if (document == null)
            {
                throw new UnityException("RishRoot requires UIDocument");
            }
            if (document.panelSettings == null)
            {
                throw new UnityException("RishRoot requires UIDocument to have Panel Settings set");
            }

            foreach (var styleSheet in StyleSheets)
            {
                document.rootVisualElement.styleSheets.Add(styleSheet);
            }
            
            Tree = new Tree(document, RootClassName);
        }

        private void OnDestroy()
        {
            Tree.Dispose();
        }

        private void LateUpdate()
        {
            Tree.Update();
        }
    }
}