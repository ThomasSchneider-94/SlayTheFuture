using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SelectCard : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private List<Image> playedCardPosition;
    [SerializeField] private GridLayoutGroup handLayout;
    [SerializeField] private Fighter player;

    [Header("Parameters")]
    [SerializeField] private MultiLayerButton cardPrefab;

    private readonly List<MultiLayerButton> buttons = new();
    private List<int> placedButtons = new();

    #region Init
    // Start is called before the first frame update
    void Start()
    {
        CreateCardButtons();

        ResetCardPosition();

        handLayout.constraintCount = player.GetMaxHandSize();
    }

    private void CreateCardButtons()
    {
        for (int i = 0; i < player.GetMaxHandSize(); i++)
        {
            CreateCardButton(i);
        }
    }

    private void CreateCardButton(int index)
    {
        MultiLayerButton button = GameObject.Instantiate<MultiLayerButton>(cardPrefab);

        button.transform.SetParent(handLayout.transform);
        button.onClick.AddListener(delegate { ChangeButtonPosition(index); });

        Debug.Log(index);

        buttons.Add(button);
    }
    #endregion Init

    private void ResetCardPosition()
    {
        foreach (int buttonIndex in placedButtons)
        {
            buttons[buttonIndex].transform.SetParent(handLayout.transform);
        }
        placedButtons.Clear();

        Stack<CardSO> hand = player.getCurrentHand();

        int i = 0;
        while (i < hand.Count)
        {





            i++;
        }
        while (i < player.GetMaxHandSize())
        {
            //butto;
        }
    }




    public void ChangeButtonPosition(int buttonIndex)
    {
        /*
        if (buttonsPlaced[buttonIndex])
        {

            Debug.Log(buttonsPlaced[buttonIndex]);
            buttonsPlaced[buttonIndex] = false;
        }
        else
        {
            Debug.Log(buttonsPlaced[buttonIndex]);



            buttonsPlaced[buttonIndex] = true;

        }*/
    }
}
