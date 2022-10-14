#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Scripting;

namespace RishUI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ExportAttribute : PreserveAttribute
    {
        public readonly string name;

        public ExportAttribute() : this(null) { }
        
        public ExportAttribute(string name)
        {
            this.name = name;
        }
    }
    
    [Serializable]
    public struct FileInfo
    {
        public string path;
        public long writeTime;
    }
    
    [CreateAssetMenu(fileName = "HelpersRoot", menuName = "Rish/Helpers Root")]
    public class HelpersRoot : ScriptableObject
    {
        private static string _projectRootPath;
        private static string ProjectRootPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_projectRootPath))
                {
                    _projectRootPath = Directory.GetParent(Application.dataPath).FullName;
                }

                return _projectRootPath;
            }
        }
        
        [SerializeField] 
        private string _customName;
        private string CustomName => _customName;
        
        [SerializeField]
        private bool _exportAll;
        private bool ExportAll => _exportAll;

        [SerializeField]
        private bool _exportNested;
        private bool ExportNested => _exportNested;
        
        private StringBuilder StringBuilder { get; } = new ();

        [HideInInspector]
        [SerializeField]
        private List<FileInfo> _fileInfos = new();
        private List<FileInfo> FileInfos => _fileInfos;

        private HashSet<string> ExistingPaths { get; } = new();
        private Dictionary<string, SyntaxTree> SyntaxTrees { get; } = new();
        private Dictionary<string, string> HelpersByNamespace { get; } = new();

        public bool GenerateCode()
        {
            var path = AssetDatabase.GetAssetPath(this);
            var folder = Path.GetDirectoryName(path);
            if (string.IsNullOrWhiteSpace(folder))
            {
                return false;
            }

            var asmName = GetAsm(folder);
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(asm => asm.GetName().Name == asmName);
            if (assembly == null)
            {
                return false;
            }
            
            #if UNITY_EDITOR_WIN
            var exportPath = $"{folder}\\Helpers.cs";
            #else
            var exportPath = $"{folder}/Helpers.cs";
            #endif
            
            var files = Directory.GetFiles(folder, "*.cs", SearchOption.AllDirectories).Where(p => p != exportPath).ToArray();

            if (files.Length <= 0)
            {
                if (!File.Exists(exportPath))
                {
                    return false;
                }
                
                File.Delete(exportPath);
                return true;
            }
            
            ExistingPaths.Clear();
            
            var dirty = false;
            foreach (var filePath in files)
            {
                ExistingPaths.Add(filePath);
                var writeTime = File.GetLastWriteTime(filePath).Ticks;

                var index = FileInfos.FindIndex(wt => wt.path == filePath);
                if (index >= 0)
                {
                    if (FileInfos[index].writeTime == writeTime)
                    {
                        continue;
                    }

                    FileInfos[index] = new FileInfo {
                        path = filePath,
                        writeTime = writeTime
                    };
                }
                else
                {
                    FileInfos.Add(new FileInfo {
                        path = filePath,
                        writeTime = writeTime
                    });
                }
                
                SyntaxTrees[filePath] = CreateSyntaxTree(filePath);
                
                dirty = true;
            }

            for (var i = FileInfos.Count - 1; i >= 0; i--)
            {
                var filePath = FileInfos[i].path;
                if (ExistingPaths.Contains(filePath))
                {
                    continue;
                }
                
                FileInfos.RemoveAt(i);
                SyntaxTrees.Remove(filePath);
            
                dirty = true;
            }

            if (!dirty)
            {
                return false;
            }
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            
            if (File.Exists(exportPath))
            {
                File.Delete(exportPath);
            }
            
            HelpersByNamespace.Clear();
            foreach (var filePath in files)
            {
                if (!SyntaxTrees.TryGetValue(filePath, out var syntaxTree))
                {
                    syntaxTree = CreateSyntaxTree(filePath);
                    SyntaxTrees[filePath] = syntaxTree;
                }
                
                var classes = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();
                foreach (var node in classes)
                {
                    var fullName = GetFullName(node, StringBuilder);
                    var type = assembly.GetType(fullName);
                    if (type == null || !type.IsPublic)
                    {
                        continue;
                    }

                    var attributeDefined = Attribute.IsDefined(type, typeof(ExportAttribute));
                    if (!attributeDefined)
                    {
                        if (!ExportAll)
                        {
                            continue;
                        }

                        if (!ExportNested && type.IsNested)
                        {
                            continue;
                        }
                    }
            
                    var rishType = GetRishElementType(type);
                    if (rishType == null)
                    {
                        continue;
                    }

                    var customName = attributeDefined
                        ? ((ExportAttribute)Attribute.GetCustomAttribute(type, typeof(ExportAttribute))).name
                        : null;

                    var key = string.IsNullOrWhiteSpace(type.Namespace) ? "-" : type.Namespace;
                    
                    StringBuilder.Clear();
                    
                    if(HelpersByNamespace.TryGetValue(key, out var helpers))
                    {
                        StringBuilder.AppendLine(helpers);
                    }

                    var propsType = rishType.GetGenericArguments()[0];
                    StringBuilder.AppendLine(propsType == typeof(NoProps)
                        ? GetHelpersForTypedElement(type, customName)
                        : GetHelpersForTypedElementWithProps(type, propsType, customName));

                    HelpersByNamespace[key] = StringBuilder.ToString();
                }
            }

            if (HelpersByNamespace.Count <= 0)
            {
                return true;
            }

            using var sw = new StreamWriter(exportPath, true);

            sw.WriteLine("using RishUI.v3;");
            foreach (var pair in HelpersByNamespace)
            {
                var (key, helpers) = (pair.Key, pair.Value);

                var hasNamespace = key != "-";

                if (hasNamespace)
                {
                    sw.WriteLine($"namespace {key}");
                    sw.WriteLine("{");
                }

                var indentation = hasNamespace ? "\t" : null;
                
                sw.WriteLine($"{indentation}public class {(string.IsNullOrWhiteSpace(CustomName) ? "Helpers" : CustomName)}");
                sw.WriteLine($"{indentation}{{");
                sw.WriteLine(helpers);
                sw.WriteLine($"{indentation}}}");

                if (hasNamespace)
                {
                    sw.WriteLine("}");
                }
            }
            
            sw.Dispose();

            return true;
        }
        
        private static string GetFullName(ClassDeclarationSyntax source, StringBuilder sb)
        {
            if (source == null)
            {
                return null;
            }

            sb.Clear();
            
            var parentClasses = new List<string>();
            var parent = source.Parent;
            while (parent.IsKind(SyntaxKind.ClassDeclaration))
            {
                if (parent is not ClassDeclarationSyntax parentClass)
                {
                    continue;
                }
                
                parentClasses.Add(parentClass.Identifier.Text);
        
                parent = parent.Parent;
            }

            if(parent is NamespaceDeclarationSyntax nameSpace)
            {
                sb.Append($"{nameSpace.Name}.");
            }
            parentClasses.Reverse();
            parentClasses.ForEach(name => sb.Append($"{name}."));
            sb.Append(source.Identifier.Text);
        
            var result = sb.ToString();
            return result;
        }
        
        private static Type GetRishElementType(Type type)
        {
            // null does not have base type
            if (type == null)
            {
                return null;
            }

            // check all base types
            var currentType = type;
            while (currentType != null)
            {
                var currentBaseType = currentType.BaseType;
                if (currentBaseType?.IsGenericType ?? false)
                {
                    if (currentBaseType.GetGenericTypeDefinition() == typeof(RishElement<,>))
                    {
                        return currentBaseType;
                    }
                }
                
                currentType = currentBaseType;
            }

            return null;
        }

        private static SyntaxTree CreateSyntaxTree(string filePath)
        {
            var text = File.ReadAllText(filePath);
            return CSharpSyntaxTree.ParseText(text);
        }

        private static string GetAsm(string folder)
        {
            var path = GetFileInParents(folder, "*.asmdef");
            if (string.IsNullOrWhiteSpace(path))
            {
                return "Assembly-CSharp";
            }

            var asmdef = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(path);
            if (asmdef == null)
            {
                var jsonPath = GetFileInParents(Path.GetDirectoryName(path), "package.json");
                var json = File.ReadAllText(jsonPath);
                var packageInfo = JsonUtility.FromJson<PackageInfo>(json);
                var relativePath = Path.GetRelativePath(ProjectRootPath, path);
                #if UNITY_EDITOR_WIN
                path = Regex.Replace(relativePath, @"Packages\\\w*\\", $"Packages\\{packageInfo.name}\\");
                #else
                path = Regex.Replace(relativePath, @"Packages/\w*/", $"Packages/{packageInfo.name}/");
                #endif
                asmdef = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(path);
            }

            return asmdef.name;
        }

        private static string GetFileInParents(string folder, string filter)
        {
            if (!Directory.Exists(folder) || folder == ProjectRootPath)
            {
                return null;
            }
            
            var paths = Directory.GetFiles(folder, filter, SearchOption.TopDirectoryOnly);
            if (paths.Length <= 0)
            {
                var parent = Directory.GetParent(folder).FullName;
                return GetFileInParents(parent, filter);
            }
            if (paths.Length > 1)
            {
                throw new UnityException($"More than one {filter} found on {folder}");
            }

            return paths[0];
        }

        private static string GetHelpersForTypedElement(Type type, string customName = null)
        {
            var functionName = string.IsNullOrWhiteSpace(customName) ? type.Name : customName;
            var indentation = string.IsNullOrWhiteSpace(type.Namespace) ? "\t" : "\t\t";
            
            return $@"{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}() => Rish.Create<{type.FullName}>(0, default, default, default);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key) => Rish.Create<{type.FullName}>(key, default, default, default);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(Name name) => Rish.Create<{type.FullName}>(0, name, default, default);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(ClassList classList) => Rish.Create<{type.FullName}>(0, default, classList, default);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(Style style) => Rish.Create<{type.FullName}>(0, default, default, style);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, Name name) => Rish.Create<{type.FullName}>(key, name, default, default);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, ClassList classList) => Rish.Create<{type.FullName}>(key, default, classList, default);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, Style style) => Rish.Create<{type.FullName}>(key, default, default, style);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(Name name, ClassList classList) => Rish.Create<{type.FullName}>(0, name, classList, default);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(Name name, Style style) => Rish.Create<{type.FullName}>(0, name, default, style);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(ClassList classList, Style style) => Rish.Create<{type.FullName}>(0, default, classList, style);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, Name name, ClassList classList) => Rish.Create<{type.FullName}>(key, name, classList, default);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, Name name, Style style) => Rish.Create<{type.FullName}>(key, name, default, style);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, ClassList classList, Style style) => Rish.Create<{type.FullName}>(key, default, classList, style);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(Name name, ClassList classList, Style style) => Rish.Create<{type.FullName}>(0, name, classList, style);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, Name name, ClassList classList, Style style) => Rish.Create<{type.FullName}>(key, name, classList, style, default);";
        }

        private static string GetHelpersForTypedElementWithProps(Type type, Type propsType, string customName = null)
        {
            var functionName = string.IsNullOrWhiteSpace(customName) ? type.Name : customName;
            var indentation = string.IsNullOrWhiteSpace(type.Namespace) ? "\t" : "\t\t";

            return $@"{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}() => Rish.Create<{type.FullName}, {propsType.FullName}>(0, default, default, default, Defaults.GetValue<{propsType.FullName}>());
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key) => Rish.Create<{type.FullName}, {propsType.FullName}>(key, default, default, default, Defaults.GetValue<{propsType.FullName}>());
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(Name name) => Rish.Create<{type.FullName}, {propsType.FullName}>(0, name, default, default, Defaults.GetValue<{propsType.FullName}>());
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(ClassList classList) => Rish.Create<{type.FullName}, {propsType.FullName}>(0, default, classList, default, Defaults.GetValue<{propsType.FullName}>());
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(Style style) => Rish.Create<{type.FullName}, {propsType.FullName}>(0, default, default, style, Defaults.GetValue<{propsType.FullName}>());
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}({propsType.FullName} props) => Rish.Create<{type.FullName}, {propsType.FullName}>(0, default, default, default, props);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, Name name) => Rish.Create<{type.FullName}, {propsType.FullName}>(key, name, default, default, Defaults.GetValue<{propsType.FullName}>());
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, ClassList classList) => Rish.Create<{type.FullName}, {propsType.FullName}>(key, default, classList, default, Defaults.GetValue<{propsType.FullName}>());
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, Style style) => Rish.Create<{type.FullName}, {propsType.FullName}>(key, default, default, style, Defaults.GetValue<{propsType.FullName}>());
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, {propsType.FullName} props) => Rish.Create<{type.FullName}, {propsType.FullName}>(key, default, default, default, props);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(Name name, ClassList classList) => Rish.Create<{type.FullName}, {propsType.FullName}>(0, name, classList, default, Defaults.GetValue<{propsType.FullName}>());
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(Name name, Style style) => Rish.Create<{type.FullName}, {propsType.FullName}>(0, name, default, style, Defaults.GetValue<{propsType.FullName}>());
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(Name name, {propsType.FullName} props) => Rish.Create<{type.FullName}, {propsType.FullName}>(0, name, default, default, props);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(ClassList classList, Style style) => Rish.Create<{type.FullName}, {propsType.FullName}>(0, default, classList, style, RishUI.Defaults.GetValue<{propsType.FullName}>());
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(ClassList classList, {propsType.FullName} props) => Rish.Create<{type.FullName}, {propsType.FullName}>(0, default, classList, default, props);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(Style style, {propsType.FullName} props) => Rish.Create<{type.FullName}, {propsType.FullName}>(0, default, default, style, props);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, Name name, ClassList classList) => Rish.Create<{type.FullName}, {propsType.FullName}>(key, name, classList, default, Defaults.GetValue<{propsType.FullName}>());
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, Name name, Style style) => Rish.Create<{type.FullName}, {propsType.FullName}>(key, name, default, style, Defaults.GetValue<{propsType.FullName}>());
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, Name name, {propsType.FullName} props) => Rish.Create<{type.FullName}, {propsType.FullName}>(key, name, default, default, props);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, ClassList classList, Style style) => Rish.Create<{type.FullName}, {propsType.FullName}>(key, default, classList, style, Defaults.GetValue<{propsType.FullName}>());
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, ClassList classList, {propsType.FullName} props) => Rish.Create<{type.FullName}, {propsType.FullName}>(key, default, classList, default, props);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, Style style, {propsType.FullName} props) => Rish.Create<{type.FullName}, {propsType.FullName}>(key, default, default, style, props);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(Name name, ClassList classList, Style style) => Rish.Create<{type.FullName}, {propsType.FullName}>(0, name, classList, style, Defaults.GetValue<{propsType.FullName}>());
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(Name name, ClassList classList, {propsType.FullName} props) => Rish.Create<{type.FullName}, {propsType.FullName}>(0, name, classList, default, props);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(Name name, Style style, {propsType.FullName} props) => Rish.Create<{type.FullName}, {propsType.FullName}>(0, name, default, style, props);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(ClassList classList, Style style, {propsType.FullName} props) => Rish.Create<{type.FullName}, {propsType.FullName}>(0, default, classList, style, props);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, Name name, ClassList classList, Style style) => Rish.Create<{type.FullName}, {propsType.FullName}>(key, name, classList, style, default);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, Name name, ClassList classList, {propsType.FullName} props) => Rish.Create<{type.FullName}, {propsType.FullName}>(key, name, classList, default, props);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, Name name, Style style, {propsType.FullName} props) => Rish.Create<{type.FullName}, {propsType.FullName}>(key, name, default, style, props);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, ClassList classList, Style style, {propsType.FullName} props) => Rish.Create<{type.FullName}, {propsType.FullName}>(key, default, classList, style, props);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(Name name, ClassList classList, Style style, {propsType.FullName} props) => Rish.Create<{type.FullName}, {propsType.FullName}>(0, name, classList, style, props);
{indentation}[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
{indentation}public static IElement {functionName}(uint key, Name name, ClassList classList, Style style, {propsType.FullName} props) => Rish.Create<{type.FullName}, {propsType.FullName}>(key, name, classList, style, props);";
        }

        private struct PackageInfo
        {
            public string name;
        }
    }
}
#endif