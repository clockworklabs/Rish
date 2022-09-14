using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Cursor = UnityEngine.UIElements.Cursor;
using Background = UnityEngine.UIElements.Background;
using VectorImage = UnityEngine.UIElements.VectorImage;
using Length = UnityEngine.UIElements.Length;
using Rotate = UnityEngine.UIElements.Rotate;
using Angle = UnityEngine.UIElements.Angle;
using Scale = UnityEngine.UIElements.Scale;
using TextShadow = UnityEngine.UIElements.TextShadow;
using TransformOrigin = UnityEngine.UIElements.TransformOrigin;
using FontDefinition = UnityEngine.UIElements.FontDefinition;
using Translate = UnityEngine.UIElements.Translate;

namespace RishUI
{
    public enum RishStyleKeyword
    {
        Null,
        Undefined,
        Auto,
        None,
        Initial
    }
    
    public struct StyleEnum<T> where T : struct, IConvertible
    {
        public RishStyleKeyword keyword;
        public T value;

        public StyleEnum(T v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleEnum(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleEnum(T v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        internal bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleEnum<T>(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleEnum<T>(T v) => new(v);

        public static implicit operator UnityEngine.UIElements.StyleEnum<T>(StyleEnum<T> style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => style.value,
                _ => style.keyword.ToNative()
            };
        }
    }
    
    public struct StyleColor
    {
        public RishStyleKeyword keyword;
        public Color value;

        public StyleColor(Color v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleColor(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleColor(Color v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        internal bool IsNull() => keyword == RishStyleKeyword.Null;
        internal bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleColor(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleColor(Color v) => new(v);

        public static implicit operator UnityEngine.UIElements.StyleColor(StyleColor style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => style.value,
                _ => style.keyword.ToNative()
            };
        }
    }
    
    public struct StyleBackground
    {
        public RishStyleKeyword keyword;
        public Background value;

        public StyleBackground(Background v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleBackground(Texture2D v) : this(Background.FromTexture2D(v), RishStyleKeyword.Undefined) { }
        public StyleBackground(RenderTexture v) : this(Background.FromRenderTexture(v), RishStyleKeyword.Undefined) { }
        public StyleBackground(Sprite v) : this(Background.FromSprite(v), RishStyleKeyword.Undefined) { }
        public StyleBackground(VectorImage v) : this(Background.FromVectorImage(v), RishStyleKeyword.Undefined) { }
        public StyleBackground(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleBackground(Background v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        internal bool IsNull() => keyword == RishStyleKeyword.Null;
        internal bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleBackground(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleBackground(Background v) => new(v);
        public static implicit operator StyleBackground(Texture2D v) => new(v);
        public static implicit operator StyleBackground(RenderTexture v) => new(v);
        public static implicit operator StyleBackground(Sprite v) => new(v);
        public static implicit operator StyleBackground(VectorImage v) => new(v);

        public static implicit operator UnityEngine.UIElements.StyleBackground(StyleBackground style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => style.value,
                _ => style.keyword.ToNative()
            };
        }
    }
    
    public struct StyleLength
    {
        public RishStyleKeyword keyword;
        public Length value;

        public StyleLength(Length v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleLength(float v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleLength(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleLength(Length v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        internal bool IsNull() => keyword == RishStyleKeyword.Null;
        internal bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleLength(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleLength(Length v) => new(v);
        public static implicit operator StyleLength(float v) => new(v);

        public static implicit operator UnityEngine.UIElements.StyleLength(StyleLength style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => style.value,
                _ => style.keyword.ToNative()
            };
        }
    }
    
    public struct StyleFloat
    {
        public RishStyleKeyword keyword;
        public float value;

        public StyleFloat(float v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleFloat(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleFloat(float v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        internal bool IsNull() => keyword == RishStyleKeyword.Null;
        internal bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleFloat(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleFloat(float v) => new(v);
        
        public static implicit operator UnityEngine.UIElements.StyleFloat(StyleFloat style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => style.value,
                _ => style.keyword.ToNative()
            };
        }
    }
    
    public struct StyleInt
    {
        public RishStyleKeyword keyword;
        public int value;

        public StyleInt(int v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleInt(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleInt(int v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        internal bool IsNull() => keyword == RishStyleKeyword.Null;
        internal bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleInt(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleInt(int v) => new(v);
        
        public static implicit operator UnityEngine.UIElements.StyleInt(StyleInt style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => style.value,
                _ => style.keyword.ToNative()
            };
        }
    }
    
    public struct StyleCursor
    {
        public RishStyleKeyword keyword;
        public Cursor value;

        public StyleCursor(Cursor v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleCursor(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleCursor(Cursor v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        internal bool IsNull() => keyword == RishStyleKeyword.Null;
        internal bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleCursor(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleCursor(Cursor v) => new(v);
        
        public static implicit operator UnityEngine.UIElements.StyleCursor(StyleCursor style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => style.value,
                _ => style.keyword.ToNative()
            };
        }
    }
    
    public struct StyleRotate
    {
        public RishStyleKeyword keyword;
        public Angle value;

        public StyleRotate(Angle v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleRotate(float v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleRotate(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleRotate(Angle v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        internal bool IsNull() => keyword == RishStyleKeyword.Null;
        internal bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleRotate(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleRotate(Angle v) => new(v);
        public static implicit operator StyleRotate(float v) => new(v);
        
        public static implicit operator UnityEngine.UIElements.StyleRotate(StyleRotate style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => new Rotate(style.value),
                _ => style.keyword.ToNative()
            };
        }
    }
    
    public struct StyleScale
    {
        public RishStyleKeyword keyword;
        public Vector3 value;

        public StyleScale(Vector3 v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleScale(float v) : this(new Vector3(v, v, v), RishStyleKeyword.Undefined) { }
        public StyleScale(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleScale(Vector3 v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        internal bool IsNull() => keyword == RishStyleKeyword.Null;
        internal bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleScale(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleScale(Vector3 v) => new(v);
        public static implicit operator StyleScale(float v) => new(v);
        
        public static implicit operator UnityEngine.UIElements.StyleScale(StyleScale style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => new Scale(style.value),
                _ => style.keyword.ToNative()
            };
        }
    }
    
    public struct StyleTextShadow
    {
        public RishStyleKeyword keyword;
        public TextShadow value;

        public StyleTextShadow(TextShadow v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleTextShadow(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleTextShadow(TextShadow v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        internal bool IsNull() => keyword == RishStyleKeyword.Null;
        internal bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleTextShadow(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleTextShadow(TextShadow v) => new(v);
        
        public static implicit operator UnityEngine.UIElements.StyleTextShadow(StyleTextShadow style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => style.value,
                _ => style.keyword.ToNative()
            };
        }
    }
    
    public struct StyleTransformOrigin
    {
        public RishStyleKeyword keyword;
        public TransformOrigin value;

        public StyleTransformOrigin(TransformOrigin v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleTransformOrigin(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleTransformOrigin(TransformOrigin v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        internal bool IsNull() => keyword == RishStyleKeyword.Null;
        internal bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleTransformOrigin(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleTransformOrigin(TransformOrigin v) => new(v);
        
        public static implicit operator UnityEngine.UIElements.StyleTransformOrigin(StyleTransformOrigin style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => style.value,
                _ => style.keyword.ToNative()
            };
        }
    }
    
    public struct StyleFont
    {
        public RishStyleKeyword keyword;
        public Font value;

        public StyleFont(Font v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleFont(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleFont(Font v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        internal bool IsNull() => keyword == RishStyleKeyword.Null;
        internal bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleFont(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleFont(Font v) => new(v);
        
        public static implicit operator UnityEngine.UIElements.StyleFont(StyleFont style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => style.value,
                _ => style.keyword.ToNative()
            };
        }
    }
    
    public struct StyleFontDefinition
    {
        public RishStyleKeyword keyword;
        public FontDefinition value;

        public StyleFontDefinition(FontDefinition v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleFontDefinition(Font v) : this(FontDefinition.FromFont(v), RishStyleKeyword.Undefined) { }
        public StyleFontDefinition(FontAsset v) : this(FontDefinition.FromSDFFont(v), RishStyleKeyword.Undefined) { }
        public StyleFontDefinition(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleFontDefinition(FontDefinition v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        internal bool IsNull() => keyword == RishStyleKeyword.Null;
        internal bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleFontDefinition(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleFontDefinition(FontDefinition v) => new(v);
        public static implicit operator StyleFontDefinition(Font v) => new(v);
        public static implicit operator StyleFontDefinition(FontAsset v) => new(v);
        
        public static implicit operator UnityEngine.UIElements.StyleFontDefinition(StyleFontDefinition style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => style.value,
                _ => style.keyword.ToNative()
            };
        }
    }
    
    public struct StyleTranslate
    {
        public RishStyleKeyword keyword;
        public Translate value;

        public StyleTranslate(Translate v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleTranslate(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleTranslate(Translate v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        internal bool IsNull() => keyword == RishStyleKeyword.Null;
        internal bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleTranslate(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleTranslate(Translate v) => new(v);
        
        public static implicit operator UnityEngine.UIElements.StyleTranslate(StyleTranslate style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => style.value,
                _ => style.keyword.ToNative()
            };
        }
    }
    
    public struct StyleList<T>
    {
        public RishStyleKeyword keyword;
        public List<T> value;

        public StyleList(List<T> v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleList(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleList(List<T> v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        internal bool IsNull() => keyword == RishStyleKeyword.Null;
        internal bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleList<T>(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleList<T>(List<T> v) => new(v);
        
        public static implicit operator UnityEngine.UIElements.StyleList<T>(StyleList<T> style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => style.value,
                _ => style.keyword.ToNative()
            };
        }
    }
    
    public static class StyleKeywordExtensions {
        public static UnityEngine.UIElements.StyleKeyword ToNative(this RishStyleKeyword keyword)
        {
            return keyword switch
            {
                RishStyleKeyword.Null => UnityEngine.UIElements.StyleKeyword.Null,
                RishStyleKeyword.Undefined => UnityEngine.UIElements.StyleKeyword.Undefined,
                RishStyleKeyword.Auto => UnityEngine.UIElements.StyleKeyword.Auto,
                RishStyleKeyword.None => UnityEngine.UIElements.StyleKeyword.None,
                RishStyleKeyword.Initial => UnityEngine.UIElements.StyleKeyword.Initial,
                _ => throw new ArgumentException("Invalid keyword")
            };
        }
    }
}