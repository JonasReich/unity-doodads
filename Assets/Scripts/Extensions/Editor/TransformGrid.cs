//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityDoodats.Editor
{
// ignore "TransformGridComponent is obsolete" warning
// -> my own warning to prohibit misuse and bugs
#pragma warning disable 0618
	public class TransformGrid
	{
		//-------------------------------------
		// This class only contains the GUI
		//
		// All the runtime logic apart from creation/destruction 
		// is handled in TransformGridComponent
		//-------------------------------------

		static string currentScene;

		[MenuItem("Tools/Show Snap Grid")]
		private static void ShowGrid ()
		{
			TransformGridComponent.red = new Color(219f / 255f, 62f / 255f, 29f / 255f, 237f / 255f);
			TransformGridComponent.green = new Color(154f / 255f, 243f / 255f, 72f / 255f, 237f / 255f);
			TransformGridComponent.blue = new Color(58f / 255f, 122f / 255f, 248f / 255f, 237f / 255f);
			
			SceneView.onSceneGUIDelegate -= OnScene;
			SceneView.onSceneGUIDelegate += OnScene;
			SceneView.RepaintAll();

			EditorApplication.hierarchyWindowChanged -= HierarchyWindowChanged;
			EditorApplication.hierarchyWindowChanged += HierarchyWindowChanged;
			currentScene = EditorApplication.currentScene;

			if (TransformGridComponent.instance != null)
				GameObject.DestroyImmediate(TransformGridComponent.instance.transform.parent.gameObject);

			var go_parent = new GameObject("Transform Grid Parent");
			go_parent.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;

			var go = new GameObject("Transform Grid");
			go.transform.SetParent(go_parent.transform);
			

			TransformGridComponent.instance = go.AddComponent<TransformGridComponent>();
		}

		private static void HierarchyWindowChanged ()
		{
			if(currentScene != EditorApplication.currentScene)
			{
				currentScene = EditorApplication.currentScene;
				ShowGrid();
			}
		}

		private static void HideGrid ()
		{
			SceneView.onSceneGUIDelegate -= OnScene;
			SceneView.RepaintAll();

			EditorApplication.hierarchyWindowChanged -= HierarchyWindowChanged;
			currentScene = "";


			if (TransformGridComponent.instance != null)
				GameObject.DestroyImmediate(TransformGridComponent.instance.transform.parent.gameObject);

			TransformGridComponent.instance = null;
		}

		private static void OnScene (SceneView sceneview)
		{
			
			//-------------------------------------
			// GUI
			//-------------------------------------

			Handles.BeginGUI();
			GUILayout.BeginArea(new Rect(10, 10, 150, 120));
			var rect = EditorGUILayout.BeginVertical();
			GUI.Box(rect, GUIContent.none);

			GUI.color = Color.white;

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Snap Grid Settings");
			GUILayout.FlexibleSpace();

			GUI.color = Color.red;
			if (GUILayout.Button("X"))
			{
				HideGrid();
			}
			GUI.color = Color.white;

			GUILayout.EndHorizontal();

			//-------------------------------------
			// XYZ Buttons
			//-------------------------------------

			GUILayout.BeginHorizontal();

			GUI.backgroundColor = TransformGridComponent.showX ? TransformGridComponent.red : Color.grey;
			if (GUILayout.Button("X"))
			{
				TransformGridComponent.showX = !TransformGridComponent.showX;
			}
			GUI.backgroundColor = TransformGridComponent.showY ? TransformGridComponent.green : Color.grey;
			if (GUILayout.Button("Y"))
			{
				TransformGridComponent.showY = !TransformGridComponent.showY;
			}
			GUI.backgroundColor = TransformGridComponent.showZ ? TransformGridComponent.blue : Color.grey;
			if (GUILayout.Button("Z"))
			{
				TransformGridComponent.showZ = !TransformGridComponent.showZ;
			}

			GUILayout.EndHorizontal();

			//-------------------------------------
			// Snap Selected
			//-------------------------------------

			GUI.backgroundColor = Color.white;
			if (GUILayout.Button("Snap selected"))
			{
				TransformGridComponent.SnapToGrid();
			}

			//-------------------------------------
			// Auto Snap
			//-------------------------------------

			TransformGridComponent.autoSnap = GUILayout.Toggle(TransformGridComponent.autoSnap, "Auto Snap");

			//-------------------------------------
			// End of window...
			//-------------------------------------

			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			GUILayout.EndArea();
			Handles.EndGUI();
		}
#pragma warning restore 0618
	}
}
