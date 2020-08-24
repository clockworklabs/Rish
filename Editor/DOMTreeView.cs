using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityObject = UnityEngine.Object;


namespace RishUI.Editor
{
	public class DOMTreeView : TreeView
	{
		public event Action<Node> OnSelection;
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

		public void OnRender(Node node)
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
		
		private void ExpandUp(Node node)
		{
			if (node == null)
			{
				return;
			}
			
			ExpandUp(node.Parent);
			
			SetExpanded(node.ID, true);
		}

		public void ExpandDown(Node node)
		{
			SetExpanded(node.ID, true);

			for (int i = 0, n = node.ChildCount; i < n; i++)
			{
				ExpandDown(node.GetChild(i));
			}
		}

		public void CollapseDown(Node node)
		{
			for (int i = 0, n = node.ChildCount; i < n; i++)
			{
				CollapseDown(node.GetChild(i));
			}
			
			SetExpanded(node.ID, false);
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

			var dom = Rish.DOM;
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

		private void AddChildrenRecursive(Node node, TreeViewItem item, ICollection<TreeViewItem> rows)
		{
			var childCount = node.ChildCount;

			item.children = new List<TreeViewItem> (childCount);
			for (var i = 0; i < childCount; ++i)
			{
				var child = node.GetChild (i);
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

		private TreeViewItem CreateItemForDOM(Node node)
		{
			var item = new TreeViewItem(node.ID, -1, $"{node.Type.Name}: {{{node.Key}}}");

			if (node.Type.IsSubclassOf(typeof(DOMElement)))
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
			var selected = Rish.DOM.Find(id);
			
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
