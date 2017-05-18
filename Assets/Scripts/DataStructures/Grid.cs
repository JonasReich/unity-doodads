//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityDoodats
{
	public interface IGrid<T> : IEnumerable<T> where T : class
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

	/// <summary>
	///
	/// </summary>
	public class Grid<T> : IGrid<T> where T : class
	{
		private T[,] cells;

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
			// Unity crashes after leaving the constructor
		}


		public T this[int x, int y] { get { return cells[x, y]; } set { cells[x, y] = value; } }
		public T this[Vec2i pos] { get { return this[pos.x, pos.y]; } set { this[pos.x, pos.y] = value; } }

		public void Remove(T t)
		{
			var pos = GetPosition(t);

			if (IsValid(pos))
				this[pos] = null;
		}

		public Vec2i GetPosition(T item)
		{
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Height; y++)
					if (cells[x, y].Equals(item))
						return new Vec2i(x, y);

			return Vec2i.invalid;
		}

		public void Swap(T A, T B)
		{
			Swap(GetPosition(A), GetPosition(B));
		}

		public void Swap(Vec2i A, Vec2i B)
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

		public Vec2i[] AdjacentTiles(Vec2i pos)
		{
			List<Vec2i> adjacentTiles = new List<Vec2i>();

			for (int x = -1; x <= +1; x++)
				for (int y = -1; y <= +1; y++)
				{
					Vec2i n = new Vec2i(pos.x + x, pos.y + y);
					if (IsValid(n))
						adjacentTiles.Add(n);
				}

			return adjacentTiles.ToArray();
		}

		public bool IsValid(Vec2i pos)
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
