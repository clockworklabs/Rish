using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityObject = UnityEngine.Object;


namespace Rish.Editor
{
	public class DOMTreeView : TreeView
	{
		private Rish Rish { get; }
		
		public DOMTreeView (Rish rish, TreeViewState state) : base (state)
		{
			Rish = rish;
			Reload ();
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

		private void AddChildrenRecursive(DOM dom, TreeViewItem item, ICollection<TreeViewItem> rows)
		{
			var childCount = dom.ChildCount;

			item.children = new List<TreeViewItem> (childCount);
			for (var i = 0; i < childCount; ++i)
			{
				var child = dom.GetChild (i);
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

		private static TreeViewItem CreateItemForDOM(DOM dom) => new TreeViewItem(dom.ID, -1, $"{dom.Type.Name}: {{{dom.Key}}}");
		
		private DOM GetDOM(int id) => Rish?.Root?.Find(id);

		protected override void RowGUI (RowGUIArgs args)
		{
			Event evt = Event.current;
			extraSpaceBeforeIconAndLabel = 18f;

			// GameObject isStatic toggle 
			var dom = GetDOM(args.item.id);
			if (dom == null)
				return;

			Rect toggleRect = args.rowRect;
			toggleRect.x += GetContentIndent(args.item);
			toggleRect.width = 16f;

			// Ensure row is selected before using the toggle (usability)
			if (evt.type == EventType.MouseDown && toggleRect.Contains(evt.mousePosition))
				SelectionClick(args.item, false);
			
			EditorGUI.Toggle(toggleRect, dom.Type.IsSubclassOf(typeof(DOMElement)));

			// Text
			base.RowGUI(args);
		}

		// Selection

		protected override void SelectionChanged (IList<int> selectedIds)
		{
			Selection.instanceIDs = selectedIds.ToArray();
		}
	}
}
