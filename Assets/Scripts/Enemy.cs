using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy enemyInstance { get; private set; }

    private void Awake()
    {
        if (enemyInstance != null && enemyInstance != this)
        {
            Destroy(this);
        }
        else
        {
            enemyInstance = this;
        }
    }
}
