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
	public class Connection
	{
		public NodeKnob inPoint;
		public NodeKnob outPoint;
		public Action<Connection> OnClickRemoveConnection;

		public Connection (NodeKnob inPoint, NodeKnob outPoint, Action<Connection> OnClickRemoveConnection)
		{
			this.inPoint = inPoint;
			this.outPoint = outPoint;
			this.OnClickRemoveConnection = OnClickRemoveConnection;
		}

		public void Draw ()
		{
			Handles.DrawBezier(
				inPoint.node.rect.position + inPoint.rect.center,
				outPoint.node.rect.position + outPoint.rect.center,
				inPoint.node.rect.position + inPoint.rect.center + Vector2.left * 50f,
				outPoint.node.rect.position + outPoint.rect.center - Vector2.left * 50f,
				Color.white,
				null,
				2f
			);
		}
	}
}
