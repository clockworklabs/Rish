using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public readonly struct ClassName
    {
        private readonly int _count;
        public int Count => _count;
        
        private readonly FixedString128Bytes _element0;
        private readonly FixedString128Bytes _element1;
        private readonly FixedString128Bytes _element2;
        private readonly FixedString128Bytes _element3;
        private readonly FixedString128Bytes _element4;
        private readonly FixedString128Bytes _element5;
        private readonly FixedString128Bytes _element6;
        private readonly FixedString128Bytes _element7;
        private readonly FixedString128Bytes _element8;
        private readonly FixedString128Bytes _element9;
        private readonly FixedString128Bytes _element10;
        private readonly FixedString128Bytes _element11;
        private readonly FixedString128Bytes _element12;
        private readonly FixedString128Bytes _element13;
        private readonly FixedString128Bytes _element14;
        private readonly FixedString128Bytes _element15;
        private readonly FixedString128Bytes _element16;
        private readonly FixedString128Bytes _element17;
        private readonly FixedString128Bytes _element18;
        private readonly FixedString128Bytes _element19;
        private readonly FixedString128Bytes _element20;
        private readonly FixedString128Bytes _element21;
        private readonly FixedString128Bytes _element22;
        private readonly FixedString128Bytes _element23;
        private readonly FixedString128Bytes _element24;
        private readonly FixedString128Bytes _element25;
        private readonly FixedString128Bytes _element26;
        private readonly FixedString128Bytes _element27;
        private readonly FixedString128Bytes _element28;
        private readonly FixedString128Bytes _element29;
        private readonly FixedString128Bytes _element30;
        private readonly FixedString128Bytes _element31;

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
            _element10 = default;
            _element11 = default;
            _element12 = default;
            _element13 = default;
            _element14 = default;
            _element15 = default;
            _element16 = default;
            _element17 = default;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
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
            _element10 = default;
            _element11 = default;
            _element12 = default;
            _element13 = default;
            _element14 = default;
            _element15 = default;
            _element16 = default;
            _element17 = default;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
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
            _element10 = default;
            _element11 = default;
            _element12 = default;
            _element13 = default;
            _element14 = default;
            _element15 = default;
            _element16 = default;
            _element17 = default;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
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
            _element10 = default;
            _element11 = default;
            _element12 = default;
            _element13 = default;
            _element14 = default;
            _element15 = default;
            _element16 = default;
            _element17 = default;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
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
            _element10 = default;
            _element11 = default;
            _element12 = default;
            _element13 = default;
            _element14 = default;
            _element15 = default;
            _element16 = default;
            _element17 = default;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
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
            _element10 = default;
            _element11 = default;
            _element12 = default;
            _element13 = default;
            _element14 = default;
            _element15 = default;
            _element16 = default;
            _element17 = default;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
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
            _element10 = default;
            _element11 = default;
            _element12 = default;
            _element13 = default;
            _element14 = default;
            _element15 = default;
            _element16 = default;
            _element17 = default;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
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
            _element10 = default;
            _element11 = default;
            _element12 = default;
            _element13 = default;
            _element14 = default;
            _element15 = default;
            _element16 = default;
            _element17 = default;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
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
            _element10 = default;
            _element11 = default;
            _element12 = default;
            _element13 = default;
            _element14 = default;
            _element15 = default;
            _element16 = default;
            _element17 = default;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
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
            _element10 = default;
            _element11 = default;
            _element12 = default;
            _element13 = default;
            _element14 = default;
            _element15 = default;
            _element16 = default;
            _element17 = default;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10)
        {
            _count = 11;

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
            _element10 = class10;
            _element11 = default;
            _element12 = default;
            _element13 = default;
            _element14 = default;
            _element15 = default;
            _element16 = default;
            _element17 = default;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11)
        {
            _count = 12;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = default;
            _element13 = default;
            _element14 = default;
            _element15 = default;
            _element16 = default;
            _element17 = default;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12)
        {
            _count = 13;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = default;
            _element14 = default;
            _element15 = default;
            _element16 = default;
            _element17 = default;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13)
        {
            _count = 14;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = default;
            _element15 = default;
            _element16 = default;
            _element17 = default;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14)
        {
            _count = 15;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = default;
            _element16 = default;
            _element17 = default;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14, string class15)
        {
            _count = 16;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = class15;
            _element16 = default;
            _element17 = default;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14, string class15, string class16)
        {
            _count = 17;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = class15;
            _element16 = class16;
            _element17 = default;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14, string class15, string class16, string class17)
        {
            _count = 18;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = class15;
            _element16 = class16;
            _element17 = class17;
            _element18 = default;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14, string class15, string class16, string class17, string class18)
        {
            _count = 19;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = class15;
            _element16 = class16;
            _element17 = class17;
            _element18 = class18;
            _element19 = default;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14, string class15, string class16, string class17, string class18, string class19)
        {
            _count = 20;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = class15;
            _element16 = class16;
            _element17 = class17;
            _element18 = class18;
            _element19 = class19;
            _element20 = default;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14, string class15, string class16, string class17, string class18, string class19, string class20)
        {
            _count = 21;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = class15;
            _element16 = class16;
            _element17 = class17;
            _element18 = class18;
            _element19 = class19;
            _element20 = class20;
            _element21 = default;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14, string class15, string class16, string class17, string class18, string class19, string class20, string class21)
        {
            _count = 22;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = class15;
            _element16 = class16;
            _element17 = class17;
            _element18 = class18;
            _element19 = class19;
            _element20 = class20;
            _element21 = class21;
            _element22 = default;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14, string class15, string class16, string class17, string class18, string class19, string class20, string class21, string class22)
        {
            _count = 23;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = class15;
            _element16 = class16;
            _element17 = class17;
            _element18 = class18;
            _element19 = class19;
            _element20 = class20;
            _element21 = class21;
            _element22 = class22;
            _element23 = default;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14, string class15, string class16, string class17, string class18, string class19, string class20, string class21, string class22, string class23)
        {
            _count = 24;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = class15;
            _element16 = class16;
            _element17 = class17;
            _element18 = class18;
            _element19 = class19;
            _element20 = class20;
            _element21 = class21;
            _element22 = class22;
            _element23 = class23;
            _element24 = default;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14, string class15, string class16, string class17, string class18, string class19, string class20, string class21, string class22, string class23, string class24)
        {
            _count = 25;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = class15;
            _element16 = class16;
            _element17 = class17;
            _element18 = class18;
            _element19 = class19;
            _element20 = class20;
            _element21 = class21;
            _element22 = class22;
            _element23 = class23;
            _element24 = class24;
            _element25 = default;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14, string class15, string class16, string class17, string class18, string class19, string class20, string class21, string class22, string class23, string class24, string class25)
        {
            _count = 26;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = class15;
            _element16 = class16;
            _element17 = class17;
            _element18 = class18;
            _element19 = class19;
            _element20 = class20;
            _element21 = class21;
            _element22 = class22;
            _element23 = class23;
            _element24 = class24;
            _element25 = class25;
            _element26 = default;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14, string class15, string class16, string class17, string class18, string class19, string class20, string class21, string class22, string class23, string class24, string class25, string class26)
        {
            _count = 27;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = class15;
            _element16 = class16;
            _element17 = class17;
            _element18 = class18;
            _element19 = class19;
            _element20 = class20;
            _element21 = class21;
            _element22 = class22;
            _element23 = class23;
            _element24 = class24;
            _element25 = class25;
            _element26 = class26;
            _element27 = default;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14, string class15, string class16, string class17, string class18, string class19, string class20, string class21, string class22, string class23, string class24, string class25, string class26, string class27)
        {
            _count = 28;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = class15;
            _element16 = class16;
            _element17 = class17;
            _element18 = class18;
            _element19 = class19;
            _element20 = class20;
            _element21 = class21;
            _element22 = class22;
            _element23 = class23;
            _element24 = class24;
            _element25 = class25;
            _element26 = class26;
            _element27 = class27;
            _element28 = default;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14, string class15, string class16, string class17, string class18, string class19, string class20, string class21, string class22, string class23, string class24, string class25, string class26, string class27, string class28)
        {
            _count = 29;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = class15;
            _element16 = class16;
            _element17 = class17;
            _element18 = class18;
            _element19 = class19;
            _element20 = class20;
            _element21 = class21;
            _element22 = class22;
            _element23 = class23;
            _element24 = class24;
            _element25 = class25;
            _element26 = class26;
            _element27 = class27;
            _element28 = class28;
            _element29 = default;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14, string class15, string class16, string class17, string class18, string class19, string class20, string class21, string class22, string class23, string class24, string class25, string class26, string class27, string class28, string class29)
        {
            _count = 30;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = class15;
            _element16 = class16;
            _element17 = class17;
            _element18 = class18;
            _element19 = class19;
            _element20 = class20;
            _element21 = class21;
            _element22 = class22;
            _element23 = class23;
            _element24 = class24;
            _element25 = class25;
            _element26 = class26;
            _element27 = class27;
            _element28 = class28;
            _element29 = class29;
            _element30 = default;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14, string class15, string class16, string class17, string class18, string class19, string class20, string class21, string class22, string class23, string class24, string class25, string class26, string class27, string class28, string class29, string class30)
        {
            _count = 31;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = class15;
            _element16 = class16;
            _element17 = class17;
            _element18 = class18;
            _element19 = class19;
            _element20 = class20;
            _element21 = class21;
            _element22 = class22;
            _element23 = class23;
            _element24 = class24;
            _element25 = class25;
            _element26 = class26;
            _element27 = class27;
            _element28 = class28;
            _element29 = class29;
            _element30 = class30;
            _element31 = default;
        }

        public ClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9, string class10, string class11, string class12, string class13, string class14, string class15, string class16, string class17, string class18, string class19, string class20, string class21, string class22, string class23, string class24, string class25, string class26, string class27, string class28, string class29, string class30, string class31)
        {
            _count = 32;

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
            _element10 = class10;
            _element11 = class11;
            _element12 = class12;
            _element13 = class13;
            _element14 = class14;
            _element15 = class15;
            _element16 = class16;
            _element17 = class17;
            _element18 = class18;
            _element19 = class19;
            _element20 = class20;
            _element21 = class21;
            _element22 = class22;
            _element23 = class23;
            _element24 = class24;
            _element25 = class25;
            _element26 = class26;
            _element27 = class27;
            _element28 = class28;
            _element29 = class29;
            _element30 = class30;
            _element31 = class31;
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
                    case 10:
                        return _element10.Value;
                    case 11:
                        return _element11.Value;
                    case 12:
                        return _element12.Value;
                    case 13:
                        return _element13.Value;
                    case 14:
                        return _element14.Value;
                    case 15:
                        return _element15.Value;
                    case 16:
                        return _element16.Value;
                    case 17:
                        return _element17.Value;
                    case 18:
                        return _element18.Value;
                    case 19:
                        return _element19.Value;
                    case 20:
                        return _element20.Value;
                    case 21:
                        return _element21.Value;
                    case 22:
                        return _element22.Value;
                    case 23:
                        return _element23.Value;
                    case 24:
                        return _element24.Value;
                    case 25:
                        return _element25.Value;
                    case 26:
                        return _element26.Value;
                    case 27:
                        return _element27.Value;
                    case 28:
                        return _element28.Value;
                    case 29:
                        return _element29.Value;
                    case 30:
                        return _element30.Value;
                    case 31:
                        return _element31.Value;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
        
        // TODO: Remove

        public unsafe ClassName Add(ClassName className)
        {
            var copy = this;
            ClassName result = default;

            var stride = 5;
            UnsafeUtility.MemCpy(UnsafeUtility.AddressOf(ref result), UnsafeUtility.AddressOf(ref copy), stride);
            UnsafeUtility.MemCpyStride(UnsafeUtility.AddressOf(ref result), stride, UnsafeUtility.AddressOf(ref className), 0, UnsafeUtility.SizeOf<ClassName>() - stride, 1);
            
            return result;
            
            // var size = Count + className.Count;
            // if (size > 32)
            // {
            //     throw new UnityException("Can't fit");
            // }
            //
            // switch (size)
            // {
            //     case 0:
            //         return default;
            //     case 1:
            //         
            //     case 2:
            //         
            //     case 3:
            //         
            //     case 4:
            //         
            //     case 5:
            //         
            //     case 6:
            //         
            //     case 7:
            //         
            //     case 8:
            //         
            //     case 9:
            //         
            //     case 10:
            //         
            //     case 11:
            //         
            //     case 12:
            //         
            //     case 13:
            //         
            //     case 14:
            //         
            //     case 15:
            //         
            //     case 16:
            //         
            //     case 17:
            //         
            //     case 18:
            //         
            //     case 19:
            //         
            //     case 20:
            //         return Count switch
            //         {
            //             0 => new ClassName(className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17], className[18], className[19]),
            //             1 => new ClassName(this[0], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18]),
            //             2 => new ClassName(this[0], this[1], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17]),
            //             3 => new ClassName(this[0], this[1], this[2], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16]),
            //             4 => new ClassName(this[0], this[1], this[2], this[3], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15]),
            //             5 => new ClassName(this[0], this[1], this[2], this[3], this[4], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13],
            //                 className[14]),
            //             6 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13]),
            //             7 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12]),
            //             8 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11]),
            //             9 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10]),
            //             10 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9]),
            //             11 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8]),
            //             12 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7]),
            //             13 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6]),
            //             14 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], className[0], className[1],
            //                 className[2], className[3], className[4], className[5]),
            //             15 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], className[0],
            //                 className[1], className[2], className[3], className[4]),
            //             16 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], className[0],
            //                 className[1], className[2], className[3]),
            //             17 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 className[0], className[1], className[2]),
            //             18 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], className[0], className[1]),
            //             19 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], className[0]),
            //             20 => this,
            //         };
            //     case 21:
            //         return Count switch
            //         {
            //             0 => new ClassName(className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17], className[18], className[19], className[20]),
            //             1 => new ClassName(this[0], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19]),
            //             2 => new ClassName(this[0], this[1], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18]),
            //             3 => new ClassName(this[0], this[1], this[2], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17]),
            //             4 => new ClassName(this[0], this[1], this[2], this[3], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16]),
            //             5 => new ClassName(this[0], this[1], this[2], this[3], this[4], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13],
            //                 className[14], className[15]),
            //             6 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14]),
            //             7 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13]),
            //             8 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12]),
            //             9 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11]),
            //             10 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10]),
            //             11 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9]),
            //             12 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8]),
            //             13 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7]),
            //             14 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6]),
            //             15 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], className[0],
            //                 className[1], className[2], className[3], className[4], className[5]),
            //             16 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], className[0],
            //                 className[1], className[2], className[3], className[4]),
            //             17 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 className[0], className[1], className[2], className[3]),
            //             18 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], className[0], className[1], className[2]),
            //             19 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], className[0], className[1]),
            //             20 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], className[0]),
            //             21 => this,
            //         };
            //     case 22:
            //         return Count switch
            //         {
            //             0 => new ClassName(className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17], className[18], className[19], className[20], className[21]),
            //             1 => new ClassName(this[0], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20]),
            //             2 => new ClassName(this[0], this[1], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19]),
            //             3 => new ClassName(this[0], this[1], this[2], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18]),
            //             4 => new ClassName(this[0], this[1], this[2], this[3], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17]),
            //             5 => new ClassName(this[0], this[1], this[2], this[3], this[4], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13],
            //                 className[14], className[15], className[16]),
            //             6 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15]),
            //             7 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14]),
            //             8 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13]),
            //             9 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12]),
            //             10 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11]),
            //             11 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10]),
            //             12 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9]),
            //             13 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8]),
            //             14 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7]),
            //             15 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6]),
            //             16 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], className[0],
            //                 className[1], className[2], className[3], className[4], className[5]),
            //             17 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 className[0], className[1], className[2], className[3], className[4]),
            //             18 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], className[0], className[1], className[2], className[3]),
            //             19 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], className[0], className[1], className[2]),
            //             20 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], className[0], className[1]),
            //             21 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], className[0]),
            //             22 => this,
            //         };
            //     case 23:
            //         return Count switch
            //         {
            //             0 => new ClassName(className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17], className[18], className[19], className[20], className[21], className[22]),
            //             1 => new ClassName(this[0], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21]),
            //             2 => new ClassName(this[0], this[1], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20]),
            //             3 => new ClassName(this[0], this[1], this[2], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19]),
            //             4 => new ClassName(this[0], this[1], this[2], this[3], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18]),
            //             5 => new ClassName(this[0], this[1], this[2], this[3], this[4], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13],
            //                 className[14], className[15], className[16], className[17]),
            //             6 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16]),
            //             7 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15]),
            //             8 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14]),
            //             9 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13]),
            //             10 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12]),
            //             11 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11]),
            //             12 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10]),
            //             13 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9]),
            //             14 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8]),
            //             15 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7]),
            //             16 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6]),
            //             17 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 className[0], className[1], className[2], className[3], className[4], className[5]),
            //             18 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], className[0], className[1], className[2], className[3], className[4]),
            //             19 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], className[0], className[1], className[2], className[3]),
            //             20 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], className[0], className[1], className[2]),
            //             21 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], className[0], className[1]),
            //             22 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], className[0]),
            //             23 => this,
            //         };
            //     case 24:
            //         return Count switch
            //         {
            //             0 => new ClassName(className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17], className[18], className[19], className[20], className[21], className[22],
            //                 className[23]),
            //             1 => new ClassName(this[0], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21],
            //                 className[22]),
            //             2 => new ClassName(this[0], this[1], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21]),
            //             3 => new ClassName(this[0], this[1], this[2], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19], className[20]),
            //             4 => new ClassName(this[0], this[1], this[2], this[3], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19]),
            //             5 => new ClassName(this[0], this[1], this[2], this[3], this[4], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13],
            //                 className[14], className[15], className[16], className[17], className[18]),
            //             6 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16], className[17]),
            //             7 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16]),
            //             8 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14], className[15]),
            //             9 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14]),
            //             10 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13]),
            //             11 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12]),
            //             12 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11]),
            //             13 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10]),
            //             14 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9]),
            //             15 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8]),
            //             16 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7]),
            //             17 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6]),
            //             18 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], className[0], className[1], className[2], className[3], className[4],
            //                 className[5]),
            //             19 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], className[0], className[1], className[2], className[3], className[4]),
            //             20 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], className[0], className[1], className[2], className[3]),
            //             21 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], className[0], className[1], className[2]),
            //             22 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], className[0], className[1]),
            //             23 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], className[0]),
            //             24 => this,
            //         };
            //     case 25:
            //         return Count switch
            //         {
            //             0 => new ClassName(className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17], className[18], className[19], className[20], className[21], className[22],
            //                 className[23], className[24]),
            //             1 => new ClassName(this[0], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21],
            //                 className[22], className[23]),
            //             2 => new ClassName(this[0], this[1], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21],
            //                 className[22]),
            //             3 => new ClassName(this[0], this[1], this[2], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19], className[20],
            //                 className[21]),
            //             4 => new ClassName(this[0], this[1], this[2], this[3], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19], className[20]),
            //             5 => new ClassName(this[0], this[1], this[2], this[3], this[4], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13],
            //                 className[14], className[15], className[16], className[17], className[18], className[19]),
            //             6 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16], className[17], className[18]),
            //             7 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16], className[17]),
            //             8 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14], className[15], className[16]),
            //             9 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14], className[15]),
            //             10 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14]),
            //             11 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13]),
            //             12 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12]),
            //             13 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11]),
            //             14 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10]),
            //             15 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9]),
            //             16 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8]),
            //             17 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7]),
            //             18 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6]),
            //             19 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], className[0], className[1], className[2], className[3], className[4],
            //                 className[5]),
            //             20 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], className[0], className[1], className[2], className[3],
            //                 className[4]),
            //             21 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], className[0], className[1], className[2],
            //                 className[3]),
            //             22 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], className[0], className[1], className[2]),
            //             23 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], className[0], className[1]),
            //             24 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], className[0]),
            //             25 => this,
            //         };
            //     case 26:
            //         return Count switch
            //         {
            //             0 => new ClassName(className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17], className[18], className[19], className[20], className[21], className[22],
            //                 className[23], className[24], className[25]),
            //             1 => new ClassName(this[0], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21],
            //                 className[22], className[23], className[24]),
            //             2 => new ClassName(this[0], this[1], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21],
            //                 className[22], className[23]),
            //             3 => new ClassName(this[0], this[1], this[2], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19], className[20],
            //                 className[21], className[22]),
            //             4 => new ClassName(this[0], this[1], this[2], this[3], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19], className[20],
            //                 className[21]),
            //             5 => new ClassName(this[0], this[1], this[2], this[3], this[4], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13],
            //                 className[14], className[15], className[16], className[17], className[18], className[19],
            //                 className[20]),
            //             6 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16], className[17], className[18],
            //                 className[19]),
            //             7 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16], className[17], className[18]),
            //             8 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14], className[15], className[16], className[17]),
            //             9 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14], className[15], className[16]),
            //             10 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15]),
            //             11 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14]),
            //             12 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13]),
            //             13 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12]),
            //             14 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11]),
            //             15 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10]),
            //             16 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9]),
            //             17 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8]),
            //             18 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7]),
            //             19 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6]),
            //             20 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], className[0], className[1], className[2], className[3],
            //                 className[4], className[5]),
            //             21 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], className[0], className[1], className[2],
            //                 className[3], className[4]),
            //             22 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], className[0], className[1], className[2],
            //                 className[3]),
            //             23 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], className[0], className[1],
            //                 className[2]),
            //             24 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], className[0],
            //                 className[1]),
            //             25 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24],
            //                 className[0]),
            //             26 => this,
            //         };
            //     case 27:
            //         return Count switch
            //         {
            //             0 => new ClassName(className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17], className[18], className[19], className[20], className[21], className[22],
            //                 className[23], className[24], className[25], className[26]),
            //             1 => new ClassName(this[0], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21],
            //                 className[22], className[23], className[24], className[25]),
            //             2 => new ClassName(this[0], this[1], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21],
            //                 className[22], className[23], className[24]),
            //             3 => new ClassName(this[0], this[1], this[2], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19], className[20],
            //                 className[21], className[22], className[23]),
            //             4 => new ClassName(this[0], this[1], this[2], this[3], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19], className[20],
            //                 className[21], className[22]),
            //             5 => new ClassName(this[0], this[1], this[2], this[3], this[4], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13],
            //                 className[14], className[15], className[16], className[17], className[18], className[19],
            //                 className[20], className[21]),
            //             6 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16], className[17], className[18],
            //                 className[19], className[20]),
            //             7 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16], className[17], className[18],
            //                 className[19]),
            //             8 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14], className[15], className[16], className[17],
            //                 className[18]),
            //             9 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14], className[15], className[16], className[17]),
            //             10 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16]),
            //             11 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15]),
            //             12 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14]),
            //             13 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13]),
            //             14 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12]),
            //             15 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11]),
            //             16 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10]),
            //             17 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9]),
            //             18 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8]),
            //             19 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7]),
            //             20 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6]),
            //             21 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], className[0], className[1], className[2],
            //                 className[3], className[4], className[5]),
            //             22 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], className[0], className[1], className[2],
            //                 className[3], className[4]),
            //             23 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], className[0], className[1],
            //                 className[2], className[3]),
            //             24 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], className[0],
            //                 className[1], className[2]),
            //             25 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24],
            //                 className[0], className[1]),
            //             26 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 className[0]),
            //             27 => this,
            //         };
            //     case 28:
            //         return Count switch
            //         {
            //             0 => new ClassName(className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17], className[18], className[19], className[20], className[21], className[22],
            //                 className[23], className[24], className[25], className[26], className[27]),
            //             1 => new ClassName(this[0], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21],
            //                 className[22], className[23], className[24], className[25], className[26]),
            //             2 => new ClassName(this[0], this[1], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21],
            //                 className[22], className[23], className[24], className[25]),
            //             3 => new ClassName(this[0], this[1], this[2], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19], className[20],
            //                 className[21], className[22], className[23], className[24]),
            //             4 => new ClassName(this[0], this[1], this[2], this[3], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19], className[20],
            //                 className[21], className[22], className[23]),
            //             5 => new ClassName(this[0], this[1], this[2], this[3], this[4], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13],
            //                 className[14], className[15], className[16], className[17], className[18], className[19],
            //                 className[20], className[21], className[22]),
            //             6 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16], className[17], className[18],
            //                 className[19], className[20], className[21]),
            //             7 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16], className[17], className[18],
            //                 className[19], className[20]),
            //             8 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14], className[15], className[16], className[17],
            //                 className[18], className[19]),
            //             9 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14], className[15], className[16], className[17],
            //                 className[18]),
            //             10 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17]),
            //             11 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16]),
            //             12 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15]),
            //             13 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14]),
            //             14 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13]),
            //             15 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12]),
            //             16 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11]),
            //             17 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10]),
            //             18 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9]),
            //             19 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8]),
            //             20 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7]),
            //             21 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6]),
            //             22 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], className[0], className[1], className[2],
            //                 className[3], className[4], className[5]),
            //             23 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], className[0], className[1],
            //                 className[2], className[3], className[4]),
            //             24 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], className[0],
            //                 className[1], className[2], className[3]),
            //             25 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24],
            //                 className[0], className[1], className[2]),
            //             26 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 className[0], className[1]),
            //             27 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 this[26], className[0]),
            //             28 => this,
            //         };
            //     case 29:
            //         return Count switch
            //         {
            //             0 => new ClassName(className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17], className[18], className[19], className[20], className[21], className[22],
            //                 className[23], className[24], className[25], className[26], className[27], className[28]),
            //             1 => new ClassName(this[0], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21],
            //                 className[22], className[23], className[24], className[25], className[26], className[27]),
            //             2 => new ClassName(this[0], this[1], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21],
            //                 className[22], className[23], className[24], className[25], className[26]),
            //             3 => new ClassName(this[0], this[1], this[2], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19], className[20],
            //                 className[21], className[22], className[23], className[24], className[25]),
            //             4 => new ClassName(this[0], this[1], this[2], this[3], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19], className[20],
            //                 className[21], className[22], className[23], className[24]),
            //             5 => new ClassName(this[0], this[1], this[2], this[3], this[4], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13],
            //                 className[14], className[15], className[16], className[17], className[18], className[19],
            //                 className[20], className[21], className[22], className[23]),
            //             6 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16], className[17], className[18],
            //                 className[19], className[20], className[21], className[22]),
            //             7 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16], className[17], className[18],
            //                 className[19], className[20], className[21]),
            //             8 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14], className[15], className[16], className[17],
            //                 className[18], className[19], className[20]),
            //             9 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14], className[15], className[16], className[17],
            //                 className[18], className[19]),
            //             10 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17], className[18]),
            //             11 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17]),
            //             12 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16]),
            //             13 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15]),
            //             14 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13],
            //                 className[14]),
            //             15 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13]),
            //             16 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12]),
            //             17 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11]),
            //             18 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10]),
            //             19 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9]),
            //             20 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8]),
            //             21 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7]),
            //             22 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6]),
            //             23 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], className[0], className[1],
            //                 className[2], className[3], className[4], className[5]),
            //             24 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], className[0],
            //                 className[1], className[2], className[3], className[4]),
            //             25 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24],
            //                 className[0], className[1], className[2], className[3]),
            //             26 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 className[0], className[1], className[2]),
            //             27 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 this[26], className[0], className[1]),
            //             28 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 this[26], this[27], className[0]),
            //             29 => this,
            //         };
            //     case 30:
            //         return Count switch
            //         {
            //             0 => new ClassName(className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17], className[18], className[19], className[20], className[21], className[22],
            //                 className[23], className[24], className[25], className[26], className[27], className[28],
            //                 className[29]),
            //             1 => new ClassName(this[0], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21],
            //                 className[22], className[23], className[24], className[25], className[26], className[27],
            //                 className[28]),
            //             2 => new ClassName(this[0], this[1], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21],
            //                 className[22], className[23], className[24], className[25], className[26], className[27]),
            //             3 => new ClassName(this[0], this[1], this[2], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19], className[20],
            //                 className[21], className[22], className[23], className[24], className[25], className[26]),
            //             4 => new ClassName(this[0], this[1], this[2], this[3], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19], className[20],
            //                 className[21], className[22], className[23], className[24], className[25]),
            //             5 => new ClassName(this[0], this[1], this[2], this[3], this[4], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13],
            //                 className[14], className[15], className[16], className[17], className[18], className[19],
            //                 className[20], className[21], className[22], className[23], className[24]),
            //             6 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16], className[17], className[18],
            //                 className[19], className[20], className[21], className[22], className[23]),
            //             7 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16], className[17], className[18],
            //                 className[19], className[20], className[21], className[22]),
            //             8 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14], className[15], className[16], className[17],
            //                 className[18], className[19], className[20], className[21]),
            //             9 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14], className[15], className[16], className[17],
            //                 className[18], className[19], className[20]),
            //             10 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17], className[18], className[19]),
            //             11 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18]),
            //             12 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17]),
            //             13 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16]),
            //             14 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13],
            //                 className[14], className[15]),
            //             15 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14]),
            //             16 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13]),
            //             17 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12]),
            //             18 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11]),
            //             19 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10]),
            //             20 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9]),
            //             21 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8]),
            //             22 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7]),
            //             23 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6]),
            //             24 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], className[0],
            //                 className[1], className[2], className[3], className[4], className[5]),
            //             25 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24],
            //                 className[0], className[1], className[2], className[3], className[4]),
            //             26 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 className[0], className[1], className[2], className[3]),
            //             27 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 this[26], className[0], className[1], className[2]),
            //             28 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 this[26], this[27], className[0], className[1]),
            //             29 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 this[26], this[27], this[28], className[0]),
            //             30 => this,
            //         };
            //     case 31:
            //         return Count switch
            //         {
            //             0 => new ClassName(className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17], className[18], className[19], className[20], className[21], className[22],
            //                 className[23], className[24], className[25], className[26], className[27], className[28],
            //                 className[29], className[30]),
            //             1 => new ClassName(this[0], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21],
            //                 className[22], className[23], className[24], className[25], className[26], className[27],
            //                 className[28], className[29]),
            //             2 => new ClassName(this[0], this[1], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21],
            //                 className[22], className[23], className[24], className[25], className[26], className[27],
            //                 className[28]),
            //             3 => new ClassName(this[0], this[1], this[2], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19], className[20],
            //                 className[21], className[22], className[23], className[24], className[25], className[26],
            //                 className[27]),
            //             4 => new ClassName(this[0], this[1], this[2], this[3], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19], className[20],
            //                 className[21], className[22], className[23], className[24], className[25], className[26]),
            //             5 => new ClassName(this[0], this[1], this[2], this[3], this[4], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13],
            //                 className[14], className[15], className[16], className[17], className[18], className[19],
            //                 className[20], className[21], className[22], className[23], className[24], className[25]),
            //             6 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16], className[17], className[18],
            //                 className[19], className[20], className[21], className[22], className[23], className[24]),
            //             7 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16], className[17], className[18],
            //                 className[19], className[20], className[21], className[22], className[23]),
            //             8 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14], className[15], className[16], className[17],
            //                 className[18], className[19], className[20], className[21], className[22]),
            //             9 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14], className[15], className[16], className[17],
            //                 className[18], className[19], className[20], className[21]),
            //             10 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17], className[18], className[19], className[20]),
            //             11 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19]),
            //             12 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18]),
            //             13 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17]),
            //             14 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13],
            //                 className[14], className[15], className[16]),
            //             15 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15]),
            //             16 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14]),
            //             17 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13]),
            //             18 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12]),
            //             19 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11]),
            //             20 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10]),
            //             21 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9]),
            //             22 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8]),
            //             23 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7]),
            //             24 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6]),
            //             25 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24],
            //                 className[0], className[1], className[2], className[3], className[4], className[5]),
            //             26 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 className[0], className[1], className[2], className[3], className[4]),
            //             27 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 this[26], className[0], className[1], className[2], className[3]),
            //             28 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 this[26], this[27], className[0], className[1], className[2]),
            //             29 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 this[26], this[27], this[28], className[0], className[1]),
            //             30 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 this[26], this[27], this[28], this[29], className[0]),
            //             31 => this,
            //         };
            //     case 32:
            //         return Count switch
            //         {
            //             0 => new ClassName(className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17], className[18], className[19], className[20], className[21], className[22],
            //                 className[23], className[24], className[25], className[26], className[27], className[28],
            //                 className[29], className[30], className[31]),
            //             1 => new ClassName(this[0], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21],
            //                 className[22], className[23], className[24], className[25], className[26], className[27],
            //                 className[28], className[29], className[30]),
            //             2 => new ClassName(this[0], this[1], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20], className[21],
            //                 className[22], className[23], className[24], className[25], className[26], className[27],
            //                 className[28], className[29]),
            //             3 => new ClassName(this[0], this[1], this[2], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19], className[20],
            //                 className[21], className[22], className[23], className[24], className[25], className[26],
            //                 className[27], className[28]),
            //             4 => new ClassName(this[0], this[1], this[2], this[3], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19], className[20],
            //                 className[21], className[22], className[23], className[24], className[25], className[26],
            //                 className[27]),
            //             5 => new ClassName(this[0], this[1], this[2], this[3], this[4], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13],
            //                 className[14], className[15], className[16], className[17], className[18], className[19],
            //                 className[20], className[21], className[22], className[23], className[24], className[25],
            //                 className[26]),
            //             6 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16], className[17], className[18],
            //                 className[19], className[20], className[21], className[22], className[23], className[24],
            //                 className[25]),
            //             7 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16], className[17], className[18],
            //                 className[19], className[20], className[21], className[22], className[23], className[24]),
            //             8 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14], className[15], className[16], className[17],
            //                 className[18], className[19], className[20], className[21], className[22], className[23]),
            //             9 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14], className[15], className[16], className[17],
            //                 className[18], className[19], className[20], className[21], className[22]),
            //             10 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13], className[14], className[15], className[16],
            //                 className[17], className[18], className[19], className[20], className[21]),
            //             11 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11], className[12], className[13], className[14], className[15],
            //                 className[16], className[17], className[18], className[19], className[20]),
            //             12 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18], className[19]),
            //             13 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10], className[11], className[12], className[13], className[14],
            //                 className[15], className[16], className[17], className[18]),
            //             14 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8], className[9], className[10], className[11], className[12], className[13],
            //                 className[14], className[15], className[16], className[17]),
            //             15 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15], className[16]),
            //             16 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7], className[8], className[9], className[10], className[11], className[12],
            //                 className[13], className[14], className[15]),
            //             17 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6], className[7], className[8], className[9], className[10], className[11],
            //                 className[12], className[13], className[14]),
            //             18 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12], className[13]),
            //             19 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], className[0], className[1], className[2], className[3], className[4],
            //                 className[5], className[6], className[7], className[8], className[9], className[10],
            //                 className[11], className[12]),
            //             20 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], className[0], className[1], className[2], className[3],
            //                 className[4], className[5], className[6], className[7], className[8], className[9],
            //                 className[10], className[11]),
            //             21 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9], className[10]),
            //             22 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], className[0], className[1], className[2],
            //                 className[3], className[4], className[5], className[6], className[7], className[8],
            //                 className[9]),
            //             23 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], className[0], className[1],
            //                 className[2], className[3], className[4], className[5], className[6], className[7],
            //                 className[8]),
            //             24 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], className[0],
            //                 className[1], className[2], className[3], className[4], className[5], className[6],
            //                 className[7]),
            //             25 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24],
            //                 className[0], className[1], className[2], className[3], className[4], className[5],
            //                 className[6]),
            //             26 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 className[0], className[1], className[2], className[3], className[4], className[5]),
            //             27 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 this[26], className[0], className[1], className[2], className[3], className[4]),
            //             28 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 this[26], this[27], className[0], className[1], className[2], className[3]),
            //             29 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 this[26], this[27], this[28], className[0], className[1], className[2]),
            //             30 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 this[26], this[27], this[28], this[29], className[0], className[1]),
            //             31 => new ClassName(this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7],
            //                 this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15], this[16],
            //                 this[17], this[18], this[19], this[20], this[21], this[22], this[23], this[24], this[25],
            //                 this[26], this[27], this[28], this[29], this[30], className[0]),
            //             32 => this,
            //         };
            //     default:
            //         throw new UnityException("Can't fit");
            // }
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
                10 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9]),
                11 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10]),
                12 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11]),
                13 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12]),
                14 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13]),
                15 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14]),
                16 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14], CachedClassNames[15]),
                17 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14], CachedClassNames[15],
                    CachedClassNames[16]),
                18 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14], CachedClassNames[15],
                    CachedClassNames[16], CachedClassNames[17]),
                19 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14], CachedClassNames[15],
                    CachedClassNames[16], CachedClassNames[17], CachedClassNames[18]),
                20 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14], CachedClassNames[15],
                    CachedClassNames[16], CachedClassNames[17], CachedClassNames[18], CachedClassNames[19]),
                21 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14], CachedClassNames[15],
                    CachedClassNames[16], CachedClassNames[17], CachedClassNames[18], CachedClassNames[19],
                    CachedClassNames[20]),
                22 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14], CachedClassNames[15],
                    CachedClassNames[16], CachedClassNames[17], CachedClassNames[18], CachedClassNames[19],
                    CachedClassNames[20], CachedClassNames[21]),
                23 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14], CachedClassNames[15],
                    CachedClassNames[16], CachedClassNames[17], CachedClassNames[18], CachedClassNames[19],
                    CachedClassNames[20], CachedClassNames[21], CachedClassNames[22]),
                24 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14], CachedClassNames[15],
                    CachedClassNames[16], CachedClassNames[17], CachedClassNames[18], CachedClassNames[19],
                    CachedClassNames[20], CachedClassNames[21], CachedClassNames[22], CachedClassNames[23]),
                25 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14], CachedClassNames[15],
                    CachedClassNames[16], CachedClassNames[17], CachedClassNames[18], CachedClassNames[19],
                    CachedClassNames[20], CachedClassNames[21], CachedClassNames[22], CachedClassNames[23],
                    CachedClassNames[24]),
                26 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14], CachedClassNames[15],
                    CachedClassNames[16], CachedClassNames[17], CachedClassNames[18], CachedClassNames[19],
                    CachedClassNames[20], CachedClassNames[21], CachedClassNames[22], CachedClassNames[23],
                    CachedClassNames[24], CachedClassNames[25]),
                27 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14], CachedClassNames[15],
                    CachedClassNames[16], CachedClassNames[17], CachedClassNames[18], CachedClassNames[19],
                    CachedClassNames[20], CachedClassNames[21], CachedClassNames[22], CachedClassNames[23],
                    CachedClassNames[24], CachedClassNames[25], CachedClassNames[26]),
                28 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14], CachedClassNames[15],
                    CachedClassNames[16], CachedClassNames[17], CachedClassNames[18], CachedClassNames[19],
                    CachedClassNames[20], CachedClassNames[21], CachedClassNames[22], CachedClassNames[23],
                    CachedClassNames[24], CachedClassNames[25], CachedClassNames[26], CachedClassNames[27]),
                29 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14], CachedClassNames[15],
                    CachedClassNames[16], CachedClassNames[17], CachedClassNames[18], CachedClassNames[19],
                    CachedClassNames[20], CachedClassNames[21], CachedClassNames[22], CachedClassNames[23],
                    CachedClassNames[24], CachedClassNames[25], CachedClassNames[26], CachedClassNames[27],
                    CachedClassNames[28]),
                30 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14], CachedClassNames[15],
                    CachedClassNames[16], CachedClassNames[17], CachedClassNames[18], CachedClassNames[19],
                    CachedClassNames[20], CachedClassNames[21], CachedClassNames[22], CachedClassNames[23],
                    CachedClassNames[24], CachedClassNames[25], CachedClassNames[26], CachedClassNames[27],
                    CachedClassNames[28], CachedClassNames[29]),
                31 => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14], CachedClassNames[15],
                    CachedClassNames[16], CachedClassNames[17], CachedClassNames[18], CachedClassNames[19],
                    CachedClassNames[20], CachedClassNames[21], CachedClassNames[22], CachedClassNames[23],
                    CachedClassNames[24], CachedClassNames[25], CachedClassNames[26], CachedClassNames[27],
                    CachedClassNames[28], CachedClassNames[29], CachedClassNames[30]),
                _ => new ClassName(CachedClassNames[0], CachedClassNames[1], CachedClassNames[2], CachedClassNames[3],
                    CachedClassNames[4], CachedClassNames[5], CachedClassNames[6], CachedClassNames[7],
                    CachedClassNames[8], CachedClassNames[9], CachedClassNames[10], CachedClassNames[11],
                    CachedClassNames[12], CachedClassNames[13], CachedClassNames[14], CachedClassNames[15],
                    CachedClassNames[16], CachedClassNames[17], CachedClassNames[18], CachedClassNames[19],
                    CachedClassNames[20], CachedClassNames[21], CachedClassNames[22], CachedClassNames[23],
                    CachedClassNames[24], CachedClassNames[25], CachedClassNames[26], CachedClassNames[27],
                    CachedClassNames[28], CachedClassNames[29], CachedClassNames[30], CachedClassNames[31])
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

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15, classes.Item16);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15, classes.Item16, classes.Item17);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15, classes.Item16, classes.Item17, classes.Item18);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15, classes.Item16, classes.Item17, classes.Item18, classes.Item19);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15, classes.Item16, classes.Item17, classes.Item18, classes.Item19, classes.Item20);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15, classes.Item16, classes.Item17, classes.Item18, classes.Item19, classes.Item20, classes.Item21);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15, classes.Item16, classes.Item17, classes.Item18, classes.Item19, classes.Item20, classes.Item21, classes.Item22);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15, classes.Item16, classes.Item17, classes.Item18, classes.Item19, classes.Item20, classes.Item21, classes.Item22, classes.Item23);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15, classes.Item16, classes.Item17, classes.Item18, classes.Item19, classes.Item20, classes.Item21, classes.Item22, classes.Item23, classes.Item24);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15, classes.Item16, classes.Item17, classes.Item18, classes.Item19, classes.Item20, classes.Item21, classes.Item22, classes.Item23, classes.Item24, classes.Item25);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15, classes.Item16, classes.Item17, classes.Item18, classes.Item19, classes.Item20, classes.Item21, classes.Item22, classes.Item23, classes.Item24, classes.Item25, classes.Item26);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15, classes.Item16, classes.Item17, classes.Item18, classes.Item19, classes.Item20, classes.Item21, classes.Item22, classes.Item23, classes.Item24, classes.Item25, classes.Item26, classes.Item27);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15, classes.Item16, classes.Item17, classes.Item18, classes.Item19, classes.Item20, classes.Item21, classes.Item22, classes.Item23, classes.Item24, classes.Item25, classes.Item26, classes.Item27, classes.Item28);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15, classes.Item16, classes.Item17, classes.Item18, classes.Item19, classes.Item20, classes.Item21, classes.Item22, classes.Item23, classes.Item24, classes.Item25, classes.Item26, classes.Item27, classes.Item28, classes.Item29);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15, classes.Item16, classes.Item17, classes.Item18, classes.Item19, classes.Item20, classes.Item21, classes.Item22, classes.Item23, classes.Item24, classes.Item25, classes.Item26, classes.Item27, classes.Item28, classes.Item29, classes.Item30);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15, classes.Item16, classes.Item17, classes.Item18, classes.Item19, classes.Item20, classes.Item21, classes.Item22, classes.Item23, classes.Item24, classes.Item25, classes.Item26, classes.Item27, classes.Item28, classes.Item29, classes.Item30, classes.Item31);
        }

        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) classes)
        {
            return new ClassName(classes.Item1, classes.Item2, classes.Item3, classes.Item4, classes.Item5, classes.Item6, classes.Item7, classes.Item8, classes.Item9, classes.Item10, classes.Item11, classes.Item12, classes.Item13, classes.Item14, classes.Item15, classes.Item16, classes.Item17, classes.Item18, classes.Item19, classes.Item20, classes.Item21, classes.Item22, classes.Item23, classes.Item24, classes.Item25, classes.Item26, classes.Item27, classes.Item28, classes.Item29, classes.Item30, classes.Item31, classes.Item32);
        }

        public void SetClasses(VisualElement element)
        {
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