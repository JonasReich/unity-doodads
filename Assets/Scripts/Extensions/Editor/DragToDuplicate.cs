//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityDoodats.Editor
{
	/// <summary>
	/// 
	/// </summary>
	[CustomEditor(typeof(GameObject))]
	public class DragToDuplicate : UnityEditor.Editor
	{
		static Vector3 originalPosition;
		static GameObject gameObject, gameObject_copy;
		static bool shiftPressed, mouseDown, mouseDrag;

		private void OnSceneGUI ()
		{
			var go = target as GameObject;
			gameObject = (gameObject != go) ? go : gameObject;

			if (go == null)
				return;

			//mouseDrag = false;

			switch (Event.current.type)
			{
				case EventType.MouseDown:
					originalPosition = go.transform.position;
					mouseDown = true;
					break;
				case EventType.MouseUp:
					mouseDown = false;
					mouseDrag = false;
					break;
				case EventType.MouseDrag:
					mouseDrag = true;
					break;
				case EventType.KeyDown:
					if(Event.current.keyCode == KeyCode.D)
					shiftPressed = true;
					break;
				case EventType.KeyUp:
					if (Event.current.keyCode == KeyCode.D)
						shiftPressed = false;
					break;
				case EventType.DragUpdated:

					mouseDrag = true;
					break;
				case EventType.DragPerform:

					mouseDrag = false;
					break;
				case EventType.DragExited:
					mouseDrag = false;
					break;
				default:
					break;
			}

			if (gameObject_copy == null && shiftPressed && mouseDrag)
			{
				gameObject_copy = Instantiate(gameObject);
				gameObject_copy.name = gameObject.name;
				gameObject_copy.transform.position = originalPosition;
				Undo.RegisterCreatedObjectUndo(gameObject_copy, "Create Object");
			}

			if (gameObject_copy != null && !mouseDown)
			{
				gameObject_copy.transform.position = gameObject.transform.position;
				gameObject.transform.position = originalPosition;
				Selection.SetActiveObjectWithContext(gameObject_copy, gameObject_copy);

				gameObject_copy = null;
			}

			Handles.BeginGUI();
			GUILayout.BeginArea(new Rect(10, 10, 150, 120));
			var rect = EditorGUILayout.BeginVertical();
			GUI.Box(rect, GUIContent.none);

			GUI.color = Color.white;

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("shiftPressed: " + shiftPressed);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("mouseDown: " + mouseDown);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("mouseDrag: " + mouseDrag);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			GUILayout.EndArea();
			Handles.EndGUI();
		}
	}
}
