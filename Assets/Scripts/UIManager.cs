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
    public Image indicationBar;
    public Color damageColor;
    public Color healColor;
    public TextMeshProUGUI heatlthCount;

    public void FillAllBars(int maxHP)
    {
        healthBar.fillAmount = 1;
        indicationBar.fillAmount = 1;
        heatlthCount.text = maxHP.ToString();
    }
}

public class UIManager : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float indicationChangeTime;
    [SerializeField] private float healthChangeTime;
    [SerializeField] private HealthBar playerHeatlthBar;
    [SerializeField] private HealthBar enemyHeatlthBar;

    private Player player;
    private Enemy enemy;

    private void Start()
    {
        player = Player.Instance;
        enemy = Enemy.Instance;

        // Health
        player.HealthChangeEvent.AddListener(ChangePlayerHealthBar);
        enemy.HealthChangeEvent.AddListener(ChangeEnemyHealthBar);

        playerHeatlthBar.FillAllBars(player.GetMaxHp());
        enemyHeatlthBar.FillAllBars(enemy.GetMaxHp());

        //player.PerceptionChangeEvent.AddListener();
    }

    public void ResetUI()
    {
        playerHeatlthBar.FillAllBars(player.GetMaxHp());
        enemyHeatlthBar.FillAllBars(enemy.GetMaxHp());
    }

    public void ApplyUI()
    {
        playerHeatlthBar.heatlthCount.text = player.GetHP().ToString();
        playerHeatlthBar.healthBar.fillAmount = (float)player.GetHP() / player.GetMaxHp();
        playerHeatlthBar.indicationBar.fillAmount = playerHeatlthBar.healthBar.fillAmount;

        enemyHeatlthBar.heatlthCount.text = enemy.GetHP().ToString();
        enemyHeatlthBar.healthBar.fillAmount = (float)enemy.GetHP() / enemy.GetMaxHp();
        enemyHeatlthBar.indicationBar.fillAmount = enemyHeatlthBar.healthBar.fillAmount;
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
        float initScore = fighter.GetHP() - hpDelta;

        float finalFill = (float)fighter.GetHP() / fighter.GetMaxHp();
        float finalScore = fighter.GetHP();

        if (hpDelta > 0)
        {
            healthBar.indicationBar.color = healthBar.healColor;

            float time = 0;
            while (time < indicationChangeTime)
            {
                healthBar.indicationBar.fillAmount = Mathf.Lerp(initFill, finalFill, time / indicationChangeTime);

                time += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            healthBar.indicationBar.color = healthBar.damageColor;
        }

        while (t < healthChangeTime)
        {
            healthBar.heatlthCount.text = ((int)Mathf.Lerp(initScore, finalScore, t / healthChangeTime)).ToString();
            healthBar.healthBar.fillAmount = Mathf.Lerp(initFill, finalFill, t / healthChangeTime);

            t += Time.deltaTime;
            yield return null;
        }

        if (hpDelta < 0)
        {
            float time = 0;
            while (time < indicationChangeTime)
            {
                healthBar.indicationBar.fillAmount = Mathf.Lerp(initFill, finalFill, time / indicationChangeTime);

                time += Time.deltaTime;
                yield return null;
            }
        }
        

        healthBar.heatlthCount.text = fighter.GetHP().ToString();
        healthBar.healthBar.fillAmount = finalFill;
        healthBar.indicationBar.fillAmount = finalFill;
    }
    #endregion Health
}
