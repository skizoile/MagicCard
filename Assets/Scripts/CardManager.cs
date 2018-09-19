using System;
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
    public Card cardPrefab;
    public Color redColor;
    public Color blackColor;
    public Sprite[] symbolsRed;
    public Sprite[] symbolsBlack;

    public float timeAppear = 0.01f;

    protected Dictionary<Color, Dictionary<Sprite, List<int>>> listCard = new Dictionary<Color, Dictionary<Sprite, List<int>>>();

    protected List<Card> list = new List<Card>();
    protected List<Card> listLeft = new List<Card>();
    protected List<Card> listMiddle = new List<Card>();
    protected List<Card> listRight = new List<Card>();

    protected void ResetGame()
    {
        listCard = new Dictionary<Color, Dictionary<Sprite, List<int>>>();
        list = new List<Card>();
        listLeft = new List<Card>();
        listMiddle = new List<Card>();
        listRight = new List<Card>();
    }

    public void RecupCard(Transform pContainer)
    {
        ResetGame();
        StartCoroutine(C_RecupCard(pContainer));
    }

    protected IEnumerator C_RecupCard(Transform pContainer)
    {
        for (int i = 0; i < nbCard; i++)
        {
            Color lColor = UnityEngine.Random.Range(0f, 1f) > 0.5f ? redColor : blackColor;
            if (!listCard.ContainsKey(lColor))
                listCard.Add(lColor, new Dictionary<Sprite, List<int>>());

            Sprite[] lArray = lColor == redColor ? symbolsRed : symbolsBlack;
            Sprite lSprite = lArray[UnityEngine.Random.Range(0, lArray.Length)];

            if (!listCard[lColor].ContainsKey(lSprite))
                listCard[lColor].Add(lSprite, new List<int>());

            int lInt = SelectNumber(lColor, lSprite);

            listCard[lColor][lSprite].Add(lInt);

            Card lCard = Instantiate(cardPrefab);
            lCard.transform.SetParent(pContainer, false);
            lCard.color = lColor;
            lCard.text.color = lColor;
            lCard.icon.color = lColor;
            lCard.icon.sprite = lSprite;
            lCard.text.text = lInt.ToString();
            list.Add(lCard);
            yield return new WaitForSeconds(timeAppear);
        }
    }

    protected int SelectNumber(Color pColor, Sprite pSprite)
    {
        int[] lList = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        List<int> lDispo = new List<int>();

        for (int i = 0; i < lList.Length; i++)
        {
            if (!listCard[pColor][pSprite].Contains(lList[i]))
                lDispo.Add(lList[i]);
        }

        return lDispo[UnityEngine.Random.Range(0, lDispo.Count)];
    }


    public void DispatchCard(Transform pContainerLeft, Transform pContainerMiddle, Transform pContainerRight)
    {
        StartCoroutine(C_DispatchCard(pContainerLeft, pContainerMiddle, pContainerRight));
    }

    protected IEnumerator C_DispatchCard(Transform pContainerLeft, Transform pContainerMiddle, Transform pContainerRight)
    {
        int index = 1;
        listLeft.Clear();
        listMiddle.Clear();
        listRight.Clear();

        for (int i = 0; i < list.Count; i++)
        {
            if (index > 3)
                index = 1;

            list[i].gameObject.SetActive(true);
            if (index == 1)
            {
                listLeft.Add(list[i]);
                list[i].transform.SetParent(pContainerLeft);
            }
            else if (index == 2)
            {
                listMiddle.Add(list[i]);
                list[i].transform.SetParent(pContainerMiddle);
            }
            else if (index == 3)
            {
                listRight.Add(list[i]);
                list[i].transform.SetParent(pContainerRight);

            }
            index++;
            yield return new WaitForSeconds(timeAppear);
        }
    }


    public void SelectLeftPaquet(Transform pContainerLeft, Transform pContainerMiddle, Transform pContainerRight, Action pCallback = null)
    {
        list.Clear();

        StartCoroutine(C_RecupLeftPaquet(pContainerLeft, pContainerMiddle, pContainerRight, pCallback));
    }

    public void SelectMiddlePaquet(Transform pContainerLeft, Transform pContainerMiddle, Transform pContainerRight, Action pCallback = null)
    {
        list.Clear();

        StartCoroutine(C_RecupMiddlePaquet(pContainerLeft, pContainerMiddle, pContainerRight, pCallback));
    }

    public void SelectRightPaquet(Transform pContainerLeft, Transform pContainerMiddle, Transform pContainerRight, Action pCallback = null)
    {
        list.Clear();

        StartCoroutine(C_RecupRightPaquet(pContainerLeft, pContainerMiddle, pContainerRight, pCallback));
    }

    protected IEnumerator C_RecupLeftPaquet(Transform pContainerLeft, Transform pContainerMiddle, Transform pContainerRight, Action pCallback = null)
    {
        for (int i = 0; i < listMiddle.Count; i++)
        {
            list.Add(listMiddle[i]);
            listMiddle[i].gameObject.SetActive(false);
            yield return new WaitForSeconds(timeAppear);
        }

        for (int i = 0; i < listLeft.Count; i++)
        {
            list.Add(listLeft[i]);
            listLeft[i].gameObject.SetActive(false);
            yield return new WaitForSeconds(timeAppear);
        }

        for (int i = 0; i < listRight.Count; i++)
        {
            list.Add(listRight[i]);
            listRight[i].gameObject.SetActive(false);
            yield return new WaitForSeconds(timeAppear);
        }

        if (pCallback == null)
            DispatchCard(pContainerLeft, pContainerMiddle, pContainerRight);
        else
            pCallback();
    }

    protected IEnumerator C_RecupMiddlePaquet(Transform pContainerLeft, Transform pContainerMiddle, Transform pContainerRight, Action pCallback = null)
    {
        for (int i = 0; i < listLeft.Count; i++)
        {
            list.Add(listLeft[i]);
            listLeft[i].gameObject.SetActive(false);
            yield return new WaitForSeconds(timeAppear);
        }

        for (int i = 0; i < listMiddle.Count; i++)
        {
            list.Add(listMiddle[i]);
            listMiddle[i].gameObject.SetActive(false);
            yield return new WaitForSeconds(timeAppear);
        }

        for (int i = 0; i < listRight.Count; i++)
        {
            list.Add(listRight[i]);
            listRight[i].gameObject.SetActive(false);
            yield return new WaitForSeconds(timeAppear);
        }
        if (pCallback == null)
            DispatchCard(pContainerLeft, pContainerMiddle, pContainerRight);
        else
            pCallback();
    }

    protected IEnumerator C_RecupRightPaquet(Transform pContainerLeft, Transform pContainerMiddle, Transform pContainerRight, Action pCallback = null)
    {
        for (int i = 0; i < listLeft.Count; i++)
        {
            list.Add(listLeft[i]);
            listLeft[i].gameObject.SetActive(false);
            yield return new WaitForSeconds(timeAppear);
        }

        for (int i = 0; i < listRight.Count; i++)
        {
            list.Add(listRight[i]);
            listRight[i].gameObject.SetActive(false);
            yield return new WaitForSeconds(timeAppear);
        }

        for (int i = 0; i < listMiddle.Count; i++)
        {
            list.Add(listMiddle[i]);
            listMiddle[i].gameObject.SetActive(false);
            yield return new WaitForSeconds(timeAppear);
        }
        if (pCallback == null)
            DispatchCard(pContainerLeft, pContainerMiddle, pContainerRight);
        else
            pCallback();
    }

    public Card TakeGoodCard()
    {
        return list[10];
    }

    private void OnDestroy()
    {
        if (this == _instance)
            _instance = null;
    }
}
