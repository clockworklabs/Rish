using RishUI.Deprecated.Components;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace RishUI.Deprecated.UnityComponents
{
    public struct UnityImageDefinition
    {
        public enum Type { Simple, Sliced, Tiled, Filled }
        
        public bool enabled;
        public Sprite sprite;
        public Color color;
        public bool raycastTarget;
        public bool maskable;
        public Type type;
        public ImageFill fill;
        public bool preserveAspectRatio;

        public void SetComponent(Image imageComponent)
        {
            if (enabled)
            {
                imageComponent.enabled = true;
                
                imageComponent.sprite = sprite;
                imageComponent.color = color;
                imageComponent.raycastTarget = raycastTarget;
                imageComponent.maskable = maskable;
                
                Image.Type unityType;
                switch (type)
                {
                    case Type.Simple:
                        unityType = Image.Type.Simple;
                        break;
                    case Type.Sliced:
                        unityType = Image.Type.Sliced;
                        break;
                    case Type.Tiled:
                        unityType = Image.Type.Tiled;
                        break;
                    case Type.Filled:
                        var valid = false;
                        if (fill.linear.HasValue)
                        {
                            var linear = fill.linear.Value;
                            switch (linear.origin)
                            {
                                case LinearFill.Origin.Top:
                                    imageComponent.fillMethod = Image.FillMethod.Vertical;
                                    imageComponent.fillOrigin = 1;
                                    break;
                                case LinearFill.Origin.Right:
                                    imageComponent.fillMethod = Image.FillMethod.Horizontal;
                                    imageComponent.fillOrigin = 1;
                                    break;
                                case LinearFill.Origin.Bottom:
                                    imageComponent.fillMethod = Image.FillMethod.Vertical;
                                    imageComponent.fillOrigin = 0;
                                    break;
                                case LinearFill.Origin.Left:
                                    imageComponent.fillMethod = Image.FillMethod.Horizontal;
                                    imageComponent.fillOrigin = 0;
                                    break;
                                default:
                                    throw new UnityException("Origin type not supported");
                            }
                            valid = true;
                        } else if (fill.radial90.HasValue)
                        {
                            var radial90 = fill.radial90.Value;

                            imageComponent.fillMethod = Image.FillMethod.Radial90;
                            switch (radial90.origin)
                            {
                                case Radial90Fill.Origin.TopRight:
                                    imageComponent.fillOrigin = 2;
                                    break;
                                case Radial90Fill.Origin.BottomRight:
                                    imageComponent.fillOrigin = 3;
                                    break;
                                case Radial90Fill.Origin.BottomLeft:
                                    imageComponent.fillOrigin = 0;
                                    break;
                                case Radial90Fill.Origin.TopLeft:
                                    imageComponent.fillOrigin = 1;
                                    break;
                                default:
                                    throw new UnityException("Origin type not supported");
                            }
                            imageComponent.fillClockwise = radial90.clockwise;
                            
                            valid = true;
                        } else if (fill.radial180.HasValue)
                        {
                            var radial180 = fill.radial180.Value;

                            imageComponent.fillMethod = Image.FillMethod.Radial180;
                            switch (radial180.origin)
                            {
                                case Radial180Fill.Origin.Top:
                                    imageComponent.fillOrigin = 2;
                                    break;
                                case Radial180Fill.Origin.Right:
                                    imageComponent.fillOrigin = 3;
                                    break;
                                case Radial180Fill.Origin.Bottom:
                                    imageComponent.fillOrigin = 0;
                                    break;
                                case Radial180Fill.Origin.Left:
                                    imageComponent.fillOrigin = 1;
                                    break;
                                default:
                                    throw new UnityException("Origin type not supported");
                            }
                            imageComponent.fillClockwise = radial180.clockwise;
                            
                            valid = true;
                        } else if (fill.radial360.HasValue)
                        {
                            var radial360 = fill.radial360.Value;

                            imageComponent.fillMethod = Image.FillMethod.Radial360;
                            switch (radial360.origin)
                            {
                                case Radial360Fill.Origin.Top:
                                    imageComponent.fillOrigin = 2;
                                    break;
                                case Radial360Fill.Origin.Right:
                                    imageComponent.fillOrigin = 1;
                                    break;
                                case Radial360Fill.Origin.Bottom:
                                    imageComponent.fillOrigin = 0;
                                    break;
                                case Radial360Fill.Origin.Left:
                                    imageComponent.fillOrigin = 3;
                                    break;
                                default:
                                    throw new UnityException("Origin type not supported");
                            }
                            imageComponent.fillClockwise = radial360.clockwise;
                            
                            valid = true;
                        }

                        imageComponent.fillAmount = fill.amount;
                        
                        unityType = valid ? Image.Type.Filled : Image.Type.Simple;
                        break;
                    default:
                        throw new UnityException("Image type not supported");
                }
                imageComponent.type = unityType;
                imageComponent.preserveAspect = preserveAspectRatio;
            }
            else
            {
                imageComponent.enabled = false;
            }
        }
    }
}
