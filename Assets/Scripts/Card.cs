using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public abstract class Card : ScriptableObject
{
    /*
    public enum ElementType
    {
        BASIC,
        FIRE,
        ICE,
        EARTH,
        POISON,
        PLANT,
        NB_ELEMENT
    }
    */
    [SerializeField] private string cardName;
    [SerializeField] private string description;
    [SerializeField] private ElementType element;

    public abstract void DoAction();

    public virtual void Init(string cardName, string description, ElementType element)
    {
        this.cardName = cardName;
        this.description = description;
        this.element = element;
    }

    #region Getter
    public string GetCardName()
    {
        return cardName;
    }

    public string GetDescription()
    {
        return cardName;
    }

    public ElementType GetElement()
    {
        return element;
    }
    #endregion Getter
}
