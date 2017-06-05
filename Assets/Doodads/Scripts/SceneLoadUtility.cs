//-------------------------------------------
// Copyright (c) 2017 - JonasReich
//-------------------------------------------

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Doodads
{
	/// <summary>
	/// Helper object that executes a set of actions and destroys itself afterwards
	/// </summary>
	public class SceneLoadUtility : MonoBehaviour
	{
		[Scene]
		public string[] scenesToLoad;
		public MonoBehaviour[] behavioursToDisableOnSceneLoad;
		public GameObject[] objectsToDeactivateOnSceneLoad;
		public UnityEvent onSceneLoad;

		// ---------------------------------------------

		private void Start ()
		{
			onSceneLoad.Invoke();

			if (scenesToLoad.Length > 0)
				foreach (var scene in scenesToLoad)
					SceneManager.LoadScene(scene, LoadSceneMode.Additive);

			foreach (var behaviour in behavioursToDisableOnSceneLoad)
				behaviour.enabled = false;

			foreach (var gameObject in objectsToDeactivateOnSceneLoad)
				gameObject.SetActive(false);

			Destroy(gameObject);
		}
	}
}
