using System.Collections.Generic;
using UnityEngine;

namespace RishUI
{
    public interface IOwner
    {
        void TakeOwnership(Element element);
    }

    public class ElementsOwner : IOwner
    {
        private List<Element> Owned { get; set; }
        private List<Element> Buffer { get; set; }
        
        public void StartClaimingOwnership()
        {
            SwapBuffers();
            
            Rish.RegisterOwner(this);
        }
        public void StopClaimingOwnership()
        {
            Rish.UnregisterOwner(this);

            ReleaseBuffer();
        }
        
        void IOwner.TakeOwnership(Element element)
        {
            Owned ??= new List<Element>();
            
            Owned.Add(element);
        }
        
        private void SwapBuffers()
        {
            if (Owned?.Count > 0)
            {
                (Owned, Buffer) = (Buffer, Owned);
            }
        }

        private void ReleaseBuffer()
        {
            if (Buffer?.Count > 0)
            {
                for (int i = 0, n = Buffer.Count; i < n; i++)
                {
                    Buffer[i].ReturnToPool();
                }
                Buffer.Clear();
            }
        }

        public void ReleaseAll()
        {
            if (Buffer?.Count > 0)
            {
                throw new UnityException("Error");
            }
            
            if (Owned?.Count > 0)
            {
                for (int i = 0, n = Owned.Count; i < n; i++)
                {
                    Owned[i].ReturnToPool();
                }
                Owned.Clear();
            }
        }
    }
}
