using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    private int hp;
    private int shield;

    private List<(int, int)> poison;


    private CardSO[] deck = new CardSO[15];
    private CardSO[] currentHand = new CardSO[3];

    public void setHP(int hpDelta)
    {
        //TODO : à implémenter
    }

    public int getHP()
    {
        return hp;
    }

    public void setShield(int shieldDelta)
    {
        //TODO : à implémenter
    }

    public int getShield()
    {
        return shield;
    }

    public void addCard(CardSO card)
    {
        //TODO : à implémenter
    }

     public void disacrdCard(CardSO card)
    {
        // TODO : à implémenter
    }

    public void addCardToHand(CardSO cardToAdd)
    {
        //TODO : à implémenter
    }

    public CardSO[] getCurrentHand()
    {
        return currentHand;
    }

    public void addPoisonStack(int damage, int duration)
    {
        //TODO : à implémenter
    }

    public void consumePoisonStack()
    {
        //TODO : à implémenter
    }
}
