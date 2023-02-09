using System.Collections;
using RishUI.Events;
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
        
        private IEnumerator Start()
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

            var wait = new WaitForEndOfFrame();
            while (true)
            {
                yield return wait;

                EndOfFrameEvent.SendEvents();
            }
        }

        private void OnDestroy()
        {
            Tree.Dispose();
            
            Rish.CleanGarbage();
        }

        private void LateUpdate()
        {
            Tree.Update();
            
            Rish.CleanGarbage();
        }

        public bool HasAnyPointerOver() => Tree.HasAnyPointerOver();
        public bool HasAnyPointerDown() => Tree.HasAnyPointerDown();

        public bool HasPointerOver(int pointerId) => Tree.HasPointerOver(pointerId);
        public bool HasPointerDown(int pointerId) => Tree.HasPointerDown(pointerId);

        public bool HasFocus() => Tree.RootVisualElement.panel.focusController.focusedElement != null;
        
#if UNITY_EDITOR
        public int PointerOverCount => Tree.PointerOverCount;
        public int PointerDownCount => Tree.PointerDownCount;
#endif
    }
}