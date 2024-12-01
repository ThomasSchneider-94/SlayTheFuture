using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Fighter
{
    public static Player Instance { get; private set; }

    private int perception;
    private readonly int maxPerception = 5;
    private bool perceptionUsedThisTurn;

    public UnityEvent PerceptionChangeEvent { get; } = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;

            perception = maxPerception;
        }
    }

    public override void SetHP(int hpDelta)
    {
        if (hp + hpDelta <= 0)
        {
            Debug.Log("GameOver");
            BattleManager.Instance.GameOver();
            return;
        }

        if (hp + hpDelta > maxHP)
        {
            hp = maxHP;
            return;
        }

        hp += hpDelta;
        Debug.Log("Player :" + hp);
        HealthChangeEvent.Invoke(hpDelta);
    }

    public int GetCurrentPerception()
    {
        return perception;
    }

    public void AddPerception()
    {
        perception += 1;
        if (perception > maxPerception)
        {
            perception = maxPerception;
        }
        PerceptionChangeEvent.Invoke();
    }

    public void UsePerception(int numberOfPerceptionUsed)
    {
        perception -= numberOfPerceptionUsed;
        perceptionUsedThisTurn = true;
        PerceptionChangeEvent.Invoke();
    }

    public bool GetPerceptionStatus() 
    {
        return perceptionUsedThisTurn;
    }

    public void SetPerceptionStatus()
    {
        perceptionUsedThisTurn = !perceptionUsedThisTurn;
    }

    public int GetMaxPerception()
    {
        return maxPerception;
    }
}
