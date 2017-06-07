//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Doodads.Editor
{
	/// <summary>
	/// Draws an AABB gizmo around MeshRenderers
	/// </summary>
	[CustomEditor(typeof(MeshRenderer)), CanEditMultipleObjects]
	public class MeshBoundingBox : UnityEditor.Editor
	{
		const string drawBoundingBox_prefkey = "DrawMeshRendererBoundingBox";
		static bool drawBoundingBox = true;

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI();

			GUILayout.Space(5);
			GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
			GUILayout.Space(2);

			var drawBoundingBoxBefore = drawBoundingBox;
			drawBoundingBox = EditorGUILayout.Toggle("Draw Bounding Box", drawBoundingBox);

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

				if (Tools.current == Tool.Scale)
				{
					GUIStyle style = new GUIStyle();
					style.fontStyle = FontStyle.Bold;
					style.alignment = TextAnchor.MiddleCenter;
					bg_tex = bg_tex == null ? MakeTex(1, 1, Color.black) : bg_tex;
					style.normal.background = bg_tex;

					style.normal.textColor = Handles.xAxisColor;
					Handles.Label(bounds.center + Vector3.right * bounds.size.x / 2f, bounds.size.x.ToString(), style);
					
					style.normal.textColor = Handles.yAxisColor;
					Handles.Label(bounds.center + Vector3.up * bounds.size.y / 2f, bounds.size.y.ToString(), style);

					style.normal.textColor = Handles.zAxisColor;
					Handles.Label(bounds.center + Vector3.forward * bounds.size.z / 2f, bounds.size.z.ToString(), style);
				}
			}
		}
		
		// helper needed to draw the dimension label backgrounds
		Texture2D bg_tex;
		private Texture2D MakeTex (int width, int height, Color col)
		{
			Color[] pix = new Color[width * height];

			for (int i = 0; i < pix.Length; i++)
				pix[i] = col;

			Texture2D result = new Texture2D(width, height);
			result.SetPixels(pix);
			result.Apply();

			return result;
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
