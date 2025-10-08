using System;
using RishUI.Events;
using Sappy;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Elements
{
    public partial class Label : TextElement, IVisualElement<LabelProps>
    {
        private Bridge<LabelProps> Bridge { get; }
        Bridge<LabelProps> IVisualElement<LabelProps>.Bridge => Bridge;
        
        VisualElement IElement.GetDOMChild() => this;
        
        private PickingManager PickingManager { get; }
        PickingManager ICustomPicking.Manager => PickingManager;

        private LengthRange? WidthRange { get; set; }
        private LengthRange? HeightRange { get; set; }
        
        private VisualElement Parent { get; set; }
        
        private Action<bool> OnElidedCallback { get; set; }

        public Label()
        {
            Bridge = new Bridge<LabelProps>(this);
            PickingManager = new RectPickingManager(Bridge);
            
            RegisterCallback<VisualChangeEvent>(SappyOnVisualChange.Callback);
            
            Bridge.OnMounted.Add(SappyOnMounted);
            Bridge.OnUnmounted.Add(SappyOnUnmounted);
            
            generateVisualContent += OnGenerateVisualContent;
        }

        void IVisualElement<LabelProps>.Setup(LabelProps props)
        {
            OnElidedCallback = props.onElided;
            
            WidthRange = props.widthRange;
            HeightRange = props.heightRange;
            text = props.text;

            parseEscapeSequences = props.parseEscapeSequences ?? true;
            enableRichText = props.enableRichText ?? true;
        }

        [SapTarget]
        private void OnMounted()
        {
            Parent = parent;
            Parent?.RegisterCallback<VisualChangeEvent>(SappyOnVisualChange.Callback);
        }

        [SapTarget]
        private void OnUnmounted()
        {
            Parent?.UnregisterCallback<VisualChangeEvent>(SappyOnVisualChange.Callback);
            Parent = null;
            OnElidedCallback = null;
        }

        [SapTarget(typeof(EventCallback<VisualChangeEvent>))]
        private void OnVisualChange(VisualChangeEvent _) => OnChange();
        
        private void OnGenerateVisualContent(MeshGenerationContext _) => OnChange();

        private void OnChange()
        {
            SetSize();

            OnElidedCallback?.Invoke(isElided);
        }

        private void SetSize()
        {
            if (!WidthRange.HasValue && !HeightRange.HasValue)
            {
                return;
            }
            
            var current = layout.size;

            var parentSize = parent.layout.size;

            var (minWidth, maxWidth) = WidthRange?.ToSize(parentSize.x) ?? (0, parentSize.x);
            var (minHeight, maxHeight) = HeightRange?.ToSize(parentSize.y) ?? (0, parentSize.y);
            
            var preferredSize = DoMeasure(maxWidth, !WidthRange.HasValue ? MeasureMode.Undefined : Mathf.Approximately(minWidth, maxWidth) ? MeasureMode.Exactly : MeasureMode.AtMost, maxHeight, !HeightRange.HasValue ? MeasureMode.Undefined : Mathf.Approximately(minHeight, maxHeight) ? MeasureMode.Exactly : MeasureMode.AtMost);
            if (preferredSize.x < minWidth)
            {
                preferredSize.x = minWidth;
            }
            if (preferredSize.y < minHeight)
            {
                preferredSize.y = minHeight;
            }

            if (!Mathf.Approximately(current.x, preferredSize.x))
            {
                style.width = preferredSize.x;
                // style.maxWidth = preferredSize.x;
            }
            if (!Mathf.Approximately(current.y, preferredSize.y))
            {
                style.height = preferredSize.y;
                // style.maxHeight = preferredSize.y;
            }
        }
        
        public override bool ContainsPoint(Vector2 localPoint) => PickingManager.ContainsPoint(localPoint);
    }

    [RishValueType]
    public struct LabelProps
    {
        public RishString text;
        public LengthRange? widthRange;
        public LengthRange? heightRange;

        public bool? enableRichText;
        public bool? parseEscapeSequences;

        [IgnoreComparison]
        public Action<bool> onElided;
    }
}