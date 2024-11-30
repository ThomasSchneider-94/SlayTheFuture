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
    [SerializeField] private Sprite hidenSprite;

    public void ApplyCard(Card card)
    {
        this.card = card;

        SetDescription();
        SetImageUpgraded();
    }

    public void ApplyCard()
    {
        SetDescription();
        SetImageUpgraded();
    }

    public void HideCard()
    {
        tmp.text = "";
        gameObject.GetComponent<Image>().sprite = hidenSprite;
    }

    public void SetPosition(int x, int y)
    {
        transform.position = new Vector2(x, y);
    }
    
    public void UpgradeLevel()
    {
        card.UpgardeCard();
        SetImageUpgraded();
    }

    public void SetImageUpgraded()
    {
        gameObject.GetComponent<Image>().sprite = card.sprite[card.currentLevel];
    }

    private void SetDescription()
    {
        tmp.text = card.cardName switch
        {
            "DamageCard" => "Dégat *" + card.damage[card.currentLevel],
            "PierceCard" => "Détruit le bouclier \n Dégats *" + card.damage[card.currentLevel],
            "ShieldCard" => "Bouclier *" + card.shield[card.currentLevel],
            "HealCard" => "Soin *" + card.heal[card.currentLevel],
            "PoisonCard" => "Empoisonne l'ennemi pour " + card.poisonDuration[card.currentLevel] + " tours, infligeant " + card.poisonDamage[card.currentLevel] + " Dégats",
            _ => ""
        };
    }
}
