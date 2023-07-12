using System;
using System.Collections;
using RishUI.Events;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public delegate void RishStart(RishRoot root);
    public delegate void ResizeEvent(Vector2 oldSize, Vector2 newSize);
    
    [RequireComponent(typeof(UIDocument))]
    public class RishRoot : MonoBehaviour
    {
        public static event RishStart OnStart;
        public  event ResizeEvent OnResize;
        
        [SerializeField]
        private StyleSheet[] _styleSheets;
        private StyleSheet[] StyleSheets => _styleSheets;
        
        [SerializeField]
        private string _rootClassName;
        private string RootClassName => _rootClassName;
        
        private bool Recovered { get; set; }

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

        private VisualElement Root => Document != null ? Document.rootVisualElement : null;
        private IPanel Panel => Root?.panel;
        
        private Tree Tree { get; set; }
        
        private IEnumerator Start()
        {
#if UNITY_EDITOR
            if (Recovered)
            {
                Debug.LogError("Recovering UI");
            }
#endif
            
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
                AddStyleSheet(styleSheet);
            }
            
            Tree = new Tree(Document, RootClassName, Recovered);
            Root?.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            
            OnStart?.Invoke(this);

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
            try
            {
                Tree.Update();
                Rish.CleanGarbage();
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogException(e);
#endif
                var newRoot = gameObject.AddComponent<RishRoot>();
                newRoot._styleSheets = _styleSheets;
                newRoot._rootClassName = _rootClassName;
                newRoot.Recovered = true;
                
                Destroy(this);
            }
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            var oldSize = evt.oldRect.size;
            var newSize = evt.newRect.size;

            if (Mathf.Approximately(oldSize.x, newSize.x) && Mathf.Approximately(oldSize.y, newSize.y))
            {
                return;
            }
            
            OnResize?.Invoke(oldSize, newSize);
        }

        public void AddStyleSheet(StyleSheet styleSheet) => Root?.styleSheets.Add(styleSheet);
        public void RemoveStyleSheet(StyleSheet styleSheet) => Root?.styleSheets.Remove(styleSheet);

        public bool HasAnyPointerOver() => Root?.IsHover() ?? false;
        public bool HasAnyPointerCaptured()
        {
            if (Panel == null)
            {
                return false;
            }
            
            for (int i = 0, n = PointerId.maxPointers; i < n; i++)
            {
                if (Panel.GetCapturingElement(i) != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasPointerOver(int pointerId) => Root?.ContainsPointer(pointerId) ?? false;
        public bool HasPointerCaptured(int pointerId) => Panel?.GetCapturingElement(pointerId) != null;

        public bool HasFocus() => Panel?.focusController.focusedElement != null;
    }
}