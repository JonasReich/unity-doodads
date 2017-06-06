//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Doodads
{
	// Source: http://www.redblobgames.com/pathfinding/a-star/implementation.html

	public class PriorityQueue<T>
	{
		// I'm using an unsorted array for this example, but ideally this
		// would be a binary heap. There's an open issue for adding a binary
		// heap to the standard C# library: https://github.com/dotnet/corefx/issues/574
		//
		// Until then, find a binary heap class:
		// * https://bitbucket.org/BlueRaja/high-speed-priority-queue-for-c/wiki/Home
		// * http://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx
		// * http://xfleury.github.io/graphsearch.html
		// * http://stackoverflow.com/questions/102398/priority-queue-in-net

		private List<KeyValuePair<int, T>> elements = new List<KeyValuePair<int, T>>();

		public int Count
		{
			get { return elements.Count; }
		}

		public void Enqueue (T item, int priority)
		{
			elements.Add(new KeyValuePair<int, T>( priority, item));
		}

		public T Dequeue ()
		{
			int bestIndex = 0;

			for (int i = 0; i < elements.Count; i++)
			{
				if (elements[i].Key < elements[bestIndex].Key)
				{
					bestIndex = i;
				}
			}

			T bestItem = elements[bestIndex].Value;
			elements.RemoveAt(bestIndex);
			return bestItem;
		}
	}
}
