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
	public class TransformGrid
	{
		static float scaleX, scaleY, scaleZ;

		static Color red, green, blue;

		static Vector3[]x_plane;
		static Vector3[]y_plane;
		static Vector3[]z_plane;

		static bool showX = true, showY = true, showZ = true;
		static bool autoSnap = false;
		static bool customGridScale = true;
		static string autoSnapText = "Auto snap (on)";
		static bool initialized;

		static Transform targetTransform = null;

		[MenuItem("Tools/Show Snap Grid")]
		private static void ShowGrid ()
		{
			if (!initialized)
				Initialize();
		}

		private static void OnScene (SceneView sceneview)
		{
			//-------------------------------------
			// Info
			//-------------------------------------

			if (UpdateSnapValues())
				UpdateGrid(100); // call only if something has changed

			//-------------------------------------
			// Grid
			//-------------------------------------

			if (showX)
			{
				Handles.color = red;
				Handles.DrawLines(x_plane);
			}
			if (showY)
			{
				Handles.color = green;
				Handles.DrawLines(y_plane);
			}
			if (showZ)
			{
				Handles.color = blue;
				Handles.DrawLines(z_plane);
			}

			//-------------------------------------
			// Auto Snap
			//-------------------------------------

			if (autoSnap)
				SnapToGrid();

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
			GUILayout.Label("Grid settings");
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			//-------------------------------------
			// XYZ Buttons
			//-------------------------------------

			GUILayout.BeginHorizontal();

			GUI.backgroundColor = showX ? red : Color.grey;
			if (GUILayout.Button("X"))
			{
				showX = !showX;
			}
			GUI.backgroundColor = showY ? green : Color.grey;
			if (GUILayout.Button("Y"))
			{
				showY = !showY;
			}
			GUI.backgroundColor = showZ ? blue : Color.grey;
			if (GUILayout.Button("Z"))
			{
				showZ = !showZ;
			}

			GUILayout.EndHorizontal();

			//-------------------------------------
			// Snap Selected
			//-------------------------------------

			GUI.backgroundColor = Color.white;
			if (GUILayout.Button("Snap selected"))
			{
				SnapToGrid();
			}

			//-------------------------------------
			// Auto Snap
			//-------------------------------------

			autoSnap = GUILayout.Toggle(autoSnap, "Auto Snap");

			//-------------------------------------
			// End of window...
			//-------------------------------------

			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			GUILayout.EndArea();
			Handles.EndGUI();
		}

		private static void Initialize ()
		{
			red = new Color(219f / 255f, 62f / 255f, 29f / 255f, 237f / 255f);
			green = new Color(154f / 255f, 243f / 255f, 72f / 255f, 237f / 255f);
			blue = new Color(58f / 255f, 122f / 255f, 248f / 255f, 237f / 255f);

			SceneView.onSceneGUIDelegate -= OnScene;
			SceneView.onSceneGUIDelegate += OnScene;

			initialized = true;
		}

		private static void UpdateGrid (int count)
		{
			// force count to be even
			if ((count % 2) != 0)
				count++;

			int upperBound0 = count * 2 + 2;

			//-------------------------------------
			// X Plane
			//-------------------------------------

			x_plane = new Vector3[count * 4 + 4];

			for (int i = 0; i <= upperBound0; i += 2)
			{
				x_plane[i] = new Vector3(
					0,
					Offset(count, upperBound0, scaleY) + i * scaleY,
					Offset(count, upperBound0, scaleZ) + 0
				);
				x_plane[i + 1] = new Vector3(
					0,
					Offset(count, upperBound0, scaleY) + i * scaleY,
					Offset(count, upperBound0, scaleZ) + count * 2 * scaleZ
				);
			}

			for (int i = upperBound0; i <= count * 4 + 2; i += 2)
			{
				x_plane[i] = new Vector3(
					0,
					Offset(count, upperBound0, scaleY) + 0,
					Offset(count, upperBound0, scaleZ) + (i - upperBound0) * scaleZ
				);
				x_plane[i + 1] = new Vector3(
					0,
					Offset(count, upperBound0, scaleY) + count * 2 * scaleY,
					Offset(count, upperBound0, scaleZ) + (i - upperBound0) * scaleZ
				);
			}

			//-------------------------------------
			// Y Plane
			//-------------------------------------

			y_plane = new Vector3[count * 4 + 4];

			for (int i = 0; i <= upperBound0; i += 2)
			{
				y_plane[i] = new Vector3(
					Offset(count, upperBound0, scaleX) + i * scaleX,
					0,
					Offset(count, upperBound0, scaleZ) + 0
				);
				y_plane[i + 1] = new Vector3(
					Offset(count, upperBound0, scaleX) + i * scaleX,
					0,
					Offset(count, upperBound0, scaleZ) + count * 2 * scaleZ
				);
			}

			for (int i = upperBound0; i <= count * 4 + 2; i += 2)
			{
				y_plane[i] = new Vector3(
					Offset(count, upperBound0, scaleX) + 0,
					0,
					Offset(count, upperBound0, scaleZ) + (i - upperBound0) * scaleZ
				);
				y_plane[i + 1] = new Vector3(
					Offset(count, upperBound0, scaleX) + count * 2 * scaleX,
					0,
					Offset(count, upperBound0, scaleZ) + (i - upperBound0) * scaleZ
				);
			}

			//-------------------------------------
			// Z Plane
			//-------------------------------------

			z_plane = new Vector3[count * 4 + 4];

			for (int i = 0; i <= upperBound0; i += 2)
			{
				z_plane[i] = new Vector3(
					Offset(count, upperBound0, scaleX) + i * scaleX,
					Offset(count, upperBound0, scaleY) + 0,
					0
				);
				z_plane[i + 1] = new Vector3(
					Offset(count, upperBound0, scaleX) + i * scaleX,
					Offset(count, upperBound0, scaleY) + count * 2 * scaleY,
					0
				);
			}

			for (int i = upperBound0; i <= count * 4 + 2; i += 2)
			{
				z_plane[i] = new Vector3(
					Offset(count, upperBound0, scaleX) + 0,
					Offset(count, upperBound0, scaleY) + (i - upperBound0) * scaleY,
					0
				);
				z_plane[i + 1] = new Vector3(
					Offset(count, upperBound0, scaleX) + count * 2 * scaleX,
					Offset(count, upperBound0, scaleY) + (i - upperBound0) * scaleY,
					0
				);
			}
		}

		// Source: http://wiki.unity3d.com/index.php/SnapToGrid
		static void SnapToGrid ()
		{
			scaleX *= 2;
			scaleY *= 2;
			scaleZ *= 2;

			foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable))
			{
				t.position = new Vector3(
					Mathf.Round(t.position.x / scaleX) * scaleX,
					Mathf.Round(t.position.y / scaleY) * scaleY,
					Mathf.Round(t.position.z / scaleZ) * scaleZ
				);
			}
		}

		
		static float snapX_, snapY_, snapZ_; 

		// returns true if something has changed
		static bool UpdateSnapValues ()
		{
			scaleX = EditorPrefs.GetFloat("MoveSnapX") / 2;
			scaleY = EditorPrefs.GetFloat("MoveSnapY") / 2;
			scaleZ = EditorPrefs.GetFloat("MoveSnapZ") / 2;

			bool result = (snapX_ != scaleX || snapY_ != scaleY || scaleZ != snapZ_);

			snapX_ = scaleX;
			snapY_ = scaleY;
			snapZ_ = scaleZ;

			return result;
		}

		static float Offset (int count, int upperBound0, float scale)
		{
			return -((count * 4 + 2 - upperBound0) * scale) / 2;
		}
	}
}
