#if RISH_HOT_RELOAD_READY
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace RishUI
{
    internal class HotReloader
    {
        public event Action<Assembly> OnSuccessfulCompilation;
        
        private Dictionary<string, SyntaxTree> SyntaxTrees { get; } = new ();
            
        private List<PortableExecutableReference> _references;
        private bool _invokeRequired;
        private List<PortableExecutableReference> References => _references ??= AppDomain.CurrentDomain.GetAssemblies().Where(asm => !asm.IsDynamic && !string.IsNullOrWhiteSpace(asm.Location)).Select(asm => MetadataReference.CreateFromFile(asm.Location)).ToList();
        
        private FileWatcher Watcher { get; }
        
        private bool Dirty { get; set; }
        private int Count { get; set; }
        
        public HotReloader(string path)
        {
            UnityThread.InitUnityThread();

            if (!Directory.Exists(path))
            {
                throw new UnityException("Invalid folder for HotReloader");
            }
            
            Watcher = new FileWatcher(path);
            Watcher.Changed += ChangedFile;
            Watcher.Created += CreatedFile;
            Watcher.Deleted += DeletedFile;
            
            Watcher.Initialize();
        }

        public void Dispose()
        {
            Watcher.Changed -= ChangedFile;
            Watcher.Created -= CreatedFile;
            Watcher.Deleted -= DeletedFile;
            
            Watcher.Dispose();
        }

        private void ChangedFile(string scriptPath)
        {
            var text = File.ReadAllText(scriptPath);
            var syntaxTree = CSharpSyntaxTree.ParseText(text);

            SyntaxTrees[scriptPath] = syntaxTree;

            SetDirty();
        }
        
        private void CreatedFile(string scriptPath)
        {
            var text = File.ReadAllText(scriptPath);
            var syntaxTree = CSharpSyntaxTree.ParseText(text);
            
            SyntaxTrees[scriptPath] = syntaxTree;
            
            SetDirty();
        }
        
        private void DeletedFile(string scriptPath)
        {
            if (SyntaxTrees.Remove(scriptPath))
            {
                SetDirty();
            }
        }

        private void SetDirty()
        {
            if (Dirty)
            {
                return;
            }
            
            Dirty = true;
            
            UnityThread.ExecuteCoroutine(Compile());
        }

        public IEnumerator Compile()
        {
            yield return new WaitForEndOfFrame();
            
            Dirty = false;
            
            using var ms = new MemoryStream();
            
            var compilation = CSharpCompilation.Create($"InterpretedAssembly{Count++}",
                SyntaxTrees.Values,
                References,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            
            var result = compilation.Emit(ms);
            
            if (!result.Success)
            {
                SetDirty();
            } else {
                Debug.Log("Hot Reload");
                ms.Seek(0, SeekOrigin.Begin);
            
                var assembly = Assembly.Load(ms.ToArray());
                
                OnSuccessfulCompilation?.Invoke(assembly);
            }
        }
    }
}
#endif