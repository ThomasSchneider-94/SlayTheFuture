using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class BattlePreparation : MonoBehaviour
{
    /* TODO :   Changer le skin des cartes
     *          PlayerHand
     *          return current hand
     */

    [Header("Parameters")]
    [SerializeField] private GridLayoutGroup handLayout;
    [SerializeField] private Fighter player;
    [SerializeField] private List<Transform> playedCardPosition;
    [SerializeField] private List<Transform> enemyCardPosition;

    [Header("Prefab")]
    [SerializeField] private MultiLayerButton cardPrefab;
    [SerializeField] private Sprite hidenCard;

    // Player Cards
    private readonly List<MultiLayerButton> playerCardButtons = new();
    private readonly List<int> placedButtons = new();
    private List<CardSO> playerHand;

    // Enemy cards
    private readonly List<MultiLayerButton> enemyCardButtons = new();
    private List<bool> isRevealed = new();

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
            CreatePlayerCardButton(i);
        }
        /*
        for (int i = 0; i < BattleManager.Instance; i++)
        {
            CreatePlayerCardButton(i);
        }*/
    }

    private void CreatePlayerCardButton(int index)
    {
        MultiLayerButton button = GameObject.Instantiate<MultiLayerButton>(cardPrefab);

        button.transform.SetParent(handLayout.transform);
        button.transform.localScale = Vector2.one;

        button.onClick.AddListener(delegate { ChangeButtonPosition(index); });

        playerCardButtons.Add(button);
    }

    private void CreateEnemyCardButton(int index)
    {
        MultiLayerButton button = GameObject.Instantiate<MultiLayerButton>(cardPrefab);

        button.transform.SetParent(enemyCardPosition[index]);

        button.onClick.AddListener(delegate { RevealCard(index); });

        enemyCardButtons.Add(button);
        isRevealed.Add(false);
    }




    #endregion Init

    public void ResetBattle()
    {
        foreach (int buttonIndex in placedButtons)
        {
            playerCardButtons[buttonIndex].transform.SetParent(handLayout.transform);
        }
        placedButtons.Clear();

        //playerHand = player.getCurrentHand();
        playerHand = new();
        for (int j = 0; j < player.GetMaxHandSize(); j++)
        {
            playerHand.Add(new CardSO()
            {
                cardName = j.ToString(),

            });
        }

        int i = 0;
        while (i < playerHand.Count)
        {
            playerCardButtons[i].gameObject.SetActive(true);


            // TODO


            i++;
        }
        while (i < player.GetMaxHandSize())
        {
            playerCardButtons[i].gameObject.SetActive(false);
            i++;
        }
    }

    private void ChangeButtonPosition(int index)
    {
        if (placedButtons.Contains(index))
        {
            placedButtons.Remove(index);
            playerCardButtons[index].transform.SetParent(handLayout.transform);
            Debug.Log("Return to hand");

            for (int i = 0; i < placedButtons.Count; i++)
            {
                playerCardButtons[placedButtons[i]].transform.SetParent(playedCardPosition[i]);
                playerCardButtons[placedButtons[i]].transform.localPosition = Vector2.zero;
            }

            for (int i = 0; i < playerCardButtons.Count; i++)
            {
                if (!placedButtons.Contains(i))
                {
                    playerCardButtons[i].transform.SetAsLastSibling();
                }
            }
        }
        else
        {
            if (placedButtons.Count >= playedCardPosition.Count) { return; }

            Debug.Log("Placed button " + index);


            placedButtons.Add(index);

            playerCardButtons[index].transform.SetParent(playedCardPosition[placedButtons.Count - 1]);
            playerCardButtons[index].transform.localPosition = Vector2.zero;
        }
    }

    private void RevealCard(int index)
    {

    }







    public void PlayTurn()
    {
        List<CardSO> cards = new();

        foreach (int cardIndex in placedButtons)
        {
            cards.Add(playerHand[cardIndex]);
        }

        BattleManager.Instance.PlayTurn(cards);
    }
}
