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
    
    [ComparersProvider]
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
    
    public enum BackgroundHorizontalPositionKeyword { Center, Left, Right }
    public struct BackgroundHorizontalPosition
    {
        public BackgroundHorizontalPositionKeyword keyword;
        public Length offset;

        public BackgroundHorizontalPosition(BackgroundHorizontalPositionKeyword keyword) : this(keyword, default) { }
        public BackgroundHorizontalPosition(Length offset) : this(BackgroundHorizontalPositionKeyword.Left, offset) { }

        public BackgroundHorizontalPosition(BackgroundHorizontalPositionKeyword keyword, Length offset)
        {
            this.keyword = keyword;
            this.offset = offset;
        }

        public static implicit operator BackgroundPosition(BackgroundHorizontalPosition position)
        {
            BackgroundPositionKeyword positionKeyword;
            Length positionOffset;
            switch (position.keyword)
            {
                case BackgroundHorizontalPositionKeyword.Left:
                    positionKeyword = BackgroundPositionKeyword.Left;
                    positionOffset = position.offset;
                    break;
                case BackgroundHorizontalPositionKeyword.Right:
                    positionKeyword = BackgroundPositionKeyword.Right;
                    positionOffset = position.offset;
                    break;
                default:
                    positionKeyword = BackgroundPositionKeyword.Center;
                    positionOffset = default;
                    break;
            }
            
            return new BackgroundPosition
            {
                keyword = positionKeyword,
                offset = positionOffset
            };
        }

        public static implicit operator BackgroundHorizontalPosition(BackgroundPosition nativePosition)
        {
            BackgroundHorizontalPositionKeyword positionKeyword;
            Length positionOffset;
            switch (nativePosition.keyword)
            {
                case BackgroundPositionKeyword.Left:
                    positionKeyword = BackgroundHorizontalPositionKeyword.Left;
                    positionOffset = nativePosition.offset;
                    break;
                case BackgroundPositionKeyword.Right:
                    positionKeyword = BackgroundHorizontalPositionKeyword.Right;
                    positionOffset = nativePosition.offset;
                    break;
                default:
                    positionKeyword = BackgroundHorizontalPositionKeyword.Center;
                    positionOffset = default;
                    break;
            }
            
            return new BackgroundHorizontalPosition
            {
                keyword = positionKeyword,
                offset = positionOffset
            };
        }
    }
    [ComparersProvider]
    public readonly struct StyleBackgroundHorizontalPosition : IStyleValue<BackgroundPosition>
    {
        public readonly BackgroundPosition value;
        BackgroundPosition IStyleValue<BackgroundPosition>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<BackgroundPosition>.keyword => keyword;
        
        public StyleBackgroundHorizontalPosition(BackgroundHorizontalPosition value) : this(value, RishStyleKeyword.Undefined) { }
        public StyleBackgroundHorizontalPosition(BackgroundHorizontalPositionKeyword keyword) : this(new BackgroundHorizontalPosition(keyword)) { }
        public StyleBackgroundHorizontalPosition(Length length) : this(new BackgroundHorizontalPosition(length)) { }
        public StyleBackgroundHorizontalPosition(BackgroundHorizontalPositionKeyword keyword, Length offset) : this(new BackgroundHorizontalPosition(keyword, offset)) { }
        public StyleBackgroundHorizontalPosition(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleBackgroundHorizontalPosition(BackgroundHorizontalPosition value, RishStyleKeyword keyword)
        {
            this.value = value;
            this.keyword = keyword;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleBackgroundHorizontalPosition(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleBackgroundHorizontalPosition(BackgroundHorizontalPosition v) => new(v);
        public static implicit operator StyleBackgroundHorizontalPosition(BackgroundHorizontalPositionKeyword keyword) => new(keyword);
        public static implicit operator StyleBackgroundHorizontalPosition(Length length) => new(length);
        public static implicit operator StyleBackgroundHorizontalPosition((BackgroundHorizontalPositionKeyword keyword, Length offset) v) => new(v.keyword, v.offset);

        public static implicit operator StyleBackgroundPosition(StyleBackgroundHorizontalPosition style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => style.value,
                _ => style.keyword.ToNative()
            };
        }

        public static implicit operator StyleBackgroundHorizontalPosition(StyleBackgroundPosition style) =>
            style.keyword == StyleKeyword.Undefined ? (BackgroundHorizontalPosition)style.value : style.keyword.FromNative();

        [Comparer]
        private static bool Equals(StyleBackgroundHorizontalPosition a, StyleBackgroundHorizontalPosition b)
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
    
    public enum BackgroundVerticalPositionKeyword { Center, Top, Bottom }
    public struct BackgroundVerticalPosition
    {
        public BackgroundVerticalPositionKeyword keyword;
        public Length offset;

        public BackgroundVerticalPosition(BackgroundVerticalPositionKeyword keyword) : this(keyword, default) { }
        public BackgroundVerticalPosition(Length offset) : this(BackgroundVerticalPositionKeyword.Top, offset) { }

        public BackgroundVerticalPosition(BackgroundVerticalPositionKeyword keyword, Length offset)
        {
            this.keyword = keyword;
            this.offset = offset;
        }

        public static implicit operator BackgroundPosition(BackgroundVerticalPosition position)
        {
            BackgroundPositionKeyword positionKeyword;
            Length positionOffset;
            switch (position.keyword)
            {
                case BackgroundVerticalPositionKeyword.Top:
                    positionKeyword = BackgroundPositionKeyword.Top;
                    positionOffset = position.offset;
                    break;
                case BackgroundVerticalPositionKeyword.Bottom:
                    positionKeyword = BackgroundPositionKeyword.Bottom;
                    positionOffset = position.offset;
                    break;
                default:
                    positionKeyword = BackgroundPositionKeyword.Center;
                    positionOffset = default;
                    break;
            }
            
            return new BackgroundPosition
            {
                keyword = positionKeyword,
                offset = positionOffset
            };
        }

        public static implicit operator BackgroundVerticalPosition(BackgroundPosition nativePosition)
        {
            BackgroundVerticalPositionKeyword positionKeyword;
            Length positionOffset;
            switch (nativePosition.keyword)
            {
                case BackgroundPositionKeyword.Top:
                    positionKeyword = BackgroundVerticalPositionKeyword.Top;
                    positionOffset = nativePosition.offset;
                    break;
                case BackgroundPositionKeyword.Bottom:
                    positionKeyword = BackgroundVerticalPositionKeyword.Bottom;
                    positionOffset = nativePosition.offset;
                    break;
                default:
                    positionKeyword = BackgroundVerticalPositionKeyword.Center;
                    positionOffset = default;
                    break;
            }
            
            return new BackgroundVerticalPosition
            {
                keyword = positionKeyword,
                offset = positionOffset
            };
        }
    }
    [ComparersProvider]
    public readonly struct StyleBackgroundVerticalPosition : IStyleValue<BackgroundPosition>
    {
        public readonly BackgroundPosition value;
        BackgroundPosition IStyleValue<BackgroundPosition>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<BackgroundPosition>.keyword => keyword;
        
        public StyleBackgroundVerticalPosition(BackgroundVerticalPosition value) : this(value, RishStyleKeyword.Undefined) { }
        public StyleBackgroundVerticalPosition(BackgroundVerticalPositionKeyword keyword) : this(new BackgroundVerticalPosition(keyword)) { }
        public StyleBackgroundVerticalPosition(Length length) : this(new BackgroundVerticalPosition(length)) { }
        public StyleBackgroundVerticalPosition(BackgroundVerticalPositionKeyword keyword, Length offset) : this(new BackgroundVerticalPosition(keyword, offset)) { }
        public StyleBackgroundVerticalPosition(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleBackgroundVerticalPosition(BackgroundVerticalPosition value, RishStyleKeyword keyword)
        {
            this.value = value;
            this.keyword = keyword;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleBackgroundVerticalPosition(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleBackgroundVerticalPosition(BackgroundVerticalPosition v) => new(v);
        public static implicit operator StyleBackgroundVerticalPosition(BackgroundVerticalPositionKeyword keyword) => new(keyword);
        public static implicit operator StyleBackgroundVerticalPosition(Length length) => new(length);
        public static implicit operator StyleBackgroundVerticalPosition((BackgroundVerticalPositionKeyword keyword, Length offset) v) => new(v.keyword, v.offset);

        public static implicit operator StyleBackgroundPosition(StyleBackgroundVerticalPosition style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => style.value,
                _ => style.keyword.ToNative()
            };
        }

        public static implicit operator StyleBackgroundVerticalPosition(StyleBackgroundPosition style) =>
            style.keyword == StyleKeyword.Undefined ? (BackgroundVerticalPosition)style.value : style.keyword.FromNative();

        [Comparer]
        private static bool Equals(StyleBackgroundVerticalPosition a, StyleBackgroundVerticalPosition b)
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
    
    public enum RepeatHorizontally { NoRepeat, Space, Round, Repeat }
    public enum RepeatVertically { NoRepeat, Space, Round, Repeat }
    public readonly struct StyleBackgroundRepeat : IStyleValue<BackgroundRepeat>
    {
        public readonly BackgroundRepeat value;
        BackgroundRepeat IStyleValue<BackgroundRepeat>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<BackgroundRepeat>.keyword => keyword;

        public StyleBackgroundRepeat(BackgroundRepeat v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleBackgroundRepeat(Repeat x) : this(new BackgroundRepeat(x, default)) { }
        public StyleBackgroundRepeat(RepeatHorizontally x) : this(new BackgroundRepeat((Repeat)x, default)) { }
        public StyleBackgroundRepeat(RepeatVertically y) : this(new BackgroundRepeat(default, (Repeat)y)) { }
        public StyleBackgroundRepeat(Repeat x, Repeat y) : this(new BackgroundRepeat(x, y)) { }
        public StyleBackgroundRepeat(RepeatHorizontally x, Repeat y) : this(new BackgroundRepeat((Repeat)x, y)) { }
        public StyleBackgroundRepeat(RepeatVertically y, Repeat x) : this(new BackgroundRepeat(x, (Repeat)y)) { }
        public StyleBackgroundRepeat(Repeat x, RepeatVertically y) : this(new BackgroundRepeat(x, (Repeat)y)) { }
        public StyleBackgroundRepeat(Repeat y, RepeatHorizontally x) : this(new BackgroundRepeat((Repeat)x, y)) { }
        public StyleBackgroundRepeat(RepeatHorizontally x, RepeatVertically y) : this(new BackgroundRepeat((Repeat)x, (Repeat)y)) { }
        public StyleBackgroundRepeat(RepeatVertically y, RepeatHorizontally x) : this(new BackgroundRepeat((Repeat)x, (Repeat)y)) { }
        public StyleBackgroundRepeat(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleBackgroundRepeat(BackgroundRepeat v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleBackgroundRepeat(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleBackgroundRepeat(BackgroundRepeat v) => new(v);
        public static implicit operator StyleBackgroundRepeat(Repeat x) => new(x);
        public static implicit operator StyleBackgroundRepeat(RepeatHorizontally x) => new(x);
        public static implicit operator StyleBackgroundRepeat(RepeatVertically y) => new(y);
        public static implicit operator StyleBackgroundRepeat((Repeat x, Repeat y) v) => new(v.x, v.y);
        public static implicit operator StyleBackgroundRepeat((RepeatHorizontally x, Repeat y) v) => new(v.x, v.y);
        public static implicit operator StyleBackgroundRepeat((RepeatVertically y, Repeat x) v) => new(v.y, v.x);
        public static implicit operator StyleBackgroundRepeat((Repeat x, RepeatVertically y) v) => new(v.x, v.y);
        public static implicit operator StyleBackgroundRepeat((Repeat y, RepeatHorizontally x) v) => new(v.y, v.x);
        public static implicit operator StyleBackgroundRepeat((RepeatHorizontally x, RepeatVertically y) v) => new(v.x, v.y);
        public static implicit operator StyleBackgroundRepeat((RepeatVertically y, RepeatHorizontally x) v) => new(v.y, v.x);

        public static implicit operator UnityEngine.UIElements.StyleBackgroundRepeat(StyleBackgroundRepeat style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => style.value,
                _ => style.keyword.ToNative()
            };
        }

        public static implicit operator StyleBackgroundRepeat(UnityEngine.UIElements.StyleBackgroundRepeat style) =>
            style.keyword == StyleKeyword.Undefined ? style.value : style.keyword.FromNative();
    }
    
    public readonly struct StyleBackgroundSize : IStyleValue<BackgroundSize>
    {
        public readonly BackgroundSize value;
        BackgroundSize IStyleValue<BackgroundSize>.value => value;
        
        public readonly RishStyleKeyword keyword;
        RishStyleKeyword IStyleValue<BackgroundSize>.keyword => keyword;

        public StyleBackgroundSize(BackgroundSize v) : this(v, RishStyleKeyword.Undefined) { }
        public StyleBackgroundSize(BackgroundSizeType type) : this(new BackgroundSize(type)) { }
        public StyleBackgroundSize(Length x) : this(new BackgroundSize(x, Length.Auto())) { }
        public StyleBackgroundSize(Length x, Length y) : this(new BackgroundSize(x, y)) { }
        public StyleBackgroundSize(RishStyleKeyword keyword) : this(default, keyword) { }
        private StyleBackgroundSize(BackgroundSize v, RishStyleKeyword keyword)
        {
            this.keyword = keyword;
            this.value = v;
        }

        public bool IsNull() => keyword == RishStyleKeyword.Null;
        public bool IsNotNull() => keyword != RishStyleKeyword.Null;
        
        public static implicit operator StyleBackgroundSize(RishStyleKeyword keyword) => new(keyword);
        public static implicit operator StyleBackgroundSize(BackgroundSize v) => new(v);
        public static implicit operator StyleBackgroundSize(BackgroundSizeType type) => new(type);
        public static implicit operator StyleBackgroundSize(Length x) => new(x);
        public static implicit operator StyleBackgroundSize((Length x, Length y) v) => new(v.x, v.y);

        public static implicit operator UnityEngine.UIElements.StyleBackgroundSize(StyleBackgroundSize style)
        {
            return style.keyword switch
            {
                RishStyleKeyword.Null => throw new UnityException("Invalid"),
                RishStyleKeyword.Undefined => style.value,
                _ => style.keyword.ToNative()
            };
        }

        public static implicit operator StyleBackgroundSize(UnityEngine.UIElements.StyleBackgroundSize style) =>
            style.keyword == StyleKeyword.Undefined ? style.value : style.keyword.FromNative();
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
    
    [ComparersProvider]
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
    
    [ComparersProvider]
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
    [ComparersProvider]
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
    
    [ComparersProvider]
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