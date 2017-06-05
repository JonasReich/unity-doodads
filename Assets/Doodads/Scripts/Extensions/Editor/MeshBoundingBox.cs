//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Doodads.Editor
{
	/// <summary>
	/// Adds an AABB gizmo to MeshRenderers
	/// </summary>
	[CustomEditor(typeof(MeshRenderer)), CanEditMultipleObjects]
	public class MeshBoundingBox : UnityEditor.Editor
	{
		const string drawBoundingBox_prefkey = "DrawMeshRendererBoundingBox";
		static bool drawBoundingBox = true;

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI();

			GUILayout.Space(20);

			var drawBoundingBoxBefore = drawBoundingBox;
			//drawBoundingBox = GUILayout.Toggle(drawBoundingBox, "Draw Bounding Box", "Button");
			drawBoundingBox = GUILayout.Toggle(drawBoundingBox, "Draw Bounding Box");

			// Repaint on value change
			if (drawBoundingBox != drawBoundingBoxBefore)
				SceneView.RepaintAll();
		}

		private void OnSceneGUI ()
		{
			if (drawBoundingBox)
			{
				MeshRenderer mr = target as MeshRenderer;
				if (mr == null)
					return;

				Bounds bounds = new Bounds(mr.transform.position, Vector3.zero);
				foreach (Renderer renderer in mr.GetComponentsInChildren<Renderer>())
					bounds.Encapsulate(renderer.bounds);

				Handles.DrawWireCube(bounds.center, bounds.size);
			}
		}
		
		void OnEnable ()
		{
			if (EditorPrefs.HasKey(drawBoundingBox_prefkey))
				drawBoundingBox = EditorPrefs.GetBool(drawBoundingBox_prefkey);
		}

		void OnDisable ()
		{
			EditorPrefs.SetBool(drawBoundingBox_prefkey, drawBoundingBox);
		}

		void OnDestroy ()
		{
			EditorPrefs.SetBool(drawBoundingBox_prefkey, drawBoundingBox);
		}
	}
}
