using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Object = System.Object;

namespace RishUI.Editor
{
	public class DOMInspectorWindow : EditorWindow
	{
		private Texture2D VirtualIcon { get; set; }
		private Texture2D RealIcon { get; set; }
		
		private TreeViewState TreeViewState { get; set; }
		private DOMTreeView TreeView { get; set; }
		
		private Rish Rish { get; set; }
		
		private float SizeRatio { get; set; } = 0.8f; 
		private bool IsResizing { get; set; } 
 
		private float ResizerHeight { get; } = 1f; 
 
		private GUIStyle ResizerStyle { get; set; }
		
		private DOM Selected { get; set; }
		private PropertyInfo Props { get; set; }
		private PropertyInfo State { get; set; }
		private string SelectedPropsJson { get; set; }
		private string SelectedStateJson { get; set; }
		
		private Vector2 InspectorScroll { get; set; }
		
		private Texture2D ExpandIcon { get; set; }
		private Texture2D CollapseIcon { get; set; }

		[MenuItem("Rish/DOM Inspector")]
		private static void ShowWindow()
		{
			var window = GetWindow<DOMInspectorWindow>();
			window.titleContent = new GUIContent("DOM Inspector", Resources.Load<Texture2D>("react-icon"));
			window.Show();
		}

		private void OnEnable ()
		{
			VirtualIcon = Resources.Load<Texture2D>("react-icon");
			RealIcon = EditorGUIUtility.IconContent("GameObject Icon").image as Texture2D;
			ExpandIcon = EditorGUIUtility.IconContent("Toolbar Plus").image as Texture2D;
			CollapseIcon = EditorGUIUtility.IconContent("Toolbar Minus").image as Texture2D;

			ResizerStyle = new GUIStyle();
			ResizerStyle.normal.background = EditorGUIUtility.IconContent("d_AvatarBlendBackground").image as Texture2D;
			
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
						Rish.OnRender += OnRender;
						TreeView.OnSelection += OnSelection;
					}
					break;
				}
				case PlayModeStateChange.ExitingPlayMode:
				{
					if (Rish != null && TreeView != null)
					{
						TreeView.OnSelection -= OnSelection;
						Rish.OnRender -= OnRender;
					}

					Rish = null;
					Selected = null;
					TreeView = null;
					break;
				}
			}
		}

		private void OnRender(DOM dom)
		{
			TreeView.OnRender(dom);
			UpdateInspector();
			
			Repaint();
		}

		private void OnSelection(DOM selected)
		{
			Selected = selected;

			var element = Selected?.Element;
			var type = element?.GetType();
				
			Props = type?.GetProperty("Props");
			State = type?.GetProperty("State");
			
			UpdateInspector();
		}

		private void OnGUI ()
		{
			DoToolbar();
			DoTreeView(Selected == null ? 1 : SizeRatio);

			if (Selected == null) return;
			
			DrawInspector(SizeRatio);
			var resizer = DrawResizer();
			ProcessEvents(resizer, Event.current);
		}

		private void DoToolbar()
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbar);

			if (Selected != null)
			{
				if (Selected.Element is MonoBehaviour monoBehaviour)
				{
					if (GUILayout.Button("Select", EditorStyles.toolbarButton))
					{
						Selection.instanceIDs = new [] { monoBehaviour.gameObject.GetInstanceID() };
					}
				}
				
				if (Selected.ChildCount > 0)
				{
					if (GUILayout.Button(ExpandIcon, EditorStyles.toolbarButton))
					{
						TreeView.ExpandDown(Selected);
					}

					if (GUILayout.Button(CollapseIcon, EditorStyles.toolbarButton))
					{
						TreeView.CollapseDown(Selected);
					}
				}
			}
			
			GUILayout.FlexibleSpace();

			if (Rish?.Root != null)
			{
				if (GUILayout.Button(ExpandIcon, EditorStyles.toolbarButton))
				{
					TreeView.ExpandDown(Rish.Root);
				}

				if (GUILayout.Button(CollapseIcon, EditorStyles.toolbarButton))
				{
					TreeView.CollapseDown(Rish.Root);
				}
			}
			
			GUILayout.EndHorizontal();
		}
		
		private void DoTreeView(float size)
		{
			var rect = new Rect(0, EditorStyles.toolbar.fixedHeight, position.width, (position.height * size) - ResizerHeight - EditorStyles.toolbar.fixedHeight);
			
			GUILayout.BeginArea(rect);
			TreeView?.OnGUI(GUILayoutUtility.GetRect (0, 100000, 0, 100000));
			GUILayout.EndArea();
		}
		
		private void DrawInspector(float size)
		{
			var rect = new Rect(0, (position.height * size) + ResizerHeight, position.width, (position.height * (1 - size)) - ResizerHeight);

			GUILayout.BeginArea(rect);
			InspectorScroll = GUILayout.BeginScrollView(InspectorScroll);
			
			EditorGUILayout.LabelField($"ID: {Selected.ID}");
			EditorGUILayout.LabelField($"Style: {Selected.Style}");
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			if (!string.IsNullOrEmpty(SelectedPropsJson))
			{
				GUILayout.Label("Props:", EditorStyles.boldLabel);
				GUILayout.Label(SelectedPropsJson);
				
				EditorGUILayout.Space();
			}

			if (!string.IsNullOrEmpty(SelectedStateJson))
			{
				GUILayout.Label("State:", EditorStyles.boldLabel);
				GUILayout.Label(SelectedStateJson);
				
				EditorGUILayout.Space();

			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}

		private Rect DrawResizer()
		{
			var rect = new Rect(0, (position.height * SizeRatio) - ResizerHeight, position.width, ResizerHeight * 2);

			GUILayout.BeginArea(new Rect(rect.position + (Vector2.up * ResizerHeight), new Vector2(position.width, 2)), ResizerStyle);
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
						IsResizing = true;
					}
					break;

				case EventType.MouseUp:
					IsResizing = false;
					break;
			}
			
			Resize(e);
		}

		private void Resize(Event e)
		{
			if (!IsResizing) return;
			
			SizeRatio = Mathf.Clamp(e.mousePosition.y / position.height, 0.2f, 0.8f);
			Repaint();
		}

		private void UpdateInspector()
		{
			SelectedPropsJson = null;
			SelectedStateJson = null;

			var element = Selected?.Element;

			var props = Props?.GetValue(element);
			if (props != null)
			{
				SelectedPropsJson = JsonUtility.ToJson(props, true);
			}
			var state = (State) State?.GetValue(element);
			if (state != null)
			{
				SelectedStateJson = JsonUtility.ToJson(state, true);
			}
		}
	}
}
