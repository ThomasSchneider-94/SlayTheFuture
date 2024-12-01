using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PostBattle : MonoBehaviour
{

    [SerializeField] SwapCardManager swapCardManager;

    public void upgradeCard(){

    }

    public void swapCard(){
        swapCardManager.showCards();
    }

    public void HealInBetweenBattle(){
        
    }


}
