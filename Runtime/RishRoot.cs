using System;
using System.Collections;
using System.Linq;
using RishUI.Events;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public delegate void RishStart(RishRoot root);
    
    [RequireComponent(typeof(UIDocument))]
    public class RishRoot : MonoBehaviour
    {
        public static event RishStart OnStart;
        
        [SerializeField]
        private StyleSheet[] _styleSheets;
        private StyleSheet[] StyleSheets => _styleSheets;

        #if UNITY_EDITOR
        [SerializeField]
        private bool _debugRender;
        private bool DebugRender => _debugRender;
        
        [SerializeField]
        private string _rootGUID;
        #endif
        [SerializeField]
        private string _rootClassName;
        private string RootClassName => _rootClassName;

        public Type RootType
        {
            get
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var type = assembly.GetType(RootClassName);
                    if (type != null)
                    {
                        return type;
                    }
                }

                return null;
            }
        }
        
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
                #if UNITY_EDITOR
                Tree.Update(DebugRender);
                #else
                Tree.Update();
                #endif
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

        public void AddStyleSheet(StyleSheet styleSheet)
        {
            if (styleSheet == null)
            {
                return;
            }

            Root?.styleSheets.Add(styleSheet);
        }
        public void RemoveStyleSheet(StyleSheet styleSheet)
        {
            if (styleSheet == null)
            {
                return;
            }

            Root?.styleSheets.Remove(styleSheet);
        }

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