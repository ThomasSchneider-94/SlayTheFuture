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
        // TODO : impl�menter la logique
        // R�cup�rer l'enemy avec son deck
    }

    private void TurnPreparation()
    {
        // Choisir les cartes du joueurs
        // Choisir les cartes de l'enemy
        // Utiliser ou non la perception
    }

    public void PlayTurn(List<CardSO> playerCards)
    {
        // Joueur les cartes de mani�res s�quentielles
        // V�rifier si un des deux fighter

        foreach (CardSO card in playerCards)
        {
            Debug.Log(card.name);
        }

        List<CardSO> enemyHand = Enemy.enemyInstance.getCurrentHand();

        /*
        while (playerHand.Count > 0 && enemyHand.Count > 0)
        {
            //CardSO currentPlayerCard = playerHand.Pop();
            //currentPlayerCard.OnUseCard(); // TDO : joueur la carte du joueur

            //CardSO currentEnemyCard = enemyHand.Pop();
            //currentEnemyCard.OnUseCard(); //TODO : joueur la carte de l'enemy
        }

        // G�rer le cas ou la taille de la main du joueur est plsu grande que celle de l'enemy
        if (playerHand.Count > 0)
        {
            while (playerHand.Count > 0)
            {
                //CardSO currentPlayerCard = playerHand.Pop();
                //currentPlayerCard.OnUseCard(); // TODO : joueur la carte du joueur
            }
        }*/

        // G�rer le cas o� la taille de la main de l'enemy est plus grande que celle du joueur
        if (enemyHand.Count > 0)
        {
            while (enemyHand.Count > 0)
            {
                //CardSO currentenemyCard = enemyHand.Pop();
                //currentenemyCard.OnUseCard(); // TODO : joueur la carte du joueur
            }
        }
    }

    private void PostTurnLogic()
    {
        // D�gats de poison � la fin de l'utilisation des cartes -> OK
        // R�initialiser les shields -> OK
        // V�rifier les cartes restantes dans le deck -> OK

        // Infliger les d�gats de poisons � la fin du tour
        Player.playerInstance.consumePoisonStack();

        // Supprimer les shields � la fin du tours
        Player.playerInstance.setShield(Player.playerInstance.getShield());
        Enemy.enemyInstance.setShield(Enemy.enemyInstance.getShield());

        // Si le joueur n'a pas utilis� de perception se tour, lui augmenter
        if (!Player.playerInstance.getPerceptionStatus())
        {
            Player.playerInstance.addPerception();
        }

        // V�rifier si il reste des cartes dans le deck du joueur et dans le deck de l'enemy
        if (Player.playerInstance.isCurrentDeckEmpty())
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
        // TODO : impl�menter la logique de d�faite
        Application.Quit();
    }

    private void NextBattle()
    {

    }

    public void WinFight()
    {
        // Gestion de la r�compense du comabt
        // Trois cas : am�liorer une carte, changer une carte, (full) heal



        // Gestion du prochain combat
        currentFight++;
        if (currentFight == maxFightNumber)
        {
            // TODO : g�rer le cas du combat de boss
        }
        else
        {
            // TODO : g�rer le cas du prochain enemy
        }
    }
}
