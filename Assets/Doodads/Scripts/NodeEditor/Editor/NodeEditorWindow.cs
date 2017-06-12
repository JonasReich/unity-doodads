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
	public class NodeEditorWindow : EditorWindow
	{
		private List<Node> nodes;

		private GUIStyle nodeStyle;
		private GUIStyle selectedNodeStyle;

		private NodeInput selectedNodeInput;
		private NodeOutput selectedNodeOutput;
		private FlowNodeInputKnob selectedFlowInput;
		private FlowNodeOutputKnob selectedFlowOutput;

		private Vector2 offset;
		private Vector2 drag;

		[MenuItem("Tools/Node Editor")]
		private static void OpenWindow ()
		{
			NodeEditorWindow window = GetWindow<NodeEditorWindow>();
			window.titleContent = new GUIContent("Node Editor");
		}

		private void OnEnable ()
		{
			nodeStyle = new GUIStyle();
			nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
			nodeStyle.border = new RectOffset(12, 12, 12, 12);

			selectedNodeStyle = new GUIStyle();
			selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
			selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
		}

		private void OnGUI ()
		{
			DrawGrid(20, 0.2f, Color.gray);
			DrawGrid(100, 0.4f, Color.gray);

			DrawNodes();
			DrawConnections();

			DrawConnectionLine(Event.current);

			ProcessNodeEvents(Event.current);
			ProcessEvents(Event.current);

			if (GUI.changed)
				Repaint();
		}

		private void DrawNodes ()
		{
			BeginWindows();
			if (nodes != null)
			{
				for (int i = 0; i < nodes.Count; i++)
				{
					nodes[i].rect = GUILayout.Window(i, nodes[i].rect, nodes[i].Draw, nodes[i].name);
					//GUI.Box(nodes[i].rect, nodes[i].name, nodes[i].style);
				}
			}
			EndWindows();
		}

		private void DrawConnections ()
		{
			if (nodes != null && nodes.Count > 0)
				foreach (var node in nodes)
				{
					node.DrawConnections();
				}
		}

		private void ProcessEvents (Event e)
		{
			drag = Vector2.zero;

			switch (e.type)
			{
				case EventType.MouseDown:
					if (e.button == 1)
					{
						ProcessContextMenu(e.mousePosition);
					}
					break;

				case EventType.MouseDrag:
					if (e.button == 0)
					{
						OnDrag(e.delta);
					}
					break;
			}
		}

		private void ProcessNodeEvents (Event e)
		{
			if (nodes != null)
			{
				for (int i = nodes.Count - 1; i >= 0; i--)
				{
					bool guiChanged = nodes[i].ProcessEvents(e);

					if (guiChanged)
					{
						GUI.changed = true;
					}
				}
			}
		}

		private void ProcessContextMenu (Vector2 mousePosition)
		{
			GenericMenu genericMenu = new GenericMenu();
			genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
			genericMenu.AddItem(new GUIContent("Add Flow Node"), false, () => OnClickAddFlowNode(mousePosition));
			genericMenu.ShowAsContext();
		}

		private void OnClickAddNode (Vector2 mousePosition)
		{
			if (nodes == null)
				nodes = new List<Node>();

			var newNode = new FloatNode(mousePosition, 200, 50, OnClickRemoveNode, OnClickDataNodeInput, OnClickDataNodeOutput);
			nodes.Add(newNode);
		}


		private void OnClickAddFlowNode (Vector2 mousePosition)
		{
			if (nodes == null)
				nodes = new List<Node>();

			var newNode = new FlowNode(mousePosition, 200, 50, OnClickRemoveNode, OnClickFlowInput, OnClickFlowOutput);
			nodes.Add(newNode);
		}

		//--------------------------
		// Flow Connections
		//--------------------------

		private void OnClickFlowInput (FlowNodeInputKnob inPoint)
		{
			selectedFlowInput = inPoint;

			TryEstablishFlowConnection();
		}

		private void OnClickFlowOutput (FlowNodeOutputKnob outPoint)
		{
			selectedFlowOutput = outPoint;

			TryEstablishFlowConnection();
		}

		void TryEstablishFlowConnection ()
		{
			if (selectedFlowInput != null && selectedFlowOutput != null)
			{
				if (selectedFlowInput.node != selectedFlowOutput.node)
					(selectedFlowOutput.node as FlowNode).ConnectFlow(selectedFlowInput.node as FlowNode);

				ClearFlowConnectionSelection();
			}
		}

		void ClearFlowConnectionSelection ()
		{
			selectedFlowInput = null;
			selectedFlowOutput = null;
		}

		//--------------------------
		// Data Connections
		//--------------------------

		private void OnClickDataNodeInput (NodeInput inPoint)
		{
			selectedNodeInput = inPoint;

			TryEstablishDataConnection();
		}

		private void OnClickDataNodeOutput (NodeOutput outPoint)
		{
			selectedNodeOutput = outPoint;

			TryEstablishDataConnection();
		}

		private void TryEstablishDataConnection ()
		{
			if (selectedNodeInput != null && selectedNodeOutput != null)
			{
				if (selectedNodeOutput.node != selectedNodeInput.node)
					selectedNodeInput.Connect(selectedNodeOutput);

				ClearConnectionSelection();
			}
		}
		
		private void ClearConnectionSelection ()
		{
			selectedNodeInput = null;
			selectedNodeOutput = null;
		}

		private void OnClickRemoveNode (Node node)
		{
			node.DisconnectAll();
			nodes.Remove(node);
		}

		private void OnDrag (Vector2 delta)
		{
			drag = delta;

			if (nodes != null)
			{
				for (int i = 0; i < nodes.Count; i++)
				{
					nodes[i].Drag(delta);
				}
			}

			GUI.changed = true;
		}

		private void DrawConnectionLine (Event e)
		{
			if (selectedNodeInput != null && selectedNodeOutput == null)
			{
				Handles.DrawBezier(
					selectedNodeInput.node.rect.position + selectedNodeInput.rect.center,
					e.mousePosition,
					selectedNodeInput.node.rect.position + selectedNodeInput.rect.center + Vector2.left * 50f,
					e.mousePosition - Vector2.left * 50f,
					Color.white,
					null,
					2f
				);

				GUI.changed = true;
			}

			if (selectedNodeOutput != null && selectedNodeInput == null)
			{
				Handles.DrawBezier(
					selectedNodeOutput.node.rect.position + selectedNodeOutput.rect.center,
					e.mousePosition,
					selectedNodeOutput.node.rect.position + selectedNodeOutput.rect.center - Vector2.left * 50f,
					e.mousePosition + Vector2.left * 50f,
					Color.white,
					null,
					2f
				);

				GUI.changed = true;
			}

			if (selectedFlowInput != null && selectedFlowOutput == null)
			{
				Handles.DrawBezier(
					selectedFlowInput.node.rect.position + selectedFlowInput.rect.center,
					e.mousePosition,
					selectedFlowInput.node.rect.position + selectedFlowInput.rect.center + Vector2.left * 50f,
					e.mousePosition - Vector2.left * 50f,
					Color.white,
					null,
					2f
				);

				GUI.changed = true;
			}

			if (selectedFlowInput == null && selectedFlowOutput != null)
			{
				Handles.DrawBezier(
					selectedFlowOutput.node.rect.position + selectedFlowOutput.rect.center,
					e.mousePosition,
					selectedFlowOutput.node.rect.position + selectedFlowOutput.rect.center - Vector2.left * 50f,
					e.mousePosition + Vector2.left * 50f,
					Color.white,
					null,
					2f
				);

				GUI.changed = true;
			}
		}

		private void DrawGrid (float gridSpacing, float gridOpacity, Color gridColor)
		{
			int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
			int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

			Handles.BeginGUI();
			Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

			offset += drag * 0.5f;
			Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

			for (int i = 0; i < widthDivs; i++)
			{
				Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
			}

			for (int j = 0; j < heightDivs; j++)
			{
				Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
			}

			Handles.color = Color.white;
			Handles.EndGUI();
		}
	}
}
