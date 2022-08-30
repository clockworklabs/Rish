using UnityEditor;
using UnityEditor.Callbacks;

namespace RishUI
{
    public static class HelpersGenerator
    {
        private static bool AutomaticCodeGeneration
        {
            get => EditorPrefs.GetBool("RishAutomaticCodeGeneration", false);
            set => EditorPrefs.SetBool("RishAutomaticCodeGeneration", value);
        }
        
        [MenuItem("Window/Rish/Helper Functions/Generate")]
        private static void CreateHelpers()
        {
            Compile();
        }
        
        [MenuItem("Window/Rish/Helper Functions/Automatic Generation")]
        private static void ToggleAutomaticCodeGeneration()
        {
            AutomaticCodeGeneration = !AutomaticCodeGeneration;

            if (AutomaticCodeGeneration)
            {
                Compile();
            }
        }

        [MenuItem("Window/Rish/Helper Functions/Automatic Generation", true)]
        private static bool ToggleAutomaticCodeGenerationValidate()
        {
            Menu.SetChecked("Window/Rish/Helpers/Automatic Generation", AutomaticCodeGeneration);

            return true;
        }
        
        [DidReloadScripts]
        private static void CompilationFinished()
        {
            if (!AutomaticCodeGeneration)
            {
                return;
            }
            
            Compile();
        }

        private static void Compile()
        {
            var dirty = false;

            var roots = AssetDatabase.FindAssets("t: HelpersRoot");
            foreach (var guid in roots)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var export = AssetDatabase.LoadAssetAtPath<HelpersRoot>(path);
                dirty |= export.GenerateCode();
            }

            if (dirty)
            {
                AssetDatabase.Refresh();
            }
        }
    }
}