//-------------------------------------------
// Copyright (c) 2017 - JonasReich
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doodads.Examples
{

	public class AdressBook : MonoBehaviour
	{
		public ScrolledContactList contactListUI;

		public List<Contact> contacts;


		// Use this for initialization
		void Start()
		{
			contactListUI.LoadView(contacts);
		}
	}
}
