using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using RishUI.Events;
using Sappy;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    [RequireComponent(typeof(UIDocument))]
    public class RishRoot : MonoBehaviour
    {
        private SapStem OnStartStem { get; } = new();
        public SapTargets<Action> OnStart => OnStartStem.Targets;
        
#if UNITY_EDITOR
        private SapStem<RishRoot> OnStepStem { get; } = new();
        public event Action<RishRoot> OnStep { add => OnStepStem.Targets.AddLessPerformant(value); remove => OnStepStem.Targets.RemoveLessPerformant(value); }
#endif

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
        private bool _chainRender;
        public bool ChainRender
        {
            get=> _chainRender;
            set => _chainRender = value;
        }

        [SerializeField]
        private uint _maxUpdatesPerStep;
        public uint MaxUpdatesPerStep {
            get => _maxUpdatesPerStep;
            set => _maxUpdatesPerStep = value;
        }

        [SerializeField]
        private float _maxTargetTimePerStep;
        public float MaxTargetTimePerStep {
            get => _maxTargetTimePerStep;
            set => _maxTargetTimePerStep = value;
        }

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
        
#if UNITY_EDITOR
        public int TreeSize => Tree?.Size ?? 0;
#endif
        
        private Stopwatch Stopwatch { get; set; }
        private const int RecordedStepsCount = 10;
        private Queue<float> RecordedExtraTimes { get; } = new(RecordedStepsCount);
        private float TotalRecordedExtraTime { get; set; }

        private IEnumerator Start()
        {
#if UNITY_EDITOR
            if (Recovered)
            {
                UnityEngine.Debug.LogError("Recovering UI");
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
            // TODO: Clean garbage
        }

        public void RegenTree()
        {
            Dispose();

            Tree = new Tree(Document, RootClassName, Recovered);
            OnStartStem.Send();
        }

        public void ForceRender(int? nodeId = null) => Tree.ForceRender(nodeId);

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
                updateTime = Tree.Update(ChainRender, MaxUpdatesPerStep, maxUpdateTime, DebugRender);
#else
                updateTime = Tree.Update(ChainRender, MaxUpdatesPerStep, maxUpdateTime);
#endif
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogException(e);
#endif
                updateTime = null;
                if (timeLimited)
                {
                    Stopwatch.Stop();
                }

                Recovered = true;
                
                RegenTree();
            }
            
#if UNITY_EDITOR
            OnStepStem.Send(this);
#endif

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