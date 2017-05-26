//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityDoodats.Editor;

namespace UnityDoodats.Editor
{
#if UNITY_EDITOR
	/// <summary>
	/// 
	/// </summary>
	[ExecuteInEditMode]
	[System.Obsolete("Don't use this outside of Editor scripts")]
	public class TransformGridComponent : MonoBehaviour
	{
		public static TransformGridComponent instance;

		//-------------------------------------
		// Settings
		//-------------------------------------

		public static float scaleX, scaleY, scaleZ;
		public static Color red, green, blue;

		public static Vector3[]x_plane;
		public static Vector3[]y_plane;
		public static Vector3[]z_plane;

		public static bool showX = true, showY = true, showZ = true;
		public static bool autoSnap = true;
		
				
		Material material;
		

		void LoadMaterial ()
		{
			material = Resources.Load("GridMaterial", typeof(Material)) as Material;
		}

		void OnDrawGizmos ()
		{

			if (instance != this)
				gameObject.hideFlags = HideFlags.None;
			

			//-------------------------------------
			// Info
			//-------------------------------------

			if (UpdateSnapValues())
				UpdateGrid(100); // call only if something has changed

			//-------------------------------------
			// Auto Snap
			//-------------------------------------

			if (autoSnap)
				SnapToGrid();

			
			if (showX)
				DrawLines(x_plane, red);

			if (showY)
				DrawLines(y_plane, green);

			if (showZ)
				DrawLines(z_plane, blue);


			/*
			GL.Begin(GL.LINES);
			lineMat.SetPass(0);
			GL.Color(new Color(0f, 0f, 0f, 1f));
			GL.Vertex3(0f, 0f, 0f);
			GL.Vertex3(1f, 1f, 1f);
			GL.End();
			*/
		}

		void DrawLines (Vector3[] points, Color color)
		{
			if (material == null)
				LoadMaterial();

			for (int i = 0; i < points.Length; i += 2)
			{
				Vector3 point0 = points[i];
				Vector3 point1 = points[i + 1];

				GL.Begin(GL.LINES);
				material.SetPass(0);
				GL.Color(color);
				GL.Vertex3(point0.x, point0.y, point0.z);
				GL.Vertex3(point1.x, point1.y, point1.z);
				GL.End();
			}
		}



		private static void UpdateGrid (int count)
		{
			// force count to be even
			if ((count % 2) != 0)
				count++;

			int upperBound0 = count * 2 + 2;

			//-------------------------------------
			// X Plane
			//-------------------------------------

			x_plane = new Vector3[count * 4 + 4];

			for (int i = 0; i <= upperBound0; i += 2)
			{
				x_plane[i] = new Vector3(
					0,
					Offset(count, upperBound0, scaleY) + i * scaleY,
					Offset(count, upperBound0, scaleZ) + 0
				);
				x_plane[i + 1] = new Vector3(
					0,
					Offset(count, upperBound0, scaleY) + i * scaleY,
					Offset(count, upperBound0, scaleZ) + count * 2 * scaleZ
				);
			}

			for (int i = upperBound0; i <= count * 4 + 2; i += 2)
			{
				x_plane[i] = new Vector3(
					0,
					Offset(count, upperBound0, scaleY) + 0,
					Offset(count, upperBound0, scaleZ) + (i - upperBound0) * scaleZ
				);
				x_plane[i + 1] = new Vector3(
					0,
					Offset(count, upperBound0, scaleY) + count * 2 * scaleY,
					Offset(count, upperBound0, scaleZ) + (i - upperBound0) * scaleZ
				);
			}

			//-------------------------------------
			// Y Plane
			//-------------------------------------

			y_plane = new Vector3[count * 4 + 4];

			for (int i = 0; i <= upperBound0; i += 2)
			{
				y_plane[i] = new Vector3(
					Offset(count, upperBound0, scaleX) + i * scaleX,
					0,
					Offset(count, upperBound0, scaleZ) + 0
				);
				y_plane[i + 1] = new Vector3(
					Offset(count, upperBound0, scaleX) + i * scaleX,
					0,
					Offset(count, upperBound0, scaleZ) + count * 2 * scaleZ
				);
			}

			for (int i = upperBound0; i <= count * 4 + 2; i += 2)
			{
				y_plane[i] = new Vector3(
					Offset(count, upperBound0, scaleX) + 0,
					0,
					Offset(count, upperBound0, scaleZ) + (i - upperBound0) * scaleZ
				);
				y_plane[i + 1] = new Vector3(
					Offset(count, upperBound0, scaleX) + count * 2 * scaleX,
					0,
					Offset(count, upperBound0, scaleZ) + (i - upperBound0) * scaleZ
				);
			}

			//-------------------------------------
			// Z Plane
			//-------------------------------------

			z_plane = new Vector3[count * 4 + 4];

			for (int i = 0; i <= upperBound0; i += 2)
			{
				z_plane[i] = new Vector3(
					Offset(count, upperBound0, scaleX) + i * scaleX,
					Offset(count, upperBound0, scaleY) + 0,
					0
				);
				z_plane[i + 1] = new Vector3(
					Offset(count, upperBound0, scaleX) + i * scaleX,
					Offset(count, upperBound0, scaleY) + count * 2 * scaleY,
					0
				);
			}

			for (int i = upperBound0; i <= count * 4 + 2; i += 2)
			{
				z_plane[i] = new Vector3(
					Offset(count, upperBound0, scaleX) + 0,
					Offset(count, upperBound0, scaleY) + (i - upperBound0) * scaleY,
					0
				);
				z_plane[i + 1] = new Vector3(
					Offset(count, upperBound0, scaleX) + count * 2 * scaleX,
					Offset(count, upperBound0, scaleY) + (i - upperBound0) * scaleY,
					0
				);
			}
		}

		// Source: http://wiki.unity3d.com/index.php/SnapToGrid
		public static void SnapToGrid ()
		{
			scaleX *= 2;
			scaleY *= 2;
			scaleZ *= 2;

			foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable))
			{
				t.position = new Vector3(
					Mathf.Round(t.position.x / scaleX) * scaleX,
					Mathf.Round(t.position.y / scaleY) * scaleY,
					Mathf.Round(t.position.z / scaleZ) * scaleZ
				);
			}
		}


		static float snapX_, snapY_, snapZ_;

		// returns true if something has changed
		static bool UpdateSnapValues ()
		{
			scaleX = EditorPrefs.GetFloat("MoveSnapX") / 2;
			scaleY = EditorPrefs.GetFloat("MoveSnapY") / 2;
			scaleZ = EditorPrefs.GetFloat("MoveSnapZ") / 2;

			bool result = (snapX_ != scaleX || snapY_ != scaleY || scaleZ != snapZ_);

			snapX_ = scaleX;
			snapY_ = scaleY;
			snapZ_ = scaleZ;

			return result;
		}

		static float Offset (int count, int upperBound0, float scale)
		{
			return -((count * 4 + 2 - upperBound0) * scale) / 2;
		}
	}
#endif
}
