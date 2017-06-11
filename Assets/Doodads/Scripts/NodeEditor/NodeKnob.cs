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
	abstract public class NodeKnob
	{
		public string name = "Node Knob name";

		public Rect rect;

		public Type type;

		public Node node;

		public GUIStyle style;

		public Action<NodeKnob> OnClickKnob;

		public NodeKnob (Node node, Type type, GUIStyle style, Action<NodeKnob> OnClickKnob)
		{
			this.node = node;
			this.type = type;
			this.style = style;
			this.OnClickKnob = OnClickKnob;
			rect = new Rect(0, 0, 10f, 20f);
		}

		abstract public void OnGUI ();

		/*
		---------------------------------------
		Old OnGUI
		---------------------------------------

		rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;

		switch (type)
		{
			case Type.In:
				rect.x = node.rect.x - rect.width + 8f;
				break;

			case Type.Out:
				rect.x = node.rect.x + node.rect.width - 8f;
				break;
		}

		if (GUI.Button(rect, "", style))
		{
			if (OnClickKnob != null)
			{
				OnClickKnob(this);
			}
		}*/

		public enum Type { In, Out }
	}
}
