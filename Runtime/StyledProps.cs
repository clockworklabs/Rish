using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public static class StyledProps {
        public static float Get(string name, ICustomStyle customStyle, float defaultValue)
        {
            var property = new CustomStyleProperty<float>(name);
            return customStyle.TryGetValue(property, out var value) ? value : defaultValue;
        }
        public static int Get(string name, ICustomStyle customStyle, int defaultValue)
        {
            var property = new CustomStyleProperty<int>(name);
            return customStyle.TryGetValue(property, out var value) ? value : defaultValue;
        }
        public static bool Get(string name, ICustomStyle customStyle, bool defaultValue)
        {
            var property = new CustomStyleProperty<bool>(name);
            return customStyle.TryGetValue(property, out var value) ? value : defaultValue;
        }
        public static Color Get(string name, ICustomStyle customStyle, Color defaultValue)
        {
            var property = new CustomStyleProperty<Color>(name);
            return customStyle.TryGetValue(property, out var value) ? value : defaultValue;
        }
        public static Texture2D Get(string name, ICustomStyle customStyle, Texture2D defaultValue)
        {
            var property = new CustomStyleProperty<Texture2D>(name);
            return customStyle.TryGetValue(property, out var value) ? value : defaultValue;
        }
        public static Sprite Get(string name, ICustomStyle customStyle, Sprite defaultValue)
        {
            var property = new CustomStyleProperty<Sprite>(name);
            return customStyle.TryGetValue(property, out var value) ? value : defaultValue;
        }
        public static VectorImage Get(string name, ICustomStyle customStyle, VectorImage defaultValue)
        {
            var property = new CustomStyleProperty<VectorImage>(name);
            return customStyle.TryGetValue(property, out var value) ? value : defaultValue;
        }
        public static T Get<T>(string name, ICustomStyle customStyle, T defaultValue) where T : Object
        {
            var property = new CustomStyleProperty<T>(name);
            return customStyle.TryGetValue(property, out var value) ? value : defaultValue;
        }
        public static string Get(string name, ICustomStyle customStyle, string defaultValue)
        {
            var property = new CustomStyleProperty<string>(name);
            return customStyle.TryGetValue(property, out var value) ? value : defaultValue;
        }
    }
    // public interface IStyledPropAttribute<T> where T : unmanaged
    // {
    //
    //
    //     T Default { get; }
    // }
    //
    // public abstract class StyledPropAttribute : PreserveAttribute
    // {
    //     protected delegate T0? CustomGetter<T0, T1>(ref T1 props) where T0 : unmanaged where T1 : struct;
    //     protected delegate void CustomSetter<T0, T1>(ref T1 props, T0? value) where T0 : unmanaged where T1 : struct;
    //     
    //     public readonly string name;
    //
    //     protected StyledPropAttribute(string name)
    //     {
    //         this.name = name;
    //     }
    //
    //     public abstract void SetValue<T>(ref T props, ICustomStyle customStyle) where T : struct;
    // }
    //
    // [AttributeUsage(AttributeTargets.Property)]
    // public class BoolStyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<bool>
    // {
    //     private readonly bool _defaultValue;
    //     bool IStyledPropAttribute<bool>.Default => _defaultValue;
    //     
    //     public BoolStyledPropAttribute(string name, bool defaultValue = false) : base(name)
    //     {
    //         _defaultValue = defaultValue;
    //     }
    //
    //     public void Set(ICustomStyle customStyle)
    //     {
    //         if (customStyle != null && customStyle.TryGetValue<T>(customProperty, out var value))
    //         {
    //             ((CustomSetter<T>) setter)(ref props, value);
    //         } else
    //         {
    //             attribute.GetDefault(out value);
    //             ((CustomSetter<T>) setter)(ref props, value);
    //         }
    //     }
    //
    //     public override void SetValue<T>(ref T props, ICustomStyle customStyle) where T : struct
    //     {
    //         var current = ((CustomGetter<bool, T>) getter)(ref props);
    //         if (current.HasValue) return;
    //     }
    // }
    //
    // [AttributeUsage(AttributeTargets.Property)]
    // public class ShortStyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<short>
    // {
    //     private readonly short _defaultValue;
    //     short IStyledPropAttribute<short>.Default => _defaultValue;
    //     
    //     public ShortStyledPropAttribute(string name, short defaultValue = 0) : base(name)
    //     {
    //         _defaultValue = defaultValue;
    //     }
    // }
    // [AttributeUsage(AttributeTargets.Property)]
    // public class UShortStyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<ushort>
    // {
    //     private readonly ushort _defaultValue;
    //     ushort IStyledPropAttribute<ushort>.Default => _defaultValue;
    //     
    //     public UShortStyledPropAttribute(string name, ushort defaultValue = 0) : base(name)
    //     {
    //         _defaultValue = defaultValue;
    //     }
    // }
    //
    // [AttributeUsage(AttributeTargets.Property)]
    // public class IntStyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<int>
    // {
    //     private readonly int _defaultValue;
    //     int IStyledPropAttribute<int>.Default => _defaultValue;
    //     
    //     public IntStyledPropAttribute(string name, int defaultValue = 0) : base(name)
    //     {
    //         _defaultValue = defaultValue;
    //     }
    // }
    //
    // [AttributeUsage(AttributeTargets.Property)]
    // public class UIntStyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<uint>
    // {
    //     private readonly uint _defaultValue;
    //     uint IStyledPropAttribute<uint>.Default => _defaultValue;
    //     
    //     public UIntStyledPropAttribute(string name, uint defaultValue = 0) : base(name)
    //     {
    //         _defaultValue = defaultValue;
    //     }
    // }
    //
    // [AttributeUsage(AttributeTargets.Property)]
    // public class LongStyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<long>
    // {
    //     private readonly long _defaultValue;
    //     long IStyledPropAttribute<long>.Default => _defaultValue;
    //     
    //     public LongStyledPropAttribute(string name, long defaultValue = 0) : base(name)
    //     {
    //         _defaultValue = defaultValue;
    //     }
    // }
    // [AttributeUsage(AttributeTargets.Property)]
    // public class ULongStyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<ulong>
    // {
    //     private readonly ulong _defaultValue;
    //     ulong IStyledPropAttribute<ulong>.Default => _defaultValue;
    //     
    //     public ULongStyledPropAttribute(string name, ulong defaultValue = 0) : base(name)
    //     {
    //         _defaultValue = defaultValue;
    //     }
    // }
    //
    // [AttributeUsage(AttributeTargets.Property)]
    // public class FloatStyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<float>
    // {
    //     private readonly float _defaultValue;
    //     float IStyledPropAttribute<float>.Default => _defaultValue;
    //     
    //     public FloatStyledPropAttribute(string name, float defaultValue = 0) : base(name)
    //     {
    //         _defaultValue = defaultValue;
    //     }
    // }
    // [AttributeUsage(AttributeTargets.Property)]
    // public class DoubleStyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<double>
    // {
    //     private readonly double _defaultValue;
    //     double IStyledPropAttribute<double>.Default => _defaultValue;
    //     
    //     public DoubleStyledPropAttribute(string name, double defaultValue = 0) : base(name)
    //     {
    //         _defaultValue = defaultValue;
    //     }
    // }
    //
    // [AttributeUsage(AttributeTargets.Property)]
    // public class ColorStyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<Color>
    // {
    //     private readonly Color _defaultValue;
    //     Color IStyledPropAttribute<Color>.Default => _defaultValue;
    //
    //     public ColorStyledPropAttribute(string name, float r, float g, float b) : this(name, r, g, b, 1) { }
    //     public ColorStyledPropAttribute(string name, float r = 0, float g = 0, float b = 0, float a = 0) : base(name)
    //     {
    //         _defaultValue = new Color(r, g, b, a);
    //     }
    // }
    //
    // [AttributeUsage(AttributeTargets.Property)]
    // public class Vector2StyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<Vector2>
    // {
    //     private readonly Vector2 _defaultValue;
    //     Vector2 IStyledPropAttribute<Vector2>.Default => _defaultValue;
    //
    //     public Vector2StyledPropAttribute(string name, float x = 0, float y = 0) : base(name)
    //     {
    //         _defaultValue = new Vector2(x, y);
    //     }
    // }
    // [AttributeUsage(AttributeTargets.Property)]
    // public class Vector3StyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<Vector3>
    // {
    //     private readonly Vector3 _defaultValue;
    //     Vector3 IStyledPropAttribute<Vector3>.Default => _defaultValue;
    //
    //     public Vector3StyledPropAttribute(string name, float x = 0, float y = 0, float z = 0) : base(name)
    //     {
    //         _defaultValue = new Vector3(x, y, z);
    //     }
    // }
    // [AttributeUsage(AttributeTargets.Property)]
    // public class Vector4StyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<Vector4>
    // {
    //     private readonly Vector4 _defaultValue;
    //     Vector4 IStyledPropAttribute<Vector4>.Default => _defaultValue;
    //
    //     public Vector4StyledPropAttribute(string name, float x = 0, float y = 0, float z = 0, float w = 0) : base(name)
    //     {
    //         _defaultValue = new Vector4(x, y, z, w);
    //     }
    // }
    //
    // [AttributeUsage(AttributeTargets.Property)]
    // public class Vector2IntStyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<Vector2Int>
    // {
    //     private readonly Vector2Int _defaultValue;
    //     Vector2Int IStyledPropAttribute<Vector2Int>.Default => _defaultValue;
    //
    //     public Vector2IntStyledPropAttribute(string name, int x = 0, int y = 0) : base(name)
    //     {
    //         _defaultValue = new Vector2Int(x, y);
    //     }
    // }
    // [AttributeUsage(AttributeTargets.Property)]
    // public class Vector3IntStyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<Vector3Int>
    // {
    //     private readonly Vector3Int _defaultValue;
    //     Vector3Int IStyledPropAttribute<Vector3Int>.Default => _defaultValue;
    //
    //     public Vector3IntStyledPropAttribute(string name, int x = 0, int y = 0, int z = 0) : base(name)
    //     {
    //         _defaultValue = new Vector3Int(x, y, z);
    //     }
    // }
    //
    // [AttributeUsage(AttributeTargets.Property)]
    // public class RectStyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<Rect>
    // {
    //     private readonly Rect _defaultValue;
    //     Rect IStyledPropAttribute<Rect>.Default => _defaultValue;
    //
    //     public RectStyledPropAttribute(string name, float x = 0, float y = 0, float width = 0, float height = 0) : base(name)
    //     {
    //         _defaultValue = new Rect(x, y, width, height);
    //     }
    // }
    //
    // [AttributeUsage(AttributeTargets.Property)]
    // public class String32StyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<FixedString32Bytes>
    // {
    //     private readonly FixedString32Bytes _defaultValue;
    //     FixedString32Bytes IStyledPropAttribute<FixedString32Bytes>.Default => _defaultValue;
    //
    //     public String32StyledPropAttribute(string name, string defaultValue = null) : base(name)
    //     {
    //         _defaultValue = string.IsNullOrWhiteSpace(defaultValue) ? default(FixedString32Bytes) : defaultValue;
    //     }
    // }
    // [AttributeUsage(AttributeTargets.Property)]
    // public class String64StyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<FixedString64Bytes>
    // {
    //     private readonly FixedString64Bytes _defaultValue;
    //     FixedString64Bytes IStyledPropAttribute<FixedString64Bytes>.Default => _defaultValue;
    //
    //     public String64StyledPropAttribute(string name, string defaultValue = null) : base(name)
    //     {
    //         _defaultValue = string.IsNullOrWhiteSpace(defaultValue) ? default(FixedString64Bytes) : defaultValue;
    //     }
    // }
    // [AttributeUsage(AttributeTargets.Property)]
    // public class String128StyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<FixedString128Bytes>
    // {
    //     private readonly FixedString128Bytes _defaultValue;
    //     FixedString128Bytes IStyledPropAttribute<FixedString128Bytes>.Default => _defaultValue;
    //
    //     public String128StyledPropAttribute(string name, string defaultValue = null) : base(name)
    //     {
    //         _defaultValue = string.IsNullOrWhiteSpace(defaultValue) ? default(FixedString128Bytes) : defaultValue;
    //     }
    // }
    // [AttributeUsage(AttributeTargets.Property)]
    // public class String512StyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<FixedString512Bytes>
    // {
    //     private readonly FixedString512Bytes _defaultValue;
    //     FixedString512Bytes IStyledPropAttribute<FixedString512Bytes>.Default => _defaultValue;
    //
    //     public String512StyledPropAttribute(string name, string defaultValue = null) : base(name)
    //     {
    //         _defaultValue = string.IsNullOrWhiteSpace(defaultValue) ? default(FixedString512Bytes) : defaultValue;
    //     }
    // }
    // [AttributeUsage(AttributeTargets.Property)]
    // public class String4096StyledPropAttribute : StyledPropAttribute, IStyledPropAttribute<FixedString4096Bytes>
    // {
    //     private readonly FixedString4096Bytes _defaultValue;
    //     FixedString4096Bytes IStyledPropAttribute<FixedString4096Bytes>.Default => _defaultValue;
    //
    //     public String4096StyledPropAttribute(string name, string defaultValue = null) : base(name)
    //     {
    //         _defaultValue = string.IsNullOrWhiteSpace(defaultValue) ? default(FixedString4096Bytes) : defaultValue;
    //     }
    // }
    //
    // // [AttributeUsage(AttributeTargets.Property)]
    // // public class StyledPropAttribute : PreserveAttribute
    // // {
    // //     public readonly string name;
    // //     private readonly bool _defaultBool;
    // //     private readonly int _defaultInt;
    // //     private readonly float _defaultFloat;
    // //     private readonly Color _defaultColor;
    // //     private readonly FixedString32Bytes _defaultString32;
    // //     private readonly FixedString64Bytes _defaultString64;
    // //     private readonly FixedString128Bytes _defaultString128;
    // //     private readonly FixedString512Bytes _defaultString512;
    // //     private readonly FixedString4096Bytes _defaultString4096;
    // //
    // //     public StyledPropAttribute(string name)
    // //     {
    // //         this.name = name;
    // //     }
    // //     public StyledPropAttribute(string name, bool defaultValue) : this(name)
    // //     {
    // //         _defaultBool = defaultValue;
    // //     }
    // //     public StyledPropAttribute(string name, int defaultValue) : this(name)
    // //     {
    // //         _defaultInt = defaultValue;
    // //     }
    // //     public StyledPropAttribute(string name, float defaultValue) : this(name)
    // //     {
    // //         _defaultFloat = defaultValue;
    // //     }
    // //     public StyledPropAttribute(string name, float r, float g, float b, float a = 1) : this(name)
    // //     {
    // //         _defaultColor = new Color(r, g, b, a);
    // //     }
    // //     public StyledPropAttribute(string name, FixedString32Bytes defaultValue) : this(name)
    // //     {
    // //         _defaultString32 = defaultValue;
    // //     }
    // //     public StyledPropAttribute(string name, FixedString64Bytes defaultValue) : this(name)
    // //     {
    // //         _defaultString64 = defaultValue;
    // //     }
    // //     public StyledPropAttribute(string name, FixedString128Bytes defaultValue) : this(name)
    // //     {
    // //         _defaultString128 = defaultValue;
    // //     }
    // //     public StyledPropAttribute(string name, FixedString512Bytes defaultValue) : this(name)
    // //     {
    // //         _defaultString512 = defaultValue;
    // //     }
    // //     public StyledPropAttribute(string name, FixedString4096Bytes defaultValue) : this(name)
    // //     {
    // //         _defaultString4096 = defaultValue;
    // //     }
    // //
    // //     public void GetDefault(out bool value) => value = _defaultBool;
    // //     public void GetDefault(out int value) => value = _defaultInt;
    // //     public void GetDefault(out float value) => value = _defaultFloat;
    // //     public void GetDefault(out Color value) => value = _defaultColor;
    // //     public void GetDefault(out FixedString32Bytes value) => value = _defaultString32;
    // //     public void GetDefault(out FixedString64Bytes value) => value = _defaultString64;
    // //     public void GetDefault(out FixedString128Bytes value) => value = _defaultString128;
    // //     public void GetDefault(out FixedString512Bytes value) => value = _defaultString512;
    // //     public void GetDefault(out FixedString4096Bytes value) => value = _defaultString4096;
    // // }
    //
    // public static class StyledProps
    // {
    //     private static readonly Type AttributeType = typeof(StyledPropAttribute);
    //
    //     private static Dictionary<Type, StyledProperty[]> StyledProperties { get; } = new();
    //
    //     public static bool Register<T>() where T : struct
    //     {
    //         var type = typeof(T);
    //         if (StyledProperties.TryGetValue(type, out var propertyInfos))
    //         {
    //             return propertyInfos != null;
    //         }
    //
    //         var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
    //             .Where(pi => Attribute.IsDefined(pi, AttributeType))
    //             .Select(StyledProperty.Create<T>)
    //             .Where(p => p != null)
    //             .ToArray();
    //
    //         if (properties.Length <= 0)
    //         {
    //             StyledProperties[type] = null;
    //             return false;
    //         }
    //
    //         StyledProperties[type] = properties;
    //         return true;
    //     }
    //
    //     public static void Style<T>(ref T props, ICustomStyle customStyle) where T : struct
    //     {
    //         var type = typeof(T);
    //
    //         var properties = StyledProperties[type];
    //         if (properties == null)
    //         {
    //             return;
    //         }
    //         
    //         foreach (var styledProperty in properties)
    //         {
    //             styledProperty.SetValue(ref props, customStyle);
    //         }
    //     }
    //
    //     // private static readonly Type BoolType = typeof(bool?);
    //     // private static readonly Type IntType = typeof(int?);
    //     // private static readonly Type FloatType = typeof(float?);
    //     // private static readonly Type ColorType = typeof(Color?);
    //     // private static readonly Type String32Type = typeof(FixedString32Bytes?);
    //     // private static readonly Type String64Type = typeof(FixedString64Bytes?);
    //     // private static readonly Type String128Type = typeof(FixedString128Bytes?);
    //     // private static readonly Type String512Type = typeof(FixedString512Bytes?);
    //     // private static readonly Type String4096Type = typeof(FixedString4096Bytes?);
    //     // // private static readonly Type TextureType = typeof(Texture2D);
    //     // // private static readonly Type SpriteType = typeof(Sprite);
    //     // // private static readonly Type VectorImageType = typeof(VectorImage);
    //     
    //     // private delegate bool? CustomBoolGetter<T>(ref T props) where T : struct;
    //     // private delegate void CustomBoolSetter<T>(ref T props, bool? value) where T : struct;
    //     // private delegate int? CustomIntGetter<T>(ref T props) where T : struct;
    //     // private delegate void CustomIntSetter<T>(ref T props, int? value) where T : struct;
    //     // private delegate float? CustomFloatGetter<T>(ref T props) where T : struct;
    //     // private delegate void CustomFloatSetter<T>(ref T props, float? value) where T : struct;
    //     // private delegate Color? CustomColorGetter<T>(ref T props) where T : struct;
    //     // private delegate void CustomColorSetter<T>(ref T props, Color? value) where T : struct;
    //     // private delegate FixedString32Bytes? CustomString32Getter<T>(ref T props) where T : struct;
    //     // private delegate void CustomString32Setter<T>(ref T props, FixedString32Bytes? value) where T : struct;
    //     // private delegate FixedString64Bytes? CustomString64Getter<T>(ref T props) where T : struct;
    //     // private delegate void CustomString64Setter<T>(ref T props, FixedString64Bytes? value) where T : struct;
    //     // private delegate FixedString128Bytes? CustomString128Getter<T>(ref T props) where T : struct;
    //     // private delegate void CustomString128Setter<T>(ref T props, FixedString128Bytes? value) where T : struct;
    //     // private delegate FixedString512Bytes? CustomString512Getter<T>(ref T props) where T : struct;
    //     // private delegate void CustomString512Setter<T>(ref T props, FixedString512Bytes? value) where T : struct;
    //     // private delegate FixedString4096Bytes? CustomString4096Getter<T>(ref T props) where T : struct;
    //     // private delegate void CustomString4096Setter<T>(ref T props, FixedString4096Bytes? value) where T : struct;
    //
    //     private class StyledProperty<T> where T : unmanaged
    //     {
    //         private readonly string name;
    //         private readonly Type type;
    //         private readonly Delegate getter;
    //         private readonly Delegate setter;
    //
    //         private readonly StyledPropAttribute attribute;
    //         
    //         private StyledProperty(string name, Type type, Delegate getter, Delegate setter, StyledPropAttribute attribute)
    //         {
    //             this.name = name;
    //             this.type = type;
    //             this.getter = getter;
    //             this.setter = setter;
    //
    //             this.attribute = attribute;
    //         }
    //
    //         public void SetValue<T>(ref T props, ICustomStyle customStyle) where T : struct
    //         {
    //
    //             attribute.SetValue<T>(ref props, customStyle);
    //             if (customStyle != null && customStyle.TryGetValue<T>(customProperty, out var value))
    //             {
    //                 ((CustomSetter<T>) setter)(ref props, value);
    //             } else
    //             {
    //                 attribute.GetDefault(out value);
    //                 ((CustomSetter<T>) setter)(ref props, value);
    //             }
    //             
    //             if (type == BoolType)
    //             {
    //             } else if (type == IntType)
    //             {
    //                 var current = ((CustomIntGetter<T>) getter)(ref props);
    //                 if (current.HasValue) return;
    //                 var customProperty = new CustomStyleProperty<int>(name);
    //                 if (customStyle != null && customStyle.TryGetValue(customProperty, out var value))
    //                 {
    //                     ((CustomIntSetter<T>) setter)(ref props, value);
    //                 } else
    //                 {
    //                     attribute.GetDefault(out value);
    //                     ((CustomIntSetter<T>) setter)(ref props, value);
    //                 }
    //             } else if (type == FloatType)
    //             {
    //                 var current = ((CustomFloatGetter<T>) getter)(ref props);
    //                 if (current.HasValue) return;
    //                 var customProperty = new CustomStyleProperty<float>(name);
    //                 if (customStyle != null && customStyle.TryGetValue(customProperty, out var value))
    //                 {
    //                     ((CustomFloatSetter<T>) setter)(ref props, value);
    //                 } else
    //                 {
    //                     attribute.GetDefault(out value);
    //                     ((CustomFloatSetter<T>) setter)(ref props, value);
    //                 }
    //             } else if (type == ColorType)
    //             {
    //                 var current = ((CustomColorGetter<T>) getter)(ref props);
    //                 if (current.HasValue) return;
    //                 var customProperty = new CustomStyleProperty<Color>(name);
    //                 if (customStyle != null && customStyle.TryGetValue(customProperty, out var value))
    //                 {
    //                     ((CustomColorSetter<T>) setter)(ref props, value);
    //                 } else
    //                 {
    //                     attribute.GetDefault(out value);
    //                     ((CustomColorSetter<T>) setter)(ref props, value);
    //                 }
    //             } else if (type == String32Type)
    //             {
    //                 var current = ((CustomString32Getter<T>) getter)(ref props);
    //                 if (current.HasValue) return;
    //                 var customProperty = new CustomStyleProperty<string>(name);
    //                 if (customStyle != null && customStyle.TryGetValue(customProperty, out var value))
    //                 {
    //                     ((CustomString32Setter<T>) setter)(ref props, value);
    //                 } else
    //                 {
    //                     attribute.GetDefault(out FixedString32Bytes fixedValue);
    //                     ((CustomString32Setter<T>) setter)(ref props, fixedValue);
    //                 }
    //             } else if (type == String64Type)
    //             {
    //                 var current = ((CustomString64Getter<T>) getter)(ref props);
    //                 if (current.HasValue) return;
    //                 var customProperty = new CustomStyleProperty<string>(name);
    //                 if (customStyle != null && customStyle.TryGetValue(customProperty, out var value))
    //                 {
    //                     ((CustomString64Setter<T>) setter)(ref props, value);
    //                 } else
    //                 {
    //                     attribute.GetDefault(out FixedString64Bytes fixedValue);
    //                     ((CustomString64Setter<T>) setter)(ref props, fixedValue);
    //                 }
    //             } else if (type == String128Type)
    //             {
    //                 var current = ((CustomString128Getter<T>) getter)(ref props);
    //                 if (current.HasValue) return;
    //                 var customProperty = new CustomStyleProperty<string>(name);
    //                 if (customStyle != null && customStyle.TryGetValue(customProperty, out var value))
    //                 {
    //                     ((CustomString128Setter<T>) setter)(ref props, value);
    //                 } else
    //                 {
    //                     attribute.GetDefault(out FixedString128Bytes fixedValue);
    //                     ((CustomString128Setter<T>) setter)(ref props, fixedValue);
    //                 }
    //             } else if (type == String512Type)
    //             {
    //                 var current = ((CustomString512Getter<T>) getter)(ref props);
    //                 if (current.HasValue) return;
    //                 var customProperty = new CustomStyleProperty<string>(name);
    //                 if (customStyle != null && customStyle.TryGetValue(customProperty, out var value))
    //                 {
    //                     ((CustomString512Setter<T>) setter)(ref props, value);
    //                 } else
    //                 {
    //                     attribute.GetDefault(out FixedString512Bytes fixedValue);
    //                     ((CustomString512Setter<T>) setter)(ref props, fixedValue);
    //                 }
    //             } else if (type == String4096Type)
    //             {
    //                 var current = ((CustomString4096Getter<T>) getter)(ref props);
    //                 if (current.HasValue) return;
    //                 var customProperty = new CustomStyleProperty<string>(name);
    //                 if (customStyle != null && customStyle.TryGetValue(customProperty, out var value))
    //                 {
    //                     ((CustomString4096Setter<T>) setter)(ref props, value);
    //                 } else
    //                 {
    //                     attribute.GetDefault(out FixedString4096Bytes fixedValue);
    //                     ((CustomString4096Setter<T>) setter)(ref props, fixedValue);
    //                 }
    //             }
    //         }
    //
    //         public static StyledProperty<T> Create<T>(PropertyInfo propertyInfo) where T : struct
    //         {
    //             var attribute = (StyledPropAttribute) Attribute.GetCustomAttribute(propertyInfo, AttributeType);
    //             if (propertyInfo.DeclaringType != typeof(T))
    //             {
    //                 throw new UnityException("Invalid property");
    //             }
    //
    //             var name = string.IsNullOrWhiteSpace(attribute.name)
    //                 ? $"--prop-{propertyInfo.Name}"
    //                 : attribute.name;
    //             
    //             var type = propertyInfo.PropertyType;
    //             
    //             if (type == BoolType)
    //             {
    //                 var getterType = typeof(CustomBoolGetter<T>);
    //                 var getter = Delegate.CreateDelegate(getterType, propertyInfo.GetGetMethod(true));
    //                 var setterType = typeof(CustomBoolSetter<T>);
    //                 var setter = Delegate.CreateDelegate(setterType, propertyInfo.GetSetMethod(true));
    //
    //                 return new StyledProperty(name, type, getter, setter, attribute);
    //             }
    //             
    //             if (type == IntType)
    //             {
    //                 var getterType = typeof(CustomIntGetter<T>);
    //                 var getter = Delegate.CreateDelegate(getterType, propertyInfo.GetGetMethod(true));
    //                 var setterType = typeof(CustomIntSetter<T>);
    //                 var setter = Delegate.CreateDelegate(setterType, propertyInfo.GetSetMethod(true));
    //
    //                 return new StyledProperty(name, type, getter, setter, attribute);
    //             }
    //             
    //             if (type == FloatType)
    //             {
    //                 var getterType = typeof(CustomFloatGetter<T>);
    //                 var getter = Delegate.CreateDelegate(getterType, propertyInfo.GetGetMethod(true));
    //                 var setterType = typeof(CustomFloatSetter<T>);
    //                 var setter = Delegate.CreateDelegate(setterType, propertyInfo.GetSetMethod(true));
    //
    //                 return new StyledProperty(name, type, getter, setter, attribute);
    //             }
    //             
    //             if (type == ColorType)
    //             {
    //                 var getterType = typeof(CustomColorGetter<T>);
    //                 var getter = Delegate.CreateDelegate(getterType, propertyInfo.GetGetMethod(true));
    //                 var setterType = typeof(CustomColorSetter<T>);
    //                 var setter = Delegate.CreateDelegate(setterType, propertyInfo.GetSetMethod(true));
    //
    //                 return new StyledProperty(name, type, getter, setter, attribute);
    //             }
    //             
    //             if (type == String32Type)
    //             {
    //                 var getterType = typeof(CustomString32Getter<T>);
    //                 var getter = Delegate.CreateDelegate(getterType, propertyInfo.GetGetMethod(true));
    //                 var setterType = typeof(CustomString32Setter<T>);
    //                 var setter = Delegate.CreateDelegate(setterType, propertyInfo.GetSetMethod(true));
    //
    //                 return new StyledProperty(name, type, getter, setter, attribute);
    //             }
    //             
    //             if (type == String64Type)
    //             {
    //                 var getterType = typeof(CustomString64Getter<T>);
    //                 var getter = Delegate.CreateDelegate(getterType, propertyInfo.GetGetMethod(true));
    //                 var setterType = typeof(CustomString64Setter<T>);
    //                 var setter = Delegate.CreateDelegate(setterType, propertyInfo.GetSetMethod(true));
    //
    //                 return new StyledProperty(name, type, getter, setter, attribute);
    //             }
    //             
    //             if (type == String128Type)
    //             {
    //                 var getterType = typeof(CustomString128Getter<T>);
    //                 var getter = Delegate.CreateDelegate(getterType, propertyInfo.GetGetMethod(true));
    //                 var setterType = typeof(CustomString128Setter<T>);
    //                 var setter = Delegate.CreateDelegate(setterType, propertyInfo.GetSetMethod(true));
    //
    //                 return new StyledProperty(name, type, getter, setter, attribute);
    //             }
    //             
    //             if (type == String512Type)
    //             {
    //                 var getterType = typeof(CustomString512Getter<T>);
    //                 var getter = Delegate.CreateDelegate(getterType, propertyInfo.GetGetMethod(true));
    //                 var setterType = typeof(CustomString512Setter<T>);
    //                 var setter = Delegate.CreateDelegate(setterType, propertyInfo.GetSetMethod(true));
    //
    //                 return new StyledProperty(name, type, getter, setter, attribute);
    //             }
    //             
    //             if (type == String4096Type)
    //             {
    //                 var getterType = typeof(CustomString4096Getter<T>);
    //                 var getter = Delegate.CreateDelegate(getterType, propertyInfo.GetGetMethod(true));
    //                 var setterType = typeof(CustomString4096Setter<T>);
    //                 var setter = Delegate.CreateDelegate(setterType, propertyInfo.GetSetMethod(true));
    //
    //                 return new StyledProperty(name, type, getter, setter, attribute);
    //             }
    //
    //             return null;
    //         }
    //     }
    // }
}