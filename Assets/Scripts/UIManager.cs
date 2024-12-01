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

    [Header("Perception")]
    [SerializeField] private Image perceptionIcon;
    [SerializeField] private TextMeshProUGUI perceptionCount;
    [SerializeField] private Sprite noPerceptionSprite;
    [SerializeField] private Sprite lowPerceptionSprite;
    [SerializeField] private Sprite highPerceptionSprite;

    [Header("Shield")]
    [SerializeField] private GameObject playerShield;
    [SerializeField] private TextMeshProUGUI playerShieldCount;
    [SerializeField] private GameObject enemyShield;
    [SerializeField] private TextMeshProUGUI enemyShieldCount;

    [Header("Poison")]
    [SerializeField] private GameObject playerPoison;
    [SerializeField] private TextMeshProUGUI playerPoisonCount;
    [SerializeField] private GameObject enemyPoison;
    [SerializeField] private TextMeshProUGUI enemyPoisonCount;

    private void Start()
    {
        // Health
        Player.Instance.HealthChangeEvent.AddListener(ChangePlayerHealthBar);
        Enemy.Instance.HealthChangeEvent.AddListener(ChangeEnemyHealthBar);

        playerHeatlthBar.FillAllBars(Player.Instance.GetMaxHp());
        enemyHeatlthBar.FillAllBars(Enemy.Instance.GetMaxHp());

        // Perception
        Player.Instance.PerceptionChangeEvent.AddListener(PerceptionUpdate);
        PerceptionUpdate();
        
        // Shield
        Player.Instance.ShieldChangeEvent.AddListener(ChangePlayerShield);
        Enemy.Instance.ShieldChangeEvent.AddListener(ChangeEnemyShield);
        ChangePlayerShield();
        ChangeEnemyShield();

        // Shield
        Player.Instance.ShieldChangeEvent.AddListener(ChangePlayerPoison);
        Enemy.Instance.ShieldChangeEvent.AddListener(ChangeEnemyPoison);
        ChangePlayerPoison();
        ChangeEnemyPoison();
    }

    public void ResetUI()
    {
        playerHeatlthBar.heatlthCount.text = Player.Instance.GetHP().ToString();
        playerHeatlthBar.healthBar.fillAmount = (float)Player.Instance.GetHP() / Player.Instance.GetMaxHp();
        playerHeatlthBar.indicationBar.fillAmount = playerHeatlthBar.healthBar.fillAmount;

        enemyHeatlthBar.heatlthCount.text = Enemy.Instance.GetHP().ToString();
        enemyHeatlthBar.healthBar.fillAmount = (float)Enemy.Instance.GetHP() / Enemy.Instance.GetMaxHp();
        enemyHeatlthBar.indicationBar.fillAmount = enemyHeatlthBar.healthBar.fillAmount;

        PerceptionUpdate();
        ChangePlayerShield();
        ChangeEnemyShield();
    }

    #region Health
    private void ChangePlayerHealthBar(int hpDelta)
    {
        StartCoroutine(ChangeHealthBar(hpDelta, Player.Instance, playerHeatlthBar));
    }

    private void ChangeEnemyHealthBar(int hpDelta)
    {
        StartCoroutine(ChangeHealthBar(hpDelta, Enemy.Instance, enemyHeatlthBar));
    }

    private IEnumerator ChangeHealthBar(int hpDelta, Fighter fighter, HealthBar healthBar)
    {
        float t = 0;
        float initFill = healthBar.healthBar.fillAmount;
        float finalFill = (float)fighter.GetHP() / fighter.GetMaxHp();

        healthBar.heatlthCount.text = fighter.GetHP().ToString();

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

        healthBar.healthBar.fillAmount = finalFill;
        healthBar.indicationBar.fillAmount = finalFill;
    }
    #endregion Health

    #region Perception
    private void PerceptionUpdate()
    {
        perceptionCount.text = Player.Instance.GetCurrentPerception().ToString();

        if (Player.Instance.GetCurrentPerception() == 0)
        {
            perceptionIcon.sprite = noPerceptionSprite;
        }
        else if (Player.Instance.GetCurrentPerception() < Player.Instance.GetMaxPerception() / 2)
        {
            perceptionIcon.sprite = lowPerceptionSprite;
        }
        else
        {
            perceptionIcon.sprite = highPerceptionSprite;
        }
    }
    #endregion Perception

    #region Shield
    private void ChangePlayerShield()
    {
        ShieldUpdate(playerShield, playerShieldCount, Player.Instance);
    }

    private void ChangeEnemyShield()
    {
        ShieldUpdate(enemyShield, enemyShieldCount, Enemy.Instance);
    }

    private static void ShieldUpdate(GameObject shield, TextMeshProUGUI counter, Fighter fighter)
    {
        shield.SetActive(fighter.GetShield() > 0);
        counter.text = fighter.GetShield().ToString();
    }
    #endregion Shield

    #region Poison
    private void ChangePlayerPoison()
    {
        PoisonUpdate(playerPoison, playerPoisonCount, Player.Instance);
    }

    private void ChangeEnemyPoison()
    {
        PoisonUpdate(enemyPoison, enemyPoisonCount, Enemy.Instance);
    }

    private static void PoisonUpdate(GameObject Poison, TextMeshProUGUI counter, Fighter fighter)
    {
        Poison.SetActive(fighter.GetPoison() > 0);
        counter.text = fighter.GetPoison().ToString();
    }
    #endregion Poison

}
