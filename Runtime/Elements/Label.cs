using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Elements
{
    public partial class Label : TextElement, IVisualElement<LabelProps>
    {
        private RishBridge<LabelProps> RishBridge { get; }
        RishBridge<LabelProps> IVisualElement<LabelProps>.Bridge => RishBridge;
        
        VisualElement IElement.GetDOMChild() => this;
        
        private PickingManager PickingManager { get; }
        PickingManager ICustomPicking.Manager => PickingManager;

        private LengthRange? WidthRange { get; set; }
        private LengthRange? HeightRange { get; set; }
        
        private VisualElement Parent { get; set; }

        public Label()
        {
            RishBridge = new RishBridge<LabelProps>(this);
            PickingManager = new RectPickingManager(this);
            
            RegisterCallback<AttachToPanelEvent>(OnMounted);
            RegisterCallback<DetachFromPanelEvent>(OnUnmounted);
            
            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        void IVisualElement<LabelProps>.Setup(LabelProps props)
        {
            WidthRange = props.widthRange;
            HeightRange = props.heightRange;
            text = props.text;

            parseEscapeSequences = props.parseEscapeSequences ?? true;
            enableRichText = props.enableRichText ?? true;
        }

        private void OnMounted(AttachToPanelEvent evt)
        {
            Parent = parent;
            Parent?.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        private void OnUnmounted(DetachFromPanelEvent evt)
        {
            Parent?.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            Parent = null;
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            SetSize();
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
    
    public struct LabelProps
    {
        public RishString text;
        public LengthRange widthRange;
        public LengthRange heightRange;

        public bool? enableRichText;
        public bool? parseEscapeSequences;
    }
}