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




        if (elementType == ElementType.ICE)
        {
            inflictIce(receiver);
        }
        else if (elementType == ElementType.FIRE)
        {
            inflictFire(receiver);

        }
        else if (elementType == ElementType.EARTH)
        {
            inflictGroud(receiver);
        }
        else if (elementType == ElementType.PLANT)
        {

            inflictPlant(receiver);
        }
        else if (elementType == ElementType.POISON)
        {
            inflictPoison(receiver);
        }

        thrower.SetShield(shield[currentLevel]);
        receiver.SetShield(-damage[currentLevel]);
        receiver.SetHP(heal[currentLevel]);

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