using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    [SerializeField] protected int maxHandSize;
    [SerializeField] protected int maxDeckSize;

    protected int hp;
    protected int maxHP;
    private int shield;

    private List<(int, int)> poison = new();


    private List<CardSO> deck;
    private int currentCardIndex = 0;

    private List<CardSO> currentHand = new();
    private List<CardSO> currentDeck = new();

    private void Start()
    {
        // TODO : initialiser le deck du joueur avec une liste de cartes

        deck = new(15);
    }

    public void InitDeck(List<CardSO> baseDeck)
    {
        this.deck = baseDeck;
    }
    public void ChangeDeckCard(CardSO oldCard, CardSO newCard)
    {
        deck.Remove(oldCard);
        deck.Add(newCard);
    }

    #region Current Deck
    public void ResetCurrentDeck()
    {
        currentDeck = new();

        foreach (var card in deck)
        {
            currentDeck.Add(card);
        }

        ShuffleList(currentDeck);
    }

    private static void ShuffleList(List<CardSO> list)
    {

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            CardSO tmp = list[k];
            list[k] = list[n];
            list[n] = tmp;
        }
    }

    public void Draw()
    {
        if (currentDeck.Count == 0 && currentHand.Count == 0)
        {
            ResetCurrentDeck();
        }

        while ((currentDeck.Count > 0) && (currentHand.Count < 5))
        {
            CardSO card = currentDeck[0];
            currentDeck.RemoveAt(0);
            currentHand.Add(card);
        }
    }

    public bool IsCurrentDeckEmpty()
    {
        return (currentDeck.Count <= 0);
    }

    public List<CardSO> GetCurrentDeck()
    {
        return currentDeck;
    }
    #endregion Current Deck

    #region Hand
    public void SetCurrentHand(List<CardSO> hand)
    {
        this.currentHand = hand;
    }

    public List<CardSO> GetCurrentHand()
    {
        return currentHand;
    }

    #endregion Hand

    public void AddPoisonStack(int damage, int duration)
    {
        poison.Add((damage, duration));
    }

    public void ConsumePoisonStack()
    {
        for (int i = 0; i < poison.Count; i++)
        {
            if (poison[i].Item1 == 1)
            {
                poison.RemoveAt(i);
            }
        }
    }


    #region Setter
    public abstract void SetHP(int hpDelta);

    public void SetShield(int shieldDelta)
    {
        shield += shieldDelta;
        if (shield < 0)
        {
            SetHP(shield);
            shield = 0;
        }
    }
    #endregion Setter

    #region Getter
    public int GetHP()
    {
        return hp;
    }

    public int GetShield()
    {
        return shield;
    }
    
    public int GetMaxHandSize()
    {
        return maxHandSize;
    }
    #endregion Getter
}
