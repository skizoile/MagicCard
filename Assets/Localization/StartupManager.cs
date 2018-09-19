///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 19/05/2018 22:45
///-----------------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Localization {
    public class StartupManager : MonoBehaviour {

        private IEnumerator Start () {

			while (!LocalizationManager.instance.GetIsReady())
			{
				yield return null;
			}

			Debug.Log("changement de scene possible");
        }

        
    }
}



