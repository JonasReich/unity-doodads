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
	public class FlowNodeInputKnob : NodeKnob
	{
		Action<FlowNodeInputKnob> OnClickKnob;

		public FlowNodeInputKnob (Node node, Type type, Action<FlowNodeInputKnob> OnClickKnob) : base(node, type)
		{
			this.OnClickKnob = OnClickKnob;
		}

		sealed override public void Draw ()
		{
			EditorGUILayout.BeginHorizontal();
			rect = GUILayoutUtility.GetRect(15, 15);
			if (GUI.Button(rect, ""))
				if (OnClickKnob != null)
					OnClickKnob(this);
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
	}
}
