using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class CardSO : ScriptableObject
{
    public int damage;
    public int shield;
    public int heal;
    public ElementType elementType;
    public Sprite sprite;
    public string description;

    


    void OnDrag()
    {
        //ToDo
    }

    void OnUseCard()
    {
        //ToDo
    }



    void shieldBreak()
    {
        //Todo
    }


    void inflictIce()
    {
        //Todo
    }

    void inflictFire()
    {
        //Todo
    }

    void inflictGroud()
    {
        //Todo
    }

    void inflictPlant()
    {
        //Todo
    }

    void inflictPoison()
    {
        //Todo
    }

}


public enum ElementType
{
    Physical,
    Ice,
    Fire,
    Ground,
    Plant,
    Poison
}