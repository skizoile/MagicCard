using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    public Color color;
    public Text text;
    public Image icon;

	// Use this for initialization
	void Start () {
        text.color = color;
        icon.color = color;
	}
}
