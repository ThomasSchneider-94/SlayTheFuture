using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;



[Serializable]
public class JsonDecks
{
    public JsonDeck[] Decks;
}

[Serializable]
public class JsonDeck
{
    public string name;
    public string[] content;
}

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

    private readonly Dictionary<string, List<CardSO>> allDeckCards = new();

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

    private void Start()
    {
        InitializeDecks();

        Player.Instance.InitDeck(allDeckCards["BalancedDeck"]);

        InitiateBattle();
    }

    private void InitializeDecks()
    {
        TextAsset test = Resources.Load<TextAsset>("DeckBuilds.json");


        JsonDecks allDeckData = JsonUtility.FromJson<JsonDecks>(test.text);



        foreach (JsonDeck deck in allDeckData.Decks)
        {
            foreach (string card in deck.content)
            {
                // Switch for a single variable
                CardSO cardSO = card switch
                {
                    "DamageCard" => dmgCard,
                    "PierceCard" => PierceCard,
                    "ShieldCard" => shieldCard,
                    "HealCard" => HealCard,
                    "PoisonCard" => PoisonCard,
                    _ => dmgCard,
                };
                allDeckCards[deck.name].Add(cardSO);
            }
        }
    }
    
    private void InitiateBattle()
    {
        // initialiser le deck de l'enemy
        // Shuffle les decks du player et de l'enemy
        Enemy.Instance.InitDeck(allDeckCards["AttackerDeck"]);

        // Shuffle
        Player.Instance.ResetCurrentDeck();
        Enemy.Instance.ResetCurrentDeck();

        battlePrep.ResetBattle();
    }

    public void PlayTurn(List<CardSO> playerCards)
    {
        // Joueur les cartes de manières séquentielles -> OK

        foreach (CardSO card in playerCards)
        {
            Debug.Log(card.cardName);
        }

        List<CardSO> enemyHand = Enemy.Instance.GetCurrentHand();
        List<CardSO> enemyCards = new();
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

            currentPlayerCard.OnUseCard(Player.Instance);
            currentEnemyCard.OnUseCard(Enemy.Instance);
        }

        // On vérifie si l'enemy a encore des cartes à jouer.
        // Cas ou le joueur décide de ne pas joueur trois cartes
        if (enemyCards.Count > 0)
        {
            while(enemyCards.Count > 0)
            {
                CardSO currentEnemyCards = enemyCards[0];
                enemyCards.RemoveAt(0);
                currentEnemyCards.OnUseCard(Enemy.Instance);
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
        Enemy.Instance.ConsumePoisonStack();
        Player.Instance.ConsumePoisonStack();

        // Supprimer les shields à la fin du tours
        Player.Instance.SetShield(Player.Instance.GetShield());
        Enemy.Instance.SetShield(Enemy.Instance.GetShield());

        // Si le joueur n'a pas utilisé de perception se tour, lui augmenter
        if (!Player.Instance.getPerceptionStatus())
        {
            Player.Instance.addPerception();
        }

        // Vérifier si il reste des cartes dans le deck du joueur et dans le deck de l'enemy
        if (Player.Instance.IsCurrentDeckEmpty() )
        {
            Player.Instance.ResetCurrentDeck();
        }

        if (Enemy.Instance.IsCurrentDeckEmpty())
        {
            Enemy.Instance.ResetCurrentDeck();
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

    public int GetMaxPlayedCard()
    {
        return maxPlayedCard;
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