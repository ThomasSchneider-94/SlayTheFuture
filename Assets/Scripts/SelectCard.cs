using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SelectCard : MonoBehaviour
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

        buttons.Add(button);
    }
    #endregion Init

    public  void ResetCardPosition()
    {
        foreach (int buttonIndex in placedButtons)
        {
            buttons[buttonIndex].transform.SetParent(handLayout.transform);
        }
        placedButtons.Clear();

        List<CardSO> hand = player.getCurrentHand();

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

            foreach(int buttonIndex in placedButtons)
            {
                buttons[buttonIndex].transform.SetParent(handLayout.transform);
            }
        }
        else
        {
            if (placedButtons.Count >= playedCardPosition.Count) { return; }

            placedButtons.Add(index);

            buttons[index].transform.SetParent(playedCardPosition[placedButtons.Count - 1]);
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
