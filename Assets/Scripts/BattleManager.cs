using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;
using UnityEngine.UI;


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
    private int currentFight = 0;

    [Header("CardSO")]
    [SerializeField] private CardSO dmgCard;
    [SerializeField] private CardSO shieldCard;
    [SerializeField] private CardSO HealCard;
    [SerializeField] private CardSO PierceCard;
    [SerializeField] private CardSO PoisonCard;

    [Header("Animation")]
    [SerializeField] private float animationTime;

    [Header("Combat")]
    [SerializeField] private List<String> enemyDeckList;
    [SerializeField] private List<Sprite> enemySpriteList;

    [Header("Panel Manager")]
    [SerializeField] private PanelManager panelManager;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject gameOverPanel;

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
    
    public void InitiateBattle()
    {
        // initialiser le deck de l'enemy
        // Shuffle les decks du player et de l'enemy
        Enemy.Instance.InitDeck(allDeckCards[enemyDeckList[currentFight]]);
        Enemy.Instance.GetComponent<Image>().sprite = enemySpriteList[currentFight];

        // Shuffle
        Player.Instance.ResetCurrentDeck();
        Enemy.Instance.ResetCurrentDeck();

        Player.Instance.ResetPoison();
        Enemy.Instance.ResetPoison();

        Enemy.Instance.SetHP(Enemy.Instance.GetMaxHp());

        Player.Instance.SetCurrentHand(new());
        Enemy.Instance.SetPlayedCards(new());

        Player.Instance.Draw();
        Enemy.Instance.Draw();

        Player.Instance.SetHP(3);


        battlePrep.ResetBattle();
    }

    public IEnumerator PlayCardAnimation(bool targetBool, int index, bool player)
    {
        CardButton cardToAnimate = battlePrep.GetCardButton(player, index);
        cardToAnimate.ApplyCard();
        GameObject target;
        if (targetBool)
        {
            target = Player.Instance.gameObject;
        }
        else
        {
            target = Enemy.Instance.gameObject;
        }

        float t = 0;
        Vector3 startPosition = cardToAnimate.transform.position;

        while (t < animationTime)
        {
            cardToAnimate.transform.position = Vector3.Lerp(startPosition, target.transform.position, t/animationTime);

            t += Time.deltaTime;
            yield return null;
        }

        cardToAnimate.gameObject.SetActive(false);
    }

    public IEnumerator PlayTurn(List<Card> playerCards)
    {
        // Joueur les cartes de manières séquentielles -> OK

        List<Card> enemyCards = Enemy.Instance.GetPlayedCards();

        int playerCardIndex = 0;
        int enemyCardIndex = 0;

        while (playerCards.Count > 0 && enemyCards.Count > 0) 
        {
            Card currentPlayerCard = playerCards[0];
            playerCards.RemoveAt(0);
            Debug.Log("Player: " + currentPlayerCard.cardName);

            Card currentEnemyCard = enemyCards[0];
            enemyCards.RemoveAt(0);
            Debug.Log("Enemy: " + currentEnemyCard.cardName);

            
            yield return StartCoroutine(PlayCardAnimation(GetCardTarget(currentPlayerCard, true) ,playerCardIndex, true));
            currentPlayerCard.OnUseCard(Player.Instance, Enemy.Instance);
            AudioManager.Instance.PlayAudio(currentPlayerCard.elementType);

            yield return StartCoroutine(PlayCardAnimation(GetCardTarget(currentEnemyCard, false) ,enemyCardIndex, false));
            currentEnemyCard.OnUseCard(Enemy.Instance, Player.Instance);
            AudioManager.Instance.PlayAudio(currentEnemyCard.elementType);

            playerCardIndex++;
            enemyCardIndex++;
        }

        // On vérifie si l'enemy a encore des cartes à jouer.
        // Cas ou le joueur décide de ne pas joueur trois cartes
        while (enemyCards.Count > 0)
        {
            Card currentEnemyCard = enemyCards[0];
            enemyCards.RemoveAt(0);
            Debug.Log("Enemy: " + currentEnemyCard.cardName);

            yield return StartCoroutine(PlayCardAnimation(GetCardTarget(currentEnemyCard, false), enemyCardIndex, false));
            currentEnemyCard.OnUseCard(Enemy.Instance, Player.Instance);
            AudioManager.Instance.PlayAudio(currentEnemyCard.elementType);

            enemyCardIndex++;
        }
        while (playerCards.Count > 0)
        {
            Card currentPlayerCard = playerCards[0];
            playerCards.RemoveAt(0);
            Debug.Log("Player: " + currentPlayerCard.cardName);

            yield return StartCoroutine(PlayCardAnimation(GetCardTarget(currentPlayerCard, true), playerCardIndex, true));
            currentPlayerCard.OnUseCard(Player.Instance, Enemy.Instance);
            AudioManager.Instance.PlayAudio(currentPlayerCard.elementType);

            playerCardIndex++;
        }

        // On passe à la logique post-tour
        PostTurnLogic();
    }

    private bool GetCardTarget(Card cardToPlay, bool player)
    {
        bool target;
        if (cardToPlay.elementType == ElementType.PLANT || cardToPlay.elementType == ElementType.EARTH)
        {
            target = true;
        }
        else
        {
            target = false;
        }

        if (!player)
        {
            target = !target;
        }

        return target;
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
        Player.Instance.SetShield(-Player.Instance.GetShield());
        Enemy.Instance.SetShield(-Enemy.Instance.GetShield());

        // Si le joueur n'a pas utilisé de perception se tour, lui augmenter
        if (!Player.Instance.GetPerceptionStatus())
        {
            Player.Instance.AddPerception();
        }
        else
        {
            Player.Instance.SetPerceptionStatus();
        }

        // Vérifier si il reste des cartes dans le deck du joueur et dans le deck de l'enemy
        Player.Instance.Draw();
        Enemy.Instance.Draw();

        battlePrep.ResetBattle();
    }

    public void GameOver()
    {
        StopAllCoroutines();
        panelManager.TogglePanel(gameOverPanel);
    }

    public void NextBattle()
    {
        currentFight++;
        if (currentFight > maxFightNumber)
        {
            currentFight = 0;
        }
        StopAllCoroutines();

        panelManager.TogglePanel(upgradePanel);
    }

    public int GetMaxPlayedCard()
    {
        return maxPlayedCard;
    }

    public CardSO GetRandomCardSO()
    {
        CardSO cardSO = UnityEngine.Random.Range(0, 5) switch
        {
            0 => dmgCard,
            1 => HealCard,
            2 => shieldCard,
            3 => PoisonCard,
            4 => PierceCard,

        };
        return cardSO;
            
            ;
    }
}