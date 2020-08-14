using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySlider = UnityEngine.UI.Slider;

namespace RishUI.Elements
{
    [RequireComponent(typeof(UnitySlider))]
    public class Slider : DOMElement<SliderProps>
    {
        public override bool IsLeaf => true;

        private UnitySlider slider;
        private UnitySlider UnitySlider
        {
            get
            {
                if (slider == null)
                {
                    slider = GetComponent<UnitySlider>();
                }

                return slider;
            }
        }

        public override void Render()
        {
            UnitySlider.value = Props.value;
        }
    }

    public struct SliderProps : Props<SliderProps>, IEquatable<SliderProps>
    {
        public float value;

        public SliderProps Default => new SliderProps();

        public bool Equals(SliderProps other) => Mathf.Approximately(value, other.value);
    }
}