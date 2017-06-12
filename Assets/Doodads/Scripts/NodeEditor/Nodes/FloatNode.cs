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
		public FloatNode (Vector2 position, float width, float height, Action<Node> OnClickRemoveNode, 
			Action<NodeInput> OnClickInput, Action<NodeOutput> OnClickOutput)
			: base(position, width, height, OnClickRemoveNode)
		{
			name = "Float Node";
			inputs.Add(new FloatInput(this, typeof(float), OnClickInput));
			outputs.Add(new FloatOutput(this, typeof(float), OnClickOutput));
		}
	}
}
