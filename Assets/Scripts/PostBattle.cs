using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PostBattle : MonoBehaviour
{

    [SerializeField] SwapCardManager swapCardManager;

    [SerializeField] UpgradeCardManager upgradeCardManager;


    public void upgradeCard(){
        upgradeCardManager.showCards();
    }

    public void swapCard(){
        swapCardManager.showCards();
    }

    public void HealInBetweenBattle(){
        Debug.Log(Player.Instance.GetHP());
        Player.Instance.SetHP(Player.Instance.GetMaxHp());
        Debug.Log(Player.Instance.GetHP());

    }


}
