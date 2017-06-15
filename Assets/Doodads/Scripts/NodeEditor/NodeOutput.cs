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
	public abstract class NodeOutput : NodeKnob
	{
		protected List<NodeInput> connectedInputs = new List<NodeInput>();
		Action<NodeOutput> OnClickKnob;

		public NodeOutput (Node node, Type type, Action<NodeOutput> OnClickKnob) : base(node, type)
		{
			this.OnClickKnob = OnClickKnob;
			name = "Output";
		}

		public override void Draw ()
		{
			EditorGUILayout.BeginHorizontal();
			DrawLabel();
			GUILayout.FlexibleSpace();
			rect = GUILayoutUtility.GetRect(15, 15);
			var color = GUI.color;
			GUI.color = TypeToColor(type);
			if (GUI.Button(rect, ""))
				if (OnClickKnob != null)
					OnClickKnob(this);
			GUI.color = color;
			EditorGUILayout.EndHorizontal();
		}

		abstract protected void DrawLabel ();

		public void Connect(NodeInput nodeInput)
		{
			connectedInputs.Add(nodeInput);
		}

		public void Disconnect ()
		{
			foreach (var nodeInput in connectedInputs)
			{
				Disconnect(nodeInput);
			}
		}

		internal void Disconnect (NodeInput nodeInput)
		{
			connectedInputs.Remove(nodeInput);
		}
	}

	public class FloatOutput : NodeOutput
	{
		float value;

		public FloatOutput (Node node, Type type, Action<NodeOutput> OnClickKnob) : base(node, type, OnClickKnob)
		{
		}

		protected override void DrawLabel ()
		{
			GUILayout.Label(name);
		}
	}
}
