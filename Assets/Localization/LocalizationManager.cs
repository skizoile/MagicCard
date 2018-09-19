///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 19/05/2018 21:30
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;
using System;
using UnityEngine.Networking;

namespace Localization {

    public class LocalizationManager : MonoBehaviour {

		public static LocalizationManager instance;

		public event Action OnLoadLocalizedText;

		private Dictionary<string, string> localizedText;
		private bool isReady = false;
		private string missingTextString = "Localized text not found";

		protected string filePath = "";
		protected string extension = "";
		protected string dataAsJson = "";


		protected char lineSeperater = '\n';
		protected char fieldSeperator = ',';

		[SerializeField] protected bool localizedFileIsJson = false;
		[SerializeField] protected bool localizedFileIsCSV = true;

		private void Awake () {
            if (instance == null)
				instance = this;
			else if (instance != this)
				Destroy(gameObject);

			DontDestroyOnLoad(gameObject);

			if (localizedFileIsJson)
				extension = ".json";
			else if (localizedFileIsCSV)
				extension = ".csv";
			else
				Debug.LogError("[ERROR] : Aucune extension pour les fichiers de traductions");

			LoadLocalizedText("localizedText_en" + extension);
        }

		protected void OnValidate()
		{
			if (localizedFileIsJson)
				localizedFileIsCSV = false;
			else if (localizedFileIsCSV)
				localizedFileIsJson = false;
		}

		public void LoadLocalizedText(string fileName)
		{
			localizedText = new Dictionary<string, string>();
			filePath = Path.Combine(Application.streamingAssetsPath, fileName);

			if (File.Exists(filePath) || Application.platform == RuntimePlatform.Android)
			{
				Debug.Log(filePath);

				StartCoroutine(FindFile());
			}
			else{
				Debug.LogError("Cannot find file ! Path = " + filePath);
			}

			isReady = true;
			
		}

		protected IEnumerator FindFile()
		{
			//Debug.Log(filePath);

			if (filePath.Contains("jar:file:/"))
			{
				UnityWebRequest www = UnityWebRequest.Get(filePath);
				yield return www.SendWebRequest();
				dataAsJson = www.downloadHandler.text;
			}
			else
			{
				dataAsJson = File.ReadAllText(filePath);
			}

			if (localizedFileIsJson)
			{
				LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

				for (int i = 0; i < loadedData.items.Length; i++)
				{
					localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
				}
			}
			else
			{
				string[] entries = dataAsJson.Split(lineSeperater);
				foreach (var line in entries)
				{
					string[] fields = line.Split(fieldSeperator);
					localizedText.Add(fields[0], fields[1]);
				}
			}

			if (OnLoadLocalizedText != null)
				OnLoadLocalizedText();

			Debug.Log("Data loaded, dictionary contains : " + localizedText.Count + " entries.");
		}


		public string GetLocalizedValue(string key)
		{
			string result = missingTextString;
			if (localizedText.ContainsKey(key))
				result = localizedText[key];

			return result;
		}

		public bool GetIsReady()
		{
			return isReady;
		}


		protected void OnDestroy()
		{
			instance = null;
		}
	}
}



