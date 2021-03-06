﻿//-------------------------------------------
// Copyright (c) 2017 - JonasReich
//-------------------------------------------

using UnityEngine;
using System.Collections.Generic;

namespace Doodads.UI
{
	public class ScrollableList<TDataType> : MonoBehaviour
	{
		ScrollableListElement<TDataType> listElementPrefab;
		public RectTransform containerTransform;

		List<GameObject> children = new List<GameObject>();


		void Awake()
		{
			listElementPrefab = GetComponentInChildren<ScrollableListElement<TDataType>>();
			listElementPrefab.gameObject.SetActive(false);
		}


		public virtual void LoadView(IEnumerable<TDataType> elements)
		{
			UpdateList(elements);
			gameObject.SetActive(true);
		}

		public virtual void UpdateList(IEnumerable<TDataType> elements)
		{
			ClearPanel();

			if (elements == null)
				return;

			foreach (var element in elements)
				AddElement(element);
		}

		public virtual void ClearPanel()
		{
			RefreshChildren();
			if (children.Count == 0)
				return;

			children.RemoveAt(0);
			children.ForEach(c => Destroy(c.gameObject));
		}


		protected void RefreshChildren()
		{
			children.Clear();
			foreach (Transform child in containerTransform)
				children.Add(child.gameObject);
		}

		protected virtual void AddElement(TDataType element)
		{
			var button = Instantiate(listElementPrefab.gameObject);
			button.SetActive(true);
			var elemScript = button.GetComponent<ScrollableListElement<TDataType>>();
			elemScript.SetData(element, containerTransform);
			children.Add(button);
		}
	}
}
