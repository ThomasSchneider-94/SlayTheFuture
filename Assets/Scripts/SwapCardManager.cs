using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapCardManager : MonoBehaviour
{
    [SerializeField] GameObject cardButton;
    Card cardReplacement;
    [SerializeField] GameObject cardReplacementGO;
    List<Card> deck;

    [Header("Panel Manager")]
    [SerializeField] private PanelManager panelManager;
    [SerializeField] private GameObject mainPanel;



    public void showCards()
    {

        cardReplacement = new Card(BattleManager.Instance.GetRandomCardSO());

        Instantiate(cardButton, cardReplacementGO.transform).GetComponent<CardButton>().ApplyCard(cardReplacement);
        
        deck = Player.Instance.GetDeck();

        Debug.Log(deck.Count);
        GameObject currentRow = null;
        for (int i = 0;i < deck.Count; i++)
        {
            if (i %(deck.Count/3) == 0)
            {
                GameObject gameObject = new GameObject();
                gameObject.AddComponent<HorizontalLayoutGroup>();
                currentRow = GameObject.Instantiate(gameObject, this.transform);
                
            }
            
            GameObject currentCardButtonO = Instantiate(cardButton, currentRow.transform);
            CardButton currentCardButton = currentCardButtonO.GetComponent<CardButton>();
            int y = i;
            currentCardButton.ApplyCard(deck[y]);

            currentCardButtonO.GetComponent<MultiLayerButton>().onClick.AddListener( () => InvokedFun(y));
            currentCardButton.card= deck[y];
 
        }

    }

    public void InvokedFun(int y)
    {


        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in cardReplacementGO.transform)
        {
            Destroy(child.gameObject);
        }
        Player.Instance.ChangeDeckCard(deck[y] , cardReplacement);
        //showCards();
        panelManager.TogglePanel(mainPanel);
        BattleManager.Instance.InitiateBattle();
    }

}





