using UnityEngine;

namespace NAMESPACE
{
	// Base types

	[System.Serializable]
	public class IntPair : Pair<int> { }

	[System.Serializable]
	public class FloatPair : Pair<float> { }

	[System.Serializable]
	public class StringPair : Pair<string> { }


	// Unity types

	[System.Serializable]
	public class GameObjectPair : Pair<GameObject> { }

	[System.Serializable]
	public class TransformPair : Pair<Transform> { }

	[System.Serializable]
	public class Vector3Pair : Pair<Vector3> { }


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
