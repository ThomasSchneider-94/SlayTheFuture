using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCard : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private List<Image> playedCardPosition;
    [SerializeField] private Transform hand;
    [SerializeField] private int maxSpaceCard;

    [Header("Parameters")]
    [SerializeField] private Button cardPrefab;

    private List<bool> buttonsPlaced = new();


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < maxSpaceCard; i++)
        {

        }
    }








    private void ResetCardPosition()
    {
        buttonsPlaced = new(new bool[playedCardPosition.Count]);





    }







}
