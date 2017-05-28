//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityDoodats.Editor
{
#if UNITY_EDITOR
	[ExecuteInEditMode]
	[System.Obsolete("Don't use this outside of Editor scripts")]
	public class SnapGridComponent : MonoBehaviour
	{
		public static SnapGridComponent instance;

		//-------------------------------------
		// Settings
		//-------------------------------------

		public float scaleX, scaleY, scaleZ;
		public static Color red, green, blue;

		public Vector3[]x_plane;
		public Vector3[]y_plane;
		public Vector3[]z_plane;

		public Vector3[]x_plane_10;
		public Vector3[]y_plane_10;
		public Vector3[]z_plane_10;


		public bool show = true;
		public bool showX = true, showY = true, showZ = true;
		public bool autoSnap = false;


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

			// call only if something has changed
			if (x_plane == null || y_plane == null || z_plane == null ||
				x_plane_10 == null || y_plane_10 == null || z_plane_10 == null || 
				UpdateSnapValues())
			{
				UpdateGrid(ref x_plane, ref y_plane, ref z_plane, 100, scaleX, scaleY, scaleZ);
				UpdateGrid(ref x_plane_10, ref y_plane_10, ref z_plane_10, 100 / 10, scaleX * 10, scaleY * 10, scaleZ * 10);
			}

			//-------------------------------------
			// Auto Snap
			//-------------------------------------

			if (autoSnap)
				SnapToGrid();

			if (show)
			{
				if (showX)
				{
					DrawLines(x_plane, red);
					DrawLines(x_plane_10, red);
				}

				if (showY)
				{
					DrawLines(y_plane, green);
					DrawLines(y_plane_10, green);
				}

				if (showZ)
				{
					DrawLines(z_plane, blue);
					DrawLines(z_plane_10, blue);
				}
			}

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



		private void UpdateGrid (
			ref Vector3[] x_plane, ref Vector3[] y_plane, ref Vector3[] z_plane,
			int count, float scaleX, float scaleY, float scaleZ)
		{
			//scaleX = scaleY = scaleZ *= 10f;

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
		public void SnapToGrid ()
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
		bool UpdateSnapValues ()
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
