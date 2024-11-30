using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    protected int hp;
    private int maxHP;
    private int shield;

    private List<(int, int)> poison;


    private CardSO[] deck = new CardSO[15];
    private int currentCardIndex = 0;

    private Stack<CardSO> currentHand = new Stack<CardSO>();
    private List<CardSO> currentDeck = new List<CardSO>();

    private void Start()
    {
        // TODO : initialiser le deck du joueur avec une liste de cartes 
    }

    public void resetCurrentDeck()
    {
        currentDeck = null;
        currentDeck = new List<CardSO>();

        foreach (var card in deck)
        {
            currentDeck.Add(card);
        }
    }

    public virtual void setHP(int hpDelta){}

    public int getHP()
    {
        return hp;
    }

    public void setShield(int shieldDelta, bool isPiercing)
    {
        shield += shieldDelta;
        if (shield < 0 && !isPiercing)
        {
            setHP(shield);
            shield = 0;
        }
        else if (shield < 0)
        {
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

     public void disacrdCard(CardSO card)
    {
        currentDeck.Remove(card);
    }

    public void addCardToHand(CardSO cardToAdd)
    {
        currentHand.Push(cardToAdd);
    }

    public Stack<CardSO> getCurrentHand()
    {
        return currentHand;
    }

    public bool isCurrentDeckEmpty()
    {
        return (currentDeck.Count > 0);
    }

    public void addPoisonStack(int damage, int duration)
    {
        poison.Add((damage, duration));
    }

    public void consumePoisonStack()
    {
        for (int i = 0; i < poison.Count; i++)
        {
            // TODO : deal damage according to the posion value

            if (poison[i].Item1 == 1)
            {
                poison.RemoveAt(i);
            }
        }
    }
}
