//-------------------------------------------
// Copyright (c) 2017 - JonasReich
//-------------------------------------------

using UnityEngine;

namespace Doodads.UI
{
	public abstract class ScrollableListElement<T> : MonoBehaviour
	{
		public T DataCache { get; protected set; }

		Transform _transform;

		protected virtual void Awake()
		{
			_transform = transform;
		}

		public virtual void SetData(T data, Transform parentTransform)
		{
			_transform.SetParent(parentTransform, false);
			UpdateData(data);
		}

		public virtual void UpdateData(T data)
		{
			DataCache = data;
		}
	}
}
