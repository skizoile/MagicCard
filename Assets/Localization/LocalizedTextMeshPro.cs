///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 18/06/2018 19:20
///-----------------------------------------------------------------

using UnityEngine;
using TMPro;
using System.Collections;
using System;

namespace Localization {
    public class LocalizedTextMeshPro : MonoBehaviour {

		public string key;
		protected TextMeshProUGUI text;
		[SerializeField] protected bool autoText = false;
		[SerializeField, Range(0.01f, 0.1f)] protected float timeGenerateText = 0.04f;

		protected bool skipAutoText = false;
		protected static event Action OnSkip;

		protected Coroutine coroutine;
		protected int index = 0;
		protected string textBase;

		protected void Start()
		{
			
			LocalizationManager.instance.OnLoadLocalizedText += Instance_OnLoadLocalizedText;
			text = GetComponent<TextMeshProUGUI>();
			Instance_OnLoadLocalizedText();
			OnSkip += LocalizedText_OnSkip;
		}

		private void LocalizedText_OnSkip()
		{
			skipAutoText = true;
			if (coroutine != null)
				StopCoroutine(coroutine);
			text.text = textBase;
		}

		private void Instance_OnLoadLocalizedText()
		{
			textBase = LocalizationManager.instance.GetLocalizedValue(key);
			if (autoText)
				coroutine = StartCoroutine(AutomaticGenerateText());
			else
				LocalizedText_OnSkip();
		}

		protected IEnumerator AutomaticGenerateText()
		{
			textBase = text.text;
			text.text = "";

			bool containSprite = textBase.Contains("<sprite=");

			while (index < textBase.Length)
			{
				if (containSprite)
				{
					if (textBase[index] == '<')
					{

						int endSprite = textBase.IndexOf('>', index);
						string replaceText = "";
						for (int i = index; i <= endSprite; i++)
						{
							replaceText += textBase[i];
						}
						text.text += replaceText;
						index = endSprite + 1;
						yield return new WaitForSecondsRealtime(timeGenerateText);
					}
				}
				text.text += textBase[index];
				index += 1;
				yield return new WaitForSecondsRealtime(timeGenerateText);
			}
		}

		protected void Update()
		{
			if (skipAutoText)
				return;
#if UNITY_EDITOR
			if (Input.GetMouseButton(0))
				if (OnSkip != null)
					OnSkip();
#elif UNITY_ANDROID || UNITY_IOS
			if (Input.GetTouch(0).phase == TouchPhase.Began)
				if (OnSkip != null)
					OnSkip();
#endif
		}
	}
}



