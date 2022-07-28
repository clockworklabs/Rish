using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

namespace RishUI.v3.Components
{
    public class TestComponent : RishComponent
    {
        public override DelegateComponent Render()
        {
            return InternalComponent2;
            
            Rish.Create(InternalComponent, 3);
            
            return null;
        }

        private DelegateComponent<int> InternalComponent = props =>
        {
            var (health, _) = UseState(31);
            
            return new VisualElement();
        };

        private DelegateComponent InternalComponent2 = () =>
        {
            return new VisualElement();
        };

        public static (T, StateUpdater<T>) UseState<T>(T initialState) where T : struct
        {
            return (initialState, _ => { });
        }

        public static (T, StateUpdater<T>) UseState<T>(T initialState, Action<T> updateEvent) where T : struct
        {
            return (initialState, _ => { });
        }

        public delegate void StateUpdater<T>(T prevState) where T : struct;
    }
}