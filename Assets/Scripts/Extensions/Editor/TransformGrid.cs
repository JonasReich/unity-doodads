//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityDoodats.Editor
{
	/// <summary>
	/// 
	/// </summary>
	[CustomEditor(typeof(Transform))]
	public class TransformGridEditor : UnityEditor.Editor
	{
		Vector3[]x_plane;
		Vector3[]y_plane;
		Vector3[]z_plane;


		private void OnSceneGUI ()
		{
			Initialize(20, 1f);

			var targetTransform = target as Transform;
			var targetPosition = targetTransform.position;

			Handles.color = Color.red;

			Handles.DrawLines(x_plane);
		}

		private void Initialize (int count, float scale)
		{
			// force count to be even
			if ((count % 2) != 0) count++;

			x_plane = new Vector3[count * 4 + 4];

			int upperBound0 = count * 2 + 2;
			float offset = -((count * 4 + 2 - upperBound0) * scale) / 2;
			
			for (int i = 0; i <= upperBound0; i += 2)
			{
				x_plane[i] = new Vector3(
					0, 
					offset + i * scale,
					offset + 0
				);
				x_plane[i + 1] = new Vector3(
					0, 
					offset + i * scale, 
					offset + count * 2 * scale
				);
			}

			for (int i = upperBound0; i <= count * 4 + 2; i += 2)
			{
				x_plane[i] = new Vector3(
					0,
					offset + 0,
					offset + (i-upperBound0) * scale
				);
				x_plane[i + 1] = new Vector3(
					0, 
					offset + count * 2 * scale,
					offset + (i-upperBound0) * scale
				);
			}
		}
	}
}
