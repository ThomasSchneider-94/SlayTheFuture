using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class CardButton : MonoBehaviour
{
    public Card card;
    [SerializeField] private TextMeshProUGUI tmp;


    private void Start()
    {
        SetDescription();
        SetImageUpgraded();
    }

    public void SetCardSO(CardSO cardSO)
    {
        card.cardSO = cardSO;
    }

    public void SetPosition(int x, int y)
    {
        transform.position = new Vector2(x, y);
    }
    
    public void UpgradeLevel()
    {
        card.cardSO.upgardeCard();
        SetImageUpgraded();
    }

    public void SetImageUpgraded()
    {
        gameObject.GetComponent<Image>().sprite = card.cardSO.sprite[card.cardSO.currentLevel];
    }

    private void SetDescription()
    {
        tmp.text = card.cardSO.cardName switch
        {
            "DamageCard" => "Dégat *" + card.cardSO.damage[card.cardSO.currentLevel],
            "PierceCard" => "Détruit le bouclier \n Dégats *" + card.cardSO.damage[card.cardSO.currentLevel],
            "ShieldCard" => "Bouclier *" + card.cardSO.shield[card.cardSO.currentLevel],
            "HealCard" => "Soin *" + card.cardSO.heal[card.cardSO.currentLevel],
            "PoisonCard" => "Empoisonne l'ennemi pour " + card.cardSO.poisonDuration[card.cardSO.currentLevel] + " tours, infligeant " + card.cardSO.poisonDamage[card.cardSO.currentLevel] + " Dégats"
        };
    }

}
