//-------------------------------------------
// Copyright (c) 2017 - JonasReich
//-------------------------------------------

using UnityEngine;

namespace UnityDoodats
{
    /// <summary>
    /// Hides/destroys a collection of GameObjects as soon as the game enters PlayMode
    /// </summary>
    public class PlaymodeHider : MonoBehaviour
    {
		/// <summary>
		/// Whether the target should be destroyed. (Default: disable only)
		/// </summary>
		[Tooltip("Whether the target should be destroyed. \n(Default: disable only)")]
        public bool destroy = false;

		/// <summary>
		/// Whether the script should affect the entire GameObject.
		/// (Default: Target Component)
		/// </summary>
		[Tooltip("Whether the script should affect the entire GameObject. \n(Default: Target Component)")]
        public bool entireGameObject = false;

		/// <summary>
		/// Whether the script should target itslef. (Default: enabled)
		/// </summary>
		[Tooltip("Whether the script should target itslef. \n(Default: enabled)")]
        public bool self = true;

		/// <summary>
		/// The list of behaviours that are to be disabled/destroyed
		/// </summary>
		[Tooltip("The list of behaviours that are to be disabled/destroyed")]
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
