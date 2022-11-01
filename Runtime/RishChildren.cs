using System.Collections.Generic;
using Unity.Collections;

namespace RishUI
{
    public static partial class Rish
    {
        public static Children Children() => RishUI.Children.Empty;
        public static Children Children(Element child0)
        {
            var array = new NativeArray<Element>(1, Allocator.Persistent);
            array[0] = child0;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1)
        {
            var array = new NativeArray<Element>(2, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2)
        {
            var array = new NativeArray<Element>(3, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3)
        {
            var array = new NativeArray<Element>(4, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4)
        {
            var array = new NativeArray<Element>(5, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4, Element child5)
        {
            var array = new NativeArray<Element>(6, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;
            array[5] = child5;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4, Element child5, Element child6)
        {
            var array = new NativeArray<Element>(7, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;
            array[5] = child5;
            array[6] = child6;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4, Element child5, Element child6, Element child7)
        {
            var array = new NativeArray<Element>(8, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;
            array[5] = child5;
            array[6] = child6;
            array[7] = child7;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4, Element child5, Element child6, Element child7, Element child8)
        {
            var array = new NativeArray<Element>(9, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;
            array[5] = child5;
            array[6] = child6;
            array[7] = child7;
            array[8] = child8;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4, Element child5, Element child6, Element child7, Element child8, Element child9)
        {
            var array = new NativeArray<Element>(10, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;
            array[5] = child5;
            array[6] = child6;
            array[7] = child7;
            array[8] = child8;
            array[9] = child9;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4, Element child5, Element child6, Element child7, Element child8, Element child9, Element child10)
        {
            var array = new NativeArray<Element>(11, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;
            array[5] = child5;
            array[6] = child6;
            array[7] = child7;
            array[8] = child8;
            array[9] = child9;
            array[10] = child10;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4, Element child5, Element child6, Element child7, Element child8, Element child9, Element child10, Element child11)
        {
            var array = new NativeArray<Element>(12, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;
            array[5] = child5;
            array[6] = child6;
            array[7] = child7;
            array[8] = child8;
            array[9] = child9;
            array[10] = child10;
            array[11] = child11;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4, Element child5, Element child6, Element child7, Element child8, Element child9, Element child10, Element child11, Element child12)
        {
            var array = new NativeArray<Element>(13, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;
            array[5] = child5;
            array[6] = child6;
            array[7] = child7;
            array[8] = child8;
            array[9] = child9;
            array[10] = child10;
            array[11] = child11;
            array[12] = child12;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4, Element child5, Element child6, Element child7, Element child8, Element child9, Element child10, Element child11, Element child12, Element child13)
        {
            var array = new NativeArray<Element>(14, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;
            array[5] = child5;
            array[6] = child6;
            array[7] = child7;
            array[8] = child8;
            array[9] = child9;
            array[10] = child10;
            array[11] = child11;
            array[12] = child12;
            array[13] = child13;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4, Element child5, Element child6, Element child7, Element child8, Element child9, Element child10, Element child11, Element child12, Element child13, Element child14)
        {
            var array = new NativeArray<Element>(15, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;
            array[5] = child5;
            array[6] = child6;
            array[7] = child7;
            array[8] = child8;
            array[9] = child9;
            array[10] = child10;
            array[11] = child11;
            array[12] = child12;
            array[13] = child13;
            array[14] = child14;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4, Element child5, Element child6, Element child7, Element child8, Element child9, Element child10, Element child11, Element child12, Element child13, Element child14, Element child15)
        {
            var array = new NativeArray<Element>(16, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;
            array[5] = child5;
            array[6] = child6;
            array[7] = child7;
            array[8] = child8;
            array[9] = child9;
            array[10] = child10;
            array[11] = child11;
            array[12] = child12;
            array[13] = child13;
            array[14] = child14;
            array[15] = child15;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4, Element child5, Element child6, Element child7, Element child8, Element child9, Element child10, Element child11, Element child12, Element child13, Element child14, Element child15, Element child16)
        {
            var array = new NativeArray<Element>(17, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;
            array[5] = child5;
            array[6] = child6;
            array[7] = child7;
            array[8] = child8;
            array[9] = child9;
            array[10] = child10;
            array[11] = child11;
            array[12] = child12;
            array[13] = child13;
            array[14] = child14;
            array[15] = child15;
            array[16] = child16;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4, Element child5, Element child6, Element child7, Element child8, Element child9, Element child10, Element child11, Element child12, Element child13, Element child14, Element child15, Element child16, Element child17)
        {
            var array = new NativeArray<Element>(18, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;
            array[5] = child5;
            array[6] = child6;
            array[7] = child7;
            array[8] = child8;
            array[9] = child9;
            array[10] = child10;
            array[11] = child11;
            array[12] = child12;
            array[13] = child13;
            array[14] = child14;
            array[15] = child15;
            array[16] = child16;
            array[17] = child17;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4, Element child5, Element child6, Element child7, Element child8, Element child9, Element child10, Element child11, Element child12, Element child13, Element child14, Element child15, Element child16, Element child17, Element child18)
        {
            var array = new NativeArray<Element>(19, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;
            array[5] = child5;
            array[6] = child6;
            array[7] = child7;
            array[8] = child8;
            array[9] = child9;
            array[10] = child10;
            array[11] = child11;
            array[12] = child12;
            array[13] = child13;
            array[14] = child14;
            array[15] = child15;
            array[16] = child16;
            array[17] = child17;
            array[18] = child18;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4, Element child5, Element child6, Element child7, Element child8, Element child9, Element child10, Element child11, Element child12, Element child13, Element child14, Element child15, Element child16, Element child17, Element child18, Element child19)
        {
            var array = new NativeArray<Element>(20, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;
            array[5] = child5;
            array[6] = child6;
            array[7] = child7;
            array[8] = child8;
            array[9] = child9;
            array[10] = child10;
            array[11] = child11;
            array[12] = child12;
            array[13] = child13;
            array[14] = child14;
            array[15] = child15;
            array[16] = child16;
            array[17] = child17;
            array[18] = child18;
            array[19] = child19;
            
            return CreateChildren(array);
        }
        public static Children Children(params Element[] children)
        {
            var length = children?.Length ?? 0;
            if (length <= 0)
            {
                return Children();
            }
            var array = new NativeArray<Element>(length, Allocator.Persistent);
            for (var i = 0; i < length; i++)
            {
                array[i] = children[i];
            }
            
            return CreateChildren(array);
        }
        public static Children Children(List<Element> children)
        {
            var length = children?.Count ?? 0;
            if (length <= 0)
            {
                return Children();
            }
            var array = new NativeArray<Element>(length, Allocator.Persistent);
            for (var i = 0; i < length; i++)
            {
                array[i] = children[i];
            }
            
            return CreateChildren(array);
        }
        internal static Children CopyChildren(Children children)
        {
            var length = children.Valid ? children.Count : 0;
            if (length <= 0)
            {
                return Children();
            }
            var array = new NativeArray<Element>(length, Allocator.Persistent);
            for (var i = 0; i < length; i++)
            {
                array[i] = children[i].Copy();
            }
            
            return CreateChildren(array);
        }
    } 
}