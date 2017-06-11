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
	public class NodeInput : NodeKnob
	{
		NodeOutput connectedOutput = null;
		Action<NodeInput> OnClickKnob;

		public NodeInput (Node node, Type type, Action<NodeInput> OnClickKnob) : base(node, type)
		{
			this.OnClickKnob = OnClickKnob;
		}

		override public void OnGUI ()
		{
			EditorGUILayout.BeginHorizontal();
			rect = GUILayoutUtility.GetRect(15, 15);
			if (GUI.Button(rect, ""))
				if (OnClickKnob != null)
					OnClickKnob(this);
			GUILayout.FlexibleSpace();
			GUILayout.Label(name);
			EditorGUILayout.EndHorizontal();
		}

		public void Connect(NodeOutput connectedOutput)
		{
			this.connectedOutput = connectedOutput;
			connectedOutput.Connect(this);
		}

		public void DrawConnection ()
		{
			if (connectedOutput == null)
				return;

			Handles.DrawBezier(
				node.rect.position + rect.center,
				connectedOutput.node.rect.position + connectedOutput.rect.center,
				node.rect.position + rect.center + Vector2.left * 50f,
				connectedOutput.node.rect.position + connectedOutput.rect.center - Vector2.left * 50f,
				Color.white,
				null,
				2f
			);
		}

		override public void Disconnect()
		{
			connectedOutput.Disconnect(this);
			connectedOutput = null;
		}
	}
}
