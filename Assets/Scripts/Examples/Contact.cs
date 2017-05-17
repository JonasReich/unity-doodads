using System;
using UnityEngine;

namespace UnityDoodats.Examples
{
	/// <summary>
	/// An adressbook contact
	/// </summary>
	[Serializable]
	public class Contact
	{
		public Sprite picture;

		public string name;

		[TextArea(5,10)]
		public string description;
	}
}
