//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System;
using UnityEditor;
using UnityEngine;

namespace Doodads.Editor
{
	// ignore "TransformGridComponent is obsolete" warning
	// -> my own warning to prohibit misuse and bugs
#pragma warning disable 0618
	public class SnapGridWindow : UnityEditor.EditorWindow
	{
		//-------------------------------------
		// This class only contains the GUI
		//
		// All the runtime logic apart from creation/destruction 
		// is handled in SnapGridComponent
		//-------------------------------------

		static Color red, green, blue;

		[MenuItem("Tools/Snap Grid")]
		private static void ShowWindow ()
		{
			var window = GetWindow<SnapGridWindow>("Grid");
			window.Show();
		}

		private void OnEnable ()
		{
			ShowGrid();
		}

		private void OnDisable ()
		{
			HideGrid();
		}

		private void ShowGrid ()
		{
			red = SnapGridComponent.red = new Color(219f / 255f, 62f / 255f, 29f / 255f, 237f / 255f);
			green = SnapGridComponent.green = new Color(154f / 255f, 243f / 255f, 72f / 255f, 237f / 255f);
			blue = SnapGridComponent.blue = new Color(58f / 255f, 122f / 255f, 248f / 255f, 237f / 255f);

			SnapGridComponent.red.a = SnapGridComponent.green.a = SnapGridComponent.blue.a *= 0.25f;

			if (SnapGridComponent.instance != null)
				GameObject.DestroyImmediate(SnapGridComponent.instance.transform.parent.gameObject);

			var go_parent = new GameObject("Snap Grid Parent Container");
			go_parent.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSave;

			var go = new GameObject("Snap Grid");
			go.transform.SetParent(go_parent.transform);
			go.hideFlags = HideFlags.DontSave;

			SnapGridComponent.instance = go.AddComponent<SnapGridComponent>();

			EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;

			SceneView.RepaintAll();
		}

		private void HideGrid ()
		{
			if (SnapGridComponent.instance != null)
				GameObject.DestroyImmediate(SnapGridComponent.instance.transform.parent.gameObject);

			SnapGridComponent.instance = null;

			EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;

			SceneView.RepaintAll();
		}

		private void OnGUI ()
		{
			GUI.color = Color.white;
			GUILayout.BeginVertical();

			GUI.backgroundColor = SnapGridComponent.instance.show ? Color.white : Color.grey;
			var showText = SnapGridComponent.instance.show ? "o.o" : "-.-";
			if (GUILayout.Button(showText))
			{
				SnapGridComponent.instance.show = !SnapGridComponent.instance.show;
				SceneView.RepaintAll();
			}

			//-------------------------------------
			// XYZ Buttons
			//-------------------------------------

			GUI.backgroundColor = SnapGridComponent.instance.showX ? red : Color.grey;
			if (GUILayout.Button("X"))
			{
				SnapGridComponent.instance.showX = !SnapGridComponent.instance.showX;
				SceneView.RepaintAll();
			}
			GUI.backgroundColor = SnapGridComponent.instance.showY ? green : Color.grey;
			if (GUILayout.Button("Y"))
			{
				SnapGridComponent.instance.showY = !SnapGridComponent.instance.showY;
				SceneView.RepaintAll();
			}
			GUI.backgroundColor = SnapGridComponent.instance.showZ ? blue : Color.grey;
			if (GUILayout.Button("Z"))
			{
				SnapGridComponent.instance.showZ = !SnapGridComponent.instance.showZ;
				SceneView.RepaintAll();
			}

			//-------------------------------------
			// Snap Selected
			//-------------------------------------

			GUI.backgroundColor = Color.white;
			if (GUILayout.Button("Snap Selected"))
				SnapGridComponent.instance.SnapToGrid();

			//-------------------------------------
			// Auto Snap
			//-------------------------------------

			SnapGridComponent.instance.autoSnap = GUILayout.Toggle(SnapGridComponent.instance.autoSnap, "Auto Snap");

			//-------------------------------------
			// End of window...
			//-------------------------------------

			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}

		void OnPlaymodeStateChanged ()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode && EditorApplication.isPlaying)
				Repaint();
		}
#pragma warning restore 0618
	}
}
