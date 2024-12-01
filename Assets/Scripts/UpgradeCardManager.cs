using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCardManager : MonoBehaviour
{
    [SerializeField] GameObject cardButton;
    List<Card> list3cards;

    [Header("Panel Manager")]
    [SerializeField] private PanelManager panelManager;
    [SerializeField] private GameObject mainPanel;

    List<Card> deck;   

    public void showCards()
    {

        list3cards = new List<Card>();

        deck = Player.Instance.GetDeck();

        List<int> canBeUpgraded = new List<int>();

        for (int i = 0; i < deck.Count; i++){
            if (deck[i].currentLevel < 2){
                canBeUpgraded.Add(i);
            }
        }

        if (canBeUpgraded.Count == 0){
            // Cas où aucune carte est upgradable, peut-être à fix
            return;
        }
        else if (canBeUpgraded.Count <= 3){
            foreach (int card_indice in canBeUpgraded){
                list3cards.Add(deck[card_indice]);
            }   
        }
        else{
            while (list3cards.Count < 3){
                int possible_upgrade = Random.Range(0, canBeUpgraded.Count);
                list3cards.Add(deck[canBeUpgraded[possible_upgrade]]);
                canBeUpgraded.RemoveAt(possible_upgrade);
            }

        }

        Debug.Log(list3cards.Count);

//        Instantiate(cardButton, cardReplacementGO.transform).GetComponent<CardButton>().ApplyCard(cardReplacement);
        

        GameObject gameObject = new GameObject();
        gameObject.AddComponent<HorizontalLayoutGroup>();
        GameObject row = GameObject.Instantiate(gameObject, this.transform);

        for (int i = 0; i < list3cards.Count; i++)
        {
            
            GameObject currentCardButtonO = Instantiate(cardButton, row.transform);
            CardButton currentCardButton = currentCardButtonO.GetComponent<CardButton>();
            int y = i;
            currentCardButton.ApplyCard(list3cards[y]);

            currentCardButtonO.GetComponent<MultiLayerButton>().onClick.AddListener( () => InvokedFun(y));
            currentCardButton.card= list3cards[y]; 
        }

    }

    public void InvokedFun(int y)
    {        
        Debug.Log("INvokeeed : " + y.ToString());

        list3cards[y].UpgardeCard();

        foreach (Transform child in this.transform){
            Destroy(child.gameObject);
        }

        //showCards();
        panelManager.ReturnToPreviousPanel();
        BattleManager.Instance.InitiateBattle();
    }
}





