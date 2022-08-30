using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Compilation;
using UnityEngine;

namespace RishUI.Deprecated.Editor
{
    public static class SetupHotReload
    {
        private const string Define = "-define:RISH_HOT_RELOAD_READY";
        
        private struct DllInfo
        {
            public string name;
            public bool includeInEditor;
        }
        
#if !RISH_HOT_RELOAD_READY
        [MenuItem("Window/Rish/Setup Hot Reload")]
        public static void EnableHotReload()
        {
            var editorPath = Path.GetDirectoryName(EditorApplication.applicationPath);
            var dllsPath = $"{editorPath}/Data/MonoBleedingEdge/lib/mono/4.5";
            
            var dataPath = Application.dataPath;
            var destPath = $"{dataPath}/Rish";
            
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
            
            var infos = new[]
            {
                new DllInfo
                {
                    name = "Microsoft.CodeAnalysis.CSharp.dll",
                    includeInEditor = true
                },
                new DllInfo
                {
                    name = "Microsoft.CodeAnalysis.dll",
                    includeInEditor = true
                },
                new DllInfo
                {
                    name = "System.Collections.Immutable.dll",
                    includeInEditor = true
                },
                new DllInfo
                {
                    name = "System.Reflection.Metadata.dll",
                    includeInEditor = false
                },
                new DllInfo
                {
                    name = "System.Runtime.CompilerServices.Unsafe.dll",
                    includeInEditor = false
                }
            };

            var mustRestart = false;
            foreach (var info in infos)
            {
                var path = $"{destPath}/{info.name}";
                if(File.Exists(path)) {
                    continue;
                }

                mustRestart = true;
                
                File.Copy($"{dllsPath}/{info.name}", path, true);
            }
            
            AssetDatabase.Refresh();
            
            foreach (var info in infos)
            {
                var importer = (PluginImporter) AssetImporter.GetAtPath($"Assets/Rish/{info.name}");
            
                importer.SetCompatibleWithAnyPlatform(false);
                importer.SetCompatibleWithEditor(info.includeInEditor);
            }

            var responsePath = $"{dataPath}/csc.rsp";
            
            if (File.Exists(responsePath))
            {
                var mustAdd = File.ReadLines(responsePath).Any(line => line == Define);

                if(mustAdd)
                {
                    using var sw = File.AppendText(responsePath);
                    sw.WriteLine(Define);
                }
            }
            else
            {
                using var sw = File.CreateText(responsePath);
                sw.WriteLine(Define);
            }

            if (mustRestart)
            {
                EditorApplication.OpenProject(Directory.GetCurrentDirectory());
            }
            else
            {
                AssetDatabase.Refresh();
            }
        }
#endif
        
        #if RISH_HOT_RELOAD_READY
        [MenuItem("Window/Rish/Disable Hot Reload")]
        public static void DisableHotReload()
        {
            var dataPath = Application.dataPath;
            
            var responsePath = $"{dataPath}/csc.rsp";
            if (!File.Exists(responsePath))
            {
                return;
            }
            
            var lines = File.ReadLines(responsePath).ToArray();
            if (lines.Length < 1 || lines.Length == 1 && lines[0] == Define)
            {
                File.Delete(responsePath);
            }
            else
            {
                File.WriteAllLines(responsePath, File.ReadLines(responsePath).Where(l => l != Define).ToList());
            }
            
            AssetDatabase.Refresh();
        }
        #endif
    }
}
