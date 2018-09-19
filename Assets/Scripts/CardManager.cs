using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {

    private static CardManager _instance;
    public static CardManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    protected int nbCard = 21;
    public Canvas canvasParent;
    public Card cardPrefab;
    public Color redColor;
    public Color blackColor;
    public Sprite[] symbolsRed;
    public Sprite[] symbolsBlack;

    public Dictionary<Color, Dictionary<Sprite, List<int>>> listCard = new Dictionary<Color, Dictionary<Sprite, List<int>>>();

    private void Start()
    {
        for (int i = 0; i < nbCard; i++)
        {
            Color lColor = Random.Range(0f, 1f) > 0.5f ? redColor : blackColor;
            if (!listCard.ContainsKey(lColor))
                listCard.Add(lColor, new Dictionary<Sprite, List<int>>());

            Sprite[] lArray = lColor == redColor ? symbolsRed : symbolsBlack;
            Sprite lSprite = lArray[Random.Range(0, lArray.Length)];

            int lInt = Random.Range(1, 10);

            if (!listCard[lColor].ContainsKey(lSprite))
                listCard[lColor].Add(lSprite, new List<int>());

            listCard[lColor][lSprite].Add(lInt);

            Card lCard = Instantiate(cardPrefab);
            lCard.transform.SetParent(canvasParent.transform, false);
            lCard.color = lColor;
            lCard.icon.sprite = lSprite;
            lCard.text.text = lInt.ToString();
        }
    }

    private void OnDestroy()
    {
        if (this == _instance)
            _instance = null;
    }
}
