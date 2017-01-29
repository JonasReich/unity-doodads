using UnityEngine;

namespace Spellfish
{
    /// <summary>
    /// Hides/destroys a collection of GameObjects as soon as the game enters PlayMode
    /// </summary>
    public class PlaymodeHider : MonoBehaviour
    {
		/// <summary>
		/// Whether the target should be destroyed. By default targets are disabled only
		/// </summary>
        public bool destroy;

		/// <summary>
		/// Whether the script should affect the entire GameObject.
		/// By default the scipt only targets the component itself
		/// </summary>
        public bool entireGameObject;

		/// <summary>
		/// Whether the script should target itslef. This is enabled by default
		/// </summary>
        public bool self = true;
		
		/// <summary>
		/// The list of behaviours that are to be disabled/destroyed
		/// </summary>
        public MonoBehaviour[] targetList;


        private void Awake ()
        {
            if (targetList.Length > 0)
                foreach (var target in targetList)
                    DestoryOrDisable(target);

            if (self)
                DestoryOrDisable(this);
        }

        void DestoryOrDisable (MonoBehaviour target)
        {
            if (destroy)
            {
                if (entireGameObject)
                    Destroy(target.gameObject);
                else
                    Destroy(target);
            }
            else
            {
                if (entireGameObject)
                    target.gameObject.SetActive(false);
                else
                    target.enabled = false;
            }
        }
    }
}
