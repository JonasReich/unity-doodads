using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Sabresaurus.PlayerPrefsExtension
{
	public class PlayerPrefsEditor : EditorWindow
	{
		[Serializable]
		private struct PlayerPrefPair
		{
			public string Key { get; set; }
			public object Value { get; set; }
		}

		enum PlayerPrefType
		{
			Float = 0,
			Int,
			String
		};


		readonly DateTime MISSING_DATETIME = new DateTime(1601, 1, 1);


		List<PlayerPrefPair> deserializedPlayerPrefs = new List<PlayerPrefPair>();
		List<PlayerPrefPair> filteredPlayerPrefs = new List<PlayerPrefPair>();

		string searchFilter = string.Empty;

		// Track last successful deserialisation to prevent doing this too often. On OSX this uses the player prefs file
		// last modified time, on Windows we just poll repeatedly and use this to prevent polling again too soon.
		DateTime? lastDeserialization = null;

		Vector2 scrollPosition;
		Vector2 lastScrollPosition;


		// Prevent OnInspector() forcing a repaint every time it's called
		int inspectorUpdateFrame = 0;

		//--------------------------------------------
		// User Settings
		//--------------------------------------------

		// Automatically attempt to decrypt keys and values that are detected as encrypted
		bool automaticDecryption = false;

		// Because of some issues with deleting from OnGUI, we defer it to OnInspectorUpdate() instead
		string keyQueuedForDeletion = null;

		// Company and product name for importing player prefs from other projects
		string importCompanyName = "";
		string importProductName = "";

		//--------------------------------------------
		// New Entry settings
		//--------------------------------------------

		PlayerPrefType newEntry_Type = PlayerPrefType.String;
		bool newEntry_IsEncrypted = false;
		string newEntry_Key = "";
		float newEntry_ValueFloat = 0;
		int newEntry_ValueInt = 0;
		string newEntry_ValueString = "";


		//--------------------------------------------
		// Unity Messages
		//--------------------------------------------

		[MenuItem("Window/PlayerPrefs Editor")]
		private static void Init()
		{
			// Get existing open window or if none, make a new one:
			PlayerPrefsEditor editor = (PlayerPrefsEditor)GetWindow(typeof(PlayerPrefsEditor));
			editor.titleContent = new GUIContent("PlayerPrefs");

			// Require the editor window to be at least 300 pixels wide
			Vector2 minSize = editor.minSize;
			minSize.x = 300;
			minSize.y = 450;
			editor.minSize = minSize;

			editor.importCompanyName = PlayerSettings.companyName;
			editor.importProductName = PlayerSettings.productName;
		}

		private void OnGUI()
		{
			LoadPlayerPrefs();

			EditorGUILayout.Space();
			DrawSearchBar();

			DrawMainList();

			automaticDecryption = EditorGUILayout.Toggle("Auto-Decryption", automaticDecryption);

			DrawAddEntryMenu();
			EditorGUILayout.Space();
			DrawImportMenu();
			GUILayout.Space(15);

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Clear All") && EditorUtility.DisplayDialog("Delete All PlayerPrefs",
					"You are about to delete all PlayerPrefs in your project.\nThis is irreversible and cannot be undone!\n\nProceed anyways?", "Delete", "Cancel"))
			{
				PlayerPrefs.DeleteAll();
				PlayerPrefs.Save();

				// Clear the cache too, for an instant visibility update for OSX
				deserializedPlayerPrefs.Clear();
			}

			// Mainly needed for OSX, this will encourage PlayerPrefs to save to file (but still may take a few seconds)
			if (GUILayout.Button("Force Save"))
			{
				PlayerPrefs.Save();
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			// If the user has scrolled, deselect - this is because control IDs within carousel will change when scrolled
			// so we'd end up with the wrong box selected.
			if (scrollPosition != lastScrollPosition)
			{
				// Deselect
				GUI.FocusControl("");
			}
		}

		// OnInspectorUpdate() is called by Unity at 10 times a second
		private void OnInspectorUpdate()
		{
			if (string.IsNullOrEmpty(keyQueuedForDeletion) == false)
			{
				// find the ID and defer it for deletion
				if (deserializedPlayerPrefs != null)
				{
					int entryCount = deserializedPlayerPrefs.Count;
					for (int i = 0; i < entryCount; i++)
					{
						if (deserializedPlayerPrefs[i].Key == keyQueuedForDeletion)
						{
							deserializedPlayerPrefs.RemoveAt(i);
							break;
						}
					}
				}

				keyQueuedForDeletion = null;

				UpdateSearch();
				Repaint();
			}
			else if (inspectorUpdateFrame % 10 == 0)
			{
				// Once a second (every 10th frame)
				Repaint();
			}

			inspectorUpdateFrame++;
		}

		//--------------------------------------------
		// Loading & (De)Serialization
		//--------------------------------------------

		private void AddNewEntry()
		{
			if (newEntry_IsEncrypted)
			{
				string encryptedKey = PlayerPrefsUtility.KEY_PREFIX + SimpleEncryption.EncryptString(newEntry_Key);

				// Note: All encrypted values are stored as string
				string encryptedValue = "";

				switch (newEntry_Type)
				{
					case PlayerPrefType.Float:
						encryptedValue = PlayerPrefsUtility.VALUE_FLOAT_PREFIX + SimpleEncryption.EncryptFloat(newEntry_ValueFloat);
						break;
					case PlayerPrefType.Int:
						encryptedValue = PlayerPrefsUtility.VALUE_INT_PREFIX + SimpleEncryption.EncryptInt(newEntry_ValueInt);
						break;
					case PlayerPrefType.String:
						encryptedValue = PlayerPrefsUtility.VALUE_STRING_PREFIX + SimpleEncryption.EncryptString(newEntry_ValueString);
						break;
				}

				PlayerPrefs.SetString(encryptedKey, encryptedValue);
				CacheRecord(encryptedKey, encryptedValue);
			}
			else
			{
				switch (newEntry_Type)
				{
					case PlayerPrefType.Float:
						PlayerPrefs.SetFloat(newEntry_Key, newEntry_ValueFloat);
						CacheRecord(newEntry_Key, newEntry_ValueFloat);
						break;
					case PlayerPrefType.Int:
						PlayerPrefs.SetInt(newEntry_Key, newEntry_ValueInt);
						CacheRecord(newEntry_Key, newEntry_ValueInt);
						break;
					case PlayerPrefType.String:
						PlayerPrefs.SetString(newEntry_Key, newEntry_ValueString);
						CacheRecord(newEntry_Key, newEntry_ValueString);
						break;
				}
			}

			PlayerPrefs.Save();
		}


		private void LoadPlayerPrefs()
		{
			if (Application.platform == RuntimePlatform.OSXEditor)
			{
				// Construct the plist filename from the project's settings
				string plistFilename = string.Format("unity.{0}.{1}.plist", PlayerSettings.companyName, PlayerSettings.productName);
				// Now construct the fully qualified path
				string playerPrefsPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library/Preferences"), plistFilename);

				// Determine when the plist was last written to
				DateTime lastWriteTime = File.GetLastWriteTimeUtc(playerPrefsPath);

				// If we haven't deserialized the player prefs already, or the written file has changed then deserialize 
				// the latest version
				if (!lastDeserialization.HasValue || lastDeserialization.Value != lastWriteTime)
				{
					// Deserialize the actual player prefs from file into a cache
					deserializedPlayerPrefs = new List<PlayerPrefPair>(DeserializePlayerPrefs(PlayerSettings.companyName, PlayerSettings.productName));

					// Record the version of the file we just read, so we know if it changes in the future
					lastDeserialization = lastWriteTime;
				}

				if (lastWriteTime != MISSING_DATETIME)
				{
					GUILayout.Label("PList Last Written: " + lastWriteTime.ToString());
				}
				else
				{
					GUILayout.Label("PList Does Not Exist");
				}
			}
			else if (Application.platform == RuntimePlatform.WindowsEditor)
			{
				// Windows works a bit differently to OSX, we just regularly query the registry. So don't query too often
				if (!lastDeserialization.HasValue || DateTime.UtcNow - lastDeserialization.Value > TimeSpan.FromMilliseconds(500))
				{
					// Deserialize the actual player prefs from registry into a cache
					deserializedPlayerPrefs = new List<PlayerPrefPair>(DeserializePlayerPrefs(PlayerSettings.companyName, PlayerSettings.productName));

					// Record the latest time, so we don't fetch again too quickly
					lastDeserialization = DateTime.UtcNow;
				}
			}
		}

		/// <summary>
		/// Load PlayerPrefs from file system (OSX) or registry (Windows)
		/// </summary>
		private PlayerPrefPair[] DeserializePlayerPrefs(string companyName, string productName)
		{
			if (Application.platform == RuntimePlatform.OSXEditor)
				return LoadSavedPrefsFromDisk(companyName, productName);
			else if (Application.platform == RuntimePlatform.WindowsEditor)
				return LoadSavedPrefsFromRegistry(companyName, productName);
			else
				throw new NotSupportedException("PlayerPrefsEditor doesn't support this Unity Editor platform");
		}

		private PlayerPrefPair[] LoadSavedPrefsFromRegistry(string companyName, string productName)
		{
			// From Unity docs: On Windows, PlayerPrefs are stored in the registry under HKCU\Software\[company name]\[product name] key, where company and product names are the names set up in Project Settings.
#if UNITY_5_5_OR_NEWER
			// From Unity 5.5 editor player prefs moved to a specific location
			Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Unity\\UnityEditor\\" + companyName + "\\" + productName);
#else
            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\" + companyName + "\\" + productName);
#endif

			if (registryKey == null)
			{
				// No existing player prefs saved (which is valid), so just return an empty array
				return new PlayerPrefPair[0];
			}

			string[] valueNames = registryKey.GetValueNames();
			PlayerPrefPair[] tempPlayerPrefs = new PlayerPrefPair[valueNames.Length];


			// Parse and convert the registry saved player prefs into our array
			for (int i = 0; i < valueNames.Length; i++)
			{
				string valueName = valueNames[i];
				string key = valueName;

				// Remove the _h193410979 style suffix used on player pref keys in Windows registry
				int index = key.LastIndexOf("_");
				key = key.Remove(index, key.Length - index);

				tempPlayerPrefs[i] = new PlayerPrefPair() { Key = key, Value = RetrieveRegistryValue(registryKey, valueName, key) };
			}

			return tempPlayerPrefs;
		}

		private object RetrieveRegistryValue(Microsoft.Win32.RegistryKey registryKey, string valueName, string key)
		{
			object value = registryKey.GetValue(valueName);

			// Unfortunately floats will come back as an int (at least on 64 bit) because the float is stored as
			// 64 bit but marked as 32 bit - which confuses the GetValue() method greatly! 
			if (value.GetType() == typeof(int))
			{
				// If the player pref is not actually an int then it must be a float, this will evaluate to true
				// (impossible for it to be 0 and -1 at the same time)
				if (PlayerPrefs.GetInt(key, -1) == -1 && PlayerPrefs.GetInt(key, 0) == 0)
				{
					// Fetch the float value from PlayerPrefs in memory
					value = PlayerPrefs.GetFloat(key);
				}
			}
			else if (value.GetType() == typeof(byte[]))
			{
				// On Unity 5 a string may be stored as binary, so convert it back to a string
				value = System.Text.Encoding.Default.GetString((byte[])value);
			}

			return value;
		}

		private PlayerPrefPair[] LoadSavedPrefsFromDisk(string companyName, string productName)
		{
			/*
			 * From Unity docs: On Mac OS X PlayerPrefs are stored in ~/Library/Preferences folder,
			 * in a file named unity.[company name].[product name].plist, 
			 * where company and product names are the names set up in Project Settings. 
			 * The same .plist file is used for both Projects run in the Editor and standalone players.
			 */

			// Construct the plist filename from the project's settings
			string plistFilename = string.Format("unity.{0}.{1}.plist", companyName, productName);
			// Now construct the fully qualified path
			string playerPrefsPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library/Preferences"), plistFilename);

			if (File.Exists(playerPrefsPath) == false)
				return new PlayerPrefPair[0];

			object plist = Plist.readPlist(playerPrefsPath);
			Dictionary<string, object> parsed = plist as Dictionary<string, object>;

			PlayerPrefPair[] tempPlayerPrefs = new PlayerPrefPair[parsed.Count];
			int i = 0;
			foreach (KeyValuePair<string, object> pair in parsed)
			{
				if (pair.Value.GetType() == typeof(double))
				{
					// Some float values may come back as double, so convert them back to floats
					tempPlayerPrefs[i] = new PlayerPrefPair() { Key = pair.Key, Value = (float)(double)pair.Value };
				}
				else
				{
					tempPlayerPrefs[i] = new PlayerPrefPair() { Key = pair.Key, Value = pair.Value };
				}
				i++;
			}

			return tempPlayerPrefs;
		}


		private void CacheRecord(string key, object value)
		{
			bool replacedExistingKey = false;

			int entryCount = deserializedPlayerPrefs.Count;
			for (int i = 0; i < entryCount; i++)
			{
				if (deserializedPlayerPrefs[i].Key == key)
				{
					deserializedPlayerPrefs[i] = new PlayerPrefPair() { Key = key, Value = value };
					replacedExistingKey = true;
					break;
				}
			}

			if (!replacedExistingKey)
			{
				deserializedPlayerPrefs.Add(new PlayerPrefPair() { Key = key, Value = value });
			}

			UpdateSearch();
		}

		private void DeleteCachedRecord(string fullKey)
		{
			keyQueuedForDeletion = fullKey;
		}


		private static void SaveStringFieldChanges(string fullKey, string initialValue, string newValue, bool isEncryptedPair, bool autoDecryptHasFailed)
		{
			if (newValue != initialValue && autoDecryptHasFailed == false)
			{
				if (isEncryptedPair)
				{
					string encryptedValue = PlayerPrefsUtility.VALUE_STRING_PREFIX + SimpleEncryption.EncryptString(newValue);
					PlayerPrefs.SetString(fullKey, encryptedValue);
				}
				else
				{
					PlayerPrefs.SetString(fullKey, newValue);
				}
				PlayerPrefs.Save();
			}
		}

		private static void SaveIntFieldChanges(string fullKey, int initialValue, int newValue, bool isEncryptedPair)
		{
			if (newValue != initialValue)
			{
				if (isEncryptedPair)
				{
					string encryptedValue = PlayerPrefsUtility.VALUE_INT_PREFIX + SimpleEncryption.EncryptInt(newValue);
					PlayerPrefs.SetString(fullKey, encryptedValue);
				}
				else
				{
					PlayerPrefs.SetInt(fullKey, newValue);
				}
				PlayerPrefs.Save();
			}
		}

		private static void SaveFloatFieldChanges(string fullKey, float initialValue, float newValue, bool isEncryptedPair)
		{
			if (newValue != initialValue)
			{
				if (isEncryptedPair)
				{
					string encryptedValue = PlayerPrefsUtility.VALUE_FLOAT_PREFIX + SimpleEncryption.EncryptFloat(newValue);
					PlayerPrefs.SetString(fullKey, encryptedValue);
				}
				else
				{
					PlayerPrefs.SetFloat(fullKey, newValue);
				}
				PlayerPrefs.Save();
			}
		}


		private void UpdateSearch()
		{
			filteredPlayerPrefs.Clear();

			// Don't attempt to find the search results if a search filter hasn't actually been supplied
			if (string.IsNullOrEmpty(searchFilter))
				return;

			int entryCount = deserializedPlayerPrefs.Count;

			for (int i = 0; i < entryCount; i++)
			{
				string fullKey = deserializedPlayerPrefs[i].Key;
				string displayKey = fullKey;

				// Special case for encrypted keys in auto decrypt mode, search should use decrypted values
				bool isEncryptedPair = PlayerPrefsUtility.IsEncryptedKey(deserializedPlayerPrefs[i].Key);
				if (automaticDecryption && isEncryptedPair)
				{
					displayKey = PlayerPrefsUtility.DecryptKey(fullKey);
				}

				// If the key contains the search filter (ToLower used on both parts to make this case insensitive)
				if (displayKey.ToLower().Contains(searchFilter.ToLower()))
				{
					filteredPlayerPrefs.Add(deserializedPlayerPrefs[i]);
				}
			}
		}

		//--------------------------------------------
		// Draw Menu Items
		//--------------------------------------------

		private void DrawSearchBar()
		{
			EditorGUILayout.BeginHorizontal();

			GUILayout.Label("Search", GUILayout.MaxWidth(50));
			string newSearchFilter = EditorGUILayout.TextField(searchFilter);

			// search filter has changed
			if (newSearchFilter != searchFilter)
			{
				searchFilter = newSearchFilter;
				UpdateSearch();
			}

			EditorGUILayout.EndHorizontal();
		}

		private void DrawMainList()
		{
			// The bold table headings
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Key", EditorStyles.boldLabel);
			GUILayout.Label("Value", EditorStyles.boldLabel);
			GUILayout.Label("Type", EditorStyles.boldLabel, GUILayout.Width(37));
			GUILayout.Label("Del", EditorStyles.boldLabel, GUILayout.Width(25));
			EditorGUILayout.EndHorizontal();

			// Create a GUIStyle that can be manipulated for the various text fields
			GUIStyle textFieldStyle = new GUIStyle(GUI.skin.textField);

			// Could be dealing with either the full list or search results, so get the right list
			List<PlayerPrefPair> activePlayerPrefs = deserializedPlayerPrefs;

			if (!string.IsNullOrEmpty(searchFilter))
			{
				activePlayerPrefs = filteredPlayerPrefs;
			}

			// Cache the entry count
			int entryCount = activePlayerPrefs.Count;

			// Record the last scroll position so we can calculate if the user has scrolled this frame
			lastScrollPosition = scrollPosition;

			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

			// Ensure the scroll doesn't go below zero
			if (scrollPosition.y < 0)
			{
				scrollPosition.y = 0;
			}

			// The following code has been optimised so that rather than attempting to draw UI for every single PlayerPref
			// it instead only draws the UI for those currently visible in the scroll view and pads above and below those
			// results to maintain the right size using GUILayout.Space(). This enables us to work with thousands of 
			// PlayerPrefs without slowing the interface to a halt.

			// Fixed height of one of the rows in the table
			float rowHeight = 18;

			// For simplicity, use Screen.height (the overhead is negligible)
			int visibleRowCount = Mathf.CeilToInt(Screen.height / rowHeight);
			int firstVisibleRowIndex = Mathf.FloorToInt(scrollPosition.y / rowHeight);
			// (last shown index + 1)
			int visibleIndexLimit = firstVisibleRowIndex + visibleRowCount;

			// cap limit to player prefs count
			if (entryCount < visibleIndexLimit)
			{
				visibleIndexLimit = entryCount;
			}

			// If the number of displayed player prefs is smaller than the number we can display (like we're at the end
			// of the list) then move the starting index back to adjust
			if (visibleIndexLimit - firstVisibleRowIndex < visibleRowCount)
			{
				firstVisibleRowIndex -= visibleRowCount - (visibleIndexLimit - firstVisibleRowIndex);
			}

			if (firstVisibleRowIndex < 0)
			{
				firstVisibleRowIndex = 0;
			}

			// Pad above the on screen results so that we're not wasting draw calls on invisible UI and the drawn player
			// prefs end up in the same place in the list
			GUILayout.Space(firstVisibleRowIndex * rowHeight);

			for (int i = firstVisibleRowIndex; i < visibleIndexLimit; i++)
			{
				bool isEncryptedPair = PlayerPrefsUtility.IsEncryptedKey(activePlayerPrefs[i].Key);

				if (isEncryptedPair)
				{
					ApplyEncryptionColor(textFieldStyle);
				}
				else
				{
					// Normal player prefs are black
					textFieldStyle.normal.textColor = GUI.skin.textField.normal.textColor;
					textFieldStyle.focused.textColor = GUI.skin.textField.focused.textColor;
				}

				// The full key is the key that's actually stored in player prefs
				string fullKey = activePlayerPrefs[i].Key;

				// Display key is used so in the case of encrypted keys, we display the decrypted version instead (in
				// auto-decrypt mode).
				string displayKey = fullKey;

				// Used for accessing the type information stored against the player pref
				object deserializedValue = activePlayerPrefs[i].Value;

				bool autoDecryptHasFailed = false;

				if (isEncryptedPair && automaticDecryption)
				{
					// This may throw exceptions (e.g. if private key changes), so wrap in a try-catch
					try
					{
						deserializedValue = PlayerPrefsUtility.GetEncryptedValue(fullKey, (string)deserializedValue);
						displayKey = PlayerPrefsUtility.DecryptKey(fullKey);
					}
					catch
					{
						textFieldStyle.normal.textColor = Color.red;
						textFieldStyle.focused.textColor = Color.red;

						autoDecryptHasFailed = true;
					}
				}

				EditorGUILayout.BeginHorizontal();

				Type valueType = GetValueType(fullKey, deserializedValue, isEncryptedPair, autoDecryptHasFailed);

				EditorGUILayout.TextField(displayKey, textFieldStyle);

				DrawValueField(fullKey, displayKey, valueType, isEncryptedPair, autoDecryptHasFailed, textFieldStyle);
				DrawDeleteButton(fullKey);

				EditorGUILayout.EndHorizontal();
			}

			// Calculate the padding at the bottom of the scroll view (because only visible player pref rows are drawn)
			float bottomPadding = (entryCount - visibleIndexLimit) * rowHeight;

			// If the padding is positive, pad the bottom so that the layout and scroll view size is correct still
			if (bottomPadding > 0)
			{
				GUILayout.Space(bottomPadding);
			}

			EditorGUILayout.EndScrollView();

			GUILayout.Label("Entry Count: " + entryCount);
		}

		private void DrawValueField(string fullKey, string displayKey, Type valueType, bool isEncryptedPair, bool autoDecryptHasFailed, GUIStyle textFieldStyle)
		{
			if (valueType == typeof(float))
			{
				float initialValue;
				if (isEncryptedPair && automaticDecryption)
					initialValue = PlayerPrefsUtility.GetEncryptedFloat(displayKey);
				else
					initialValue = PlayerPrefs.GetFloat(fullKey);

				float newValue = EditorGUILayout.FloatField(initialValue, textFieldStyle);
				SaveFloatFieldChanges(fullKey, initialValue, newValue, isEncryptedPair);

				GUILayout.Label("float", GUILayout.Width(37));
			}
			else if (valueType == typeof(int))
			{
				int initialValue;
				if (isEncryptedPair && automaticDecryption)
					initialValue = PlayerPrefsUtility.GetEncryptedInt(displayKey);
				else
					initialValue = PlayerPrefs.GetInt(fullKey);

				int newValue = EditorGUILayout.IntField(initialValue, textFieldStyle);
				SaveIntFieldChanges(fullKey, initialValue, newValue, isEncryptedPair);

				GUILayout.Label("int", GUILayout.Width(37));
			}
			else if (valueType == typeof(string))
			{
				string initialValue;
				if (isEncryptedPair && automaticDecryption && !autoDecryptHasFailed)
					initialValue = PlayerPrefsUtility.GetEncryptedString(displayKey);
				else
					initialValue = PlayerPrefs.GetString(fullKey);

				string newValue = EditorGUILayout.TextField(initialValue, textFieldStyle);

				SaveStringFieldChanges(fullKey, initialValue, newValue, isEncryptedPair, autoDecryptHasFailed);

				if (isEncryptedPair && !automaticDecryption && !string.IsNullOrEmpty(initialValue))
				{
					// Because encrypted values when not in auto-decrypt mode are stored as string, determine their
					// encrypted type and display that instead for these encrypted PlayerPrefs
					PlayerPrefType playerPrefType = (PlayerPrefType)(int)char.GetNumericValue(initialValue[0]);
					GUILayout.Label(playerPrefType.ToString().ToLower(), GUILayout.Width(37));
				}
				else
				{
					GUILayout.Label("string", GUILayout.Width(37));
				}
			}
		}

		private void DrawDeleteButton(string fullKey)
		{

			if (GUILayout.Button("X", GUILayout.Width(25)) && EditorUtility.DisplayDialog("Delete PlayerPref",
					"You are about to delete the PlayerPref key\n'" + fullKey + "'", "Delete", "Cancel"))
			{
				PlayerPrefs.DeleteKey(fullKey);
				PlayerPrefs.Save();
				DeleteCachedRecord(fullKey);
			}
		}

		private void DrawAddEntryMenu()
		{
			GUIStyle textFieldStyle = new GUIStyle(GUI.skin.textField);

			EditorGUILayout.Space();
			GUILayout.Label("Add New Entry", EditorStyles.boldLabel);

			//---------------
			// Encryption/Type Settings
			//---------------
			EditorGUILayout.BeginHorizontal();
			newEntry_IsEncrypted = GUILayout.Toggle(newEntry_IsEncrypted, "Encrypt");
			newEntry_Type = (PlayerPrefType)GUILayout.SelectionGrid((int)newEntry_Type, new string[] { "float", "int", "string" }, 3);
			EditorGUILayout.EndHorizontal();

			if (newEntry_IsEncrypted)
				ApplyEncryptionColor(textFieldStyle);

			EditorGUILayout.BeginHorizontal();

			//---------------
			// Key and Value Fields
			//---------------
			// Track the next control so we can detect key events in it
			GUI.SetNextControlName("newEntryKey");

			EditorGUILayout.BeginVertical();
			EditorGUILayout.PrefixLabel("Key");
			newEntry_Key = EditorGUILayout.TextField(newEntry_Key, textFieldStyle);
			EditorGUILayout.EndVertical();

			// Track the next control so we can detect key events in it
			GUI.SetNextControlName("newEntryValue");

			EditorGUILayout.BeginVertical();
			EditorGUILayout.PrefixLabel("Value");

			switch (newEntry_Type)
			{
				case PlayerPrefType.Float:
					newEntry_ValueFloat = EditorGUILayout.FloatField(newEntry_ValueFloat, textFieldStyle);
					break;
				case PlayerPrefType.Int:
					newEntry_ValueInt = EditorGUILayout.IntField(newEntry_ValueInt, textFieldStyle);
					break;
				case PlayerPrefType.String:
					newEntry_ValueString = EditorGUILayout.TextField(newEntry_ValueString, textFieldStyle);
					break;
			}

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

			//---------------
			// Add Button
			//---------------
			bool enterPressed = Event.current.isKey && Event.current.keyCode == KeyCode.Return && Event.current.type == EventType.KeyUp;
			bool keyboardAddPressed = enterPressed && (GUI.GetNameOfFocusedControl() == "newEntryKey" || GUI.GetNameOfFocusedControl() == "newEntryValue");

			if ((GUILayout.Button("Add") || keyboardAddPressed) && string.IsNullOrEmpty(newEntry_Key) == false)
			{
				AddNewEntry();

				Repaint();

				// Reset the values
				newEntry_Key = "";
				newEntry_ValueFloat = 0;
				newEntry_ValueInt = 0;
				newEntry_ValueString = "";

				// Deselect
				GUI.FocusControl("");
			}
		}

		private void DrawImportMenu()
		{
			GUILayout.Label("Import PlayerPrefs from other project", EditorStyles.boldLabel);

			GUIStyle textFieldStyle = new GUIStyle(GUI.skin.textField);

			if (newEntry_IsEncrypted)
			{
				ApplyEncryptionColor(textFieldStyle);
			}

			importCompanyName = EditorGUILayout.TextField("Company Name", importCompanyName, textFieldStyle);
			importProductName = EditorGUILayout.TextField("Product Name", importProductName, textFieldStyle);

			if (GUILayout.Button("Import"))
			{
				PlayerPrefPair[] importedPairs = DeserializePlayerPrefs(importCompanyName, importProductName);
				for (int i = 0; i < importedPairs.Length; i++)
				{
					Type type = importedPairs[i].Value.GetType();
					if (type == typeof(float))
						PlayerPrefs.SetFloat(importedPairs[i].Key, (float)importedPairs[i].Value);
					else if (type == typeof(int))
						PlayerPrefs.SetInt(importedPairs[i].Key, (int)importedPairs[i].Value);
					else if (type == typeof(string))
						PlayerPrefs.SetString(importedPairs[i].Key, (string)importedPairs[i].Value);

					CacheRecord(importedPairs[i].Key, importedPairs[i].Value);
				}

				PlayerPrefs.Save();
			}
		}

		//--------------------------------------------
		// Utility
		//--------------------------------------------

		bool UsingProSkin
		{
			get
			{
#if UNITY_3_4
			if(EditorPrefs.GetInt("UserSkin") == 1)		
			{
				return true;
			}
			else
			{
				return false;
			}
#else
				return EditorGUIUtility.isProSkin;
#endif
			}
		}

		private Type GetValueType(string fullKey, object deserializedValue, bool isEncryptedPair, bool autoDecryptHasFailed)
		{
			Type valueType;
			// If it's an encrypted playerpref, we're automatically decrypting and it didn't fail the earlier 
			// auto decrypt test
			if (isEncryptedPair && automaticDecryption && autoDecryptHasFailed == false)
			{
				string encryptedValue = PlayerPrefs.GetString(fullKey);
				// Set valueType appropiately based on which type identifier prefix the encrypted string starts with
				if (encryptedValue.StartsWith(PlayerPrefsUtility.VALUE_FLOAT_PREFIX))
				{
					valueType = typeof(float);
				}
				else if (encryptedValue.StartsWith(PlayerPrefsUtility.VALUE_INT_PREFIX))
				{
					valueType = typeof(int);
				}
				else if (encryptedValue.StartsWith(PlayerPrefsUtility.VALUE_STRING_PREFIX) || string.IsNullOrEmpty(encryptedValue))
				{
					// empty encrypted values will also report as strings
					valueType = typeof(string);
				}
				else
				{
					throw new InvalidOperationException("Could not decrypt item, no match found in known encrypted key prefixes");
				}
			}
			else
			{
				// Otherwise fallback to the type of the cached value (for non-encrypted values this will be 
				// correct). For encrypted values when not in auto-decrypt mode, this will return string type
				valueType = deserializedValue.GetType();
			}

			return valueType;
		}

		/// <summary>
		/// Applys Color Scheme of encrypted keys
		/// </summary>
		/// <param name="textFieldStyle"></param>
		private void ApplyEncryptionColor(GUIStyle textFieldStyle)
		{
			if (UsingProSkin)
			{
				textFieldStyle.normal.textColor = new Color(0.5f, 0.5f, 1);
				textFieldStyle.focused.textColor = new Color(0.5f, 0.5f, 1);
			}
			else
			{
				textFieldStyle.normal.textColor = new Color(0, 0, 1);
				textFieldStyle.focused.textColor = new Color(0, 0, 1);
			}
		}
	}
}