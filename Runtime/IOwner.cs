using System.Collections.Generic;
using UnityEngine;

namespace RishUI
{
    public interface IOwner { 
        uint GetID();
    }
//     public interface IOwner
//     {
//         void TakeOwnership(Children element);
//     }
//
//     public class ElementsOwner : IOwner
//     {
//         private List<Children> Owned { get; set; }
//         private List<Children> Buffer { get; set; }
//         
//         private Tree Tree { get; set; }
//         private bool Claiming { set; get; }
//
//         public void Init(Tree tree)
//         {
// #if UNITY_EDITOR
//             if (Tree != null)
//             {
//                 throw new UnityException("Was already initialized");
//             }
// #endif
//             
//             Tree = tree;
//         }
//         
//         public void StartClaimingOwnership()
//         {
// #if UNITY_EDITOR
//             if (Claiming)
//             {
//                 throw new UnityException("Already claiming ownership");
//             }
// #endif
//             
//             Claiming = true;
//             
//             SwapBuffers();
//             Rish.RegisterOwner(this);
//         }
//         public void StopClaimingOwnership()
//         {
// #if UNITY_EDITOR
//             if (!Claiming)
//             {
//                 throw new UnityException("Wasn't claiming ownership");
//             }
// #endif
//
//             Claiming = false;
//             
//             Rish.UnregisterOwner(this);
//             ReleaseBuffer();
//         }
//         
//         void IOwner.TakeOwnership(Children element)
//         {
//             Owned ??= new List<Children>();
//             
//             Owned.Add(element);
//         }
//         
//         private void SwapBuffers()
//         {
//             if (Owned?.Count > 0)
//             {
//                 (Owned, Buffer) = (Buffer, Owned);
//             }
//         }
//
//         private void ReleaseBuffer()
//         {
//             if (Buffer?.Count > 0)
//             {
//                 for (int i = 0, n = Buffer.Count; i < n; i++)
//                 {
//                     Tree.Release(Buffer[i]);
//                 }
//                 Buffer.Clear();
//             }
//         }
//
//         public void ReleaseAll()
//         {
// #if UNITY_EDITOR
//             if (Tree == null)
//             {
//                 throw new UnityException("Wasn't initialized");
//             }
//             
//             if (Buffer?.Count > 0)
//             {
//                 throw new UnityException("Error");
//             }
// #endif
//             
//             if (Owned?.Count > 0)
//             {
//                 for (int i = 0, n = Owned.Count; i < n; i++)
//                 {
//                     Tree.Release(Owned[i]);
//                 }
//                 Owned.Clear();
//             }
//
//             Tree = null;
//         }
//     }
}
