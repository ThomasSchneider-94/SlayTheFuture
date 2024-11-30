using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{
    public static Enemy Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public override void SetHP(int hpDelta)
    {
        if (hp + hpDelta <= 0)
        {
            BattleManager.Instance.NextBattle();
            return;
        }

        if (hp + hpDelta > maxHP)
        {
            hp = maxHP;
            return;
        }

        hp += hpDelta;
    }
}
