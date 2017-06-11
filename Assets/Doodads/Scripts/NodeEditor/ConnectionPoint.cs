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
	public class ConnectionPoint
	{
		
		public Rect rect;

		public Type type;

		public Node node;

		public GUIStyle style;

		public Action<ConnectionPoint> OnClickConnectionPoint;

		public ConnectionPoint (Node node, Type type, GUIStyle style, Action<ConnectionPoint> OnClickConnectionPoint)
		{
			this.node = node;
			this.type = type;
			this.style = style;
			this.OnClickConnectionPoint = OnClickConnectionPoint;
			rect = new Rect(0, 0, 10f, 20f);
		}

		public void Draw ()
		{
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
				if (OnClickConnectionPoint != null)
				{
					OnClickConnectionPoint(this);
				}
			}
		}

		public enum Type { In, Out }
	}
}
