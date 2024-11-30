using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    private int currentFight;
    private int maxFightNumber;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void InitiateBattle()
    {
        // TODO : implémenter la logique
        // Récupérer l'enemy avec son deck
    }

    private void TurnPreparation()
    {
        // Choisir les cartes du joueurs
        // Choisir les cartes de l'enemy
        // Utiliser ou non la perception
    }

    public void PlayTurn(List<CardSO> playerCards)
    {
        // Joueur les cartes de manières séquentielles -> OK

        foreach (CardSO card in playerCards)
        {
            Debug.Log(card.cardName);
        }

        List<CardSO> enemyHand = Enemy.enemyInstance.getCurrentHand();
        List<CardSO> enemyCards = new List<CardSO>();
        int numberOfEnemyCard = 0;

        for (int i = 0; i <3; i++)
        {
            if (enemyHand.Count > 0)
            {
                enemyCards.Add(enemyCards[0]);
                enemyHand.RemoveAt(0);
                numberOfEnemyCard++;
            }
            else
            {
                numberOfEnemyCard = i;
                return;
            }
        }

        while (playerCards.Count > 0 && numberOfEnemyCard > 0) 
        {
            CardSO currentPlayerCard = playerCards[0];
            playerCards.RemoveAt(0);

            CardSO currentEnemyCard = enemyCards[0];
            enemyCards.RemoveAt(0);
            numberOfEnemyCard--;

            currentPlayerCard.OnUseCard(Player.playerInstance);
            currentEnemyCard.OnUseCard(Enemy.enemyInstance);
        }

        // On vérifie il reste des cartes au joueur ou à l'enemy
        if (playerCards.Count > 0)
        {
            while(playerCards.Count > 0)
            {
                CardSO currentPlayerCard = playerCards[0];
                playerCards.RemoveAt(0);
                currentPlayerCard.OnUseCard(Player.playerInstance);
            }
        }        
    }

    private void PostTurnLogic()
    {
        // Dégats de poison à la fin de l'utilisation des cartes -> OK
        // Réinitialiser les shields -> OK
        // Vérifier les cartes restantes dans le deck -> OK

        // Application des dégâts de poison
        Enemy.enemyInstance.consumePoisonStack();
        Player.playerInstance.consumePoisonStack();

        // Supprimer les shields à la fin du tours
        Player.playerInstance.setShield(Player.playerInstance.getShield());
        Enemy.enemyInstance.setShield(Enemy.enemyInstance.getShield());

        // Si le joueur n'a pas utilisé de perception se tour, lui augmenter
        if (!Player.playerInstance.getPerceptionStatus())
        {
            Player.playerInstance.addPerception();
        }

        // Vérifier si il reste des cartes dans le deck du joueur et dans le deck de l'enemy
        if (Player.playerInstance.isCurrentDeckEmpty() )
        {
            Player.playerInstance.resetCurrentDeck();
        }

        if (Enemy.enemyInstance.isCurrentDeckEmpty())
        {
            Enemy.enemyInstance.resetCurrentDeck();
        }
    }

    private void GameOver()
    {
        // TODO : implémenter la logique de défaite
        Application.Quit();
    }

    private void NextBattle()
    {

    }

    public void WinFight()
    {
        // Gestion de la récompense du comabt
        // Trois cas : améliorer une carte, changer une carte, (full) heal



        // Gestion du prochain combat
        currentFight++;
        if (currentFight == maxFightNumber)
        {
            // TODO : gérer le cas du combat de boss
        }
        else
        {
            // TODO : gérer le cas du prochain enemy
        }
    }
}
