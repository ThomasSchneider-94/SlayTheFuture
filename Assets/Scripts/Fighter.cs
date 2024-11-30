using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    [Header("Hand Size")]
    [SerializeField] protected int maxHandSize;
    [SerializeField] protected int maxDeckSize;

    [Header("Hand Size")]
    [SerializeField] protected int maxHP;
    protected int hp;
    private int shield;

    private List<(int, int)> poison = new();

    private List<CardSO> deck;

    protected List<CardSO> currentHand = new();
    private List<CardSO> currentDeck = new();

    private void Start()
    {
        // TODO : initialiser le deck du joueur avec une liste de cartes

        hp = maxHP;
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
        Debug.Log("Reset");
        currentDeck.Clear();

        Debug.Log(deck.Count);

        foreach (var card in deck)
        {
            CardSO cardSO = ScriptableObject.CreateInstance<CardSO>();
            cardSO.Init(card);

            currentDeck.Add(cardSO);
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

    public virtual void Draw()
    {
        Debug.Log(currentDeck.Count);
        Debug.Log(currentHand.Count);


        if (currentDeck.Count == 0 && currentHand.Count == 0)
        {
            ResetCurrentDeck();
        }

        while (currentDeck.Count > 0 && currentHand.Count < maxHandSize)
        {
            CardSO card = currentDeck[0];
            currentDeck.RemoveAt(0);
            currentHand.Add(card);

            Debug.Log(currentDeck.Count);

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
