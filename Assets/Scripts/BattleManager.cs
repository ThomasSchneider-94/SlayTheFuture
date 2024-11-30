using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    private int currentFight;
    private int maxFightNumber;

    [SerializeField] private int maxPlayedCard = 3;

    [SerializeField] private BattlePreparation battlePrep;

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
        // initialiser le deck de l'enemy
        // Shuffle les decks du player et de l'enemy

        // TODO : initialiser le deck de l'enemy � partir du JSON

        // Shuffle
        Player.playerInstance.resetCurrentDeck();
        Enemy.enemyInstance.resetCurrentDeck();

        battlePrep.ResetBattle();
    }

    public void PlayTurn(List<CardSO> playerCards)
    {
        // Joueur les cartes de mani�res s�quentielles -> OK

        foreach (CardSO card in playerCards)
        {
            Debug.Log(card.cardName);
        }

        List<CardSO> enemyHand = Enemy.enemyInstance.getCurrentHand();
        List<CardSO> enemyCards = new List<CardSO>();
        int numberOfEnemyCard = 0;

        for (int i = 0; i < maxPlayedCard; i++)
        {
            enemyCards.Add(enemyCards[0]);
            enemyHand.RemoveAt(0);
            numberOfEnemyCard++;
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

        // On v�rifie si l'enemy a encore des cartes � jouer.
        // Cas ou le joueur d�cide de ne pas joueur trois cartes
        if (enemyCards.Count > 0)
        {
            while(enemyCards.Count > 0)
            {
                CardSO currentEnemyCards = enemyCards[0];
                enemyCards.RemoveAt(0);
                currentEnemyCards.OnUseCard(Enemy.enemyInstance);
            }
        }

        // On passe � la logique post-tour
        PostTurnLogic();
    }

    private void PostTurnLogic()
    {
        // D�gats de poison � la fin de l'utilisation des cartes -> OK
        // R�initialiser les shields -> OK
        // V�rifier les cartes restantes dans le deck -> OK

        // Application des d�g�ts de poison
        Enemy.enemyInstance.consumePoisonStack();
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
        if (Player.playerInstance.isCurrentDeckEmpty() )
        {
            Player.playerInstance.resetCurrentDeck();
        }

        if (Enemy.enemyInstance.isCurrentDeckEmpty())
        {
            Enemy.enemyInstance.resetCurrentDeck();
        }
    }

    public void GameOver()
    {
        // TODO : impl�menter la logique de d�faite
        Application.Quit();
    }

    public void NextBattle()
    {
        // TODO : loot de fin de combat � impl�menter


        currentFight++;
        InitiateBattle();
    }
}
