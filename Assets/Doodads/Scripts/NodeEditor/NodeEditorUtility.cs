//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Doodads.Editor
{
	/// <summary>
	/// 
	/// </summary>
	public class NodeEditorUtility
	{
		public static void DrawConnection(NodeKnob outputKnob, NodeKnob inputKnob)
		{
			Handles.DrawBezier(
				inputKnob.node.rect.position + inputKnob.rect.center,
				outputKnob.node.rect.position + outputKnob.rect.center,
				inputKnob.node.rect.position + inputKnob.rect.center + Vector2.left * 50f,
				outputKnob.node.rect.position + outputKnob.rect.center + Vector2.right * 50f,
				NodeKnob.TypeToColor(outputKnob.type),
				null,
				4f
			);

			GUI.changed = true;
		}

		public static void DrawInputConnection (NodeKnob inputKnob, Event e)
		{
			Handles.DrawBezier(
					inputKnob.node.rect.position + inputKnob.rect.center,
					e.mousePosition,
					inputKnob.node.rect.position + inputKnob.rect.center + Vector2.left * 50f,
					e.mousePosition + Vector2.right * 50f,
					NodeKnob.TypeToColor(inputKnob.type),
					null,
					4f
				);

			GUI.changed = true;
		}

		public static void DrawOutputConnection (NodeKnob outputKnob, Event e)
		{
			Handles.DrawBezier(
					outputKnob.node.rect.position + outputKnob.rect.center,
					e.mousePosition,
					outputKnob.node.rect.position + outputKnob.rect.center + Vector2.right * 50f,
					e.mousePosition + Vector2.left * 50f,
					NodeKnob.TypeToColor(outputKnob.type),
					null,
					4f
				);

			GUI.changed = true;
		}
	}
}
