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
	abstract public class PathfindingAlgorithms
	{
		public static List<T> BreathFirstSearch<T> (XY origin, XY target, IGrid<T> grid)
		{
			if (grid.IsValid(origin) == false)
				throw new System.ArgumentException("origin " + origin + " is not a valid tile of grid " + grid);

			if (grid.IsValid(target) == false)
				throw new System.ArgumentException("target " + target + " is not a valid tile of grid " + grid);
			
			var predecessors = BFS_Flood(grid, origin, target);
			return Backtrack(grid, predecessors, target);
		}

		public static List<T> Dijkstra<T> (XY origin, XY target, IGridWithCosts<T> grid)
		{
			if (grid.IsValid(origin) == false || grid.IsValid(target) == false)
				return null;

			var predecessors = Dijkstra_Flood(grid, origin, target);
			return Backtrack(grid, predecessors, target);
		}

		// leave target empty to flood entire grid
		static Grid<XY> BFS_Flood<T> (IGrid<T> grid, XY origin, XY target = null)
		{
			var frontier = new Queue<XY>();
			frontier.Enqueue(origin);
			var predecessors = new Grid<XY>(grid.Width, grid.Height);
			predecessors[origin] = origin;

			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();
				
				if (current == target)
					break;

				foreach (var next in grid.OrthogonalTiles(current))
					if (predecessors[next] == null)
					{
						frontier.Enqueue(next);
						predecessors[next] = current;
					}
			}
			return predecessors;
		}

		// leave target empty to flood entire grid
		static Grid<XY> Dijkstra_Flood<T> (IGridWithCosts<T> grid, XY origin, XY target = null)
		{
			var frontier = new SortedList<XY, int>();
			var predecessors = new Grid<XY>(grid.Width, grid.Height);
			predecessors.Initialize();
			var costs = new Grid<int>(grid.Width, grid.Height);
			costs.Initialize(int.MaxValue);

			frontier.Add(origin, 0);
			predecessors[origin] = origin;
			costs[origin] = 0;

			while (frontier.Count > 0)
			{
				var current = frontier.Keys[0];
				frontier.Remove(current);

				if (current == target)
					break;

				foreach (var next in grid.OrthogonalTiles(current))
				{
					var newCosts = costs[current] + grid.GetCosts(current, next);

					// new or better (works because costs are initialized with int.MaxValue)
					if (newCosts < costs[next])
					{
						costs[next] = newCosts;
						frontier.Add(next, newCosts);
						predecessors[next] = current;
					}
				}
			}
			return predecessors;
		}

		static List<T> Backtrack<T> (IGrid<T> grid, Grid<XY> predecessors, XY target)
		{
			var current = target;
			var path = new List<T>();
			while (predecessors[current] != current)
			{
				path.Add(grid[current]);
				if (predecessors[current] == null)
					throw new System.NullReferenceException("predecessors[" + current + "] is null");
				current = predecessors[current];
			}

			path.Reverse();
			return path;
		}
	}
}
