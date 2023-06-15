using System;
using RishUI.Events;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Elements
{
    public class Button : RishBaseElement<ButtonProps>, IMountingListener, IPropsListener
    {
        private Form Form { get; set; }
        private bool JustMounted { get; set; }
        
        private uint FocusIndex { get; set; }

        public Button()
        {
            RegisterCallback<VisualChangeEvent>(OnVisualChange);
            RegisterCallback<KeyDownEvent>(OnKeyDown);
        }
        
        void IMountingListener.ComponentDidMount()
        {
            Form = GetFirstAncestorOfType<Form>();
            FocusIndex = Form?.RegisterElement() ?? 0;

            JustMounted = true;
        }
        void IMountingListener.ComponentWillUnmount()
        {
            Form?.UnregisterElement();
            NotFocusable();
        }

        void IPropsListener.PropsDidChange()
        {
            if (Props.focusable)
            {
                Focusable(FocusIndex);
            }
            else
            {
                NotFocusable();
            }
        }
        void IPropsListener.PropsWillChange() { }

        
        protected override Element Render()
        {
            return Rish.Create<Component, ButtonProps>(Props);
        }

        private void OnVisualChange(VisualChangeEvent evt)
        {
            if (!JustMounted)
            {
                return;
            }
            
            JustMounted = false;

            if (Props.focusable && Props.autoFocus)
            {
                    Focus();
            }
        }

        private void OnKeyDown(KeyDownEvent evt)
        {
            if (!HasFocus)
            {
                return;
            }

            var keyCode = evt.keyCode;
            if (keyCode != KeyCode.Space)
            {
                return;
            }
            
            Props.action?.Invoke();

            evt.StopPropagation();
        }
        
        private class Component : RishBaseElement<ButtonProps, ComponentState>, ICustomComponent
        {
            private bool Listening { get; set; }
            private int PointerId { get; set; }

            public Component()
            {
                RegisterCallback<HoverStartEvent>(OnHoverStart);
                RegisterCallback<HoverEndEvent>(OnHoverEnd);
                
                RegisterCallback<PointerDownEvent>(OnPointerDown);
                RegisterCallback<PointerUpEvent>(OnPointerUp);
                // RegisterCallback<PointerStationaryEvent>(OnPointerStationary);
                RegisterCallback<PointerCancelEvent>(OnPointerCancel);
                
                // TODO: Add longPress
            }

            void ICustomComponent.Restart()
            {
                Listening = false;
                PointerId = 0;
            }
            
            protected override Element Render()
            {
                Element element;
                if (!Props.interactable)
                {
                    element = Props.disabled.Valid 
                        ? Props.disabled 
                        : Props.normal;
                } else if(State.pressed && State.hovered && Props.pressed.Valid)
                {
                    element = Props.pressed;
                } else if(State.hovered && Props.hovered.Valid)
                {
                    element = Props.hovered;
                }
                else
                {
                    element = Props.normal;
                }

                return element;
            }

            private void OnHoverStart(HoverStartEvent evt)
            {
                Debug.Log("Hover start");
                var state = State;
                state.hovered = true;
                State = state;
            }

            private void OnHoverEnd(HoverEndEvent evt)
            {
                Debug.Log("Hover end");
                var state = State;
                state.hovered = false;
                State = state;
            }

            private void OnPointerDown(PointerDownEvent evt)
            {
                if (Listening || !Props.interactable)
                {
                    return;
                }

                Listening = true;
                PointerId = evt.pointerId;
                
                CapturePointer(PointerId);

                var state = State;
                state.pressed = true;
                State = state;
                
                // evt.StopPropagation();
            }

            private void OnPointerUp(PointerUpEvent evt)
            {
                if (!Listening || PointerId != evt.pointerId)
                {
                    return;
                }
                
                ReleasePointer(PointerId);

                Listening = false;
                PointerId = 0;
                
                if (Props.interactable && State.hovered)
                {
                    if (evt.button == 1)
                    {
                        Props.secondaryAction?.Invoke();
                    }
                    else
                    {
                        Props.action?.Invoke();
                    }
                }

                var state = State;
                state.pressed = false;
                State = state;
                
                evt.StopPropagation();
            }

            // TODO: Does this work?
            // private void OnPointerStationary(PointerStationaryEvent evt)
            // {
            //     if (!Listening || PointerId != evt.pointerId)
            //     {
            //         return;
            //     }
            //     
            //     this.ReleasePointer(PointerId);
            //
            //     Listening = false;
            //     PointerId = 0;
            //     
            //     
            //     if (ContainsPoint(this.WorldToLocal(evt.position)) && Props.interactable)
            //     {
            //         Props.secondaryAction?.Invoke();
            //     }
            //
            //     var state = State;
            //     state.pressed = false;
            //     State = state;
            //     
            //     evt.StopPropagation();
            // }

            // TODO: Is this necessary?
            private void OnPointerCancel(PointerCancelEvent evt)
            {
                if (!Listening || PointerId != evt.pointerId)
                {
                    return;
                }

                ReleasePointer(PointerId);

                Listening = false;
                PointerId = 0;
                
                // TODO: Is it necessary?
                // if(ContainsPoint(WorldToLocal(evt.position)) && Props.interactable)
                // {
                //     if (evt.button == 1)
                //     {
                //         Props.secondaryAction?.Invoke();
                //     }
                //     else
                //     {
                //         Props.action?.Invoke();
                //     }
                // }

                var state = State;
                state.pressed = false;
                State = state;
                
                evt.StopPropagation();
            }
        }

        private struct ComponentState
        {
            public bool hovered;
            public bool pressed;
        }
    }

    public struct ButtonProps
    {
        public bool interactable;
        
        public Action action;
        public Action secondaryAction;
        
        public Element normal;
        public Element hovered;
        public Element pressed;
        public Element disabled;
        // TODO: Add focused

        public bool focusable;
        public bool autoFocus;

        // TODO: Doing something similar to StyledProps is a better approach
        [Default]
        public static ButtonProps Default => new ButtonProps
        {
            interactable = true
        };

        public ButtonProps(ButtonProps other)
        {
            interactable = other.interactable;
            action = other.action;
            secondaryAction = other.secondaryAction;
            normal = other.normal;
            hovered = other.hovered;
            pressed = other.pressed;
            disabled = other.disabled;
            focusable = other.focusable;
            autoFocus = other.autoFocus;
        }

        [Comparer]
        public static bool Equals(ButtonProps a, ButtonProps b)
        {
            return a.interactable == b.interactable && a.focusable == b.focusable && a.autoFocus == b.autoFocus &&
                RishUtils.Compare<Element>(a.normal, b.normal) &&
                RishUtils.Compare<Element>(a.hovered, b.hovered) &&
                RishUtils.Compare<Element>(a.pressed, b.pressed) &&
                RishUtils.Compare<Element>(a.disabled, b.disabled);
        }

        [ReferencesGetter]
        private static References GetReferences(ButtonProps owner) => (owner.normal, owner.hovered, owner.pressed, owner.disabled);
    }
}
