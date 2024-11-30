using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class BattlePreparation : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private List<Transform> playedCardPosition;
    [SerializeField] private GridLayoutGroup handLayout;
    [SerializeField] private Fighter player;

    [Header("Parameters")]
    [SerializeField] private MultiLayerButton cardPrefab;

    private readonly List<MultiLayerButton> buttons = new();
    [SerializeField] private readonly List<int> placedButtons = new();

    #region Init
    // Start is called before the first frame update
    void Start()
    {
        CreateCardButtons();

        ResetBattle();

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

        buttons.Add(button);
    }
    #endregion Init

    public void ResetBattle()
    {
        foreach (int buttonIndex in placedButtons)
        {
            buttons[buttonIndex].transform.SetParent(handLayout.transform);
        }
        placedButtons.Clear();



        //List<CardSO> hand = player.getCurrentHand();
        List<CardSO> hand = new();
        for (int j = 0; j < player.GetMaxHandSize(); j++)
        {
            hand.Add(new CardSO()
            {
                cardName = j.ToString(),

            });
        }


        int i = 0;
        while (i < hand.Count)
        {
            buttons[i].gameObject.SetActive(true);


            // TODO


            i++;
        }
        while (i < player.GetMaxHandSize())
        {
            buttons[i].gameObject.SetActive(false);
            i++;
        }
    }

    public void ChangeButtonPosition(int index)
    {
        if (placedButtons.Contains(index))
        {
            placedButtons.Remove(index);
            buttons[index].transform.SetParent(handLayout.transform);
            Debug.Log("Return to hand");

            for (int i = 0; i < placedButtons.Count; i++)
            {
                buttons[placedButtons[i]].transform.SetParent(playedCardPosition[i]);
                buttons[placedButtons[i]].transform.localPosition = Vector2.zero;
            }
        }
        else
        {
            if (placedButtons.Count >= playedCardPosition.Count) { return; }

            Debug.Log("Placed button " + index);


            placedButtons.Add(index);

            buttons[index].transform.SetParent(playedCardPosition[placedButtons.Count - 1]);
            buttons[index].transform.localPosition = Vector2.zero;
        }
    }
















    public void PlayTurn()
    {
        List<CardSO> cards = new();
        List<CardSO> hand = player.getCurrentHand();

        foreach (int cardIndex in placedButtons)
        {
            cards.Add(hand[cardIndex]);
        }

        BattleManager.Instance.PlayTurn(cards);
    }
}
