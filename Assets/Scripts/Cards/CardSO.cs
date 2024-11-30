using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



[CreateAssetMenu()]

public class CardSO : ScriptableObject
{
    [SerializeField] private int MAX_CARD_LEVEL = 3;

    public int currentLevel;
    public string cardName;
    public int[] damage = new int[3];
    public int[] shield= new int[3];
    public int[] heal= new int[3];
    public int[] poisonDamage = new int[3];
    public int[] poisonDuration = new int[3];
    public ElementType elementType;
    public Sprite[] sprite = new Sprite[3];
    public string description;

    public void Init(CardSO card)
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

    public void OnUseCard(Fighter thrower)
    {
        Fighter receiver;
        if (thrower == Player.Instance)
        {
            receiver = Enemy.Instance;
        }

        else {
            receiver = Player.Instance;
        }

        /*
        if (elementType == ElementType.ICE)
        {
            InflictIce(receiver);
        }
        else if (elementType == ElementType.FIRE)
        {
            InflictFire(receiver);

        }
        else if (elementType == ElementType.EARTH)
        {
            InflictGroud(receiver);
        }
        else if (elementType == ElementType.PLANT)
        {

            InflictPlant(receiver);
        }
        else if (elementType == ElementType.POISON)
        {
            InflictPoison(receiver);
        }*/


        thrower.SetShield(shield[currentLevel]);
        thrower.SetHP(heal[currentLevel]);
    }

    void ShieldBreak(Fighter receiver)
    {
        receiver.SetShield(receiver.GetShield());
    }


    void InflictIce(Fighter receiver)
    {
        //Todo
    }

    void InflictFire(Fighter receiver)
    {
        //Todo
    }

    void InflictGroud(Fighter receiver)
    {
        //Todo
    }

    void InflictPlant(Fighter receiver)
    {
        //Todo
    }

    void InflictPoison(Fighter receiver)
    {
        receiver.AddPoisonStack(poisonDamage[currentLevel], poisonDuration[currentLevel]);
    }

    void shieldBreak(Fighter receiver)
    {
        receiver.SetShield(receiver.GetShield());
    }

    void inflictIce(Fighter receiver)
    {
        //Todo
    }

    void inflictFire(Fighter receiver)
    {
        shieldBreak(receiver);
    }

    void inflictGroud(Fighter receiver)
    {
        //Todo
    }

    void inflictPlant(Fighter receiver)
    {
        //Todo
    }

    void inflictPoison(Fighter receiver)
    {
        receiver.AddPoisonStack(poisonDamage[currentLevel], poisonDuration[currentLevel]);
    }

    public void upgardeCard()
    {
        if (currentLevel < 2)
            currentLevel++;
        
    }

    
}


public enum ElementType
{
    PHYSICAL,
    ICE,
    FIRE,
    EARTH,
    PLANT,
    POISON,
}