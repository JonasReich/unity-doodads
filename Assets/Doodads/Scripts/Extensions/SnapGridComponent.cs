//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System;
using UnityEditor;
using UnityEngine;

namespace Doodads.Editor
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

		public float snapValue_X, snapValue_Y, snapValue_Z, snapValue_Rotation, snapValue_Scale;
		public static Color red, green, blue;

		public Vector3[]x_plane;
		public Vector3[]y_plane;
		public Vector3[]z_plane;

		public Vector3[]x_plane_10;
		public Vector3[]y_plane_10;
		public Vector3[]z_plane_10;

		public bool isDirty = true;
		public bool show = true;
		public bool showX = true, showY = true, showZ = true;
		public bool doSnapPosition = false, doSnapRotation = false, doSnapScale = false;

		Material material;

		private void OnEnable ()
		{
			Load();
			EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
		}

		private void OnDisable ()
		{
			Save();
			EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;
		}
		
		void OnPlaymodeStateChanged ()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				if (EditorApplication.isPlaying)
				{
					// 2. reset, therefore load
					Load();
				}
				else
				{
					// 1. not reset
					Save();
				}
			}
			else
			{
				if (EditorApplication.isPlaying)
				{
					// 3. not reset
				}
				else
				{
					// 4. not reset
				}
			}
		}

		public void Save ()
		{
			EditorPrefs.SetBool("SnapGrid_Show", show);
			EditorPrefs.SetBool("SnapGrid_ShowX", showX);
			EditorPrefs.SetBool("SnapGrid_ShowY", showY);
			EditorPrefs.SetBool("SnapGrid_ShowZ", showZ);
			EditorPrefs.SetBool("SnapGrid_DoSnapPosition", doSnapPosition);
			EditorPrefs.SetBool("SnapGrid_DoSnapRotation", doSnapRotation);
			EditorPrefs.SetBool("SnapGrid_DoSnapScale", doSnapScale);

			EditorPrefs.SetFloat("SnapGrid_x", snapValue_X);
			EditorPrefs.SetFloat("SnapGrid_y", snapValue_Y);
			EditorPrefs.SetFloat("SnapGrid_z", snapValue_Z);

			EditorPrefs.SetFloat("SnapGrid_rotation", snapValue_Rotation);
			EditorPrefs.SetFloat("SnapGrid_scale", snapValue_Scale);

		}

		void Load ()
		{
			show = EditorPrefs.GetBool("SnapGrid_Show");
			showX = EditorPrefs.GetBool("SnapGrid_ShowX");
			showY = EditorPrefs.GetBool("SnapGrid_ShowY");
			showZ = EditorPrefs.GetBool("SnapGrid_ShowZ");
			doSnapPosition = EditorPrefs.GetBool("SnapGrid_DoSnapPosition");
			doSnapRotation = EditorPrefs.GetBool("SnapGrid_DoSnapRotation");
			doSnapScale = EditorPrefs.GetBool("SnapGrid_DoSnapScale");

			snapValue_X = EditorPrefs.GetFloat("SnapGrid_x");
			snapValue_Y = EditorPrefs.GetFloat("SnapGrid_y");
			snapValue_Z = EditorPrefs.GetFloat("SnapGrid_z");

			snapValue_Rotation = EditorPrefs.GetFloat("SnapGrid_rotation");
			snapValue_Scale = EditorPrefs.GetFloat("SnapGrid_scale");
		}

		void LoadMaterial ()
		{
			material = Resources.Load("GridMaterial", typeof(Material)) as Material;
		}

		
		/*
		public void SnapRotation (Transform t)
		{
			var r = t.localEulerAngles;
			r.Set(
					Mathf.Round(r.x / snapValue_Rotation) * snapValue_Rotation,
					Mathf.Round(r.y / snapValue_Rotation) * snapValue_Rotation,
					Mathf.Round(r.z / snapValue_Rotation) * snapValue_Rotation
			);
			t.localEulerAngles = r;
		}
		*/

		public void SnapPosition (Transform t)
		{
			t.position = new Vector3(
					Mathf.Round(t.position.x / snapValue_X) * snapValue_X,
					Mathf.Round(t.position.y / snapValue_Y) * snapValue_Y,
					Mathf.Round(t.position.z / snapValue_Z) * snapValue_Z
				);
		}

		public void SnapScale(Transform t)
		{
			t.localScale = new Vector3(
					Mathf.Round(t.localScale.x / snapValue_Scale) * snapValue_Scale,
					Mathf.Round(t.localScale.y / snapValue_Scale) * snapValue_Scale,
					Mathf.Round(t.localScale.z / snapValue_Scale) * snapValue_Scale
			);
		}

		private void Update ()
		{
			if (Application.isEditor)
			{
				/*
				snapValue_X *= 2;
				snapValue_Y *= 2;
				snapValue_Z *= 2;
				*/

				foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable))
				{
					if(doSnapPosition)
						SnapPosition(t);

					/*
					// Doesn't work properly -> Snaps at random intervals??
					if (doSnapRotation)
						SnapRotation(t);
					*/

					if (doSnapScale)
						SnapScale(t);
				}
			}
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
				isDirty)
			{
				isDirty = false;
				UpdateGrid(ref x_plane, ref y_plane, ref z_plane, 100, snapValue_X, snapValue_Y, snapValue_Z);
				UpdateGrid(ref x_plane_10, ref y_plane_10, ref z_plane_10, 100 / 10, snapValue_X * 10, snapValue_Y * 10, snapValue_Z * 10);
			}

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

		static float Offset (int count, int upperBound0, float scale)
		{
			return -((count * 4 + 2 - upperBound0) * scale) / 2;
		}
	}
#endif
}
