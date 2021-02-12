using System;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityObject = UnityEngine.Object;


namespace RishUI.Editor
{
	public class DOMTreeView : TreeView
	{
		public event Action<StateNode> OnSelection;
		private Rish Rish { get; }

		private Texture2D VirtualIcon { get; }
		private Texture2D RealIcon { get; }

		private Dictionary<int, EditorCoroutine> recentlyRendered { get; } = new Dictionary<int, EditorCoroutine>();
		
		public DOMTreeView (Rish rish, Texture2D vIcon, Texture2D rIcon, TreeViewState state) : base (state)
		{
			Rish = rish;
			VirtualIcon = vIcon;
			RealIcon = rIcon;
			
			Reload();
		}

		public void OnRender(StateNode node)
		{
			Reload();

			var id = node.ID;

			ExpandUp(node);
			
			if (recentlyRendered.ContainsKey(id))
			{
				EditorCoroutineUtility.StopCoroutine(recentlyRendered[id]);
			}
			recentlyRendered[id] = EditorCoroutineUtility.StartCoroutine(Highlight(id), this);
		}
		
		private void ExpandUp(StateNode node)
		{
			if (node == null)
			{
				return;
			}
			
			ExpandUp(node.Parent);
			
			SetExpanded(node.ID, true);
		}

		public void ExpandDown(StateNode stateNode)
		{
			SetExpanded(stateNode.ID, true);

			for (int i = 0, n = stateNode.ChildCount; i < n; i++)
			{
				ExpandDown(stateNode.GetChild(i));
			}
		}

		public void CollapseDown(StateNode stateNode)
		{
			for (int i = 0, n = stateNode.ChildCount; i < n; i++)
			{
				CollapseDown(stateNode.GetChild(i));
			}
			
			SetExpanded(stateNode.ID, false);
		}

		private IEnumerator Highlight(int id)
		{
			yield return new EditorWaitForSeconds(0.3f);

			recentlyRendered.Remove(id);

			Repaint();
		}

		protected override TreeViewItem BuildRoot()
		{
			return new TreeViewItem {id = 0, depth = -1};
		}

		protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
		{
			var rows = GetRows () ?? new List<TreeViewItem> (200);
			rows.Clear ();

			var dom = Rish.Root;
			if (dom != null)
			{
				var item = CreateItemForDOM(dom);
				root.AddChild(item);
				rows.Add(item);

				if (dom.ChildCount > 0)
				{
					if (IsExpanded(item.id))
					{
						AddChildrenRecursive(dom, item, rows);
					}
					else
					{
						item.children = CreateChildListForCollapsedParent();
					}
				}
			}
			
			SetupDepthsFromParentsAndChildren (root);
			
			return rows;
		}

		private void AddChildrenRecursive(StateNode stateNode, TreeViewItem item, ICollection<TreeViewItem> rows)
		{
			var childCount = stateNode.ChildCount;

			item.children = new List<TreeViewItem> (childCount);
			for (var i = 0; i < childCount; ++i)
			{
				var child = stateNode.GetChild (i);
				var childItem = CreateItemForDOM(child);
				item.AddChild (childItem);
				rows.Add (childItem);

				if (child.ChildCount <= 0) continue;
				if (IsExpanded (childItem.id))
				{
					AddChildrenRecursive(child, childItem, rows);
				}
				else
				{
					childItem.children = CreateChildListForCollapsedParent();
				}
			}
		}

		private TreeViewItem CreateItemForDOM(StateNode stateNode)
		{
			var item = new TreeViewItem(stateNode.ID, -1, $"{stateNode.Type.Name}: {{{stateNode.Key}}}");

			if (stateNode.Type.IsSubclassOf(typeof(UnityComponent)))
			{
				item.icon = RealIcon;
			}
			else
			{
				item.icon = VirtualIcon;
			}
			
			return item;
		}

		protected override void SelectionChanged(IList<int> selectedIds)
		{
			base.SelectionChanged(selectedIds);

			if (selectedIds.Count != 1)
			{
				return;
			}

			var id = selectedIds[0];
			var selected = Rish.Root.Find(id);
			
			OnSelection?.Invoke(selected);
		}
		
		protected override void RowGUI (RowGUIArgs args)
		{
			var iconRect = args.rowRect;
			iconRect.x += GetContentIndent(args.item);
			iconRect.width = 16f;

			var labelRect = args.rowRect;
			labelRect.x = iconRect.xMax;
			labelRect.width -= labelRect.x;
			
			GUI.DrawTexture(iconRect, args.item.icon, ScaleMode.ScaleToFit);
			GUI.Label(labelRect, args.item.displayName, recentlyRendered.ContainsKey(args.item.id) ? DefaultStyles.boldLabel : DefaultStyles.label);
		}
	}
}
