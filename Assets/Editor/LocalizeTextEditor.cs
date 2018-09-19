///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 19/05/2018 23:03
///-----------------------------------------------------------------

using UnityEngine;
using UnityEditor;
using System.IO;

namespace Localization {
    public class LocalizeTextEditor : EditorWindow {

		public LocalizationData localizationData;

		[MenuItem("Window/Localized Text Editor")]
		private static void Init()
		{
			GetWindow(typeof(LocalizeTextEditor)).Show();
		}

		private void OnGUI()
		{
			if (localizationData != null)
			{
				SerializedObject serializedObject = new SerializedObject(this);
				SerializedProperty serializedProperty = serializedObject.FindProperty("localizationData");
				EditorGUILayout.PropertyField(serializedProperty, true);
				serializedObject.ApplyModifiedProperties();

				if (GUILayout.Button("Save data"))
				{
					SaveGameData();
				}
			}

			if (GUILayout.Button("Load data"))
			{
				LoadGameData();
			}

			if (GUILayout.Button("Create new data"))
			{
				CreateNewData();
			}
		}

		private void LoadGameData()
		{
			string filePath = EditorUtility.OpenFilePanel("Select localized data file", Application.streamingAssetsPath, "json");
			if (!string.IsNullOrEmpty(filePath))
			{
				string dataAsJson = File.ReadAllText(filePath);

				localizationData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
			}

		}

		private void SaveGameData()
		{
			string filePath = EditorUtility.SaveFilePanel("Save localization data file", Application.streamingAssetsPath, "", "json");
			if (!string.IsNullOrEmpty(filePath))
			{
				string dataAsJson = JsonUtility.ToJson(localizationData);
				File.WriteAllText(filePath, dataAsJson);
			}
		}

        private void CreateNewData()
		{
			localizationData = new LocalizationData();

		}
    }
}


