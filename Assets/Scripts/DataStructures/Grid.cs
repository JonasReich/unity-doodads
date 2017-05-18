//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JonasReich.UnityDoodats
{
	public interface IGrid<T> : IEnumerable<T> where T : class
	{
		int Width { get; }
		int Height { get; }
		int ItemCount { get; }

		T this[int x, int y] { get; set; }
		T this[vec2i pos] { get; set; }

		void Remove(T item);

		vec2i GetPosition(T item);

		void Swap(T A, T B);
		void Swap(vec2i A, vec2i B);

		T[] AdjacentItems(T item);
		vec2i[] AdjacentTiles(vec2i pos);

		bool IsValid(vec2i pos);
		void Clear();
	}

	/// <summary>
	/// 2-dimensional integer vector
	/// </summary>
	public struct vec2i
	{
		public int x, y;
		public vec2i(int x, int y) { this.x = x; this.y = y; }
		public static readonly vec2i invalid = new vec2i(-1, -1);
	}



	/// <summary>
	///
	/// </summary>
	public class Grid<T> : IGrid<T> where T : class
	{
		readonly T[,] cells;

		public int Width { get { return cells.GetLength(0); } }
		public int Height { get { return cells.GetLength(1); } }
		public int ItemCount
		{
			get
			{
				int i = 0;

				foreach (var item in this)
					if (item != null)
						i++;

				return i;
			}
		}


		public Grid(int width, int height)
		{
			cells = new T[width, height];
		}


		public T this[int x, int y] { get { return cells[x, y]; } set { cells[x, y] = value; } }
		public T this[vec2i pos] { get { return this[pos.x, pos.y]; } set { this[pos.x, pos.y] = value; } }

		public void Remove(T t)
		{
			var pos = GetPosition(t);

			if (IsValid(pos))
				this[pos] = null;
		}

		public vec2i GetPosition(T item)
		{
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Height; y++)
					if (cells[x, y].Equals(item))
						return new vec2i(x, y);

			return vec2i.invalid;
		}

		public void Swap(T A, T B)
		{
			Swap(GetPosition(A), GetPosition(B));
		}

		public void Swap(vec2i A, vec2i B)
		{
			T temp = this[A];
			this[A] = this[B];
			this[B] = this[A];
		}

		public T[] AdjacentItems(T item)
		{
			List<T> adjacentTiles = new List<T>();

			foreach (var pos in AdjacentTiles(GetPosition(item)))
				if (this[pos] != null)
					adjacentTiles.Add(this[pos]);

			return adjacentTiles.ToArray();
		}

		public vec2i[] AdjacentTiles(vec2i pos)
		{
			List<vec2i> adjacentTiles = new List<vec2i>();

			for (int x = -1; x <= +1; x++)
				for (int y = -1; y <= +1; y++)
				{
					vec2i n = new vec2i(pos.x + x, pos.y + y);
					if (IsValid(n))
						adjacentTiles.Add(n);
				}

			return adjacentTiles.ToArray();
		}

		public bool IsValid(vec2i pos)
		{
			return pos.x > 0 && pos.x < Width && pos.y > 0 && pos.y < Width;
		}

		public void Clear()
		{
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Height; y++)
					this[x, y] = null;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return cells.GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
