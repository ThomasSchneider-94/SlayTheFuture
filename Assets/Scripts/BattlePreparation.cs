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
    [SerializeField] private Button startButton;

    [Header("Player")]
    [SerializeField] private List<Transform> playerCardPosition;

    [Header("Enemy")]
    [SerializeField] private List<Transform> enemyCardPosition;

    [Header("Prefab")]
    [SerializeField] private CardButton cardPrefab;

    // Player Cards
    private readonly List<CardButton> playerCardButtons = new();
    private readonly List<int> placedButtons = new();

    // Enemy cards
    private readonly List<CardButton> enemyCardButtons = new();
    private List<bool> enemyCardRevealed = new();

    // Both
    private readonly List<Button> buttons = new();

    #region Init
    // Start is called before the first frame update
    void Awake()
    {
        if (BattleManager.Instance.GetMaxPlayedCard() != playerCardPosition.Count || BattleManager.Instance.GetMaxPlayedCard() != enemyCardPosition.Count)
        {
            Debug.LogError("Max play card and played card position size does not match");
        }

        CreateCardButtons();

        handLayout.constraintCount = Player.Instance.GetMaxHandSize();
    }

    private void CreateCardButtons()
    {
        for (int i = 0; i < Player.Instance.GetMaxHandSize(); i++)
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
        button.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
        button.transform.localScale = Vector2.one;

        button.GetComponent<Button>().onClick.AddListener(delegate { ChangeButtonPosition(index); });
        buttons.Add(button.GetComponent<Button>());

        playerCardButtons.Add(button);
    }

    private void CreateEnemyCardButton(int index)
    {
        CardButton button = GameObject.Instantiate<CardButton>(cardPrefab);

        button.transform.SetParent(enemyCardPosition[index]);
        button.transform.localScale = Vector2.one;
        button.transform.localPosition = Vector2.zero;

        button.GetComponent<Button>().onClick.AddListener(delegate { RevealCard(index); });
        buttons.Add(button.GetComponent<Button>());

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

        // Buttons
        foreach (Button button in buttons)
        {
            button.interactable = true;
            button.gameObject.SetActive(true);
        }
        startButton.interactable = true;

        // Player Cards
        int i = 0;
        List<Card> playerHand = Player.Instance.GetCurrentHand();

        while (i < playerHand.Count)
        {
            playerCardButtons[i].gameObject.SetActive(true);

            playerCardButtons[i].GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);


            playerCardButtons[i].ApplyCard(playerHand[i]);

            i++;
        }
        while (i < Player.Instance.GetMaxHandSize())
        {
            playerCardButtons[i].GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);

            playerCardButtons[i].gameObject.SetActive(false);
            i++;
        }

        // Enemy Cards
        int j = 0;
        List<Card> enemyHand = Enemy.Instance.GetPlayedCards();
        enemyCardRevealed = new (new bool[BattleManager.Instance.GetMaxPlayedCard()]);

        while (j < enemyHand.Count)
        {
            enemyCardButtons[j].gameObject.SetActive(true);
            enemyCardButtons[j].transform.localPosition = Vector3.zero;
            enemyCardButtons[j].ApplyCard(enemyHand[j]);
            enemyCardButtons[j].HideCard();

            j++;
        }
        while (j < BattleManager.Instance.GetMaxPlayedCard())
        {
            enemyCardButtons[j].gameObject.SetActive(false);
            enemyCardButtons[j].transform.localPosition = Vector3.zero;
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

            playerCardButtons[index].GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);


            AudioManager.Instance.PlayCard();
        }
        else
        {
            if (placedButtons.Count >= playerCardPosition.Count) { return; }


            placedButtons.Add(index);

            playerCardButtons[index].GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

            playerCardButtons[index].transform.SetParent(playerCardPosition[placedButtons.Count - 1]);
            playerCardButtons[index].transform.localPosition = Vector2.zero;

            AudioManager.Instance.PlayCard();
        }
    }

    private void RevealCard(int index)
    {
        if (enemyCardRevealed[index]) { return; }

        if (Player.Instance.GetCurrentPerception() > 0)
        {
            Player.Instance.UsePerception(1);

            enemyCardButtons[index].ApplyCard();

            enemyCardRevealed[index] = true;

            AudioManager.Instance.PlayCard();
        }
    }

    public void PlayTurn()
    {
        List<Card> cardsToPlay = new();
        List<Card> cardsInHand = new();
        List<Card> playerHand = Player.Instance.GetCurrentHand();

        foreach (int cardIndex in placedButtons)
        {
            cardsToPlay.Add(playerHand[cardIndex]);
        }

        for (int i = 0; i < playerHand.Count; i++)
        {
            if (!placedButtons.Contains(i))
            {
                cardsInHand.Add(playerHand[i]);
            }
        }

        Player.Instance.SetCurrentHand(cardsInHand);

        foreach (Button button in buttons) 
        {
            button.interactable = false;
        }
        startButton.interactable = false;

        StartCoroutine(BattleManager.Instance.PlayTurn(cardsToPlay));
    }

    public CardButton GetCardButton(bool player, int index)
    {
        if (player)
        {
            return playerCardButtons[placedButtons[index]];
        }
        else
        {
            return enemyCardButtons[index];
        }
    } 
}
