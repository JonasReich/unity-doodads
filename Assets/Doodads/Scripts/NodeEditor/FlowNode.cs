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
			flowInput = new FlowNodeInputKnob(this, typeof(FlowNode), OnClickFlowInput);
			flowOutput = new FlowNodeOutputKnob(this, typeof(FlowNode), OnClickFlowOutput);
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

			Handles.DrawBezier(
				rect.position + flowOutput.rect.center,
				connectedFlowNode.rect.position + connectedFlowNode.flowInput.rect.center,
				rect.position + flowOutput.rect.center + Vector2.right * 50f,
				connectedFlowNode.rect.position + connectedFlowNode.flowInput.rect.center - Vector2.right * 50f,
				Color.white,
				null,
				2f
			);
		}

		public void ConnectFlow (FlowNode node)
		{
			connectedFlowNode = node;
			node.OnRemoveNode += OnRemoveConnectedFlowNode;
		}

		public void DisconnectFlow ()
		{
			connectedFlowNode.OnRemoveNode -= OnRemoveConnectedFlowNode;
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
