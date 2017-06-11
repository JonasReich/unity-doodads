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

		sealed override public void OnGUI ()
		{
			EditorGUILayout.BeginHorizontal();
			rect = GUILayoutUtility.GetRect(15, 15);
			if (GUI.Button(rect, ""))
				if (OnClickKnob != null)
					OnClickKnob(this);
			GUILayout.FlexibleSpace();
			//GUILayout.Label(name);
			OnGUILabel();
			EditorGUILayout.EndHorizontal();
		}

		abstract protected void OnGUILabel ();

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

	public class FloatInput : NodeInput
	{
		float value;

		public FloatInput (Node node, Type type, Action<NodeInput> OnClickKnob) : base(node, type, OnClickKnob)
		{
		}

		protected override void OnGUILabel ()
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
