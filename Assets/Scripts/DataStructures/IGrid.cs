//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System.Collections.Generic;
using UnityDoodats;
using UnityEngine;

namespace UnityDoodats
{
	/// <summary>
	///
	/// </summary>
	public interface IGrid<T> : IEnumerable<T> where T : Component
	{
		int Width { get; }
		int Height { get; }
		int ItemCount { get; }

		T this[int x, int y] { get; set; }
		T this[Vec2i pos] { get; set; }

		void Remove(T item);

		Vec2i GetPosition(T item);

		void Swap(T A, T B);
		void Swap(Vec2i A, Vec2i B);

		T[] AdjacentItems(T item);
		Vec2i[] AdjacentTiles(Vec2i pos);

		bool IsValid(Vec2i pos);
		void Clear();
	}
}
