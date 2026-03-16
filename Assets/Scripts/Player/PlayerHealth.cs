using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    private float durationTimer;

    [Header("Health Bar")]
    public float maxHealth = 100f;
    public float chipSpeed = 5f;

    [Header("Damage Overlay")]
    public float duration;
    public float fadeSpeed;
    public float permanentOverlayThreshold; 

    [Header("UI Components")]
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI healthText;
    public Image damageOverlay;
    
    void Start()
    {
        health = maxHealth;
        damageOverlay.gameObject.SetActive(true);
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 0);
        permanentOverlayThreshold /= 100f;
    }

    
    void Update()
    {
        health = Mathf.Clamp(health, 0f, maxHealth);
        UpdateHealthBar();
        UpdateDamageOverlay();
    }

    private void UpdateHealthBar()
    {
        healthText.text = health + "/" + maxHealth;
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float fraction = health / maxHealth;

        if (fillB > fraction)
        {
            frontHealthBar.fillAmount = fraction;
            backHealthBar.color = Color.darkRed;
            lerpTimer += Time.deltaTime;
            float percentageComplete = lerpTimer / chipSpeed;
            percentageComplete *= percentageComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, fraction, percentageComplete);
        }
        else if (fillF < fraction)
        {
            backHealthBar.color = Color.limeGreen;
            backHealthBar.fillAmount = fraction;
            lerpTimer += Time.deltaTime;
            float percentageComplete = lerpTimer / chipSpeed;
            percentageComplete *= percentageComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, fraction, percentageComplete);
        } 
    }

    private void UpdateDamageOverlay()
    {
        if (damageOverlay.color.a > 0)
        {
            if (health < maxHealth * permanentOverlayThreshold)
                return;
            
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = damageOverlay.color.a;
                tempAlpha -= fadeSpeed * Time.deltaTime;
                damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, tempAlpha);
            }
            
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0;
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 1);
    }
    
    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }
}
