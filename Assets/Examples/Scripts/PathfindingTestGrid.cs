//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Doodads.Examples
{
	/// <summary>
	///
	/// </summary>
	public class PathfindingTestGrid : MonoBehaviour
	{
		public PathfindingTestTile prefab;
		
		[SerializeField, HideInInspector]
		Grid grid;

		[SerializeField]
		int width, height;
		[SerializeField]
		XY origin, target;

		LayerMask layerMask;

		void Awake ()
		{
			layerMask = layerMask.Add(prefab.gameObject.layer);
		}

		public void Update ()
		{
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 300f, layerMask) == false)
				return;

			target = new XY((int)hit.point.x, (int)hit.point.y);


			if (grid.IsValid(target) == false)
				return;

			foreach (var item in grid)
				item.meshRenderer.material.color = Color.white;

			var path = PathfindingAlgorithms.Dijkstra(origin, target, grid);
			grid[origin].meshRenderer.material.color = Color.black;
			grid[target].meshRenderer.material.color = Color.blue;
			foreach (var item in path)
				item.meshRenderer.material.color = Color.red;
		}

		public void Refresh ()
		{
			while (transform.childCount != 0)
				DestroyImmediate(transform.GetChild(0).gameObject);

			grid = new Grid(width, height, prefab, transform);
		}

		[System.Serializable]

		public class Grid : ComponentGridWithCosts<PathfindingTestTile>
		{
			public Grid (int width, int height, PathfindingTestTile prefab, Transform root) : base(width, height, prefab, root)
			{
			}
		}
	}
}
