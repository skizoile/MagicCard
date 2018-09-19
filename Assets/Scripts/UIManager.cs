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

    protected void ActiveSelectionCard()
    {
        DisableResult();
        DisableChoicePaquet();
        selectionCard.SetActive(true);
        btnStart.onClick.AddListener(BtnStart_OnClick);
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
        choicePaquet.SetActive(true);
        choiceLeft.onClick.AddListener(ChoiceLeft_OnClick);
        choiceMiddle.onClick.AddListener(ChoiceMiddle_OnClick);
        choiceRight.onClick.AddListener(ChoiceRight_OnClick);
    }

    private void ChoiceLeft_OnClick()
    {
        if (nbChoice == 2)
        {
            ActiveResult();
            return;
        }
        nbChoice += 1;
    }

    private void ChoiceMiddle_OnClick()
    {
        if (nbChoice == 2)
        {
            ActiveResult();
            return;
        }
        nbChoice += 1;
    }

    private void ChoiceRight_OnClick()
    {
        if (nbChoice == 2)
        {
            ActiveResult();
            return;
        }
        nbChoice += 1;

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
