//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Doodads.Editor
{
	/// <summary>
	/// 
	/// </summary>
	public class Node
	{
		public Rect rect;
		public string name = "Default Node";
		public bool isDragged;
		public bool isSelected;

		public NodeInput input;
		public NodeOutput output;

		public GUIStyle style;

		public Action<Node> OnRemoveNode;

		public Node (Vector2 position, float width, float height, Action<NodeInput> OnClickInPoint, Action<NodeOutput> OnClickOutPoint, Action<Node> OnClickRemoveNode)
		{
			rect = new Rect(position.x, position.y, width, height);
			input = new NodeInput(this, NodeKnob.Type.In, OnClickInPoint);
			output = new NodeOutput(this, NodeKnob.Type.Out, OnClickOutPoint);
			OnRemoveNode = OnClickRemoveNode;
		}

		public void Drag (Vector2 delta)
		{
			rect.position += delta;
		}
		
		public void OnGUI (int id)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			// Inputs
			input.OnGUI();

			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical();
			// Outputs
			output.OnGUI();

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

			//GUI.DragWindow();
		}

		public bool ProcessEvents (Event e)
		{
			switch (e.type)
			{
				case EventType.MouseDown:
					if (e.button == 0)
					{
						if (rect.Contains(e.mousePosition))
						{
							isDragged = true;
							GUI.changed = true;
							isSelected = true;
						}
						else
						{
							GUI.changed = true;
							isSelected = false;
						}
					}

					if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
					{
						ProcessContextMenu();
						e.Use();
					}
					break;

				case EventType.MouseUp:
					isDragged = false;
					break;

				case EventType.MouseDrag:
					if (e.button == 0 && isDragged)
					{
						Drag(e.delta);
						e.Use();
						return true;
					}
					break;
			}

			return false;
		}

		private void ProcessContextMenu ()
		{
			GenericMenu genericMenu = new GenericMenu();
			genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
			genericMenu.ShowAsContext();
		}

		private void OnClickRemoveNode ()
		{
			if (OnRemoveNode != null)
			{
				OnRemoveNode(this);
			}
		}

		public void Disconnect ()
		{
			input.Disconnect();
			output.Disconnect();
		}
	}
}
