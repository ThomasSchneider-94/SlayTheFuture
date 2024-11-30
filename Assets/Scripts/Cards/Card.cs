using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card: MonoBehaviour
{
    public CardSO cardSO;

    public int currentLevel;
    public string cardName;
    public int[] damage = new int[3];
    public int[] shield = new int[3];
    public int[] heal = new int[3];
    public int[] poisonDamage = new int[3];
    public int[] poisonDuration = new int[3];
    public ElementType elementType;
    public Sprite[] sprite = new Sprite[3];
    public string description;


    void LoadSO(string cardName)
    {
        currentLevel = 0;
        cardName = cardSO.cardName;
        damage = cardSO.damage; shield = cardSO.shield; heal = cardSO.heal; 
        poisonDamage = cardSO.poisonDamage; poisonDuration = cardSO.poisonDuration;
        elementType = cardSO.elementType;
        sprite = cardSO.sprite;
        description = cardSO.description;
    }
}
