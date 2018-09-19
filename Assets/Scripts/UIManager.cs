using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }
    public CardManager cardManager;

    [Header("SELECTION CARD")]
    public GameObject selectionCard;
    public Transform containerChoice;
    public Button btnStart;

    [Header("CHOIX PAQUET")]
    public GameObject choicePaquet;
    public Transform containerLeft;
    public Button choiceLeft;
    public Transform containerMiddle;
    public Button choiceMiddle;
    public Transform containerRight;
    public Button choiceRight;

    [Header("RESULT")]
    public GameObject result;
    public Card visualCard;
    public Button restartButton;

    protected int nbChoice = 0;

    #region SELECTION_CARD

    protected void Start()
    {
        ActiveSelectionCard();
    }

    protected void DrawCard()
    {
        cardManager.RecupCard(containerChoice, ActiveBtnStart);
    }

    protected void ActiveSelectionCard()
    {
        DrawCard();
        DisableResult();
        DisableChoicePaquet();
        selectionCard.SetActive(true);
        btnStart.interactable = false;
        btnStart.onClick.AddListener(BtnStart_OnClick);
    }

    protected void ActiveBtnStart()
    {
        btnStart.interactable = true;
    }

    private void BtnStart_OnClick()
    {
        ActiveChoicePaquet();
    }

    protected void DisableSelectionCard()
    {
        selectionCard.SetActive(false);
        btnStart.onClick.RemoveListener(BtnStart_OnClick);
    }


    #endregion

    #region CHOICE_PAQUET

    protected void ActiveChoicePaquet()
    {
        DisableSelectionCard();
        nbChoice = 0;

        cardManager.DispatchCard(containerLeft, containerMiddle, containerRight, ActiveBtnChoice);

        choicePaquet.SetActive(true);

        DisableBtnChoice();

        choiceLeft.onClick.AddListener(ChoiceLeft_OnClick);
        choiceMiddle.onClick.AddListener(ChoiceMiddle_OnClick);
        choiceRight.onClick.AddListener(ChoiceRight_OnClick);
    }


    protected void DisableBtnChoice()
    {
        choiceLeft.interactable = false;
        choiceMiddle.interactable = false;
        choiceRight.interactable = false;
    }

    protected void ActiveBtnChoice()
    {
        choiceLeft.interactable = true;
        choiceMiddle.interactable = true;
        choiceRight.interactable = true;
    }

    private void ChoiceLeft_OnClick()
    {
        if (nbChoice == 2)
        {
            cardManager.SelectLeftPaquet(containerLeft, containerMiddle, containerRight, ActiveResult, true);
            return;
        }
        nbChoice += 1;
        DisableBtnChoice();
        cardManager.SelectLeftPaquet(containerLeft, containerMiddle, containerRight, ActiveBtnChoice);
    }

    private void ChoiceMiddle_OnClick()
    {
        if (nbChoice == 2)
        {
            cardManager.SelectMiddlePaquet(containerLeft, containerMiddle, containerRight, ActiveResult, true);

            return;
        }
        nbChoice += 1;
        DisableBtnChoice();
        cardManager.SelectMiddlePaquet(containerLeft, containerMiddle, containerRight, ActiveBtnChoice);

    }

    private void ChoiceRight_OnClick()
    {
        if (nbChoice == 2)
        {
            cardManager.SelectRightPaquet(containerLeft, containerMiddle, containerRight, ActiveResult, true);
            return;
        }
        nbChoice += 1;
        DisableBtnChoice();
        cardManager.SelectRightPaquet(containerLeft, containerMiddle, containerRight, ActiveBtnChoice);

    }

    protected void DisableChoicePaquet()
    {
        choicePaquet.SetActive(false);
        choiceLeft.onClick.RemoveListener(ChoiceLeft_OnClick);
        choiceMiddle.onClick.RemoveListener(ChoiceMiddle_OnClick);
        choiceRight.onClick.RemoveListener(ChoiceRight_OnClick);
    }


    #endregion


    #region RESULT

    protected void ActiveResult()
    {
        DisableChoicePaquet();
        Card lCard = cardManager.TakeGoodCard();
        visualCard.text.text = lCard.text.text;
        visualCard.icon.sprite = lCard.icon.sprite;
        visualCard.text.color = lCard.color;
        visualCard.icon.color = lCard.color;

        result.SetActive(true);
        restartButton.onClick.AddListener(Restart_OnClick);
    }

    private void Restart_OnClick()
    {
        ActiveSelectionCard();
    }

    protected void DisableResult()
    {
        result.SetActive(false);
        restartButton.onClick.RemoveListener(Restart_OnClick);
    }


    #endregion
    private void OnDestroy()
    {
        if (this == _instance)
            _instance = null;
    }
}
