//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Doodads
{
	/// <summary>
	/// 
	/// </summary>
	public class VisibilityMesh : MonoBehaviour, IComparer<Vector3>
	{
		public List<Collider2D> visionBlockingColliders;

		public Transform targetTransform;

		List<Vector3> uniqueEndpoints;
		Mesh mesh;
		MeshFilter meshFilter;

		Vector3[] vertices;

		void Awake ()
		{
			//-------------------
			// Components
			//-------------------

			meshFilter = GetComponent<MeshFilter>();
			if (meshFilter == null)
				meshFilter = gameObject.AddComponent<MeshFilter>();

			MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
			if (meshRenderer == null)
				meshRenderer = gameObject.AddComponent<MeshRenderer>();



			this.uniqueEndpoints = GenerateEndpointList(visionBlockingColliders);


			GetComponent<MeshFilter>().sharedMesh = mesh = new Mesh();
			mesh.name = "Visibility Mesh";


			
			GenerateMesh();



			/*
			meshRenderer.material = new Material(Shader.Find("Standard"));



			

			mesh = meshFilter.sharedMesh = new Mesh();
			mesh.vertices = new Vector3[3];
			mesh.triangles = new int[3] ;
			for (int i = 0; i < mesh.triangles.Length; i++)
			{
				mesh.triangles[i] = i;
			}


			mesh.uv = new Vector2[] { Vector2.zero, Vector2.one, Vector2.right };
			*/
		}

		private List<Vector3> GenerateEndpointList (List<Collider2D> visionBlockingColliders)
		{
			var uniqueEndpoints = new List<Vector3>();

			foreach (var collider in visionBlockingColliders)
			{
				var edgeCollider = collider as EdgeCollider2D;

				if (edgeCollider != null)
				{
					for (int i = 0; i < edgeCollider.pointCount; i++)
					{
						uniqueEndpoints.Add((Vector2)edgeCollider.transform.position + (Vector2)(edgeCollider.transform.rotation * (edgeCollider.offset + edgeCollider.points[i])));
					}
				}
			}
			return uniqueEndpoints;
		}

		private void GenerateMesh ()
		{
			var smallPositiveRotation = Quaternion.Euler(0, 0, +0.0001f);
			var smallNegativeRotation = Quaternion.Euler(0, 0, -0.0001f);

			// Sort uniqueEndpoints depending on angle
			this.uniqueEndpoints.Sort(this);

			var hitPoints = new List<Vector3>();

			for (int i = 0; i < uniqueEndpoints.Count; i++)
			{
				var direction = uniqueEndpoints[i] - targetTransform.position;

				var hit = Physics2D.Raycast(targetTransform.position, smallNegativeRotation * direction);
				if (hit)
				{
					Debug.DrawLine((Vector2)targetTransform.position, hit.point, Color.red);
					hitPoints.Add(hit.point);
				}

				hit = Physics2D.Raycast(targetTransform.position, direction);
				if (hit)
				{
					Debug.DrawLine((Vector2)targetTransform.position, hit.point, Color.red);
					hitPoints.Add(hit.point);
				}

				hit = Physics2D.Raycast(targetTransform.position, smallPositiveRotation * direction);
				if (hit)
				{
					Debug.DrawLine((Vector2)targetTransform.position, hit.point, Color.red);
					hitPoints.Add(hit.point);
				}
			}

			hitPoints.Reverse();
			hitPoints.Insert(0, targetTransform.position);
			


			int[] triangles = new int[(hitPoints.Count-1) * 3];
			/*triangles[0] = 0;
			triangles[1] = 1;
			triangles[2] = 2;

			triangles[3] = 0;
			triangles[4] = 3;
			triangles[5] = 4;

			triangles[6] = 0;
			triangles[7] = 5;
			triangles[8] = 6;

			triangles[9] = 0;
			triangles[10] = 7;
			triangles[11] = 8;
			*/



			int index = 0;
			for (int i = 1; i < hitPoints.Count - 1; i++)
			{
				triangles[index] = 0;
				index++;
				triangles[index] = i;
				index++;
				triangles[index] = i + 1;
				index++;
			}

			triangles[index] = 0;

			index++;
			triangles[index] = hitPoints.Count-1;

			index++;
			triangles[index] = 1;

			mesh.Clear();
			mesh.vertices = vertices = hitPoints.ToArray();
			mesh.triangles = triangles;
		}



		void Start ()
		{

		}

		void Update ()
		{
			GenerateMesh();
			
		}


		public struct Line
		{
			public Vector2 A;
			public Vector2 B;

			public Line (Vector2 A, Vector2 B)
			{
				this.A = A;
				this.B = B;
			}
		}

		private void OnDrawGizmos ()
		{
			if (vertices == null)
				return;

			Gizmos.color = Color.black;
			for (int i = 0; i < vertices.Length; i++)
			{
				if (i == 0 || i == 1 || i == 3)
					Gizmos.color = Color.yellow;
				else
					Gizmos.color = Color.red;
				Gizmos.DrawSphere(vertices[i], 0.1f);
			}
		}

		// Compare two vector3 positions depending on angle to transform
		public int Compare (Vector3 A, Vector3 B)
		{
			// Ascending order

			double angleA = Math.Atan2(A.y - targetTransform.position.y, A.x - targetTransform.position.x);
			double angleB = Math.Atan2(B.y - targetTransform.position.y, B.x - targetTransform.position.x);


			if (angleA == angleB)
				return 0;
			if (angleA > angleB)
				return 1;
			else // (angleA < angleB)
				return -1;
		}
	}
}
