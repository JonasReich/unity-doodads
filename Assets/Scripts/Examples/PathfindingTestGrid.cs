//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityDoodats
{
	/// <summary>
	///
	/// </summary>
	public class PathfindingTestGrid : MonoBehaviour
	{
		public PathfindingTestTile prefab;
		public Grid grid;

		[SerializeField]
		int width, height;
		[SerializeField]
		XY origin, target;

		LayerMask layerMask;

		void Awake ()
		{
			grid = new Grid(width, height, prefab, transform);
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

		[System.Serializable]

		public class Grid : ComponentGridWithCosts<PathfindingTestTile>
		{
			public Grid (int columnCount, int rowCount, PathfindingTestTile prefab, Transform root) : base(columnCount, rowCount, prefab, root)
			{
			}
		}
	}
}
