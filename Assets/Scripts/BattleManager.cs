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
    
    [SerializeField] private int maxPlayedCard = 3;
    [SerializeField] private BattlePreparation battlePrep;

    [Header("CardSO")]
    [SerializeField] CardSO dmgCard;
    [SerializeField] CardSO shieldCard;
    [SerializeField] CardSO HealCard;
    [SerializeField] CardSO PierceCard;
    [SerializeField] CardSO PoisonCard;

    private int currentFight;
    private int maxFightNumber;

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
        JsonDecks allDeckData = JsonUtility.FromJson<JsonDecks>(Resources.Load<TextAsset>("DeckBuilds").text);

        foreach (JsonDeck deck in allDeckData.Decks)
        {
            allDeckCards[deck.name] = new();
            foreach (string card in deck.content)
            {
                CardSO cardSO = ScriptableObject.CreateInstance<CardSO>();

                // Switch for a single variable
                CardSO modelCard = card switch
                {
                    "DamageCard" => dmgCard,
                    "PierceCard" => PierceCard,
                    "ShieldCard" => shieldCard,
                    "HealCard" => HealCard,
                    "PoisonCard" => PoisonCard,
                    _ => dmgCard,
                };

                cardSO.Init(modelCard);
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

        Player.Instance.Draw();
        Enemy.Instance.Draw();

        battlePrep.ResetBattle();
    }

    public void PlayTurn(List<CardSO> playerCards)
    {
        // Joueur les cartes de mani�res s�quentielles -> OK

        List<CardSO> enemyCards = Enemy.Instance.GetPlayedCards();

        while (playerCards.Count > 0 && enemyCards.Count > 0) 
        {
            CardSO currentPlayerCard = playerCards[0];
            playerCards.RemoveAt(0);
            Debug.Log("Player: " + currentPlayerCard.cardName);

            CardSO currentEnemyCard = enemyCards[0];
            enemyCards.RemoveAt(0);
            Debug.Log("Enemy: " + currentEnemyCard.cardName);

            currentPlayerCard.OnUseCard(Player.Instance);
            currentEnemyCard.OnUseCard(Enemy.Instance);
        }

        // On v�rifie si l'enemy a encore des cartes � jouer.
        // Cas ou le joueur d�cide de ne pas joueur trois cartes
        while (enemyCards.Count > 0)
        {
            CardSO currentEnemyCard = enemyCards[0];
            enemyCards.RemoveAt(0);
            Debug.Log("Enemy: " + currentEnemyCard.cardName);
            currentEnemyCard.OnUseCard(Enemy.Instance);
        }
        while (playerCards.Count > 0)
        {
            CardSO currentPlayerCard = playerCards[0];
            playerCards.RemoveAt(0);
            Debug.Log("Player: " + currentPlayerCard.cardName);
            currentPlayerCard.OnUseCard(Enemy.Instance);
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
        Enemy.Instance.ConsumePoisonStack();
        Player.Instance.ConsumePoisonStack();

        // Supprimer les shields � la fin du tours
        Player.Instance.SetShield(Player.Instance.GetShield());
        Enemy.Instance.SetShield(Enemy.Instance.GetShield());

        // Si le joueur n'a pas utilis� de perception se tour, lui augmenter
        if (!Player.Instance.getPerceptionStatus())
        {
            Player.Instance.addPerception();
        }

        // V�rifier si il reste des cartes dans le deck du joueur et dans le deck de l'enemy
        Player.Instance.Draw();
        Enemy.Instance.Draw();

        battlePrep.ResetBattle();
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

    public int GetMaxPlayedCard()
    {
        return maxPlayedCard;
    }
}