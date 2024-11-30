using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
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
