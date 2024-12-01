using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HealthBar
{
    public Image healthBar;
    public Image greenRectangle;
    public Image redRectangle;
    public TextMeshProUGUI heatlthCount;
}

public class UIManager : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float healthChangeTime;
    [SerializeField] private HealthBar playerHeatlthBar;
    [SerializeField] private HealthBar enemyHeatlthBar;


    private Player player;
    private Enemy enemy;


    private void Start()
    {
        player = Player.Instance;
        enemy = Enemy.Instance;

        player.HealthChangeEvent.AddListener(ChangePlayerHealthBar);
        enemy.HealthChangeEvent.AddListener(ChangeEnemyHealthBar);

        //player.PerceptionChangeEvent.AddListener();

    }




    #region Health
    private void ChangePlayerHealthBar(int hpDelta)
    {
        StartCoroutine(ChangeHealthBar(hpDelta,player, playerHeatlthBar));
    }

    private void ChangeEnemyHealthBar(int hpDelta)
    {
        StartCoroutine(ChangeHealthBar(hpDelta, enemy, enemyHeatlthBar));
    }


    private IEnumerator ChangeHealthBar(int hpDelta, Fighter fighter, HealthBar healthBar)
    {
        float t = 0;
        float initFill = healthBar.healthBar.fillAmount;
        float initScore = fighter.GetHP() + hpDelta;

        float finalFill = (float)fighter.GetHP() / fighter.GetMaxHp();
        float finalScore = fighter.GetHP();

        if (hpDelta > 0)
        {
            healthBar.greenRectangle.fillAmount = finalFill;
        }

        while (t < healthChangeTime)
        {
            healthBar.heatlthCount.text = ((int)Mathf.Lerp(initScore, finalScore, t / healthChangeTime)).ToString();
            healthBar.healthBar.fillAmount = Mathf.Lerp(initFill, finalFill, t / healthChangeTime);

            t += Time.deltaTime;
            yield return null;
        }

        healthBar.heatlthCount.text = fighter.GetHP().ToString();
        healthBar.healthBar.fillAmount = finalFill;

        healthBar.greenRectangle.fillAmount = finalFill;
        healthBar.redRectangle.fillAmount = finalFill;
    }
    #endregion Health

}
