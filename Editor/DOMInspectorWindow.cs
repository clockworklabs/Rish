using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

namespace Rish.Editor
{
	public class DOMInspectorWindow : EditorWindow
	{
		private TreeViewState TreeViewState { get; set; }
		private TreeView TreeView { get; set; }

		[MenuItem("Rish/DOM Inspector")]
		private static void ShowWindow()
		{
			var window = GetWindow<DOMInspectorWindow>();
			window.titleContent = new GUIContent("DOM Inspector");
			window.Show();
		}

		private void OnEnable ()
		{
			TreeViewState = new TreeViewState();
			
			EditorApplication.playModeStateChanged += OnPlayModeChange;

		}

		private void OnDisable()
		{
			EditorApplication.playModeStateChanged -= OnPlayModeChange;
		}

		private void OnPlayModeChange(PlayModeStateChange state)
		{
			switch (state)
			{
				case PlayModeStateChange.EnteredPlayMode:
				{
					var rish = FindObjectOfType<Rish>();
					TreeView = new DOMTreeView(rish, TreeViewState);
					
					break;
				}
				case PlayModeStateChange.ExitingPlayMode:
				{
					TreeView = null;
					break;
				}
			}
		}

		private void OnHierarchyChange()
		{
			TreeView?.Reload();

			Repaint ();
		}

		private void OnGUI ()
		{
			DoToolbar();
			DoTreeView();
		}
		
		private void DoTreeView ()
		{
			var rect = GUILayoutUtility.GetRect (0, 100000, 0, 100000);
			TreeView?.OnGUI(rect);
		}

		private void DoToolbar()
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
	}
}
