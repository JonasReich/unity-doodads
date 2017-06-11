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
	public class NodeInput : NodeKnob
	{
		public NodeInput (Node node, Type type, GUIStyle style, Action<NodeKnob> OnClickKnob) : base(node, type, style, OnClickKnob)
		{
		}

		public override void OnGUI ()
		{
			EditorGUILayout.BeginHorizontal();
			rect = GUILayoutUtility.GetRect(15, 15);
			if (GUI.Button(rect, ""))
				if (OnClickKnob != null)
					OnClickKnob(this);
			GUILayout.FlexibleSpace();
			GUILayout.Label(name);
			EditorGUILayout.EndHorizontal();
		}
	}
}
