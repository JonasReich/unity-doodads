using UnityEngine;
using System.Collections.Generic;

namespace UnityDoodats.UI
{
	public class ScrolledList<TDataType> : MonoBehaviour
	{
		ScrolledListElement<TDataType> listElementPrefab;

		RectTransform containerTransform;
		bool isInitialized;
		List<GameObject> children = new List<GameObject>();


		private void Awake()
		{
			Init();
		}


		public void Init()
		{
			if (isInitialized)
				return;

			listElementPrefab = GetComponentInChildren<ScrolledListElement<TDataType>>();
			containerTransform = GetComponent<RectTransform>();

			listElementPrefab.gameObject.SetActive(false);

			isInitialized = true;
		}

		public virtual void LoadView(IEnumerable<TDataType> elements)
		{
			Init();

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
			var elemScript = button.GetComponent<ScrolledListElement<TDataType>>();
			elemScript.SetData(element, containerTransform);
			children.Add(button);
		}
	}
}
