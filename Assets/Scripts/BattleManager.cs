using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager battleManagerInstance { get; private set; }

    private void Awake()
    {
        if (battleManagerInstance != null && battleManagerInstance != this)
        {
            Destroy(this);
        }
        else
        {
            battleManagerInstance = this;
        }
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
        List<Card> playerSelectedCards = new List<Card>(); //TODO : get the selected cards of the player
        List<Card> enemySelectedCards = new List<Card>(); //TODO : get the selected cards of the enemy

        Stack<Card> playerHand = getCurrentHand(playerSelectedCards);
        Stack<Card> enemyHand = getCurrentHand(enemySelectedCards);

        while (playerHand.Count > 0 && enemyHand.Count > 0) 
        {
            Card currentPlayerCard = playerHand.Pop();
            //card.OnUseCard(0); // TDO : joueur la carte du joueur

            Card currentEnemyCard = enemyHand.Pop();
            //currentEnemyCard.OnUseCard(1); //TODO : joueur la carte de l'enemy
        }

        // Gérer le cas ou la taille de la main du joueur est plsu grande que celle de l'enemy
        if (playerHand.Count > 0 && enemyHand.Count == 0)
        {
            while(playerHand.Count > 0)
            {
                Card currentPlayerCard = playerHand.Pop();
                //card.OnUseCard(0); // TODO : joueur la carte du joueur
            }
        }
    }

}
