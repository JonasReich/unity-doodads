//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Doodads.Editor
{
	/// <summary>
	/// Adds D + drag duplicate functionality to Editor. Only works on single objects
	/// </summary>
	[CanEditMultipleObjects, CustomEditor(typeof(Transform))]
	public class TransformEditor : DecoratorEditor
	{
		static Vector3 originalPosition;
		static GameObject gameObject, gameObject_copy;
		static bool shiftPressed, mouseDown, mouseDrag;


		public TransformEditor() : base("TransformInspector") { }


		public override void OnSceneGUI()
		{
			var go = ((Transform) target).gameObject;
			gameObject = (gameObject != go) ? go : gameObject;
			
			//mouseDrag = false;

			switch (Event.current.type)
			{
				case EventType.MouseDown:
					if (Event.current.button == 0)
					{
						originalPosition = go.transform.position;
						mouseDown = true;
					}
					break;
				case EventType.MouseUp:
					if (Event.current.button == 0)
					{
						mouseDown = false;
						mouseDrag = false;
					}
					break;
				case EventType.MouseDrag:
					if (Event.current.button == 0)
						mouseDrag = true;
					break;
				case EventType.KeyDown:
					if (Event.current.keyCode == KeyCode.D)
						shiftPressed = true;
					break;
				case EventType.KeyUp:
					if (Event.current.keyCode == KeyCode.D)
						shiftPressed = false;
					break;
				case EventType.DragUpdated:
					if (Event.current.button == 0)
						mouseDrag = true;
					break;
				case EventType.DragPerform:
					if (Event.current.button == 0)
						mouseDrag = false;
					break;
				case EventType.DragExited:
					if (Event.current.button == 0)
						mouseDrag = false;
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
		}
	}
}
