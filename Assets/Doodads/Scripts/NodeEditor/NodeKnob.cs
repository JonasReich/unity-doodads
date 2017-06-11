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

		public NodeKnob (Node node, Type type, GUIStyle style)
		{
			this.node = node;
			this.type = type;
			this.style = style;
			rect = new Rect(0, 0, 10f, 20f);
		}

		abstract public void OnGUI ();
		abstract public void Disconnect ();
		
		public enum Type { In, Out }
	}
}
