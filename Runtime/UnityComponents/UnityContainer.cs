using UnityEngine;
using UnityEngine.UI;

namespace RishUI.UnityComponents
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Mask))]
    [RequireComponent(typeof(RectMask2D))]
    public class UnityContainer : UnityComponent<UnityContainerProps>
    {
        [SerializeField] 
        private Image _image;
        private Image Image => _image;
        [SerializeField] 
        private Mask _mask;
        private Mask Mask => _mask;
        [SerializeField] 
        private RectMask2D _rectMask;
        private RectMask2D RectMask => _rectMask;

        public override void Render()
        {
            var image = Props.image;
            if (image.enabled)
            {
                Image.enabled = true;
                
                Image.sprite = Props.image.sprite;
                Image.color = image.color;
                Image.raycastTarget = image.raycastTarget;
                Image.maskable = image.maskable;
                
                Image.Type type;
                switch (image.type)
                {
                    case ImageDef.Type.Simple:
                        type = Image.Type.Simple;
                        break;
                    case ImageDef.Type.Sliced:
                        type = Image.Type.Sliced;
                        break;
                    case ImageDef.Type.Tiled:
                        type = Image.Type.Tiled;
                        break;
                    default:
                        throw new UnityException("Image type not supported");
                }
                Image.type = type;
                Image.preserveAspect = image.preserveAspectRatio;
            }
            else
            {
                Image.enabled = false;
            }

            var mask = Props.mask;
            if (mask.enabled)
            {
                Mask.enabled = mask.type == MaskDef.Type.Graphic || mask.type == MaskDef.Type.Both;
                Mask.showMaskGraphic = mask.showGraphic;
                
                RectMask.enabled = mask.type == MaskDef.Type.Rect || mask.type == MaskDef.Type.Both;
                RectMask.softness = mask.rectMaskSoftness;
            }
            else
            {
                Mask.enabled = false;
                RectMask.enabled = false;
            }
        }
    }
    
    public struct ImageDef
    {
        public enum Type { Simple, Sliced, Tiled }
        
        public bool enabled;
        public Sprite sprite;
        public Color color;
        public bool raycastTarget;
        public bool maskable;
        public Type type;
        public bool preserveAspectRatio;
        /*
        public UnityImage.FillMethod fillMethod;
        public int fillOrigin;
        public float fillAmount;
        */
    }
    
    public struct MaskDef {
        public enum Type { Graphic, Rect, Both }

        public bool enabled;
        public Type type;
        public bool showGraphic;
        public Vector2Int rectMaskSoftness;
    }
    
    public struct UnityContainerProps
    {
        public ImageDef image;
        public MaskDef mask;
    }
}

