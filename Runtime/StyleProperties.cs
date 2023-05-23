using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
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
    
    public interface IStyleValue<out T>
    {
        T value { get; }

        RishStyleKeyword keyword { get; }
    }
    
    public readonly struct StyleBackground : IStyleValue<Background>
    {
        public readonly Background value;
        Background IStyleValue<Background>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<Background>.keyword => keyword;
        
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

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
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

        public static implicit operator StyleBackground(UnityEngine.UIElements.StyleBackground style) =>
            style.keyword == StyleKeyword.Undefined ? style.value : style.keyword.FromNative();

        [Comparer]
        private static bool Equals(StyleBackground a, StyleBackground b)
        {
            var aNotNull = a.IsNotNull();
            var bNotNull = b.IsNotNull();

            if (aNotNull && bNotNull)
            {
                return a.value.Equals(b.value);
            }

            return aNotNull == bNotNull;
        }
    }
    
    public readonly struct StyleColor : IStyleValue<Color>
    {
        public readonly Color value;
        Color IStyleValue<Color>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<Color>.keyword => keyword;

        public StyleColor(Color v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleColor(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleColor(Color v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
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

        public static implicit operator StyleColor(UnityEngine.UIElements.StyleColor style) =>
            style.keyword == StyleKeyword.Undefined ? style.value : style.keyword.FromNative();
    }
    
    public readonly struct StyleCursor : IStyleValue<Cursor>
    {
        public readonly Cursor value;
        Cursor IStyleValue<Cursor>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<Cursor>.keyword => keyword;

        public StyleCursor(Cursor v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleCursor(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleCursor(Cursor v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
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

        public static implicit operator StyleCursor(UnityEngine.UIElements.StyleCursor style) =>
            style.keyword == StyleKeyword.Undefined ? style.value : style.keyword.FromNative();

        [Comparer]
        private static bool Equals(StyleCursor a, StyleCursor b)
        {
            var aNotNull = a.IsNotNull();
            var bNotNull = b.IsNotNull();

            if (aNotNull && bNotNull)
            {
                return a.value.Equals(b.value);
            }

            return aNotNull == bNotNull;
        }
    }
    
    public readonly struct StyleEnum<T> : IStyleValue<T> where T : unmanaged, IConvertible
    {
        public readonly T value;
        T IStyleValue<T>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<T>.keyword => keyword;

        public StyleEnum(T v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleEnum(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleEnum(T v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
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

        public static implicit operator StyleEnum<T>(UnityEngine.UIElements.StyleEnum<T> style) =>
            style.keyword == StyleKeyword.Undefined ? style.value : style.keyword.FromNative();
    }
    
    public readonly struct StyleFloat : IStyleValue<float>
    {
        public readonly float value;
        float IStyleValue<float>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<float>.keyword => keyword;

        public StyleFloat(float v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleFloat(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleFloat(float v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
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

        public static implicit operator StyleFloat(UnityEngine.UIElements.StyleFloat style) =>
            style.keyword == StyleKeyword.Undefined ? style.value : style.keyword.FromNative();
    }
    
    public readonly struct StyleFont : IStyleValue<Font>
    {
        public readonly Font value;
        Font IStyleValue<Font>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<Font>.keyword => keyword;

        public StyleFont(Font v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleFont(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleFont(Font v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
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

        public static implicit operator StyleFont(UnityEngine.UIElements.StyleFont style) =>
            style.keyword == StyleKeyword.Undefined ? style.value : style.keyword.FromNative();

        [Comparer]
        private static bool Equals(StyleFont a, StyleFont b)
        {
            var aNotNull = a.IsNotNull();
            var bNotNull = b.IsNotNull();

            if (aNotNull && bNotNull)
            {
                return a.value == b.value;
            }

            return aNotNull == bNotNull;
        }
    }
    
    public readonly struct StyleFontDefinition : IStyleValue<FontDefinition>
    {
        public readonly FontDefinition value;
        FontDefinition IStyleValue<FontDefinition>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<FontDefinition>.keyword => keyword;

        public StyleFontDefinition(FontDefinition v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleFontDefinition(Font v) : this(FontDefinition.FromFont(v), RishStyleKeyword.Undefined) { }
        public StyleFontDefinition(FontAsset v) : this(FontDefinition.FromSDFFont(v), RishStyleKeyword.Undefined) { }
        public StyleFontDefinition(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleFontDefinition(FontDefinition v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
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

        public static implicit operator StyleFontDefinition(UnityEngine.UIElements.StyleFontDefinition style) =>
            style.keyword == StyleKeyword.Undefined ? style.value : style.keyword.FromNative();

        [Comparer]
        private static bool Equals(StyleFontDefinition a, StyleFontDefinition b)
        {
            var aNotNull = a.IsNotNull();
            var bNotNull = b.IsNotNull();

            if (aNotNull && bNotNull)
            {
                return a.value.Equals(b.value);
            }

            return aNotNull == bNotNull;
        }
    }
    
    public readonly struct StyleInt : IStyleValue<int>
    {
        public readonly int value;
        int IStyleValue<int>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<int>.keyword => keyword;

        public StyleInt(int v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleInt(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleInt(int v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
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

        public static implicit operator StyleInt(UnityEngine.UIElements.StyleInt style) =>
            style.keyword == StyleKeyword.Undefined ? style.value : style.keyword.FromNative();
    }
    
    public readonly struct StyleLength : IStyleValue<Length>
    {
        public readonly Length value;
        Length IStyleValue<Length>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<Length>.keyword => keyword;

        public StyleLength(Length v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleLength(float v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleLength(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleLength(Length v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
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

        public static implicit operator StyleLength(UnityEngine.UIElements.StyleLength style) =>
            style.keyword == StyleKeyword.Undefined ? style.value : style.keyword.FromNative();
        public static implicit operator StyleLength(UnityEngine.UIElements.StyleFloat style) =>
            style.keyword == StyleKeyword.Undefined ? style.value : style.keyword.FromNative();
    }
    
    public readonly struct StyleList<T> : IStyleValue<List<T>>
    {
        public readonly List<T> value;
        List<T> IStyleValue<List<T>>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<List<T>>.keyword => keyword;

        public StyleList(List<T> v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleList(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleList(List<T> v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
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

        public static implicit operator StyleList<T>(UnityEngine.UIElements.StyleList<T> style) =>
            style.keyword == StyleKeyword.Undefined ? style.value : style.keyword.FromNative();

        [Comparer]
        private static bool Equals(StyleList<T> a, StyleList<T> b)
        {
            var aNotNull = a.IsNotNull();
            var bNotNull = b.IsNotNull();

            if (aNotNull && bNotNull)
            {
                return a.value == b.value;
            }

            return aNotNull == bNotNull;
        }
    }
    
    public readonly struct StyleRotate : IStyleValue<Angle>
    {
        public readonly Angle value;
        Angle IStyleValue<Angle>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<Angle>.keyword => keyword;

        public StyleRotate(Angle v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleRotate(float v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleRotate(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleRotate(Angle v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
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

        public static implicit operator StyleRotate(UnityEngine.UIElements.StyleRotate style) =>
            style.keyword == StyleKeyword.Undefined ? style.value.angle : style.keyword.FromNative();
    }
    
    public readonly struct StyleScale : IStyleValue<Vector3>
    {
        public readonly Vector3 value;
        Vector3 IStyleValue<Vector3>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<Vector3>.keyword => keyword;

        public StyleScale(Vector3 v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleScale(float v) : this(new Vector3(v, v, v), RishStyleKeyword.Undefined) { }
        public StyleScale(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleScale(Vector3 v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
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

        public static implicit operator StyleScale(UnityEngine.UIElements.StyleScale style) =>
            style.keyword == StyleKeyword.Undefined ? style.value.value : style.keyword.FromNative();
    }
    
    public readonly struct StyleTextShadow : IStyleValue<TextShadow>
    {
        public readonly TextShadow value;
        TextShadow IStyleValue<TextShadow>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<TextShadow>.keyword => keyword;

        public StyleTextShadow(TextShadow v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleTextShadow(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleTextShadow(TextShadow v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
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

        public static implicit operator StyleTextShadow(UnityEngine.UIElements.StyleTextShadow style) =>
            style.keyword == StyleKeyword.Undefined ? style.value : style.keyword.FromNative();
    }
    
    public readonly struct StyleTransformOrigin : IStyleValue<TransformOrigin>
    {
        public readonly TransformOrigin value;
        TransformOrigin IStyleValue<TransformOrigin>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<TransformOrigin>.keyword => keyword;

        public StyleTransformOrigin(TransformOrigin v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleTransformOrigin(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleTransformOrigin(TransformOrigin v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
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

        public static implicit operator StyleTransformOrigin(UnityEngine.UIElements.StyleTransformOrigin style) =>
            style.keyword == StyleKeyword.Undefined ? style.value : style.keyword.FromNative();
    }
    
    public readonly struct StyleTranslate : IStyleValue<Translate>
    {
        public readonly Translate value;
        Translate IStyleValue<Translate>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<Translate>.keyword => keyword;

        public StyleTranslate(Translate v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleTranslate(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleTranslate(Translate v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
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

        public static implicit operator StyleTranslate(UnityEngine.UIElements.StyleTranslate style) =>
            style.keyword == StyleKeyword.Undefined ? style.value : style.keyword.FromNative();
    }
    
    public static class StyleKeywordExtensions {
        public static StyleKeyword ToNative(this RishStyleKeyword keyword)
        {
            return keyword switch
            {
                RishStyleKeyword.Null => StyleKeyword.Null,
                RishStyleKeyword.Undefined => StyleKeyword.Undefined,
                RishStyleKeyword.Auto => StyleKeyword.Auto,
                RishStyleKeyword.None => StyleKeyword.None,
                RishStyleKeyword.Initial => StyleKeyword.Initial,
                _ => throw new ArgumentException("Invalid keyword")
            };
        }
        public static RishStyleKeyword FromNative(this StyleKeyword keyword)
        {
            return keyword switch
            {
                StyleKeyword.Null => RishStyleKeyword.Null,
                StyleKeyword.Undefined => RishStyleKeyword.Undefined,
                StyleKeyword.Auto => RishStyleKeyword.Auto,
                StyleKeyword.None => RishStyleKeyword.None,
                StyleKeyword.Initial => RishStyleKeyword.Initial,
                _ => throw new ArgumentException("Invalid keyword")
            };
        }
    }
}