using UnityEngine;

namespace NAMESPACE
{
	/// <summary>
	/// Singleton with a collection of LayerMasks
	/// </summary>
	public class LayerManager : MonoBehaviour
	{
		public static LayerManager Instance { get; private set; }

		public LayerMask uiLayers;
		public LayerMask enemyLayers;
		public LayerMask groundLayers;
		public LayerMask pitLayers;
		public LayerMask telekinesisLayers;

		private void Awake ()
		{
			if (Instance != null)
				Destroy(this);
			else
				Instance = this;
		}
	}
}
