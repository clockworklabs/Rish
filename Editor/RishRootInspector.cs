using System;
using RishUI.MemoryManagement;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace RishUI.Editor
{
    [CustomEditor(typeof(RishRoot))]
    public class RishRootInspector : UnityEditor.Editor
    {
        private SerializedProperty RootGUIDProperty { get; set; }
        private SerializedProperty RootClassNameProperty { get; set; }
        
        private Label TreeSizeLabel { get; set; }
        private Label NodesPoolSizeLabel { get; set; }
        private Label NodesTotalCountLabel { get; set; }
        private Label ManagedPoolSizeLabel { get; set; }
        private Label ManagedTotalCountLabel { get; set; }
        private Label ContextsPoolSizeLabel { get; set; }
        private Label ContextsTotalCountLabel { get; set; }
        
        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();
            
            container.Add(new PropertyField(serializedObject.FindProperty("_manualUpdate")));
            container.Add(new PropertyField(serializedObject.FindProperty("_chainRender")));
            container.Add(new PropertyField(serializedObject.FindProperty("_maxUpdatesPerStep")));
            container.Add(new PropertyField(serializedObject.FindProperty("_maxTargetTimePerStep")));
            
            container.Add(new PropertyField(serializedObject.FindProperty("_debugRender")));
            
            container.Add(new PropertyField(serializedObject.FindProperty("_styleSheets")));

            RootGUIDProperty = serializedObject.FindProperty("_rootGUID");
            RootClassNameProperty = serializedObject.FindProperty("_rootClassName");

            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(RootGUIDProperty.stringValue));
            var scriptField = new ObjectField("Root App")
            {
                allowSceneObjects = false,
                objectType = typeof(MonoScript),
                value = script
            };
            scriptField.RegisterValueChangedCallback(OnRootChange);
            
            container.Add(scriptField);
            if (script != null)
            {
                container.Add(new Label($"\t({RootClassNameProperty.stringValue})"));
            }

            if(Application.isPlaying)
            {
                if (target is RishRoot root)
                {
                    root.OnStep += SappyOnStep;
                    
                    container.Add(new ToolbarSpacer
                    {
                        style =
                        {
                            height = 20
                        }
                    });
                    
                    TreeSizeLabel = new Label();
                    NodesPoolSizeLabel = new Label();
                    NodesTotalCountLabel = new Label();
                    ManagedPoolSizeLabel = new Label();
                    ManagedTotalCountLabel = new Label();
                    ContextsPoolSizeLabel = new Label();
                    ContextsTotalCountLabel = new Label();
                    container.Add(TreeSizeLabel);
                    container.Add(new ToolbarSpacer
                    {
                        style =
                        {
                            height = 10
                        }
                    });
                    container.Add(NodesPoolSizeLabel);
                    container.Add(NodesTotalCountLabel);
                    container.Add(ManagedPoolSizeLabel);
                    container.Add(ManagedTotalCountLabel);
                    container.Add(ContextsPoolSizeLabel);
                    container.Add(ContextsTotalCountLabel);
                }
            }
            
            return container;
        }

        private void OnDisable()
        {
            if (target is RishRoot root)
            {
                root.OnStep -= SappyOnStep;
            }
        }

        private Action<RishRoot> _sappyOnStep;
        private Action<RishRoot> SappyOnStep => _sappyOnStep ??= OnStep;
        
        private void OnStep(RishRoot root)
        {
            TreeSizeLabel.text = $"Tree Size: {root.TreeSize}";
            NodesPoolSizeLabel.text = $"Nodes Pool Size: {Node.PoolSize}";
            NodesTotalCountLabel.text = $"Nodes Total Count: {Node.TotalCount}";
            ManagedPoolSizeLabel.text = $"Managed Pool Size: {Rish.PoolSize}";
            ManagedTotalCountLabel.text = $"Managed Total Count: {Rish.TotalCount}";
            ContextsPoolSizeLabel.text = $"Contexts Pool Size: {ManagedContext.GetPoolSize()}";
            ContextsTotalCountLabel.text = $"Contexts Total Count: {ManagedContext.GetTotalCount()}";
        }

        private void OnRootChange(ChangeEvent<Object> evt)
        {
            if (evt.target is not ObjectField target)
            {
                return;
            }

            if (evt.newValue is not MonoScript monoScript)
            {
                RootGUIDProperty.stringValue = null;
                RootClassNameProperty.stringValue = null;
                serializedObject.ApplyModifiedProperties();
                return;
            }

            var type = monoScript.GetClass();
            
            if (!typeof(IApp).IsAssignableFrom(type))
            {
                target.UnregisterValueChangedCallback(OnRootChange);
                target.value = evt.previousValue;
                target.RegisterValueChangedCallback(OnRootChange);
            }
            else
            {
                var guid = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(monoScript)).ToString();
                
                RootGUIDProperty.stringValue = guid;
                RootClassNameProperty.stringValue = type.FullName;
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
