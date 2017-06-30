//-------------------------------------------
// Copyright (c) 2017 - JonasReich
//-------------------------------------------

using Doodads.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Doodads.Examples
{
	/// <summary>
	/// The elements stored in the ScrollableContactList
	/// </summary>
	public class ScrollableContactListElement : ScrollableListElement<Contact>
	{
		public Image pictureImage;
		public Text nameText;
		public Text descriptionText;

		public override void UpdateData(Contact data)
		{
			base.UpdateData(data);

			// Update UI
			pictureImage.sprite = data.picture;
			nameText.text = data.name;
			descriptionText.text = data.description;
		}
	}
}
