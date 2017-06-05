//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Doodads.Examples.Editor
{
	/// <summary>
	/// 
	/// </summary>
	[CustomEditor(typeof(PathfindingTestGrid))]
	public class PathfindingTestGridEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI ()
		{
			DrawDefaultInspector();
			PathfindingTestGrid grid = (PathfindingTestGrid)target;
			
			if (GUILayout.Button("Refresh"))
			{
				grid.Refresh();
			}
		}
	}
}
