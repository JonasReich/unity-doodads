//-------------------------------------------
// Copyright (c) 2017 - JonasReich
//-------------------------------------------

using UnityEngine;

namespace UnityDoodats
{
	/// <summary>
	/// 2-dimensional integer vector
	/// </summary>
	public class XY : IntPair
	{
		public int x { get { return first; }set { first = value; } }
		public int y { get { return second; } set { second = value; } }
		public XY(int x, int y) : base(x, y) { }
		public static readonly XY invalid = new XY(-1, -1);
	}

	// Base types

	[System.Serializable]
	public class IntPair : Pair<int>
	{
		public IntPair() : base() { }
		public IntPair(int first, int second) : base(first, second) { }
	}

	[System.Serializable]
	public class FloatPair : Pair<float>
	{
		public FloatPair() : base() { }
		public FloatPair(float first, float second) : base(first, second) { }
	}

	[System.Serializable]
	public class StringPair : Pair<string>
	{
		public StringPair() : base() { }
		public StringPair(string first, string second) : base(first, second) { }
	}


	// Unity types

	[System.Serializable]
	public class GameObjectPair : Pair<GameObject>
	{
		public GameObjectPair() : base() { }
		public GameObjectPair(GameObject first, GameObject second) : base(first, second) { }
	}

	[System.Serializable]
	public class TransformPair : Pair<Transform>
	{
		public TransformPair() : base() { }
		public TransformPair(Transform first, Transform second) : base(first, second) { }
	}

	[System.Serializable]
	public class Vector3Pair : Pair<Vector3>
	{
		public Vector3Pair() : base() { }
		public Vector3Pair(Vector3 first, Vector3 second) : base(first, second) { }
	}


	/// <summary>
	/// A pair of two values - each of type T
	/// </summary>
	[System.Serializable]
	public class Pair<T> : Pair<T, T>
	{
		public Pair () : base() { }
		public Pair (T first, T second) : base(first, second) { }
	}

	/// <summary>
	/// A pair of two values - the first of type T, the second of type U
	/// </summary>
	[System.Serializable]
	public class Pair<T, U>
	{
		public T first;
		public U second;


		public Pair () { }

		public Pair (T first, U second)
		{
			this.first = first;
			this.second = second;
		}


		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;
			if (obj == this)
				return true;
			Pair<T, U> other = obj as Pair<T, U>;
			if (other == null)
				return false;

			return
				(((first == null) && (other.first == null))
					|| ((first != null) && first.Equals(other.first)))
				  &&
				(((second == null) && (other.second == null))
					|| ((second != null) && second.Equals(other.second)));
		}

		public override int GetHashCode ()
		{
			int hashcode = 0;
			if (first != null)
				hashcode += first.GetHashCode();
			if (second != null)
				hashcode += second.GetHashCode();

			return hashcode;
		}
	}
}
