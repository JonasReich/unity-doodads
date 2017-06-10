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
	/// Procedural mesh which displays a characters field of vision
	/// </summary>
	public class VisibilityMesh : MonoBehaviour, IComparer<Vector3>
	{
		/* ---------------------------------------
		 * References:
		 * ---------------------------------------
		 * 1. Amit Patel's "Red Blob Games" blog:
		 * http://www.redblobgames.com/articles/visibility/
		 *
		 * 2. Nicky Case's "SIGHT & LIGHT":
		 * http://ncase.me/sight-and-light/
		 *
		 * 3. Catlike Coding's Tutorial on procedural Unity meshes:
		 * http://catlikecoding.com/unity/tutorials/procedural-grid/
		 * ---------------------------------------*/

		// These small Quaternions are used for the secondary raycasts
		// that are executed to reduce edge collision problems
		static readonly Quaternion smallPositiveRotation = Quaternion.Euler(0, 0, +0.0001f);
		static readonly Quaternion smallNegativeRotation = Quaternion.Euler(0, 0, -0.0001f);

		/// <summary>
		/// Location and orientation of the "eyes" that the mesh is generated for.
		/// Usually this would be a characters head.
		/// </summary>
		public Transform eyeTransform;

		/// <summary>
		/// All layers that contain 2D colliders which should be respected by the mesh generation algorithm
		/// </summary>
		public LayerMask visionBlockingLayers;

		/// <summary>
		/// Material to draw the mesh with
		/// </summary>
		public Material material;

		List<Collider2D> visionBlockingColliders;
		List<Vector3> colliderEndpoints;
		List<Vector3> vertices = new List<Vector3>();
		Mesh mesh;
		MeshFilter meshFilter;
		

		void Awake ()
		{
			//-------------------
			// Get references to colliders
			//-------------------
			visionBlockingColliders = new List<Collider2D>();
			var allColliders = FindObjectsOfType<Collider2D>();
			foreach (var item in allColliders)
				if (visionBlockingLayers.Contains(item.gameObject.layer))
					visionBlockingColliders.Add(item);

			colliderEndpoints = GenerateEndpointList(visionBlockingColliders);

			//-------------------
			// Add Components
			//-------------------

			meshFilter = GetComponent<MeshFilter>();
			if (meshFilter == null)
				meshFilter = gameObject.AddComponent<MeshFilter>();

			meshFilter.sharedMesh = mesh = new Mesh();
			mesh.name = "Visibility Mesh";


			MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
			if (meshRenderer == null)
				meshRenderer = gameObject.AddComponent<MeshRenderer>();

			meshRenderer.material = material;

			//-------------------
			// Generate first mesh instance
			//-------------------

			GenerateMesh();
		}

		void Update ()
		{
			GenerateMesh();
		}

		void OnDrawGizmosSelected ()
		{
			if (vertices == null)
				return;

			Gizmos.color = Color.red;
			for (int i = 0; i < vertices.Count; i++)
				Gizmos.DrawSphere(vertices[i], 0.1f);
		}

		
		/// <summary>
		/// Compare two vector3 positions depending on angle to transform. Used to implement IComparer<Vector3> for Vector sorting
		/// </summary>
		public int Compare (Vector3 A, Vector3 B)
		{
			double angleA = Math.Atan2(A.y - (eyeTransform.position).y, A.x - (eyeTransform.position).x);
			double angleB = Math.Atan2(B.y - (eyeTransform.position).y, B.x - (eyeTransform.position).x);

			if (angleA == angleB)
				return 0;
			if (angleA > angleB)
				return 1;
			else // (angleA < angleB)
				return -1;
		}


		List<Vector3> GenerateEndpointList (List<Collider2D> visionBlockingColliders)
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
					continue;
				}

				var boxCollider = collider as BoxCollider2D;
				if (boxCollider != null)
				{
					Vector2 size = boxCollider.size;
					Vector2 worldPos = (Vector2)boxCollider.transform.position + boxCollider.offset;

					var p0 = worldPos + (Vector2)(boxCollider.transform.rotation * new Vector2(+boxCollider.size.x / 2f, +boxCollider.size.y / 2f));
					var p1 = worldPos + (Vector2)(boxCollider.transform.rotation * new Vector2(+boxCollider.size.x / 2f, -boxCollider.size.y / 2f));
					var p2 = worldPos + (Vector2)(boxCollider.transform.rotation * new Vector2(-boxCollider.size.x / 2f, -boxCollider.size.y / 2f));
					var p3 = worldPos + (Vector2)(boxCollider.transform.rotation * new Vector2(-boxCollider.size.x / 2f, +boxCollider.size.y / 2f));

					uniqueEndpoints.Add(p0);
					uniqueEndpoints.Add(p1);
					uniqueEndpoints.Add(p2);
					uniqueEndpoints.Add(p3);

					continue;
				}
			}
			return uniqueEndpoints;
		}

		void GenerateMesh ()
		{
			Vector2 targetPosition = eyeTransform.position;

			// Sort uniqueEndpoints depending on angle
			colliderEndpoints.Sort(this);

			vertices.Clear();

			for (int i = 0; i < colliderEndpoints.Count; i++)
			{
				var direction = (Vector2)colliderEndpoints[i] - targetPosition;

				var hit = Physics2D.Raycast(targetPosition, smallNegativeRotation * direction);
				if (hit)
				{
					Debug.DrawLine((Vector2)targetPosition, hit.point, Color.red);
					vertices.Add(hit.point);
				}

				hit = Physics2D.Raycast(targetPosition, direction);
				if (hit)
				{
					Debug.DrawLine((Vector2)targetPosition, hit.point, Color.red);
					vertices.Add(hit.point);
				}

				hit = Physics2D.Raycast(targetPosition, smallPositiveRotation * direction);
				if (hit)
				{
					Debug.DrawLine((Vector2)targetPosition, hit.point, Color.red);
					vertices.Add(hit.point);
				}
			}

			vertices.Add(targetPosition);


			//-------------------
			// Generate triangles
			//-------------------

			int[] triangles = new int[vertices.Count * 3];

			int index = 0;
			for (int i = 0; i < vertices.Count - 1; i++)
			{
				triangles[index] = vertices.Count - 1;
				index++;
				triangles[index] = i;
				index++;
				triangles[index] = i + 1;
				index++;
			}

			// Last set of triangles
			triangles[index] = vertices.Count - 1;
			index++;
			triangles[index] = vertices.Count - 2;
			index++;
			triangles[index] = 0;

			//-------------------
			// Apply changes
			//-------------------

			mesh.Clear();
			mesh.SetVertices(vertices);
			mesh.triangles = triangles;
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
	}
}
