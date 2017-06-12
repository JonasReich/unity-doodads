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
	public abstract class Node
	{
		public Rect rect;
		public string name = "Default Node";
		public bool isDragged;
		public bool isSelected;

		public List<NodeInput> inputs = new List<NodeInput>();
		public List<NodeOutput> outputs = new List<NodeOutput>();

		public GUIStyle style;

		public Action<Node> OnRemoveNode;

		public Node (Vector2 position, float width, float height, Action<Node> OnClickRemoveNode)
		{
			rect = new Rect(position.x, position.y, width, height);
			OnRemoveNode = OnClickRemoveNode;
		}

		public void Drag (Vector2 delta)
		{
			rect.position += delta;
		}

		public virtual void Draw (int id)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();

			foreach (var input in inputs)
				input.Draw();

			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical();

			foreach (var output in outputs)
				output.Draw();

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
						GUI.changed = true;
						isSelected = true;
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

		public virtual void DrawConnections ()
		{
			if (inputs.Count > 0)
				foreach (var input in inputs)
				{
					input.DrawConnection();
				}
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

		public virtual void DisconnectAll ()
		{
			foreach (var input in inputs)
				input.Disconnect();
			foreach (var output in outputs)
				output.Disconnect();
		}
	}
}
