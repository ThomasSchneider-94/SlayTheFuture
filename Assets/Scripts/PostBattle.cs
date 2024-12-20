using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PostBattle : MonoBehaviour
{

    [SerializeField] SwapCardManager swapCardManager;

    [SerializeField] UpgradeCardManager upgradeCardManager;

    [Header("Panel Manager")]
    [SerializeField] private PanelManager panelManager;


    public void upgradeCard(){
        upgradeCardManager.showCards();
    }

    public void swapCard(){
        swapCardManager.showCards();
    }

    public void HealInBetweenBattle(){
        Player.Instance.SetHP(Player.Instance.GetMaxHp());

        panelManager.ReturnToPreviousPanel();
        BattleManager.Instance.InitiateBattle();
    }
}
