using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class BattlePreparation : MonoBehaviour
{
    /* TODO :   Changer le skin des cartes
     *          PlayerHand
     *          return current hand
     */

    [Header("General")]
    [SerializeField] private GridLayoutGroup handLayout;

    [Header("Player")]
    [SerializeField] private Player player;
    [SerializeField] private List<Transform> playerCardPosition;

    [Header("Enemy")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private List<Transform> enemyCardPosition;

    [Header("Prefab")]
    [SerializeField] private CardButton cardPrefab;
    [SerializeField] private Sprite hidenCard;

    // Player Cards
    private readonly List<CardButton> playerCardButtons = new();
    private readonly List<int> placedButtons = new();

    // Enemy cards
    private readonly List<CardButton> enemyCardButtons = new();
    private readonly List<bool> enemyCardRevealed = new();

    #region Init
    // Start is called before the first frame update
    void Awake()
    {
        if (BattleManager.Instance.GetMaxPlayedCard() != playerCardPosition.Count || BattleManager.Instance.GetMaxPlayedCard() != enemyCardPosition.Count)
        {
            Debug.LogError("Max play card and played card position size does not match");
        }

        CreateCardButtons();

        handLayout.constraintCount = player.GetMaxHandSize();
    }

    private void CreateCardButtons()
    {
        for (int i = 0; i < player.GetMaxHandSize(); i++)
        {
            CreatePlayerCardButton(i);
        }
        
        for (int i = 0; i < BattleManager.Instance.GetMaxPlayedCard(); i++)
        {
            CreateEnemyCardButton(i);
        }
    }

    private void CreatePlayerCardButton(int index)
    {
        CardButton button = GameObject.Instantiate<CardButton>(cardPrefab);

        button.transform.SetParent(handLayout.transform);
        button.transform.localScale = Vector2.one;

        button.GetComponent<Button>().onClick.AddListener(delegate { ChangeButtonPosition(index); });

        playerCardButtons.Add(button);
    }

    private void CreateEnemyCardButton(int index)
    {
        CardButton button = GameObject.Instantiate<CardButton>(cardPrefab);

        button.transform.SetParent(enemyCardPosition[index]);
        button.transform.localScale = Vector2.one;
        button.transform.localPosition = Vector2.zero;

        button.GetComponent<Button>().onClick.AddListener(delegate { RevealCard(index); });

        enemyCardButtons.Add(button);
        enemyCardRevealed.Add(false);
    }
    #endregion Init

    public void ResetBattle()
    {
        foreach (int buttonIndex in placedButtons)
        {
            playerCardButtons[buttonIndex].transform.SetParent(handLayout.transform);
        }
        placedButtons.Clear();

        // Player Cards
        int i = 0;
        List<Card> playerHand = player.GetCurrentHand();
        Debug.Log(playerHand.Count);

        while (i < playerHand.Count)
        {
            playerCardButtons[i].gameObject.SetActive(true);

            playerCardButtons[i].ApplyCard(playerHand[i]);

            i++;
        }
        while (i < player.GetMaxHandSize())
        {
            playerCardButtons[i].gameObject.SetActive(false);
            i++;
        }

        // Enemy Cards
        int j = 0;
        List<Card> enemyHand = enemy.GetCurrentHand();

        while (j < enemyHand.Count)
        {
            enemyCardButtons[j].gameObject.SetActive(true);

            enemyCardButtons[j].ApplyCard(enemyHand[j]);
            enemyCardButtons[j].HideCard();

            j++;
        }
        while (j < BattleManager.Instance.GetMaxPlayedCard())
        {
            enemyCardButtons[j].gameObject.SetActive(false);
            j++;
        }
    }

    private void ChangeButtonPosition(int index)
    {
        if (placedButtons.Contains(index))
        {
            placedButtons.Remove(index);
            playerCardButtons[index].transform.SetParent(handLayout.transform);

            for (int i = 0; i < placedButtons.Count; i++)
            {
                playerCardButtons[placedButtons[i]].transform.SetParent(playerCardPosition[i]);
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
            if (placedButtons.Count >= playerCardPosition.Count) { return; }


            placedButtons.Add(index);

            playerCardButtons[index].transform.SetParent(playerCardPosition[placedButtons.Count - 1]);
            playerCardButtons[index].transform.localPosition = Vector2.zero;
        }
    }

    private void RevealCard(int index)
    {
        if (enemyCardRevealed[index]) { return; }

        if (player.GetCurrentPerception() > 0)
        {
            Debug.Log(player.GetCurrentPerception());

            player.UsePerception(1);

            enemyCardButtons[index].ApplyCard();

            enemyCardRevealed[index] = true;
        }
    }

    public void PlayTurn()
    {
        List<Card> cardsToPlay = new();
        List<Card> cardsInHand = new();
        List<Card> playerHand = player.GetCurrentHand();

        for (int i = 0; i < playerHand.Count; i++)
        {
            if (placedButtons.Contains(i))
            {
                cardsToPlay.Add(playerHand[i]);
            }
            else
            {
                cardsInHand.Add(playerHand[i]);
            }
        }

        player.SetCurrentHand(cardsInHand);

        BattleManager.Instance.PlayTurn(cardsToPlay);
    }
}
