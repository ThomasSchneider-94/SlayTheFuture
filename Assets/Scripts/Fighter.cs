using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class Fighter : MonoBehaviour
{
    [Header("Hand Size")]
    [SerializeField] protected int maxHandSize;
    [SerializeField] protected int maxDeckSize;

    [Header("Stats")]
    [SerializeField] protected int maxHP;
    protected int hp;
    private int shield;

    private List<(int, int)> poison = new();

    private List<Card> deck;

    protected List<Card> currentHand = new();
    private readonly List<Card> currentDeck = new();

    public UnityEvent<int> HealthChangeEvent { get; } = new();
    public UnityEvent ShieldChangeEvent { get; } = new();
    public UnityEvent PoisonChangeEvent { get; } = new();

    private void Start()
    {
        // TODO : initialiser le deck du joueur avec une liste de cartes

        hp = maxHP;
    }

    public void InitDeck(List<Card> baseDeck)
    {
        this.deck = baseDeck;
    }

    public void ChangeDeckCard(Card oldCard, Card newCard)
    {
        deck.Remove(oldCard);
        deck.Add(newCard);
    }

    public virtual void BattleInit()
    {
        ResetCurrentDeck();
        
        Draw();

        shield = 0;
        poison.Clear();

        ShieldChangeEvent.Invoke();
        PoisonChangeEvent.Invoke();
    }



    #region Current Deck
    public void ResetCurrentDeck()
    {
        currentDeck.Clear();

        foreach (var card in deck)
        {
            currentDeck.Add(card.Clone());
        }

        ShuffleList(currentDeck);
    }

    private static void ShuffleList(List<Card> list)
    {

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    public virtual void Draw()
    {
        if (currentDeck.Count == 0 && currentHand.Count == 0)
        {
            ResetCurrentDeck();
        }

        while (currentDeck.Count > 0 && currentHand.Count < maxHandSize)
        {
            Card card = currentDeck[0];
            currentDeck.RemoveAt(0);
            currentHand.Add(card);
        }
    }

    public bool IsCurrentDeckEmpty()
    {
        return (currentDeck.Count <= 0);
    }

    public List<Card> GetCurrentDeck()
    {
        return currentDeck;
    }
    #endregion Current Deck

    #region Hand
    public void SetCurrentHand(List<Card> hand)
    {
        this.currentHand = hand;
    }

    public List<Card> GetCurrentHand()
    {
        return currentHand;
    }

    #endregion Hand

    public void AddPoisonStack(int damage, int duration)
    {
        poison.Add((damage, duration));
        PoisonChangeEvent.Invoke();

    }

    public void ConsumePoisonStack()
    {
        for (int i = 0; i < poison.Count; i++)
        {
            SetHP(-poison[i].Item1);
            if (poison[i].Item2 == 1)
            {
                poison.RemoveAt(i);
            }
            else
            {
                poison[i] = (poison[i].Item1, poison[i].Item2 - 1);
            }
        }
        PoisonChangeEvent.Invoke();
    }

    #region Setter
    public abstract void SetHP(int hpDelta);

    public void SetShield(int shieldDelta)
    {
        shield += shieldDelta;
        if (shield < 0)
        {
            SetHP(shield);
            shield = 0;
        }
        ShieldChangeEvent.Invoke();
    }

    #endregion Setter

    #region Getter
    public int GetHP()
    {
        return hp;
    }

    public int GetMaxHp()
    {
        return maxHP;
    }

    public int GetShield()
    {
        return shield;
    }
    
    public int GetMaxHandSize()
    {
        return maxHandSize;
    }

    public List<Card> GetDeck()
    {
        return deck;
    }

    public int GetPoison()
    {
        int som = 0;
        foreach((int, int) poi in poison)
        {
            som += poi.Item1;
        }
        return som;
    }
    #endregion Getter
}
