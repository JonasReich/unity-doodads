//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using UnityEditor;
using UnityEngine;

namespace JonasReich.UnityDoodats.Editor
{
	/// <summary>
	/// Adds a 'Group' command to Unity's scene editor
	/// </summary>
	public class GroupCommand
	{
		// Inspired by Unity3D Forum thread "Grouping objects in the Hierarchy?":
		// http://answers.unity3d.com/answers/1011241/view.html

		[MenuItem("GameObject/Group Selected %g")]
		private static void GroupSelected()
		{
			if (!Selection.activeTransform)
				return;

			var groupRoot = new GameObject(Selection.activeTransform.name + " Group");

			Undo.RegisterCreatedObjectUndo(groupRoot, "Group Selected");
			groupRoot.transform.SetParent(Selection.activeTransform.parent, false);

			foreach (var transform in Selection.transforms)
				Undo.SetTransformParent(transform, groupRoot.transform, "Group Selected");

			Selection.activeGameObject = groupRoot;
		}
	}
}
