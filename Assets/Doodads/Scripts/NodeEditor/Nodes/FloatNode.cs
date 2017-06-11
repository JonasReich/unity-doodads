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
	public class FloatNode : Node
	{
		public FloatNode (Vector2 position, float width, float height, Action<NodeInput> OnClickInPoint, Action<NodeOutput> OnClickOutPoint, Action<Node> OnClickRemoveNode) : base(position, width, height, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode)
		{
			name = "Float Node";
			inputs.Add(new FloatInput(this, typeof(float), OnClickInPoint));
			outputs.Add(new FloatOutput(this, typeof(float), OnClickOutPoint));
		}
	}
}
