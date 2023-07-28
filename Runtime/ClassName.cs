using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using UnityEngine.UIElements;

namespace RishUI
{
    public struct ClassName : IEnumerable<FixedString32Bytes>
    {
        // MaxCount: 4096 -> 128, 512 -> 16, 128 -> 4, 64 -> 2, 32 -> 1
        private FixedList512Bytes<FixedString32Bytes> _elements;
        
        public int Count => _elements.Length;

        public ClassName(FixedList512Bytes<FixedString32Bytes> elements)
        {
            _elements = elements;
        }

        public ClassName(ClassName other)
        {
            _elements = other._elements;
        }

        public ClassName(string element)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>
            {
                element
            };
        }
        public ClassName(string element0, string element1)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>
            {
                element0,
                element1
            };
        }
        public ClassName(string element0, string element1, string element2)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>
            {
                element0,
                element1,
                element2
            };
        }
        public ClassName(string element0, string element1, string element2, string element3)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>
            {
                element0,
                element1,
                element2,
                element3
            };
        }
        public ClassName(string element0, string element1, string element2, string element3, string element4)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>
            {
                element0,
                element1,
                element2,
                element3,
                element4
            };
        }
        public ClassName(string element0, string element1, string element2, string element3, string element4, string element5)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>
            {
                element0,
                element1,
                element2,
                element3,
                element4,
                element5
            };
        }
        public ClassName(string element0, string element1, string element2, string element3, string element4, string element5, string element6)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>
            {
                element0,
                element1,
                element2,
                element3,
                element4,
                element5,
                element6
            };
        }
        public ClassName(string element0, string element1, string element2, string element3, string element4, string element5, string element6, string element7)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>
            {
                element0,
                element1,
                element2,
                element3,
                element4,
                element5,
                element6,
                element7
            };
        }
        public ClassName(string element0, string element1, string element2, string element3, string element4, string element5, string element6, string element7, string element8)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>
            {
                element0,
                element1,
                element2,
                element3,
                element4,
                element5,
                element6,
                element7,
                element8
            };
        }
        public ClassName(string element0, string element1, string element2, string element3, string element4, string element5, string element6, string element7, string element8, string element9)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>
            {
                element0,
                element1,
                element2,
                element3,
                element4,
                element5,
                element6,
                element7,
                element8,
                element9
            };
        }
        public ClassName(string element0, string element1, string element2, string element3, string element4, string element5, string element6, string element7, string element8, string element9, string element10)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>
            {
                element0,
                element1,
                element2,
                element3,
                element4,
                element5,
                element6,
                element7,
                element8,
                element9,
                element10
            };
        }
        public ClassName(string element0, string element1, string element2, string element3, string element4, string element5, string element6, string element7, string element8, string element9, string element10, string element11)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>
            {
                element0,
                element1,
                element2,
                element3,
                element4,
                element5,
                element6,
                element7,
                element8,
                element9,
                element10,
                element11
            };
        }
        public ClassName(string element0, string element1, string element2, string element3, string element4, string element5, string element6, string element7, string element8, string element9, string element10, string element11, string element12)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>
            {
                element0,
                element1,
                element2,
                element3,
                element4,
                element5,
                element6,
                element7,
                element8,
                element9,
                element10,
                element11,
                element12
            };
        }
        public ClassName(string element0, string element1, string element2, string element3, string element4, string element5, string element6, string element7, string element8, string element9, string element10, string element11, string element12, string element13)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>
            {
                element0,
                element1,
                element2,
                element3,
                element4,
                element5,
                element6,
                element7,
                element8,
                element9,
                element10,
                element11,
                element12,
                element13
            };
        }
        public ClassName(string element0, string element1, string element2, string element3, string element4, string element5, string element6, string element7, string element8, string element9, string element10, string element11, string element12, string element13, string element14)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>
            {
                element0,
                element1,
                element2,
                element3,
                element4,
                element5,
                element6,
                element7,
                element8,
                element9,
                element10,
                element11,
                element12,
                element13,
                element14
            };
        }
        public ClassName(string element0, string element1, string element2, string element3, string element4, string element5, string element6, string element7, string element8, string element9, string element10, string element11, string element12, string element13, string element14, string element15)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>
            {
                element0,
                element1,
                element2,
                element3,
                element4,
                element5,
                element6,
                element7,
                element8,
                element9,
                element10,
                element11,
                element12,
                element13,
                element14,
                element15
            };
        }

        public ClassName(params string[] elements)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>();
            foreach (var element in elements)
            {
                _elements.Add(element);
            }
        }
        public ClassName(params FixedString32Bytes[] elements)
        {
            _elements = new FixedList512Bytes<FixedString32Bytes>();
            foreach (var element in elements)
            {
                _elements.Add(element);
            }
        }

        public string this[int index] => _elements[index].Value;

        public void Add(FixedString32Bytes element) => _elements.Add(element);

        public static implicit operator ClassName(string element0) => new ClassName(element0);
        public static implicit operator ClassName((string e0, string e1) elements) => new ClassName(elements.e0, elements.e1);
        public static implicit operator ClassName((string e0, string e1, string e2) elements) => new ClassName(elements.e0, elements.e1, elements.e2);
        public static implicit operator ClassName((string e0, string e1, string e2, string e3) elements) => new ClassName(elements.e0, elements.e1, elements.e2, elements.e3);
        public static implicit operator ClassName((string e0, string e1, string e2, string e3, string e4) elements) => new ClassName(elements.e0, elements.e1, elements.e2, elements.e3, elements.e4);
        public static implicit operator ClassName((string e0, string e1, string e2, string e3, string e4, string e5) elements) => new ClassName(elements.e0, elements.e1, elements.e2, elements.e3, elements.e4, elements.e5);
        public static implicit operator ClassName((string e0, string e1, string e2, string e3, string e4, string e5, string e6) elements) => new ClassName(elements.e0, elements.e1, elements.e2, elements.e3, elements.e4, elements.e5, elements.e6);
        public static implicit operator ClassName((string e0, string e1, string e2, string e3, string e4, string e5, string e6, string e7) elements) => new ClassName(elements.e0, elements.e1, elements.e2, elements.e3, elements.e4, elements.e5, elements.e6, elements.e7);
        public static implicit operator ClassName((string e0, string e1, string e2, string e3, string e4, string e5, string e6, string e7, string e8) elements) => new ClassName(elements.e0, elements.e1, elements.e2, elements.e3, elements.e4, elements.e5, elements.e6, elements.e7, elements.e8);
        public static implicit operator ClassName((string e0, string e1, string e2, string e3, string e4, string e5, string e6, string e7, string e8, string e9) elements) => new ClassName(elements.e0, elements.e1, elements.e2, elements.e3, elements.e4, elements.e5, elements.e6, elements.e7, elements.e8, elements.e9);
        public static implicit operator ClassName((string e0, string e1, string e2, string e3, string e4, string e5, string e6, string e7, string e8, string e9, string e10) elements) => new ClassName(elements.e0, elements.e1, elements.e2, elements.e3, elements.e4, elements.e5, elements.e6, elements.e7, elements.e8, elements.e9, elements.e10);
        public static implicit operator ClassName((string e0, string e1, string e2, string e3, string e4, string e5, string e6, string e7, string e8, string e9, string e10, string e11) elements) => new ClassName(elements.e0, elements.e1, elements.e2, elements.e3, elements.e4, elements.e5, elements.e6, elements.e7, elements.e8, elements.e9, elements.e10, elements.e11);
        public static implicit operator ClassName((string e0, string e1, string e2, string e3, string e4, string e5, string e6, string e7, string e8, string e9, string e10, string e11, string e12) elements) => new ClassName(elements.e0, elements.e1, elements.e2, elements.e3, elements.e4, elements.e5, elements.e6, elements.e7, elements.e8, elements.e9, elements.e10, elements.e11, elements.e12);
        public static implicit operator ClassName((string e0, string e1, string e2, string e3, string e4, string e5, string e6, string e7, string e8, string e9, string e10, string e11, string e12, string e13) elements) => new ClassName(elements.e0, elements.e1, elements.e2, elements.e3, elements.e4, elements.e5, elements.e6, elements.e7, elements.e8, elements.e9, elements.e10, elements.e11, elements.e12, elements.e13);
        public static implicit operator ClassName((string e0, string e1, string e2, string e3, string e4, string e5, string e6, string e7, string e8, string e9, string e10, string e11, string e12, string e13, string e14) elements) => new ClassName(elements.e0, elements.e1, elements.e2, elements.e3, elements.e4, elements.e5, elements.e6, elements.e7, elements.e8, elements.e9, elements.e10, elements.e11, elements.e12, elements.e13, elements.e14);
        public static implicit operator ClassName((string e0, string e1, string e2, string e3, string e4, string e5, string e6, string e7, string e8, string e9, string e10, string e11, string e12, string e13, string e14, string e15) elements) => new ClassName(elements.e0, elements.e1, elements.e2, elements.e3, elements.e4, elements.e5, elements.e6, elements.e7, elements.e8, elements.e9, elements.e10, elements.e11, elements.e12, elements.e13, elements.e14, elements.e15);

        IEnumerator<FixedString32Bytes> IEnumerable<FixedString32Bytes>.GetEnumerator() => _elements.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _elements.GetEnumerator();

        public override string ToString()
        {
            var builder = new StringBuilder("(");
            for(int i = 0, n = _elements.Length, lastIndex = n - 1; i < n; i++)
            {
                var element = _elements[i];
                builder.Append(element);
                if (i < lastIndex)
                {
                    builder.Append(", ");
                }
            }
            builder.Append(")");

            return builder.ToString();
        }
        
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
            
            if (element is ICustomPicking customPicking)
            {
                customPicking.Manager.StyleSheetsPointerDetection = null;
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
            private readonly ClassName _className;

            private int _index;
            private string _current;

            public Enumerator(ClassName className)
            {
                _className = className;
                _index = 0;
                _current = default;
            }

            public string Current => _current;

            public bool MoveNext()
            {
                if (_index >= _className.Count)
                {
                    return false;
                }
                
                _current = _className[_index++];
                
                return true;
            }
        }
    }
    // public struct ClassName16 : IEnumerable<FixedString32Bytes>
    // {
    //     private FixedList512Bytes<FixedString32Bytes> _elements;
    //
    //     public void Add(FixedString32Bytes element) => _elements.Add(element);
    //
    //     IEnumerator<FixedString32Bytes> IEnumerable<FixedString32Bytes>.GetEnumerator() => _elements.GetEnumerator();
    //     IEnumerator IEnumerable.GetEnumerator() => _elements.GetEnumerator();
    //
    //     public static implicit operator ClassName(ClassName16 className) => new ClassName(className._elements);
    // }
    // public struct ClassName4 : IEnumerable<FixedString32Bytes>
    // {
    //     private FixedList128Bytes<FixedString32Bytes> _elements;
    //
    //     public void Add(FixedString32Bytes element) => _elements.Add(element);
    //
    //     IEnumerator<FixedString32Bytes> IEnumerable<FixedString32Bytes>.GetEnumerator() => _elements.GetEnumerator();
    //     IEnumerator IEnumerable.GetEnumerator() => _elements.GetEnumerator();
    //
    //     public static implicit operator ClassName(ClassName4 className) => new ClassName(className._elements);
    // }
}