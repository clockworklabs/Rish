using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using RishUI.Events;
using Sappy;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

namespace RishUI
{
    [RequireComponent(typeof(UIDocument))]
    public class RishRoot : MonoBehaviour
    {
        private Phloem OnStartHandler { get; } = new();
        public event Action OnStart { add => OnStartHandler.AddTarget(value); remove => OnStartHandler.RemoveTarget(value); }

        [SerializeField]
        private bool _manualUpdate;
        public bool ManualUpdate
        {
            get => _manualUpdate;
            set
            {
                if (_manualUpdate == value) return;
                _manualUpdate = value;
                enabled = !value;
            }
        }

        [SerializeField]
        private uint _maxUpdatesPerStep;
        private uint MaxUpdatesPerStep => _maxUpdatesPerStep;

        [SerializeField]
        private float _maxTargetTimePerStep;
        private float MaxTargetTimePerStep => _maxTargetTimePerStep;

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

        private Stopwatch Stopwatch { get; set; }
        private const int RecordedStepsCount = 10;
        private Queue<float> RecordedExtraTimes { get; } = new(RecordedStepsCount);
        private float TotalRecordedExtraTime { get; set; }

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

            RegenTree();

            var wait = new WaitForEndOfFrame();
            while (true)
            {
                yield return wait;

                EndOfFrameEvent.SendEvents();
            }
        }

        private void OnDestroy() => Dispose();

        private void LateUpdate()
        {
            if (ManualUpdate)
            {
                enabled = false;
                return;
            }

            Step();
        }

        private void Dispose()
        {
            if (Tree == null) return;
            Tree.Dispose();
            Rish.CleanGarbage();
        }

        public void RegenTree()
        {
            Dispose();

            Tree = new Tree(Document, RootClassName, Recovered);
            OnStartHandler?.Send();
        }

        public void Step()
        {
            var timeLimited = MaxTargetTimePerStep > 0;
            
            if(timeLimited)
            {
                if (Stopwatch == null)
                {
                    Stopwatch = Stopwatch.StartNew();
                }
                else
                {
                    Stopwatch.Restart();
                }
            }

            float? maxUpdateTime;
            if (timeLimited)
            {
                var averageExtraTime = TotalRecordedExtraTime / RecordedExtraTimes.Count;
                maxUpdateTime = MaxTargetTimePerStep - averageExtraTime;
            }
            else
            {
                maxUpdateTime = null;
            }

            double? updateTime;
            try
            {
#if UNITY_EDITOR
                updateTime = Tree.Update(MaxUpdatesPerStep, maxUpdateTime, DebugRender);
#else
                updateTime = Tree.Update(MaxUpdatesPerStep, maxUpdateTime);
#endif
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogException(e);
#endif
                updateTime = null;
                if (timeLimited)
                {
                    Stopwatch.Stop();
                }

                Recovered = true;
                
                RegenTree();
            }
            
            Rish.CleanGarbage();

            if (!timeLimited || !updateTime.HasValue) return;

            Stopwatch.Stop();
            
            var stepTime = Stopwatch.Elapsed.TotalSeconds;
            var extraTime = (float)(stepTime - updateTime.Value);

            if (RecordedExtraTimes.Count >= RecordedStepsCount)
            {
                TotalRecordedExtraTime -= RecordedExtraTimes.Dequeue();
            }
            TotalRecordedExtraTime += extraTime;
            RecordedExtraTimes.Enqueue(extraTime);
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

        public bool HasAnyPointerOver(bool skipRoot = true)
        {
            if (Root == null) return false;
            if(!skipRoot && Root.IsHover()) return true;
            for (int i = 0, n = Root.childCount; i < n; i++)
            {
                if (Root[i].IsHover()) return true;
            }
            return false;
        }
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

        public bool HasPointerOver(int pointerId, bool skipRoot = true)
        {
            if (Root == null) return false;
            if(!skipRoot && Root.ContainsPointer(pointerId)) return true;
            for (int i = 0, n = Root.childCount; i < n; i++)
            {
                if (Root[i].ContainsPointer(pointerId)) return true;
            }
            return false;
        }
        public bool HasPointerCaptured(int pointerId) => Panel?.GetCapturingElement(pointerId) != null;

        public bool HasFocus() => Panel?.focusController.focusedElement != null;
    }
}