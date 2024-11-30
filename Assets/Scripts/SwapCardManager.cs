using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapCardManager : MonoBehaviour
{
    [SerializeField] GameObject cardButton;
    [SerializeField] GameObject rowPrefab;

    List<Card> deck;

    void showCards()
    {
        //deck = Player.Instance.GetDeck();

        GameObject currentRow = null;

        for (int i = 0;i < deck.Count; i++)
        {
            if (i %deck.Count/5 == 0)
            {
                GameObject gameObject = new GameObject();
                gameObject.AddComponent<HorizontalLayoutGroup>();
                currentRow = GameObject.Instantiate(gameObject);
                
            }

            GameObject.Instantiate(cardButton, currentRow.transform);
        
        
        
        }

    }
}





