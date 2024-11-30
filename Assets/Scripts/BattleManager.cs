using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;



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
    
    [SerializeField] private int maxPlayedCard = 3;
    [SerializeField] private int maxFightNumber;
    [SerializeField] private BattlePreparation battlePrep;
    private int currentFight;

    [Header("CardSO")]
    [SerializeField] private CardSO dmgCard;
    [SerializeField] private CardSO shieldCard;
    [SerializeField] private CardSO HealCard;
    [SerializeField] private CardSO PierceCard;
    [SerializeField] private CardSO PoisonCard;


    private readonly Dictionary<string, List<Card>> allDeckCards = new();

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
        JsonDecks allDeckData = JsonUtility.FromJson<JsonDecks>(Resources.Load<TextAsset>("DeckBuilds").text);

        foreach (JsonDeck deck in allDeckData.Decks)
        {
            allDeckCards[deck.name] = new();
            foreach (string cardName in deck.content)
            {
                // Switch for a single variable
                CardSO modelCard = cardName switch
                {
                    "DamageCard" => dmgCard,
                    "PierceCard" => PierceCard,
                    "ShieldCard" => shieldCard,
                    "HealCard" => HealCard,
                    "PoisonCard" => PoisonCard,
                    _ => dmgCard,
                };
                Card card = new(modelCard);
                allDeckCards[deck.name].Add(card);
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

        Player.Instance.Draw();
        Enemy.Instance.Draw();

        battlePrep.ResetBattle();
    }

    public void PlayTurn(List<Card> playerCards)
    {
        // Joueur les cartes de manières séquentielles -> OK

        List<Card> enemyCards = Enemy.Instance.GetPlayedCards();

        while (playerCards.Count > 0 && enemyCards.Count > 0) 
        {
            Card currentPlayerCard = playerCards[0];
            playerCards.RemoveAt(0);
            Debug.Log("Player: " + currentPlayerCard.cardName);

            Card currentEnemyCard = enemyCards[0];
            enemyCards.RemoveAt(0);
            Debug.Log("Enemy: " + currentEnemyCard.cardName);

            currentPlayerCard.OnUseCard(Player.Instance, Enemy.Instance);
            currentEnemyCard.OnUseCard(Enemy.Instance, Player.Instance);
        }

        // On vérifie si l'enemy a encore des cartes à jouer.
        // Cas ou le joueur décide de ne pas joueur trois cartes
        while (enemyCards.Count > 0)
        {
            Card currentEnemyCard = enemyCards[0];
            enemyCards.RemoveAt(0);
            Debug.Log("Enemy: " + currentEnemyCard.cardName);
            currentEnemyCard.OnUseCard(Enemy.Instance, Player.Instance);
        }
        while (playerCards.Count > 0)
        {
            Card currentPlayerCard = playerCards[0];
            playerCards.RemoveAt(0);
            Debug.Log("Player: " + currentPlayerCard.cardName);
            currentPlayerCard.OnUseCard(Player.Instance, Enemy.Instance);
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
        if (!Player.Instance.GetPerceptionStatus())
        {
            Player.Instance.AddPerception();
        }

        // Vérifier si il reste des cartes dans le deck du joueur et dans le deck de l'enemy
        Player.Instance.Draw();
        Enemy.Instance.Draw();

        battlePrep.ResetBattle();
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