//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using UnityEngine;

namespace Doodads
{
	/// <summary>
	/// Makes an object follow the player cursor in screen space
	/// </summary>
	public class FollowCursor : MonoBehaviour
	{	
		void Update ()
		{
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
	}
}
