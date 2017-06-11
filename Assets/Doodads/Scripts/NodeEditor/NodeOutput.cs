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
	public class NodeOutput : NodeKnob
	{
		public NodeOutput (Node node, Type type, GUIStyle style, Action<NodeKnob> OnClickKnob) : base(node, type, style, OnClickKnob)
		{
		}

		public override void OnGUI ()
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(name);
			GUILayout.FlexibleSpace();
			rect = GUILayoutUtility.GetRect(15, 15);
			if (GUI.Button(rect, ""))
				if (OnClickKnob != null)
					OnClickKnob(this);
			EditorGUILayout.EndHorizontal();
		}
	}
}
