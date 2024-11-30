using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu()]

public class CardSO : ScriptableObject
{
    [SerializeField] private int MAX_CARD_LEVEL = 3;

    private int currentLevel;
    public string cardName;
    public int[] damage = new int[3];
    public int[] shield= new int[3];
    public int[] heal= new int[3];
    public int[] poisonDamage = new int[3];
    public int[] poisonDuration = new int[3];
    public ElementType elementType;
    public Sprite sprite;
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

        receiver.SetShield(damage[currentLevel]);
        thrower.SetShield(shield[currentLevel]);
        thrower.SetHP(heal[currentLevel]);



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
            //Todo
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