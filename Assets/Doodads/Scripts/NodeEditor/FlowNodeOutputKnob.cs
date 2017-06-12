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
	public class FlowNodeOutputKnob : NodeKnob
	{
		Action<FlowNodeOutputKnob> OnClickKnob;

		public FlowNodeOutputKnob (Node node, Type type, Action<FlowNodeOutputKnob> OnClickKnob) : base(node, type)
		{
			this.OnClickKnob = OnClickKnob;
		}

		public override void Draw ()
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			rect = GUILayoutUtility.GetRect(15, 15);
			if (GUI.Button(rect, ""))
				if (OnClickKnob != null)
					OnClickKnob(this);
			EditorGUILayout.EndHorizontal();
		}
	}
}
