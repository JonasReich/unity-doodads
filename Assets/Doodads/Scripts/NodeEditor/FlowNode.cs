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
	public class FlowNode : Node
	{
		FlowNode connectedFlowNode;
		FlowNodeInputKnob flowInput;
		FlowNodeOutputKnob flowOutput;

		public FlowNode (Vector2 position, float width, float height,
			Action<Node> OnClickRemoveNode, Action<FlowNodeInputKnob> OnClickFlowInput, Action<FlowNodeOutputKnob> OnClickFlowOutput) 
			: base(position, width, height, OnClickRemoveNode)
		{
			flowInput = new FlowNodeInputKnob(this, NodeKnob.Type.Flow, OnClickFlowInput);
			flowOutput = new FlowNodeOutputKnob(this, NodeKnob.Type.Flow, OnClickFlowOutput);
		}

		public override void Draw (int id)
		{
			DrawFlow();
			GUILayout.Space(5);
			GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
			GUILayout.Space(2);
			base.Draw(id);
		}

		void DrawFlow()
		{
			EditorGUILayout.BeginHorizontal();
			flowInput.Draw();
			flowOutput.Draw();
			EditorGUILayout.EndHorizontal();
		}

		public override void DrawConnections ()
		{
			base.DrawConnections();

			if (connectedFlowNode == null)
				return;

			NodeEditorUtility.DrawConnection(flowOutput, connectedFlowNode.flowInput);
		}

		public void ConnectFlow (FlowNode node)
		{
			connectedFlowNode = node;
			node.NodeRemovedCallback += OnRemoveConnectedFlowNode;
		}

		public void DisconnectFlow ()
		{
			connectedFlowNode.NodeRemovedCallback -= OnRemoveConnectedFlowNode;
			connectedFlowNode = null;
		}

		public void OnRemoveConnectedFlowNode(Node n)
		{
			DisconnectFlow();
		}

		override public void DisconnectAll ()
		{
			base.DisconnectAll();
		}
	}
}
