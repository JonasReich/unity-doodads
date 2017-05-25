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
	public class Grid<T> : IGrid<T>
	{
		protected T[,] cells;


		public Grid(int x, int y)
		{
			cells = new T[x, y];
			Width = x;
			Height = y;
		}

		public void Initialize()
		{
			for (int x = 0; x < Height; x++)
				for (int y = 0; y < Width; y++)
					cells[x, y] = default(T);
		}

		public void Initialize(T defaultValue)
		{
			for (int x = 0; x < Height; x++)
				for (int y = 0; y < Width; y++)
					cells[x, y] = defaultValue;
		}

		//--------------------------------------
		// Properties
		//--------------------------------------

		public int Width { get; private set; }
		public int Height { get; private set; }
		public int ItemCount
		{
			get
			{
				int i = 0;

				foreach (var item in cells)
					if (item != null)
						i++;

				return i;
			}
		}

		//--------------------------------------
		// Overload [] operators
		//--------------------------------------

		public T this[int x, int y] { get { return cells[x, y]; } set { cells[x, y] = value; } }
		public T this[XY pos] { get { return cells[pos.x, pos.y]; } set { cells[pos.x, pos.y] = value; } }

		//--------------------------------------
		// Get Information
		//--------------------------------------

		public XY GetPosition(T item)
		{
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Height; y++)
					if (cells[x, y].Equals(item))
						return new XY(x, y);

			return XY.invalid;
		}

		public bool Contains(T item)
		{
			foreach (var _item in this)
				if (item.Equals(_item))
					return true;

			return false;
		}

		public bool IsValid(XY pos)
		{
			return pos.x >= 0 && pos.x < Width && pos.y >= 0 && pos.y < Height;
		}

		public T[] AdjacentItems(T item)
		{
			List<T> adjacentTiles = new List<T>();

			foreach (var pos in AdjacentTiles(GetPosition(item)))
				if (this[pos] != null)
					adjacentTiles.Add(this[pos]);

			return adjacentTiles.ToArray();
		}

		public XY[] AdjacentTiles(XY pos)
		{
			List<XY> adjacentTiles = new List<XY>();

			for (int x = -1; x <= +1; x++)
				for (int y = -1; y <= +1; y++)
				{
					XY n = new XY(pos.x + x, pos.y + y);
					if (IsValid(n))
						adjacentTiles.Add(n);
				}

			return adjacentTiles.ToArray();
		}

		public T[] OrthogonalItems(T item)
		{
			List<T> orthogonalItems = new List<T>();

			foreach (var pos in OrthogonalTiles(GetPosition(item)))
				if (this[pos] != null)
					orthogonalItems.Add(this[pos]);

			return orthogonalItems.ToArray();
		}

		public XY[] OrthogonalTiles(XY pos)
		{
			List<XY> orthogonalTiles = new List<XY>();

			var n = new XY(pos.x-1, pos.y);
			if (IsValid(n)) orthogonalTiles.Add(n);

			n = new XY(pos.x+1, pos.y);
			if (IsValid(n)) orthogonalTiles.Add(n);

			n = new XY(pos.x, pos.y-1);
			if (IsValid(n)) orthogonalTiles.Add(n);

			n = new XY(pos.x, pos.y+1);
			if (IsValid(n)) orthogonalTiles.Add(n);

			return orthogonalTiles.ToArray();
		}

		//--------------------------------------
		// Modify
		//--------------------------------------

		public void Remove(T t)
		{
			var pos = GetPosition(t);

			if (IsValid(pos))
				this[pos] = default(T);
		}

		public void Swap(T A, T B)
		{
			Swap(GetPosition(A), GetPosition(B));
		}

		public void Swap(XY A, XY B)
		{
			T temp = this[A];
			this[A] = this[B];
			this[B] = this[A];
		}

		public void Clear()
		{
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Height; y++)
					this[x, y] = default(T);
		}

		//--------------------------------------
		// Enumerate
		//--------------------------------------

		public IEnumerator<T> GetEnumerator()
		{
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Height; y++)
					yield return cells[x, y];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
