using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEngine.UI;

namespace Rish.Editor
{
	public class DOMInspectorWindow : EditorWindow
	{
		private Texture2D VirtualIcon { get; set; }
		private Texture2D RealIcon { get; set; }
		
		private TreeViewState TreeViewState { get; set; }
		private DOMTreeView TreeView { get; set; }
		
		private Rish Rish { get; set; }
		
		private float sizeRatio = 0.8f; 
		private bool isResizing; 
 
		private float resizerHeight = 5f; 
 
		private GUIStyle resizerStyle;
		
		private DOM Selected { get; set; }
		private Props SelectedProps { get; set; }
		private State SelectedState { get; set; }
		
		private Vector2 InspectorScroll { get; set; }

		[MenuItem("Rish/DOM Inspector")]
		private static void ShowWindow()
		{
			var window = GetWindow<DOMInspectorWindow>();
			window.titleContent = new GUIContent("DOM Inspector");
			window.Show();
		}

		private void OnEnable ()
		{
			VirtualIcon = Resources.Load<Texture2D>("react-icon");
			RealIcon = EditorGUIUtility.IconContent("GameObject Icon").image as Texture2D;
			
			resizerStyle = new GUIStyle();
			resizerStyle.normal.background = EditorGUIUtility.Load("icons/d_AvatarBlendBackground.png") as Texture2D;
			
			TreeViewState = new TreeViewState();
			
			EditorApplication.playModeStateChanged += OnPlayModeChange;

			if (Application.isPlaying)
			{
				OnPlayModeChange(PlayModeStateChange.EnteredPlayMode);
			}
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
					Selected = null;
					Rish = FindObjectOfType<Rish>();
					if (Rish != null)
					{
						TreeView = new DOMTreeView(Rish, VirtualIcon, RealIcon,TreeViewState);
						Rish.OnRender += TreeView.OnRender;
						TreeView.OnSelection += OnSelection;
					}
					break;
				}
				case PlayModeStateChange.ExitingPlayMode:
				{
					if (Rish != null && TreeView != null)
					{
						TreeView.OnSelection -= OnSelection;
						Rish.OnRender -= TreeView.OnRender;
					}
					Selected = null;
					TreeView = null;
					break;
				}
			}
		}

		private void OnSelection(DOM selected)
		{
			Selected = selected;

			if (Selected == null)
			{
				SelectedProps = null;
				SelectedState = null;
			}
			else
			{
				var element = Selected.Element;
				var type = element?.GetType();
				
				var propsProperty = type?.GetProperty("Props");
				var stateProperty = type?.GetProperty("State");

				SelectedProps = (Props) propsProperty?.GetValue(element);
				SelectedState = (State) stateProperty?.GetValue(element);
			}
		}

		private void OnGUI ()
		{
			DoToolbar();
			DoTreeView(Selected == null ? 1 : sizeRatio);

			if (Selected == null) return;
			
			DrawInspector(sizeRatio);
			var resizer = DrawResizer();
			ProcessEvents(resizer, Event.current);
		}

		private void DoToolbar()
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		
		private void DoTreeView(float size)
		{
			var rect = new Rect(0, 0, position.width, (position.height * size) - resizerHeight);
			
			GUILayout.BeginArea(rect);
			TreeView?.OnGUI(GUILayoutUtility.GetRect (0, 100000, 0, 100000));
			GUILayout.EndArea();
		}
		
		private void DrawInspector(float size)
		{
			var rect = new Rect(0, (position.height * size) + resizerHeight, position.width, (position.height * (1 - size)) - resizerHeight);

			GUILayout.BeginArea(rect);
			InspectorScroll = GUILayout.BeginScrollView(InspectorScroll);
			if (SelectedProps != null)
			{
				var json = JsonUtility.ToJson(SelectedProps, true);
				GUILayout.Label("Props:", EditorStyles.boldLabel);
				GUILayout.Label(json);
				
				EditorGUILayout.Space();
			}

			if (SelectedState != null)
			{
				var json = JsonUtility.ToJson(SelectedProps, true);
				GUILayout.Label("State:", EditorStyles.boldLabel);
				GUILayout.Label(json);
				
				EditorGUILayout.Space();

			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}

		private Rect DrawResizer()
		{
			var rect = new Rect(0, (position.height * sizeRatio) - resizerHeight, position.width, resizerHeight * 2);

			GUILayout.BeginArea(new Rect(rect.position + (Vector2.up * resizerHeight), new Vector2(position.width, 2)), resizerStyle);
			GUILayout.EndArea();

			EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeVertical);

			return rect;
		}
		
		private void ProcessEvents(Rect rect, Event e)
		{
			switch (e.type)
			{
				case EventType.MouseDown:
					if (e.button == 0 && rect.Contains(e.mousePosition))
					{
						isResizing = true;
					}
					break;

				case EventType.MouseUp:
					isResizing = false;
					break;
			}
			
			Resize(e);
		}

		private void Resize(Event e)
		{
			if (!isResizing) return;
			
			sizeRatio = Mathf.Clamp(e.mousePosition.y / position.height, 0.2f, 0.8f);
			Repaint();
		}
	}
}
