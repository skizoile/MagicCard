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
    public UIManager uiManager;
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

            if (!listCard[lColor].ContainsKey(lSprite))
                listCard[lColor].Add(lSprite, new List<int>());

            int lInt = SelectNumber(lColor, lSprite);

            listCard[lColor][lSprite].Add(lInt);

            Card lCard = Instantiate(cardPrefab);
            lCard.transform.SetParent(canvasParent.transform, false);
            lCard.color = lColor;
            lCard.icon.sprite = lSprite;
            lCard.text.text = lInt.ToString();
        }
    }

    protected int SelectNumber(Color pColor, Sprite pSprite)
    {
        int[] lList = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        List<int> lDispo = new List<int>();

        for (int i = 0; i < lList.Length; i++)
        {
            if (!listCard[pColor][pSprite].Contains(i))
                lDispo.Add(i);
        }

        return lDispo[Random.Range(0, lDispo.Count)];
    }

    private void OnDestroy()
    {
        if (this == _instance)
            _instance = null;
    }
}
