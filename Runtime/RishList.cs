using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace RishUI
{
    public readonly struct RishList<T> where T : struct
    {
        private readonly int _childCount;
        private readonly T _child0;
        private readonly T _child1;
        private readonly T _child2;
        private readonly T _child3;
        private readonly T _child4;
        private readonly T _child5;
        private readonly T _child6;
        private readonly T _child7;
        private readonly T _child8;
        private readonly T _child9;
        private readonly T _child10;
        private readonly T _child11;
        private readonly T _child12;
        private readonly T _child13;
        private readonly T _child14;
        private readonly T _child15;
        private readonly T _child16;
        private readonly T _child17;
        private readonly T _child18;
        private readonly T _child19;
        private readonly IList<T> _children;
        
        private static bool IsUnmanaged { get; } = UnsafeUtility.IsUnmanaged<T>();

        private RishList(T child)
        {
            _childCount = 1;
            _child0 = child;
            _child1 = default;
            _child2 = default;
            _child3 = default;
            _child4 = default;
            _child5 = default;
            _child6 = default;
            _child7 = default;
            _child8 = default;
            _child9 = default;
            _child10 = default;
            _child11 = default;
            _child12 = default;
            _child13 = default;
            _child14 = default;
            _child15 = default;
            _child16 = default;
            _child17 = default;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1)
        {
            _childCount = 2;
            _child0 = child0;
            _child1 = child1;
            _child2 = default;
            _child3 = default;
            _child4 = default;
            _child5 = default;
            _child6 = default;
            _child7 = default;
            _child8 = default;
            _child9 = default;
            _child10 = default;
            _child11 = default;
            _child12 = default;
            _child13 = default;
            _child14 = default;
            _child15 = default;
            _child16 = default;
            _child17 = default;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2)
        {
            _childCount = 3;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = default;
            _child4 = default;
            _child5 = default;
            _child6 = default;
            _child7 = default;
            _child8 = default;
            _child9 = default;
            _child10 = default;
            _child11 = default;
            _child12 = default;
            _child13 = default;
            _child14 = default;
            _child15 = default;
            _child16 = default;
            _child17 = default;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3)
        {
            _childCount = 4;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = default;
            _child5 = default;
            _child6 = default;
            _child7 = default;
            _child8 = default;
            _child9 = default;
            _child10 = default;
            _child11 = default;
            _child12 = default;
            _child13 = default;
            _child14 = default;
            _child15 = default;
            _child16 = default;
            _child17 = default;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3, T child4)
        {
            _childCount = 5;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = child4;
            _child5 = default;
            _child6 = default;
            _child7 = default;
            _child8 = default;
            _child9 = default;
            _child10 = default;
            _child11 = default;
            _child12 = default;
            _child13 = default;
            _child14 = default;
            _child15 = default;
            _child16 = default;
            _child17 = default;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3, T child4, T child5)
        {
            _childCount = 6;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = child4;
            _child5 = child5;
            _child6 = default;
            _child7 = default;
            _child8 = default;
            _child9 = default;
            _child10 = default;
            _child11 = default;
            _child12 = default;
            _child13 = default;
            _child14 = default;
            _child15 = default;
            _child16 = default;
            _child17 = default;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3, T child4, T child5, T child6)
        {
            _childCount = 7;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = child4;
            _child5 = child5;
            _child6 = child6;
            _child7 = default;
            _child8 = default;
            _child9 = default;
            _child10 = default;
            _child11 = default;
            _child12 = default;
            _child13 = default;
            _child14 = default;
            _child15 = default;
            _child16 = default;
            _child17 = default;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3, T child4, T child5, T child6, T child7)
        {
            _childCount = 8;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = child4;
            _child5 = child5;
            _child6 = child6;
            _child7 = child7;
            _child8 = default;
            _child9 = default;
            _child10 = default;
            _child11 = default;
            _child12 = default;
            _child13 = default;
            _child14 = default;
            _child15 = default;
            _child16 = default;
            _child17 = default;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3, T child4, T child5, T child6, T child7, T child8)
        {
            _childCount = 9;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = child4;
            _child5 = child5;
            _child6 = child6;
            _child7 = child7;
            _child8 = child8;
            _child9 = default;
            _child10 = default;
            _child11 = default;
            _child12 = default;
            _child13 = default;
            _child14 = default;
            _child15 = default;
            _child16 = default;
            _child17 = default;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3, T child4, T child5, T child6, T child7, T child8, T child9)
        {
            _childCount = 10;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = child4;
            _child5 = child5;
            _child6 = child6;
            _child7 = child7;
            _child8 = child8;
            _child9 = child9;
            _child10 = default;
            _child11 = default;
            _child12 = default;
            _child13 = default;
            _child14 = default;
            _child15 = default;
            _child16 = default;
            _child17 = default;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3, T child4, T child5, T child6, T child7, T child8, T child9, T child10)
        {
            _childCount = 11;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = child4;
            _child5 = child5;
            _child6 = child6;
            _child7 = child7;
            _child8 = child8;
            _child9 = child9;
            _child10 = child10;
            _child11 = default;
            _child12 = default;
            _child13 = default;
            _child14 = default;
            _child15 = default;
            _child16 = default;
            _child17 = default;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3, T child4, T child5, T child6, T child7, T child8, T child9, T child10, T child11)
        {
            _childCount = 12;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = child4;
            _child5 = child5;
            _child6 = child6;
            _child7 = child7;
            _child8 = child8;
            _child9 = child9;
            _child10 = child10;
            _child11 = child11;
            _child12 = default;
            _child13 = default;
            _child14 = default;
            _child15 = default;
            _child16 = default;
            _child17 = default;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3, T child4, T child5, T child6, T child7, T child8, T child9, T child10, T child11, T child12)
        {
            _childCount = 13;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = child4;
            _child5 = child5;
            _child6 = child6;
            _child7 = child7;
            _child8 = child8;
            _child9 = child9;
            _child10 = child10;
            _child11 = child11;
            _child12 = child12;
            _child13 = default;
            _child14 = default;
            _child15 = default;
            _child16 = default;
            _child17 = default;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3, T child4, T child5, T child6, T child7, T child8, T child9, T child10, T child11, T child12, T child13)
        {
            _childCount = 14;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = child4;
            _child5 = child5;
            _child6 = child6;
            _child7 = child7;
            _child8 = child8;
            _child9 = child9;
            _child10 = child10;
            _child11 = child11;
            _child12 = child12;
            _child13 = child13;
            _child14 = default;
            _child15 = default;
            _child16 = default;
            _child17 = default;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3, T child4, T child5, T child6, T child7, T child8, T child9, T child10, T child11, T child12, T child13, T child14)
        {
            _childCount = 15;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = child4;
            _child5 = child5;
            _child6 = child6;
            _child7 = child7;
            _child8 = child8;
            _child9 = child9;
            _child10 = child10;
            _child11 = child11;
            _child12 = child12;
            _child13 = child13;
            _child14 = child14;
            _child15 = default;
            _child16 = default;
            _child17 = default;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3, T child4, T child5, T child6, T child7, T child8, T child9, T child10, T child11, T child12, T child13, T child14, T child15)
        {
            _childCount = 15;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = child4;
            _child5 = child5;
            _child6 = child6;
            _child7 = child7;
            _child8 = child8;
            _child9 = child9;
            _child10 = child10;
            _child11 = child11;
            _child12 = child12;
            _child13 = child13;
            _child14 = child14;
            _child15 = child15;
            _child16 = default;
            _child17 = default;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3, T child4, T child5, T child6, T child7, T child8, T child9, T child10, T child11, T child12, T child13, T child14, T child15, T child16)
        {
            _childCount = 15;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = child4;
            _child5 = child5;
            _child6 = child6;
            _child7 = child7;
            _child8 = child8;
            _child9 = child9;
            _child10 = child10;
            _child11 = child11;
            _child12 = child12;
            _child13 = child13;
            _child14 = child14;
            _child15 = child15;
            _child16 = child16;
            _child17 = default;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3, T child4, T child5, T child6, T child7, T child8, T child9, T child10, T child11, T child12, T child13, T child14, T child15, T child16, T child17)
        {
            _childCount = 15;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = child4;
            _child5 = child5;
            _child6 = child6;
            _child7 = child7;
            _child8 = child8;
            _child9 = child9;
            _child10 = child10;
            _child11 = child11;
            _child12 = child12;
            _child13 = child13;
            _child14 = child14;
            _child15 = child15;
            _child16 = child16;
            _child17 = child17;
            _child18 = default;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3, T child4, T child5, T child6, T child7, T child8, T child9, T child10, T child11, T child12, T child13, T child14, T child15, T child16, T child17, T child18)
        {
            _childCount = 15;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = child4;
            _child5 = child5;
            _child6 = child6;
            _child7 = child7;
            _child8 = child8;
            _child9 = child9;
            _child10 = child10;
            _child11 = child11;
            _child12 = child12;
            _child13 = child13;
            _child14 = child14;
            _child15 = child15;
            _child16 = child16;
            _child17 = child17;
            _child18 = child18;
            _child19 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3, T child4, T child5, T child6, T child7, T child8, T child9, T child10, T child11, T child12, T child13, T child14, T child15, T child16, T child17, T child18, T child19)
        {
            _childCount = 15;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = child4;
            _child5 = child5;
            _child6 = child6;
            _child7 = child7;
            _child8 = child8;
            _child9 = child9;
            _child10 = child10;
            _child11 = child11;
            _child12 = child12;
            _child13 = child13;
            _child14 = child14;
            _child15 = child15;
            _child16 = child16;
            _child17 = child17;
            _child18 = child18;
            _child19 = child19;
            _children = null;
        }

        private RishList(IList<T> children)
        {
            if (children == null)
            {
                _childCount = 0;
                _child0 = default;
                _child1 = default;
                _child2 = default;
                _child3 = default;
                _child4 = default;
                _child5 = default;
                _child6 = default;
                _child7 = default;
                _child8 = default;
                _child9 = default;
                _child10 = default;
                _child11 = default;
                _child12 = default;
                _child13 = default;
                _child14 = default;
                _child15 = default;
                _child16 = default;
                _child17 = default;
                _child18 = default;
                _child19 = default;

                _children = default;

                return;
            }
            
            var count = children.Count;
            if (count > 20)
            {
                _childCount = 0;
                _child0 = default;
                _child1 = default;
                _child2 = default;
                _child3 = default;
                _child4 = default;
                _child5 = default;
                _child6 = default;
                _child7 = default;
                _child8 = default;
                _child9 = default;
                _child10 = default;
                _child11 = default;
                _child12 = default;
                _child13 = default;
                _child14 = default;
                _child15 = default;
                _child16 = default;
                _child17 = default;
                _child18 = default;
                _child19 = default;

                _children = children;
            }
            else
            {
                _childCount = count;
                _child0 = count >= 1 ? children[0] : default;
                _child1 = count >= 2 ? children[1] : default;
                _child2 = count >= 3 ? children[2] : default;
                _child3 = count >= 4 ? children[3] : default;
                _child4 = count >= 5 ? children[4] : default;
                _child5 = count >= 6 ? children[5] : default;
                _child6 = count >= 7 ? children[6] : default;
                _child7 = count >= 8 ? children[7] : default;
                _child8 = count >= 9 ? children[8] : default;
                _child9 = count >= 10 ? children[9] : default;
                _child10 = count >= 11 ? children[10] : default;
                _child11 = count >= 12 ? children[11] : default;
                _child12 = count >= 13 ? children[12] : default;
                _child13 = count >= 14 ? children[13] : default;
                _child14 = count >= 15 ? children[14] : default;
                _child15 = count >= 16 ? children[15] : default;
                _child16 = count >= 17 ? children[16] : default;
                _child17 = count >= 18 ? children[17] : default;
                _child18 = count >= 19 ? children[18] : default;
                _child19 = count >= 20 ? children[19] : default;
            
                _children = default;
            }
        }

        private bool Collection => _children is { Count: > 0 };

        public int Count => Collection ? _children.Count : _childCount;

        public T First => this[0];
        public T Last => this[Count - 1];

        public T this[int index]
        {
            get
            {
                if (Collection) return _children[index];

                switch (index)
                {
                    case 0:
                        return _child0;
                    case 1:
                        return _child1;
                    case 2:
                        return _child2;
                    case 3:
                        return _child3;
                    case 4:
                        return _child4;
                    case 5:
                        return _child5;
                    case 6:
                        return _child6;
                    case 7:
                        return _child7;
                    case 8:
                        return _child8;
                    case 9:
                        return _child9;
                    case 10:
                        return _child10;
                    case 11:
                        return _child11;
                    case 12:
                        return _child12;
                    case 13:
                        return _child13;
                    case 14:
                        return _child14;
                    case 15:
                        return _child15;
                    case 16:
                        return _child16;
                    case 17:
                        return _child17;
                    case 18:
                        return _child18;
                    case 19:
                        return _child19;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public bool Contains(T item)
        {
            for (int i = 0, n = Count; i < n; i++)
            {
                if (RishUtils.Compare<T>(item, this[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        [Comparer]
        public static bool Equals(RishList<T> a, RishList<T> b) => a.Equals(b);
        
        public bool Equals(RishList<T> other)
        {
            var count = Count;
            if (count != other.Count)
            {
                return false;
            }

            if (Collection && _children == other._children)
            {
                return false;
            }

            if (count <= 0)
            {
                return true;
            }

            for (var i = Count - 1; i >= 0; i--)
            {
                if (!RishUtils.Compare<T>(this[i], other[i]))
                {
                    return false;
                }
            }

            return true;
        }
        
        public override string ToString()
        {
            return Count switch
            {
                0 => "()",
                1 => $"({_child0})",
                2 => $"({_child0}, {_child1})",
                3 => $"({_child0}, {_child1}, {_child2})",
                4 => $"({_child0}, {_child1}, {_child2}, {_child3})",
                5 => $"({_child0}, {_child1}, {_child2}, {_child3}, {_child4})",
                6 => $"({_child0}, {_child1}, {_child2}, {_child3}, {_child4}, {_child5})",
                7 => $"({_child0}, {_child1}, {_child2}, {_child3}, {_child4}, {_child5}, {_child6})",
                8 => $"({_child0}, {_child1}, {_child2}, {_child3}, {_child4}, {_child5}, {_child6}, {_child7})",
                9 => $"({_child0}, {_child1}, {_child2}, {_child3}, {_child4}, {_child5}, {_child6}, {_child7}, {_child8})",
                10 => $"({_child0}, {_child1}, {_child2}, {_child3}, {_child4}, {_child5}, {_child6}, {_child7}, {_child8}, {_child9})",
                11 => $"({_child0}, {_child1}, {_child2}, {_child3}, {_child4}, {_child5}, {_child6}, {_child7}, {_child8}, {_child9}, {_child10})",
                12 => $"({_child0}, {_child1}, {_child2}, {_child3}, {_child4}, {_child5}, {_child6}, {_child7}, {_child8}, {_child9}, {_child10}, {_child11})",
                13 => $"({_child0}, {_child1}, {_child2}, {_child3}, {_child4}, {_child5}, {_child6}, {_child7}, {_child8}, {_child9}, {_child10}, {_child11}, {_child12})",
                14 => $"({_child0}, {_child1}, {_child2}, {_child3}, {_child4}, {_child5}, {_child6}, {_child7}, {_child8}, {_child9}, {_child10}, {_child11}, {_child12}, {_child13})",
                15 => $"({_child0}, {_child1}, {_child2}, {_child3}, {_child4}, {_child5}, {_child6}, {_child7}, {_child8}, {_child9}, {_child10}, {_child11}, {_child12}, {_child13}, {_child14})",
                16 => $"({_child0}, {_child1}, {_child2}, {_child3}, {_child4}, {_child5}, {_child6}, {_child7}, {_child8}, {_child9}, {_child10}, {_child11}, {_child12}, {_child13}, {_child14}, {_child15})",
                17 => $"({_child0}, {_child1}, {_child2}, {_child3}, {_child4}, {_child5}, {_child6}, {_child7}, {_child8}, {_child9}, {_child10}, {_child11}, {_child12}, {_child13}, {_child14}, {_child15}, {_child16})",
                18 => $"({_child0}, {_child1}, {_child2}, {_child3}, {_child4}, {_child5}, {_child6}, {_child7}, {_child8}, {_child9}, {_child10}, {_child11}, {_child12}, {_child13}, {_child14}, {_child15}, {_child16}, {_child17})",
                19 => $"({_child0}, {_child1}, {_child2}, {_child3}, {_child4}, {_child5}, {_child6}, {_child7}, {_child8}, {_child9}, {_child10}, {_child11}, {_child12}, {_child13}, {_child14}, {_child15}, {_child16}, {_child17}, {_child18})",
                20 => $"({_child0}, {_child1}, {_child2}, {_child3}, {_child4}, {_child5}, {_child6}, {_child7}, {_child8}, {_child9}, {_child10}, {_child11}, {_child12}, {_child13}, {_child14}, {_child15}, {_child16}, {_child17}, {_child18}, {_child19})",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public T[] ToArray()
        {
            var array = new T[Count];
            for (int i = 0, n = Count; i < n; i++)
            {
                array[i] = this[i];
            }

            return array;
        }

        public List<T> ToList()
        {
            var list = new List<T>(Count);
            for (int i = 0, n = Count; i < n; i++)
            {
                list.Add(this[i]);
            }

            return list;
        }

        public References GetReferences()
        {
            var references = new FixedList4096Bytes<Children>();
            for (int i = 0, n = Count; i < n; i++)
            {
                var element = this[i];
                if (element is not IReferencesHolder referencesHolder)
                {
                    return default;
                }

                var elementReferences = referencesHolder.GetReferences();
                foreach (var reference in elementReferences)
                {
                    references.Add(reference);
                }
            }
            
            return references;
        }

        public static implicit operator RishList<T>(T element)
        {
            return new RishList<T>(element);
        }

        public static implicit operator RishList<T>(T[] children)
        {
            return new RishList<T>(children);
        }

        public static implicit operator RishList<T>(List<T> children)
        {
            return new RishList<T>(children);
        }

        public static implicit operator RishList<T>((T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2);
        }

        public static implicit operator RishList<T>((T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3);
        }

        public static implicit operator RishList<T>((T, T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4);
        }

        public static implicit operator RishList<T>((T, T, T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5);
        }

        public static implicit operator RishList<T>((T, T, T, T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6);
        }

        public static implicit operator RishList<T>((T, T, T, T, T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7);
        }

        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8);
        }

        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9);
        }

        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10);
        }

        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item11);
        }

        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item12);
        }

        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item12, children.Item13);
        }

        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item12, children.Item13, children.Item14);
        }

        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item12, children.Item13, children.Item14, children.Item15);
        }

        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item12, children.Item13, children.Item14, children.Item15, children.Item16);
        }

        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item12, children.Item13, children.Item14, children.Item15, children.Item16, children.Item17);
        }

        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item12, children.Item13, children.Item14, children.Item15, children.Item16, children.Item17, children.Item18);
        }

        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item12, children.Item13, children.Item14, children.Item15, children.Item16, children.Item17, children.Item18, children.Item19);
        }

        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) children)
        {
            return new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item12, children.Item13, children.Item14, children.Item15, children.Item16, children.Item17, children.Item18, children.Item19, children.Item20);
        }

        public struct Enumerator
        {
            private readonly RishList<T> _list;

            private int _index;
            private T _current;

            public Enumerator(RishList<T> list)
            {
                _list = list;
                _index = 0;
                _current = default;
            }

            public T Current => _current;

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