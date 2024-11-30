using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [SerializeField] CardSO dmgCard;
    [SerializeField] CardSO shieldCard;
    [SerializeField] CardSO HealCard;
    [SerializeField] CardSO PierceCard;
    [SerializeField] CardSO PoisonCard;

    private int currentFight;
    private int maxFightNumber;

    [SerializeField] private int maxPlayedCard = 3;

    [SerializeField] private BattlePreparation battlePrep;

    Dictionary<string, List<Card>> allDeckCards = new Dictionary<string, List<Card>>();

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

    private List<Card> ParseJson()
    {
        Dictionary<string, string[]> allDeckData = JsonUtility.FromJson<Dictionary<string,string[]>>(Resources.Load("DeckBuilds").ToString());


        List<Card> playerDeck = new List<Card>();
        foreach (string key in allDeckData.Keys)
        {
            foreach (string cardname in allDeckData[key])
            {
                Card card = new Card();
                switch (cardname)
                {
                    case "DamageCard":
                        card.SetCardSO(dmgCard);
                        break;
                    case "PierceCard":
                        card.SetCardSO(PierceCard);
                        break;
                    case "ShieldCard":
                        card.SetCardSO(shieldCard);
                        break;
                    case "HealCard":
                        card.SetCardSO(HealCard);
                        break;
                    case "PoisonCard":
                        card.SetCardSO(PoisonCard);
                        break;


                }

                allDeckCards[key].Add(card);
            }
        }

        playerDeck = allDeckCards["BalancedDeck"];
        return playerDeck;

    }

    


    private void InitiateBattle()
    {
        // initialiser le deck de l'enemy
        // Shuffle les decks du player et de l'enemy
        if (Player.playerInstance.getCurrentDeck() == null)
        {
            Player.playerInstance.setCurrentDeck(ParseJson());
        }

        Enemy.enemyInstance.setCurrentDeck(allDeckCards["AttackerDeck"]);

        // TODO : initialiser le deck de l'enemy à partir du JSON

        // Shuffle
        Player.playerInstance.resetCurrentDeck();
        Enemy.enemyInstance.resetCurrentDeck();

        battlePrep.ResetBattle();
    }

    public void PlayTurn(List<Card> playerCards)
    {
        // Joueur les cartes de manières séquentielles -> OK

        foreach (Card card in playerCards)
        {
            Debug.Log(card.cardSO.cardName);
        }

        List<Card> enemyHand = Enemy.enemyInstance.getCurrentHand();
        List<Card> enemyCards = new List<Card>();
        int numberOfEnemyCard = 0;

        for (int i = 0; i < maxPlayedCard; i++)
        {
            enemyCards.Add(enemyCards[0]);
            enemyHand.RemoveAt(0);
            numberOfEnemyCard++;
        }

        while (playerCards.Count > 0 && numberOfEnemyCard > 0) 
        {
            Card currentPlayerCard = playerCards[0];
            playerCards.RemoveAt(0);

            Card currentEnemyCard = enemyCards[0];
            enemyCards.RemoveAt(0);
            numberOfEnemyCard--;

            currentPlayerCard.cardSO.OnUseCard(Player.playerInstance);
            currentEnemyCard.cardSO.OnUseCard(Enemy.enemyInstance);
        }

        // On vérifie si l'enemy a encore des cartes à jouer.
        // Cas ou le joueur décide de ne pas joueur trois cartes
        if (enemyCards.Count > 0)
        {
            while(enemyCards.Count > 0)
            {
                Card currentEnemyCards = enemyCards[0];
                enemyCards.RemoveAt(0);
                currentEnemyCards.cardSO.OnUseCard(Enemy.enemyInstance);
            }
        }

        // On passe à la logique post-tour
        PostTurnLogic();
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

    public void GameOver()
    {
        // TODO : implémenter la logique de défaite
        Application.Quit();
    }

    public void NextBattle()
    {
        // TODO : loot de fin de combat à implémenter


        currentFight++;
        InitiateBattle();
    }
}


class JsonTemp
{
    public string[] BalancedDeck;
    public string[] AttackerDeck;
    public string[] DefenderDeck;
    public string[] PierceDeck;
    public string[] PoisonDeck;
    public string[] HealDeck;
}