using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card
{
    public int currentLevel;
    public string cardName;
    public int[] damage;
    public int[] shield;
    public int[] heal;
    public int[] poisonDamage;
    public int[] poisonDuration;
    public ElementType elementType;
    public Sprite[] sprite;
    public string description;

    public Card(int currentLevel, string cardName, int[] damage, int[] shield, int[] heal, int[] poisonDamage, int[] poisonDuration, ElementType elementType, Sprite[] sprite, string description)
    {
        this.currentLevel = currentLevel;
        this.cardName = cardName;
        this.damage = damage;
        this.shield = shield;
        this.heal = heal;
        this.poisonDamage = poisonDamage;
        this.poisonDuration = poisonDuration;
        this.elementType = elementType;
        this.sprite = sprite;
        this.description = description;
    }

    public Card(CardSO card)
    {
        this.currentLevel = card.currentLevel;
        this.cardName = card.cardName;
        this.damage = card.damage;
        this.shield = card.shield;
        this.heal = card.heal;
        this.poisonDamage = card.poisonDamage;
        this.poisonDuration = card.poisonDuration;
        this.elementType = card.elementType;
        this.sprite = card.sprite;
        this.description = card.description;
    }

    public Card Clone()
    {
        Card card = new (currentLevel, cardName, damage, shield, heal, poisonDamage, poisonDuration, elementType, sprite, description);
        return card;
    }

    public void OnUseCard(Fighter thrower, Fighter receiver)
    {

        switch (elementType)
        {
            case ElementType.POISON:
                InflictPoison(receiver);
                break;
            case ElementType.FIRE:
                ShieldBreak(receiver);
                break;
        }
        receiver.SetShield(-damage[currentLevel]);
        thrower.SetShield(shield[currentLevel]);
        thrower.SetHP(heal[currentLevel]);
    }


    void InflictPoison(Fighter receiver)
    {
        receiver.AddPoisonStack(poisonDamage[currentLevel], poisonDuration[currentLevel]);
    }

    void ShieldBreak(Fighter receiver)
    {
        receiver.SetShield(receiver.GetShield());
    }


    public void UpgardeCard()
    {
        if (currentLevel < 2)
            currentLevel++;
    }
}
