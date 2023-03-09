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

        private UIDocument _document;
        private UIDocument Document
        {
            get
            {
                if (_document == null)
                {
                    _document = GetComponent<UIDocument>();
                }

                return _document;
            }
        }

        private VisualElement Root => Document?.rootVisualElement;
        private IPanel Panel => Root.panel;
        
        private Tree Tree { get; set; }
        
        private IEnumerator Start()
        {
            if (Document == null)
            {
                throw new UnityException("RishRoot requires UIDocument");
            }
            if (Document.panelSettings == null)
            {
                throw new UnityException("RishRoot requires UIDocument to have Panel Settings set");
            }

            foreach (var styleSheet in StyleSheets)
            {
                Root.styleSheets.Add(styleSheet);
            }
            
            Tree = new Tree(Document, RootClassName);

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

        public bool HasAnyPointerOver() => Root.IsHover();
        public bool HasAnyPointerCaptured()
        {
            for (int i = 0, n = PointerId.maxPointers; i < n; i++)
            {
                if (Panel.GetCapturingElement(i) != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasPointerOver(int pointerId) => Root.ContainsPointer(pointerId);
        public bool HasPointerCaptured(int pointerId) => Panel.GetCapturingElement(pointerId) != null;

        public bool HasFocus() => Panel.focusController.focusedElement != null;
    }
}