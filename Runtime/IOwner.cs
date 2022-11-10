using System.Collections.Generic;
using UnityEngine;

namespace RishUI
{
    public interface IOwner
    {
        void TakeOwnership(Children element);
    }

    public class ElementsOwner : IOwner
    {
        private List<Children> Owned { get; set; }
        private List<Children> Buffer { get; set; }
        
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
        
        void IOwner.TakeOwnership(Children element)
        {
            Owned ??= new List<Children>();
            
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
