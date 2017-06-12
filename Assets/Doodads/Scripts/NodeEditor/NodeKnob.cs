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
		public string name = "abstract knob";

		public Rect rect;

		public Type type;

		public Node node;

		public NodeKnob (Node node, Type type)
		{
			this.node = node;
			this.type = type;
			rect = new Rect(0, 0, 10f, 20f);
		}

		abstract public void Draw ();

		public enum Type
		{
			Flow,
			Number,
			String,
			Color,
			Vector,
			GameObject,
			MonoBehaviour,

		}

		public static Color TypeToColor(Type t)
		{
			switch (t)
			{
				case Type.Flow:
					return Color.white;
				case Type.Number:
					return Color.green;
				case Type.String:
					return Color.red;
				case Type.Color:
					return Color.magenta;
				case Type.Vector:
					return Color.yellow;
				case Type.GameObject:
					return Color.blue;
				case Type.MonoBehaviour:
					return Color.cyan;
				default:
					break;
			}

			return Color.black;
		}
	}
}
