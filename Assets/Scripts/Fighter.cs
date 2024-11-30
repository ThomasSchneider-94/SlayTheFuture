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


    private List<Card> deck;
    private int currentCardIndex = 0;

    private List<Card> currentHand = new List<Card>();
    private List<Card> currentDeck = new List<Card>();

    private void Start()
    {
        // TODO : initialiser le deck du joueur avec une liste de cartes

        deck = new(15);
    }

    public void ResetCurrentDeck()
    {
        currentDeck = new List<Card>();

        foreach (var card in deck)
        {
            currentDeck.Add(card);
        }

        ShuffleList(currentDeck);
    }

    public void ShuffleList(List<Card> list){
        
    int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = Random.Range(0, n);
            Card tmp = list[k];  
            list[k] = list[n];  
            list[n] = tmp;  
        }
    }

    public void Draw()
    {
        if ((currentDeck.Count == 0) && (currentHand.Count == 0)){
            ResetCurrentDeck();
        }

        while ((currentDeck.Count > 0) && (currentHand.Count < 5)){
            Card card = currentDeck[0];
            currentDeck.RemoveAt(0);
            currentHand.Add(card);
        }

    }

    public void AddCard(Card card)
    {
        deck[currentCardIndex] = card;
        currentCardIndex++;
    }

    public void DiscardCard(Card card)
    {
        currentDeck.Remove(card);
    }

    public void AddCardToHand(Card cardToAdd)
    {
        // TODO : � impl�menter
    }

    public bool IsCurrentDeckEmpty()
    {
        return (currentDeck.Count <= 0);
    }

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

    public bool IsCurrentHandEmpty()
    {
        return (currentHand.Count <= 0);
    }

    #region Setter
    public void SetCurrentDeck(List<Card> deck)
    {
        currentDeck = deck;
    }

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

    public void SetCurrentHand(List<Card> hand)
    {
        this.currentHand = hand;
    }
    #endregion Setter

    #region Getter
    public List<Card> GetCurrentDeck()
    {
        return currentDeck;
    }

    public int GetHP()
    {
        return hp;
    }

    public int GetShield()
    {
        return shield;
    }

    public List<Card> GetCurrentHand()
    {
        return currentHand;
    }

    public int GetMaxHandSize()
    {
        return maxHandSize;
    }
    #endregion Getter
}
