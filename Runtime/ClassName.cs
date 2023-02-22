using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.UIElements;

namespace RishUI
{
    public readonly struct ClassName
    {
        // It can go up to 14 elements (index 13)
        private readonly FixedString32Bytes _element0;
        private readonly FixedString32Bytes _element1;
        private readonly FixedString32Bytes _element2;
        private readonly FixedString32Bytes _element3;
        private readonly FixedString32Bytes _element4;
        private readonly FixedString32Bytes _element5;
        private readonly FixedString32Bytes _element6;
        private readonly FixedString32Bytes _element7;
        private readonly FixedString32Bytes _element8;
        private readonly FixedString32Bytes _element9;
        // private readonly FixedString32Bytes _element10;
        // private readonly FixedString32Bytes _element11;
        // private readonly FixedString32Bytes _element12;
        // private readonly FixedString32Bytes _element13;
        
        private readonly int _count;
        public int Count => _count;

        public ClassName(string class0)
        {
            _count = 1;

            _element0 = class0;
            _element1 = default;
            _element2 = default;
            _element3 = default;
            _element4 = default;
            _element5 = default;
            _element6 = default;
            _element7 = default;
            _element8 = default;
            _element9 = default;
            // _element10 = default;
            // _element11 = default;
            // _element12 = default;
            // _element13 = default;
        }

        public ClassName(string class0, string class1)
        {
            _count = 2;

            _element0 = class0;
            _element1 = class1;
            _element2 = default;
            _element3 = default;
            _element4 = default;
            _element5 = default;
            _element6 = default;
            _element7 = default;
            _element8 = default;
            _element9 = default;
            // _element10 = default;
            // _element11 = default;
            // _element12 = default;
            // _element13 = default;
        }

        public ClassName(string class0, string class1, string class2)
        {
            _count = 3;

            _element0 = class0;
            _element1 = class1;
            _element2 = class2;
            _element3 = default;
            _element4 = default;
            _element5 = default;
            _element6 = default;
            _element7 = default;
            _element8 = default;
            _element9 = default;
            // _element10 = default;
            // _element11 = default;
            // _element12 = default;
            // _element13 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3)
        {
            _count = 4;

            _element0 = class0;
            _element1 = class1;
            _element2 = class2;
            _element3 = class3;
            _element4 = default;
            _element5 = default;
            _element6 = default;
            _element7 = default;
            _element8 = default;
            _element9 = default;
            // _element10 = default;
            // _element11 = default;
            // _element12 = default;
            // _element13 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4)
        {
            _count = 5;

            _element0 = class0;
            _element1 = class1;
            _element2 = class2;
            _element3 = class3;
            _element4 = class4;
            _element5 = default;
            _element6 = default;
            _element7 = default;
            _element8 = default;
            _element9 = default;
            // _element10 = default;
            // _element11 = default;
            // _element12 = default;
            // _element13 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5)
        {
            _count = 6;

            _element0 = class0;
            _element1 = class1;
            _element2 = class2;
            _element3 = class3;
            _element4 = class4;
            _element5 = class5;
            _element6 = default;
            _element7 = default;
            _element8 = default;
            _element9 = default;
            // _element10 = default;
            // _element11 = default;
            // _element12 = default;
            // _element13 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6)
        {
            _count = 7;

            _element0 = class0;
            _element1 = class1;
            _element2 = class2;
            _element3 = class3;
            _element4 = class4;
            _element5 = class5;
            _element6 = class6;
            _element7 = default;
            _element8 = default;
            _element9 = default;
            // _element10 = default;
            // _element11 = default;
            // _element12 = default;
            // _element13 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7)
        {
            _count = 8;

            _element0 = class0;
            _element1 = class1;
            _element2 = class2;
            _element3 = class3;
            _element4 = class4;
            _element5 = class5;
            _element6 = class6;
            _element7 = class7;
            _element8 = default;
            _element9 = default;
            // _element10 = default;
            // _element11 = default;
            // _element12 = default;
            // _element13 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8)
        {
            _count = 9;
        
            _element0 = class0;
            _element1 = class1;
            _element2 = class2;
            _element3 = class3;
            _element4 = class4;
            _element5 = class5;
            _element6 = class6;
            _element7 = class7;
            _element8 = class8;
            _element9 = default;
            // _element10 = default;
            // _element11 = default;
            // _element12 = default;
            // _element13 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9)
        {
            _count = 10;
        
            _element0 = class0;
            _element1 = class1;
            _element2 = class2;
            _element3 = class3;
            _element4 = class4;
            _element5 = class5;
            _element6 = class6;
            _element7 = class7;
            _element8 = class8;
            _element9 = class9;
            // _element10 = default;
            // _element11 = default;
            // _element12 = default;
            // _element13 = default;
        }
        
        public string this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _element0.Value;
                    case 1:
                        return _element1.Value;
                    case 2:
                        return _element2.Value;
                    case 3:
                        return _element3.Value;
                    case 4:
                        return _element4.Value;
                    case 5:
                        return _element5.Value;
                    case 6:
                        return _element6.Value;
                    case 7:
                        return _element7.Value;
                    case 8:
                        return _element8.Value;
                    case 9:
                        return _element9.Value;
                    // case 10:
                    //     return _element10.Value;
                    // case 11:
                    //     return _element11.Value;
                    // case 12:
                    //     return _element12.Value;
                    // case 13:
                    //     return _element13.Value;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public unsafe ClassName Add(ClassName className)
        {
            var copy = this;
            ClassName result = default;

            var resultPtr = new IntPtr(UnsafeUtility.AddressOf(ref result));
            var copyPtr = UnsafeUtility.AddressOf(ref copy);
            var classNamePtr = UnsafeUtility.AddressOf(ref className);

            var size = 32 * copy.Count;
            UnsafeUtility.MemCpy(resultPtr.ToPointer(), copyPtr, size);
            resultPtr = IntPtr.Add(resultPtr, size);
            size = 10 * 32 - size;
            UnsafeUtility.MemCpy(resultPtr.ToPointer(), classNamePtr, size);

            resultPtr = IntPtr.Add(resultPtr, size);
            size = 4;
            
            var count = copy.Count + className.Count;
            UnsafeUtility.MemCpy(resultPtr.ToPointer(), UnsafeUtility.AddressOf(ref count), size);

            return result;
        }

        public override string ToString()
        {
            return Count switch
            {
                0 => "()",
                1 => $"({_element0})",
                2 => $"({_element0}, {_element1})",
                3 => $"({_element0}, {_element1}, {_element2})",
                4 => $"({_element0}, {_element1}, {_element2}, {_element3})",
                5 => $"({_element0}, {_element1}, {_element2}, {_element3}, {_element4})",
                6 => $"({_element0}, {_element1}, {_element2}, {_element3}, {_element4}, {_element5})",
                7 => $"({_element0}, {_element1}, {_element2}, {_element3}, {_element4}, {_element5}, {_element6})",
                8 => $"({_element0}, {_element1}, {_element2}, {_element3}, {_element4}, {_element5}, {_element6}, {_element7})",
                9 => $"({_element0}, {_element1}, {_element2}, {_element3}, {_element4}, {_element5}, {_element6}, {_element7}, {_element8})",
                10 => $"({_element0}, {_element1}, {_element2}, {_element3}, {_element4}, {_element5}, {_element6}, {_element7}, {_element8}, {_element9})",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static List<string> CachedClassNames { get; } = new();
        public static ClassName FromElement(VisualElement element)
        {
            if (element == null)
            {
                return default;
            }
            
            CachedClassNames.Clear();
            CachedClassNames.AddRange(element.GetClasses());

            return CachedClassNames.Count switch
            {
                0 => default,
                1 => new ClassName(CachedClassNames[0]),
                2 => new ClassName(CachedClassNames[0], CachedClassNames[1]),
                3 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2]),
                4 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3]),
                5 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4]),
                6 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5]),
                7 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6]),
                8 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7]),
                9 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8]),
                _ => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9]),
                // 11 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                //     CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                //     CachedClassNames[8], CachedClassNames[9], CachedClassNames[10]),
                // 12 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                //     CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                //     CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11]),
                // _ => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                //     CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                //     CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                //     CachedClassNames[12]),
            };
        }

        public static implicit operator ClassName(string class0)
        {
            return new ClassName(class0);
        }

        public static implicit operator ClassName((string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2);
        }

        public static implicit operator ClassName((string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3);
        }

        public static implicit operator ClassName((string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4);
        }

        public static implicit operator ClassName((string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5);
        }

        public static implicit operator ClassName((string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9);
        }
        
        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10);
        }
        
        // public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string) classes)
        // {
        //     return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11);
        // }
        //
        // public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string) classes)
        // {
        //     return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12);
        // }
        //
        // public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        // {
        //     return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13);
        // }
        //
        // public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        // {
        //     return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14);
        // }

        public void SetClasses(VisualElement element)
        {
            var currentCount = (element.GetClasses() as List<string>)?.Count ?? 0;
            if (currentCount == Count)
            {
                var equals = true;
                foreach (var className in this)
                {
                    if (!element.ClassListContains(className))
                    {
                        equals = false;
                    }
                }
                
                if (equals)
                {
                    return;
                }
            }
            
            if (element is IAdvancedPicking advancedPicking)
            {
                advancedPicking.Manager.StyleSheetsPointerDetection = null;
            }
            
            element.ClearClassList();
            
            if (Count <= 0)
            {
                return;
            }
            
            foreach (var className in this)
            {
                if (!string.IsNullOrWhiteSpace(className))
                {
                    element.AddToClassList(className);
                }
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator
        {
            private readonly ClassName _list;

            private int _index;
            private string _current;

            public Enumerator(ClassName list)
            {
                _list = list;
                _index = 0;
                _current = default;
            }

            public string Current => _current;

            public bool MoveNext()
            {
                if (_index >= _list.Count)
                {
                    return false;
                }
                
                _current = _list[_index++];
                
                return true;
            }
        }
    }
}