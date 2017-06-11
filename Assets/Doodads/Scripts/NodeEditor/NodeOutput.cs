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
	public class NodeOutput : NodeKnob
	{
		List<NodeInput> connectedInputs = new List<NodeInput>();
		Action<NodeOutput> OnClickKnob;

		public NodeOutput (Node node, Type type, Action<NodeOutput> OnClickKnob) : base(node, type)
		{
			this.OnClickKnob = OnClickKnob;
		}

		public override void OnGUI ()
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(name);
			GUILayout.FlexibleSpace();
			rect = GUILayoutUtility.GetRect(15, 15);
			if (GUI.Button(rect, ""))
				if (OnClickKnob != null)
					OnClickKnob(this);
			EditorGUILayout.EndHorizontal();
		}

		public void Connect(NodeInput nodeInput)
		{
			connectedInputs.Add(nodeInput);
		}

		override public void Disconnect ()
		{
			foreach (var nodeInput in connectedInputs)
				nodeInput.Disconnect();
		}

		internal void Disconnect (NodeInput nodeInput)
		{
			connectedInputs.Remove(nodeInput);
		}
	}
}
