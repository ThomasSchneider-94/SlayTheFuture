using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager battleManagerInstance { get; private set; }

    private int currentFight;
    private int maxFightNumber;

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

    private void initiateBattle()
    {
        // TODO : implémenter la logique
        // Récupérer l'enemy avec son deck
    }

    private void turnPreparation()
    {
        // Choisir les cartes du joueurs
        // Choisir les cartes de l'enemy
        // Utiliser ou non la perception
    }

    private void playTurn()
    {
        // Joueur les cartes de manières séquentielles
        // Vérifier si un des deux fighter

        List<CardSO> playerHand = Player.playerInstance.getCurrentHand();
        List<CardSO> enemyHand = Enemy.enemyInstance.getCurrentHand();

        while (playerHand.Count > 0 && enemyHand.Count > 0)
        {
            //CardSO currentPlayerCard = playerHand.Pop();
            //currentPlayerCard.OnUseCard(); // TDO : joueur la carte du joueur

            //CardSO currentEnemyCard = enemyHand.Pop();
            //currentEnemyCard.OnUseCard(); //TODO : joueur la carte de l'enemy
        }

        // Gérer le cas ou la taille de la main du joueur est plsu grande que celle de l'enemy
        if (playerHand.Count > 0)
        {
            while (playerHand.Count > 0)
            {
                //CardSO currentPlayerCard = playerHand.Pop();
                //currentPlayerCard.OnUseCard(); // TODO : joueur la carte du joueur
            }
        }

        // Gérer le cas où la taille de la main de l'enemy est plus grande que celle du joueur
        if (enemyHand.Count > 0)
        {
            while (enemyHand.Count > 0)
            {
                //CardSO currentenemyCard = enemyHand.Pop();
                //currentenemyCard.OnUseCard(); // TODO : joueur la carte du joueur
            }
        }
    }

    private void postTurnLogic()
    {
        // Dégats de poison à la fin de l'utilisation des cartes -> OK
        // Réinitialiser les shields -> OK
        // Vérifier les cartes restantes dans le deck -> OK

        // Infliger les dégats de poisons à la fin du tour
        Player.playerInstance.consumePoisonStack();

        // Supprimer les shields à la fin du tours
        Player.playerInstance.setShield(Player.playerInstance.getShield(), false);
        Enemy.enemyInstance.setShield(Enemy.enemyInstance.getShield(), false);

        // Si le joueur n'a pas utilisé de perception se tour, lui augmenter
        if (!Player.playerInstance.getPerceptionStatus())
        {
            Player.playerInstance.addPerception();
        }

        // Vérifier si il reste des cartes dans le deck du joueur et dans le deck de l'enemy
        if (Player.playerInstance.isCurrentDeckEmpty())
        {
            Player.playerInstance.resetCurrentDeck();
        }

        if (Enemy.enemyInstance.isCurrentDeckEmpty())
        {
            Enemy.enemyInstance.resetCurrentDeck();
        }
    }

    private void gameOver()
    {
        // TODO : implémenter la logique de défaite
        Application.Quit();
    }

    private void nextBattle()
    {

    }

    public void winFight()
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
