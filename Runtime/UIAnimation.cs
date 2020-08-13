using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rish
{
    public interface UIAnimation
    {
        void Intro(Action endCallback);
        
        void Outro(Action endCallback);
    }
}