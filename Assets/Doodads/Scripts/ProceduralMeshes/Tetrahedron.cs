//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using UnityEngine;

namespace Doodads
{
	/// <summary>
	/// 
	/// </summary>
	public class Tetrahedron : MonoBehaviour
	{
		void Awake ()
		{
			Initialize();
		}

		public void Initialize ()
		{
			//-------------------
			// Mesh Filter
			//-------------------

			MeshFilter meshFilter = GetComponent<MeshFilter>();
			if (meshFilter == null)
				meshFilter = gameObject.AddComponent<MeshFilter>();

			//-------------------
			// The Tetahedron itself
			//-------------------

			float sqrt075 = Mathf.Sqrt(0.75f);

			Vector3 p0 = new Vector3(0, 0, 0);
			Vector3 p1 = new Vector3(1, 0, 0);
			Vector3 p2 = new Vector3(0.5f, 0, sqrt075);
			Vector3 p3 = new Vector3(0.5f, sqrt075, sqrt075 / 3f);

			p0 += new Vector3(-0.5f, -sqrt075 / 3f, -sqrt075 / 3f);
			p1 += new Vector3(-0.5f, -sqrt075 / 3f, -sqrt075 / 3f);
			p2 += new Vector3(-0.5f, -sqrt075 / 3f, -sqrt075 / 3f);
			p3 += new Vector3(-0.5f, -sqrt075 / 3f, -sqrt075 / 3f);

			Mesh mesh = meshFilter.sharedMesh = new Mesh();
			mesh.name = "Tetrahedron";

			mesh.vertices = new Vector3[] {
				p0,p1,p2,
				p0,p2,p3,
				p2,p1,p3,
				p0,p3,p1
			};
			mesh.triangles = new int[] {
				0,1,2,
				3,4,5,
				6,7,8,
				9,10,11
			};

			Vector2 uv3a = new Vector2(0, 0);
			Vector2 uv1 = new Vector2(0.5f, 0);
			Vector2 uv0 = new Vector2(0.25f, sqrt075 / 2);
			Vector2 uv2 = new Vector2(0.75f, sqrt075 / 2);
			Vector2 uv3b = new Vector2(0.5f, sqrt075);
			Vector2 uv3c = new Vector2(1, 0);

			mesh.uv = new Vector2[]{
				uv0,uv1,uv2,
				uv0,uv2,uv3b,
				uv0,uv1,uv3a,
				uv1,uv2,uv3c
			};

			mesh.RecalculateNormals();
			mesh.RecalculateBounds();

			//-------------------
			// Mesh Renderer
			//-------------------

			MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
			if (meshRenderer == null)
				meshRenderer = gameObject.AddComponent<MeshRenderer>();

			meshRenderer.material = new Material(Shader.Find("Standard"));

			DestroyImmediate(this);
		}
	}
}
