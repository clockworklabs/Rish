using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RishUI.v3
{
    public static class ExportReloader
    {
        public static int Counter { get; set; }

        static ExportReloader()
        {
            Debug.Log("New reloader");
        }
        
        [UnityEditor.Callbacks.DidReloadScripts]
        public static void WriteExporter()
        {
            Debug.Log($"Export types: {Counter++}");
        }
    }
}