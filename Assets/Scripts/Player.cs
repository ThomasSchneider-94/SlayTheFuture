using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player playerInstance { get; private set; }

    private void Awake()
    {
        if (playerInstance != null && playerInstance != this)
        {
            Destroy(this);
        }
        else
        {
            playerInstance = this;
        }
    }
}
