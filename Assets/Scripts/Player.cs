using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{
    public static Player playerInstance { get; private set; }

    private int perception;
    private readonly int maxPerception = 5;
    private bool perceptionUsedThisTurn;

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

    public override void setHP(int hpDelta)
    {
        if (hp + hpDelta <= 0)
        {
            //BattleManager.battleManagerInstance.loseFight();
        }
    }

    public int getCurrentPerception()
    {
        return perception;
    }

    public void addPerception()
    {
        perception += 1;
        if (perception > maxPerception)
        {
            perception = maxPerception;
        }
    }

    public void usePerception(int numberOfPerceptionUsed)
    {
        perception -= numberOfPerceptionUsed;
    }

    public bool getPerceptionStatus() 
    {
        return perceptionUsedThisTurn;
    }

    public void setPerceptionStatus()
    {
        perceptionUsedThisTurn = !perceptionUsedThisTurn;
    }
}
