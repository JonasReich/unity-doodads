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

		public NodeKnob input;
		public NodeKnob output;

		public GUIStyle style;
		public GUIStyle defaultNodeStyle;
		public GUIStyle selectedNodeStyle;

		public Action<Node> OnRemoveNode;

		public Node (Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<NodeKnob> OnClickInPoint, Action<NodeKnob> OnClickOutPoint, Action<Node> OnClickRemoveNode)
		{
			rect = new Rect(position.x, position.y, width, height);
			style = nodeStyle;
			input = new NodeInput(this, NodeKnob.Type.In, inPointStyle, OnClickInPoint);
			output = new NodeOutput(this, NodeKnob.Type.Out, outPointStyle, OnClickOutPoint);
			defaultNodeStyle = nodeStyle;
			selectedNodeStyle = selectedStyle;
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
							style = selectedNodeStyle;
						}
						else
						{
							GUI.changed = true;
							isSelected = false;
							style = defaultNodeStyle;
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
	}
}
