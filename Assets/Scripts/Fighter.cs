using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] protected int maxHandSize;
    [SerializeField] protected int maxDeckSize;

    protected int hp;
    protected int maxHP;
    private int shield;

    private List<(int, int)> poison = new();


    private List<CardSO> deck;
    private int currentCardIndex = 0;

    private List<CardSO> currentHand = new List<CardSO>();
    private List<CardSO> currentDeck = new List<CardSO>();

    private void Start()
    {
        // TODO : initialiser le deck du joueur avec une liste de cartes

        deck = new(15);
    }

    public void resetCurrentDeck()
    {
        currentDeck = null;
        currentDeck = new List<CardSO>();

        foreach (var card in deck)
        {
            currentDeck.Add(card);
        }

        ShuffleList(currentDeck);        
    }

    public void ShuffleList(List<CardSO> list){
        
    int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = Random.Range(0, n);
            CardSO tmp = list[k];  
            list[k] = list[n];  
            list[n] = tmp;  
        }
    }


    public void pioche()
    {
        if ((currentDeck.Count == 0) && (currentHand.Count == 0)){
            resetCurrentDeck();
        }

        while ((currentDeck.Count > 0) && (currentHand.Count < 5)){
            CardSO card = currentDeck[0];
            currentDeck.RemoveAt(0);
            currentHand.Add(card);
        }

    }



    public virtual void setHP(int hpDelta){}

    public int getHP()
    {
        return hp;
    }

    public void setShield(int shieldDelta)
    {
        shield += shieldDelta;
        if (shield < 0)
        {
            setHP(shield);
            shield = 0;
        }
    }

    public int getShield()
    {
        return shield;
    }

    public void addCard(CardSO card)
    {
        deck[currentCardIndex] = card;
        currentCardIndex++;
    }

     public void discardCard(CardSO card)
    {
        currentDeck.Remove(card);
    }

    public void addCardToHand(CardSO cardToAdd)
    {
        // TODO : � impl�menter
    }

    public List<CardSO> getCurrentHand()
    {
        return currentHand;
    }

    public bool isCurrentDeckEmpty()
    {
        return (currentDeck.Count <= 0);
    }

    public void addPoisonStack(int damage, int duration)
    {
        poison.Add((damage, duration));
    }

    public void consumePoisonStack()
    {
        for (int i = 0; i < poison.Count; i++)
        {
            if (poison[i].Item1 == 1)
            {
                poison.RemoveAt(i);
            }
        }
    }

    public bool isCurrentHandEmpty()
    {
        return (currentHand.Count <= 0);
    }

    public int GetMaxHandSize()
    {
        return maxHandSize;
    }
}
