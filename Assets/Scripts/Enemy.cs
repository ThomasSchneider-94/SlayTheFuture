using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{
    public static Enemy Instance { get; private set; }

    private readonly List<Card> playedCards = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public override void SetHP(int hpDelta)
    {
        if (hp + hpDelta <= 0)
        {
            BattleManager.Instance.NextBattle();
            return;
        }

        if (hp + hpDelta > maxHP)
        {
            hp = maxHP;
            return;
        }

        hp += hpDelta;
        HealthChangeEvent.Invoke(hpDelta);
    }

    public override void Draw()
    {
        base.Draw();


        PlayCards();
    }
    
    private void PlayCards()
    {
        playedCards.Clear();

        List<Card> hand = new(currentHand);
        
        int i = 0;
        while (i < BattleManager.Instance.GetMaxPlayedCard() && hand.Count > 0)
        {
            Card card = currentHand[Random.Range(0, currentHand.Count - 1)];

            playedCards.Add(card);
            hand.Remove(card);
            i++;
        }

        SetCurrentHand(hand);
    }

    public List<Card> GetPlayedCards()
    {
        return playedCards;
    }
}
