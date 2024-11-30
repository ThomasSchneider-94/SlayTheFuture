using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance { get; private set; }

    private List<Card> playerDeck;
    private List<Card> enemyDeck;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


    public void setPlayerDeck(List<Card> playerCards)
    {
        this.playerDeck = playerCards;
    }

    public void setEnemyDeck(List<Card> enemyCards)
    {
        this.enemyDeck = enemyCards;
    }

    private Stack<Card> getCurrentHand(List<Card> selectedCards)
    {
        Stack<Card> cardStack = new Stack<Card>();

        if (selectedCards.Count > 0)
        {
            for (int i = selectedCards.Count; i > 0; i--)
            {
                cardStack.Push(selectedCards[i - 1]);
            }
        }
        else
        {
            cardStack = null;
        }

        return cardStack;
    }

    private void startBattle()
    {
        playerDeck = new List<Card>(); //TODO : get the current deck of the player
        enemyDeck = new List<Card>(); //TODO : get the current decl of the enemy

        List<Card> playerSelectedCards = new List<Card>(); //TODO : get the selected cards of the player
        List<Card> enemySelectedCards = new List<Card>(); //TODO : get the selected cards of the enemy

        Stack<Card> playerHand = getCurrentHand(playerSelectedCards);
        Stack<Card> enemyHand = getCurrentHand(enemySelectedCards);

        while (playerHand.Count > 0 && enemyHand.Count > 0) 
        {
            Card currentPlayerCard = playerHand.Pop();
            //card.OnUseCard(0); // TDO : joueur la carte du joueur

            Card currentEnemyCard = enemyHand.Pop();
            //currentEnemyCard.OnUseCard(1); //TODO : joueur la carte
        }

        if (playerHand.Count > 0)
        {

        }
    }

}
