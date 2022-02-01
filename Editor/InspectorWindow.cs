using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

namespace RishUI.Editor
{
	public class InspectorWindow : EditorWindow
	{
		private Texture2D VirtualIcon { get; set; }
		private Texture2D RealIcon { get; set; }
		
		private TreeViewState TreeViewState { get; set; }
		private DOMTreeView TreeView { get; set; }
		
		private Rish Rish { get; set; }
		
		private float SizeRatio { get; set; } 
		private bool IsResizing { get; set; } 
 
		private float ResizerHeight { get; } = 1f; 
 
		private GUIStyle ResizerStyle { get; set; }
		
		private RishNode Selected { get; set; }
		private PropertyInfo Props { get; set; }
		private PropertyInfo State { get; set; }
		private string SelectedPropsJson { get; set; }
		private string SelectedStateJson { get; set; }

		private bool DebugID { get; set; }
		private bool DebugTransform { get; set; } = true;
		private bool DebugExtras { get; set; }
		private bool AbstractTransform { get; set; }
		private bool GlobalTransform { get; set; }
		
		private bool PrevPointerOver { get; set; }
		private bool PrevPointerDown { get; set; }
		
		private Vector2 InspectorScroll { get; set; }
		
		private Texture2D ExpandIcon { get; set; }
		private Texture2D CollapseIcon { get; set; }

		[MenuItem("Window/Rish/Inspector")]
		private static void ShowWindow()
		{
			var window = GetWindow<InspectorWindow>();
			window.titleContent = new GUIContent("Rish Inspector", Resources.Load<Texture2D>("react-icon"));
			window.Show();
		}

		private void OnEnable ()
		{
			VirtualIcon = Resources.Load<Texture2D>("react-icon");
			RealIcon = EditorGUIUtility.IconContent("GameObject Icon").image as Texture2D;
			ExpandIcon = EditorGUIUtility.IconContent("Toolbar Plus").image as Texture2D;
			CollapseIcon = EditorGUIUtility.IconContent("Toolbar Minus").image as Texture2D;

			ResizerStyle = new GUIStyle
			{
				normal = {background = EditorGUIUtility.IconContent("d_AvatarBlendBackground").image as Texture2D}
			};

			TreeViewState = new TreeViewState();

			SizeRatio = 1 - 300 / position.height;
			
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

		private void OnRender(RishNode node)
		{
			TreeView.OnRender(node);

			if (Selected != null)
			{
				if (!Selected.Active)
				{
					OnSelection(null);
				}
				else
				{
					UpdateInspector();
				}
			}

			Repaint();
		}

		private void OnSelection(RishNode selected)
		{
			Selected = selected;

			var element = Selected?.Component;
			var type = element?.GetType();
				
			Props = type?.GetProperty("Props", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			State = type?.GetProperty("State", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			
			UpdateInspector();
		}
		
		void Update()
		{
			if (Selected == null) return;
			
			var pointerOver = RishUtils.HasPointerOver(Selected.Component);
			var pointerDown = RishUtils.HasPointerDown(Selected.Component);
			if (pointerOver != PrevPointerOver || pointerDown != PrevPointerDown)
			{
				Repaint();
			}

			PrevPointerOver = pointerOver;
			PrevPointerDown = pointerDown;
		}

		private void OnGUI ()
		{
			DoTreeView(Selected == null ? 1 : SizeRatio);

			DrawInspector(SizeRatio);
			var resizer = DrawResizer();
			ProcessEvents(resizer, Event.current);
		}
		
		private void DoTreeView(float size)
		{
			DoTreeToolbar();
			
			var rect = new Rect(0, EditorStyles.toolbar.fixedHeight, position.width, (position.height * size) - ResizerHeight - EditorStyles.toolbar.fixedHeight);
			
			GUILayout.BeginArea(rect);
			TreeView?.OnGUI(GUILayoutUtility.GetRect (0, 100000, 0, 100000));
			GUILayout.EndArea();
		}

		private void DoTreeToolbar()
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbar);

			if (Selected != null)
			{
				if (Selected.Component is MonoBehaviour monoBehaviour)
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

			if (Rish?.RootNode != null)
			{
				if (GUILayout.Button(ExpandIcon, EditorStyles.toolbarButton))
				{
					TreeView.ExpandDown(Rish.RootNode);
				}

				if (GUILayout.Button(CollapseIcon, EditorStyles.toolbarButton))
				{
					TreeView.CollapseDown(Rish.RootNode);
				}
			}
			
			GUILayout.EndHorizontal();
		}
		
		private void DrawInspector(float size)
		{
			var rect = new Rect(0, (position.height * size) + ResizerHeight, position.width, (position.height * (1 - size)) - ResizerHeight);
			
			GUILayout.BeginArea(rect);
			
			DoInspectorToolbar();
			
			if (Selected != null)
			{
				InspectorScroll = GUILayout.BeginScrollView(InspectorScroll);

				if (DebugTransform)
				{
					TransformDebug.Draw(rect.width, Selected, AbstractTransform, GlobalTransform);
					EditorGUILayout.Space();
				}

				if (DebugID)
				{
					if (Selected.Component is UnityComponent unityComponent)
					{
						GUILayout.Label($"ID: {Selected.ID} ({unityComponent.GetInstanceID()})");
					}
					else
					{
						GUILayout.Label($"ID: {Selected.ID}");
					}
				}

				GUILayout.Label($"Pointer Over: {RishUtils.HasPointerOver(Selected.Component)}");
				GUILayout.Label($"Pointer Down: {RishUtils.HasPointerDown(Selected.Component)}");
				
				if (DebugExtras && Selected.Component is IExtraInspection extra)
				{
					GUILayout.Label($"{extra.GetExtraDescription()}");
				}
				
				EditorGUILayout.Space();
				
				if (!string.IsNullOrEmpty(SelectedPropsJson) && SelectedPropsJson != "{}")
				{
					GUILayout.Label("Props:", EditorStyles.boldLabel);
					GUILayout.Label(SelectedPropsJson);
					
					EditorGUILayout.Space();
				}

				if (!string.IsNullOrEmpty(SelectedStateJson) && SelectedStateJson != "{}")
				{
					GUILayout.Label("State:", EditorStyles.boldLabel);
					GUILayout.Label(SelectedStateJson);
					
					EditorGUILayout.Space();

				}
				GUILayout.EndScrollView();
			}
			GUILayout.EndArea();
		}

		private void DoInspectorToolbar()
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbar);

			DebugID = GUILayout.Toggle(DebugID, "ID", EditorStyles.toolbarButton);
			DebugTransform = GUILayout.Toggle(DebugTransform, "Transform", EditorStyles.toolbarButton);
			DebugExtras = GUILayout.Toggle(DebugExtras, "Extras", EditorStyles.toolbarButton);

			GUILayout.FlexibleSpace();

			GUI.enabled = DebugTransform;
			AbstractTransform = GUILayout.Toggle(AbstractTransform, "Abstract", EditorStyles.toolbarButton);
			GlobalTransform = GUILayout.Toggle(GlobalTransform, "Global", EditorStyles.toolbarButton);
			GUI.enabled = true;
			
			GUILayout.EndHorizontal();
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

			var min = 150 / position.height;
			var max = 1 - 250 / position.height;
			
			SizeRatio = Mathf.Clamp(e.mousePosition.y / position.height, min, max);
			Repaint();
		}

		private void UpdateInspector()
		{
			SelectedPropsJson = null;
			SelectedStateJson = null;

			var element = Selected?.Component;
			
			var props = Props?.GetValue(element);
			if (props != null)
			{
				SelectedPropsJson = JsonUtility.ToJson(props, true);
			}
			var state = State?.GetValue(element);
			if (state != null)
			{
				SelectedStateJson = JsonUtility.ToJson(state, true);
			}
		}
	}
}
