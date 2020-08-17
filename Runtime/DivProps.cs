using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RishUI
{
    public struct DivProps : IEquatable<DivProps>
    {
        public Vector2 anchorMin;
        public Vector2 anchorMax;
        public float top;
        public float left;
        public float bottom;
        public float right;
        
        public static DivProps Default => new DivProps
        {
            anchorMax = Vector2.one
        };
        
        public bool Equals(DivProps other)
        {
            if(!Mathf.Approximately(anchorMin.x, other.anchorMin.x))
            {
                return false;
            }
            if(!Mathf.Approximately(anchorMin.y, other.anchorMin.y))
            {
                return false;
            }
            if(!Mathf.Approximately(anchorMax.x, other.anchorMax.x))
            {
                return false;
            }
            if(!Mathf.Approximately(anchorMax.y, other.anchorMax.y))
            {
                return false;
            }
            if(!Mathf.Approximately(top, other.top))
            {
                return false;
            }
            if(!Mathf.Approximately(left, other.left))
            {
                return false;
            }
            if(!Mathf.Approximately(bottom, other.bottom))
            {
                return false;
            }
            if(!Mathf.Approximately(right, other.right))
            {
                return false;
            }

            return true;
        }
    }
}