using Unity.Collections;

namespace RishUI
{
    public struct References
    {
        private readonly bool _initialized;
        // private NativeArray<Children> _elements;
        private NativeList<Children> _elements; // TODO: Use NativeArray after migration
        public int Count => _initialized ? _elements.Length : 0;

        public References(FixedList4096Bytes<Children> elements)
        {
            _initialized = true;
            // _elements = elements.ToNativeArray(Allocator.Persistent);
            var nativeArray = elements.ToNativeArray(Allocator.Temp);
            _elements = new NativeList<Children>(Allocator.Persistent);
            _elements.AddRange(nativeArray);
            nativeArray.Dispose();
        }

        public References(Children element0) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0
        }) { }
        public References(Children element0, Children element1) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1
        }) { }
        public References(Children element0, Children element1, Children element2) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14, element15
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14, element15, element16
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14, element15, element16, element17
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14, element15, element16, element17, element18
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14, element15, element16, element17, element18, element19
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14, element15, element16, element17, element18, element19, element20
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14, element15, element16, element17, element18, element19, element20, element21
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14, element15, element16, element17, element18, element19, element20, element21, element22
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14, element15, element16, element17, element18, element19, element20, element21, element22, element23
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23, Children element24) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14, element15, element16, element17, element18, element19, element20, element21, element22, element23, element24
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23, Children element24, Children element25) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14, element15, element16, element17, element18, element19, element20, element21, element22, element23, element24, element25
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23, Children element24, Children element25, Children element26) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14, element15, element16, element17, element18, element19, element20, element21, element22, element23, element24, element25, element26
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23, Children element24, Children element25, Children element26, Children element27) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14, element15, element16, element17, element18, element19, element20, element21, element22, element23, element24, element25, element26, element27
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23, Children element24, Children element25, Children element26, Children element27, Children element28) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14, element15, element16, element17, element18, element19, element20, element21, element22, element23, element24, element25, element26, element27, element28
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23, Children element24, Children element25, Children element26, Children element27, Children element28, Children element29) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14, element15, element16, element17, element18, element19, element20, element21, element22, element23, element24, element25, element26, element27, element28, element29
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23, Children element24, Children element25, Children element26, Children element27, Children element28, Children element29, Children element30) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14, element15, element16, element17, element18, element19, element20, element21, element22, element23, element24, element25, element26, element27, element28, element29, element30
        }) { }
        public References(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23, Children element24, Children element25, Children element26, Children element27, Children element28, Children element29, Children element30, Children element31) : this(new NativeList<Children>(Allocator.Persistent)
        {
            element0, element1, element2, element3, element4, element5, element6, element7, element8, element9, element10, element11, element12, element13, element14, element15, element16, element17, element18, element19, element20, element21, element22, element23, element24, element25, element26, element27, element28, element29, element30, element31
        }) { }

        private References(NativeList<Children> elements)
        {
            _initialized = true;
            _elements = elements;
        }

        internal void Dispose()
        {
            if (!_initialized)
            {
                return;
            }
            
            _elements.Dispose();
        }

        internal bool IsValid()
        {
            if (!_initialized)
            {
                return false;
            }

            var valid = false;
            foreach (var element in _elements)
            {
                if (!element.Valid)
                {
                    continue;
                }

                valid = true;
                break;
            }

            return valid;
        }

        public static References Empty => new References(new NativeList<Children>(32, Allocator.Persistent));

        public Children this[int index] => _elements[index];

        public void Add(Children element) => _elements.Add(element);
        public void Add(Children element0, Children element1)
        {
            _elements.Add(element0);
            _elements.Add(element1);
        }
        public void Add(Children element0, Children element1, Children element2)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
            _elements.Add(element15);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
            _elements.Add(element15);
            _elements.Add(element16);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
            _elements.Add(element15);
            _elements.Add(element16);
            _elements.Add(element17);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
            _elements.Add(element15);
            _elements.Add(element16);
            _elements.Add(element17);
            _elements.Add(element18);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
            _elements.Add(element15);
            _elements.Add(element16);
            _elements.Add(element17);
            _elements.Add(element18);
            _elements.Add(element19);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
            _elements.Add(element15);
            _elements.Add(element16);
            _elements.Add(element17);
            _elements.Add(element18);
            _elements.Add(element19);
            _elements.Add(element20);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
            _elements.Add(element15);
            _elements.Add(element16);
            _elements.Add(element17);
            _elements.Add(element18);
            _elements.Add(element19);
            _elements.Add(element20);
            _elements.Add(element21);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
            _elements.Add(element15);
            _elements.Add(element16);
            _elements.Add(element17);
            _elements.Add(element18);
            _elements.Add(element19);
            _elements.Add(element20);
            _elements.Add(element21);
            _elements.Add(element22);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
            _elements.Add(element15);
            _elements.Add(element16);
            _elements.Add(element17);
            _elements.Add(element18);
            _elements.Add(element19);
            _elements.Add(element20);
            _elements.Add(element21);
            _elements.Add(element22);
            _elements.Add(element23);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23, Children element24)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
            _elements.Add(element15);
            _elements.Add(element16);
            _elements.Add(element17);
            _elements.Add(element18);
            _elements.Add(element19);
            _elements.Add(element20);
            _elements.Add(element21);
            _elements.Add(element22);
            _elements.Add(element23);
            _elements.Add(element24);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23, Children element24, Children element25)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
            _elements.Add(element15);
            _elements.Add(element16);
            _elements.Add(element17);
            _elements.Add(element18);
            _elements.Add(element19);
            _elements.Add(element20);
            _elements.Add(element21);
            _elements.Add(element22);
            _elements.Add(element23);
            _elements.Add(element24);
            _elements.Add(element25);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23, Children element24, Children element25, Children element26)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
            _elements.Add(element15);
            _elements.Add(element16);
            _elements.Add(element17);
            _elements.Add(element18);
            _elements.Add(element19);
            _elements.Add(element20);
            _elements.Add(element21);
            _elements.Add(element22);
            _elements.Add(element23);
            _elements.Add(element24);
            _elements.Add(element25);
            _elements.Add(element26);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23, Children element24, Children element25, Children element26, Children element27)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
            _elements.Add(element15);
            _elements.Add(element16);
            _elements.Add(element17);
            _elements.Add(element18);
            _elements.Add(element19);
            _elements.Add(element20);
            _elements.Add(element21);
            _elements.Add(element22);
            _elements.Add(element23);
            _elements.Add(element24);
            _elements.Add(element25);
            _elements.Add(element26);
            _elements.Add(element27);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23, Children element24, Children element25, Children element26, Children element27, Children element28)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
            _elements.Add(element15);
            _elements.Add(element16);
            _elements.Add(element17);
            _elements.Add(element18);
            _elements.Add(element19);
            _elements.Add(element20);
            _elements.Add(element21);
            _elements.Add(element22);
            _elements.Add(element23);
            _elements.Add(element24);
            _elements.Add(element25);
            _elements.Add(element26);
            _elements.Add(element27);
            _elements.Add(element28);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23, Children element24, Children element25, Children element26, Children element27, Children element28, Children element29)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
            _elements.Add(element15);
            _elements.Add(element16);
            _elements.Add(element17);
            _elements.Add(element18);
            _elements.Add(element19);
            _elements.Add(element20);
            _elements.Add(element21);
            _elements.Add(element22);
            _elements.Add(element23);
            _elements.Add(element24);
            _elements.Add(element25);
            _elements.Add(element26);
            _elements.Add(element27);
            _elements.Add(element28);
            _elements.Add(element29);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23, Children element24, Children element25, Children element26, Children element27, Children element28, Children element29, Children element30)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
            _elements.Add(element15);
            _elements.Add(element16);
            _elements.Add(element17);
            _elements.Add(element18);
            _elements.Add(element19);
            _elements.Add(element20);
            _elements.Add(element21);
            _elements.Add(element22);
            _elements.Add(element23);
            _elements.Add(element24);
            _elements.Add(element25);
            _elements.Add(element26);
            _elements.Add(element27);
            _elements.Add(element28);
            _elements.Add(element29);
            _elements.Add(element30);
        }
        public void Add(Children element0, Children element1, Children element2, Children element3, Children element4, Children element5, Children element6, Children element7, Children element8, Children element9, Children element10, Children element11, Children element12, Children element13, Children element14, Children element15, Children element16, Children element17, Children element18, Children element19, Children element20, Children element21, Children element22, Children element23, Children element24, Children element25, Children element26, Children element27, Children element28, Children element29, Children element30, Children element31)
        {
            _elements.Add(element0);
            _elements.Add(element1);
            _elements.Add(element2);
            _elements.Add(element3);
            _elements.Add(element4);
            _elements.Add(element5);
            _elements.Add(element6);
            _elements.Add(element7);
            _elements.Add(element8);
            _elements.Add(element9);
            _elements.Add(element10);
            _elements.Add(element11);
            _elements.Add(element12);
            _elements.Add(element13);
            _elements.Add(element14);
            _elements.Add(element15);
            _elements.Add(element16);
            _elements.Add(element17);
            _elements.Add(element18);
            _elements.Add(element19);
            _elements.Add(element20);
            _elements.Add(element21);
            _elements.Add(element22);
            _elements.Add(element23);
            _elements.Add(element24);
            _elements.Add(element25);
            _elements.Add(element26);
            _elements.Add(element27);
            _elements.Add(element28);
            _elements.Add(element29);
            _elements.Add(element30);
            _elements.Add(element31);
        }
        public void Add(FixedList32Bytes<Children> elements)
        {
            foreach (var element in elements)
            {
                _elements.Add(element);
            }
        }
        public void Add(FixedList64Bytes<Children> elements)
        {
            foreach (var element in elements)
            {
                _elements.Add(element);
            }
        }
        public void Add(FixedList128Bytes<Children> elements)
        {
            foreach (var element in elements)
            {
                _elements.Add(element);
            }
        }
        public void Add(FixedList512Bytes<Children> elements)
        {
            foreach (var element in elements)
            {
                _elements.Add(element);
            }
        }
        public void Add(FixedList4096Bytes<Children> elements)
        {
            foreach (var element in elements)
            {
                _elements.Add(element);
            }
        }
        
        public static implicit operator References(FixedList4096Bytes<Children> elements) => new(elements);
        
        public static implicit operator References(Children element0)
        {
            return new References(element0);
        }

        public static implicit operator References(Element element0) => (Children)element0;

        public static implicit operator References((Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2);
        }

        public static implicit operator References((Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3);
        }

        public static implicit operator References((Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4);
        }

        public static implicit operator References((Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5);
        }

        public static implicit operator References((Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6);
        }

        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7);
        }

        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8);
        }

        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19, elements.Item20);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19, elements.Item20, elements.Item21);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19, elements.Item20, elements.Item21, elements.Item22);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19, elements.Item20, elements.Item21, elements.Item22, elements.Item23);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19, elements.Item20, elements.Item21, elements.Item22, elements.Item23, elements.Item24);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19, elements.Item20, elements.Item21, elements.Item22, elements.Item23, elements.Item24, elements.Item25);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19, elements.Item20, elements.Item21, elements.Item22, elements.Item23, elements.Item24, elements.Item25, elements.Item26);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19, elements.Item20, elements.Item21, elements.Item22, elements.Item23, elements.Item24, elements.Item25, elements.Item26, elements.Item27);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19, elements.Item20, elements.Item21, elements.Item22, elements.Item23, elements.Item24, elements.Item25, elements.Item26, elements.Item27, elements.Item28);
        }
        
        public static implicit operator References((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) elements)
        {
            return new References(elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19, elements.Item20, elements.Item21, elements.Item22, elements.Item23, elements.Item24, elements.Item25, elements.Item26, elements.Item27, elements.Item28, elements.Item29);
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator
        {
            private readonly References _list;

            private int _index;
            private Children _current;

            public Enumerator(References list)
            {
                _list = list;
                _index = 0;
                _current = default;
            }

            public Children Current => _current;

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