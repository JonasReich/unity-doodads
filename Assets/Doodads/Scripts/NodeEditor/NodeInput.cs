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
	public abstract class NodeInput : NodeKnob
	{
		protected NodeOutput connectedOutput = null;
		Action<NodeInput> OnClickKnob;

		public NodeInput (Node node, Type type, Action<NodeInput> OnClickKnob) : base(node, type)
		{
			this.OnClickKnob = OnClickKnob;
			name = "Input";
		}

		sealed override public void Draw ()
		{
			EditorGUILayout.BeginHorizontal();
			rect = GUILayoutUtility.GetRect(15, 15);
			var color = GUI.color;
			GUI.color = TypeToColor(type);
			if (GUI.Button(rect, ""))
				if (OnClickKnob != null)
					OnClickKnob(this);
			GUI.color = color;
			GUILayout.FlexibleSpace();
			DrawLabel();
			EditorGUILayout.EndHorizontal();
		}

		abstract protected void DrawLabel ();

		public void Connect (NodeOutput connectedOutput)
		{
			this.connectedOutput = connectedOutput;
			connectedOutput.Connect(this);
		}

		public void DrawConnection ()
		{
			if (connectedOutput == null)
				return;

			NodeEditorUtility.DrawConnection(connectedOutput, this);
		}

		public void Disconnect ()
		{
			if (connectedOutput != null)
			{
				connectedOutput.Disconnect(this);
				connectedOutput = null;
			}
		}
	}

	public class FloatInput : NodeInput
	{
		float value;

		public FloatInput (Node node, Type type, Action<NodeInput> OnClickKnob) : base(node, type, OnClickKnob)
		{
		}

		protected override void DrawLabel ()
		{
			if (connectedOutput != null)
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label(name);
				GUILayout.FlexibleSpace();
				GUILayout.Label(value.ToString());
				EditorGUILayout.EndHorizontal();
			}
			else
				value = EditorGUILayout.FloatField(name, value);
		}
	}
}
