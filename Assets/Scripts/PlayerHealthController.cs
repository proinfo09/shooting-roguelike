using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int currentHealth;
    public int maxHealth;

    public float damageInvincLength = 1f;
    private float invincCount;
    Vector4 oriColor;

    public int hurtSFX = 5, deathSFX = 6;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

        //currentHealth = CharacterTracker.instance.currentHealth;
        maxHealth = (int)PlayerPrefs.GetFloat("MaxHealth");
        currentHealth = (int)PlayerPrefs.GetFloat("CurrentHealth", maxHealth);
        
        //currentHealth = maxHealth;
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = $"{currentHealth} / {maxHealth}";
        oriColor = PlayerController.instance.bodySR.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(invincCount > 0)
        {
            invincCount -= Time.deltaTime;
            if(invincCount <= 0)
            {
                PlayerController.instance.bodySR.color = new Color(oriColor.x, oriColor.y, oriColor.z, 1f);
            }
        }
    }

    public void DamagePlayer()
    {
        if (invincCount <= 0)
        {
            currentHealth--;
            AudioManager.instance.PlaySFX(hurtSFX);
            invincCount = damageInvincLength;
            PlayerController.instance.bodySR.color = new Color(oriColor.x, oriColor.y, oriColor.z, 0.5f);

            if (currentHealth <= 0)
            {
                PlayerController.instance.gameObject.SetActive(false);
                UIController.instance.deathSceen.SetActive(true);
                AudioManager.instance.PlaySFX(deathSFX);
                AudioManager.instance.PlayGameOver();
                PlayerPrefs.SetFloat("CurrentHealth", maxHealth);
            }

            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }

    public void MakeInvincible(float lenght)
    {
        invincCount = lenght;
        PlayerController.instance.bodySR.color = new Color(oriColor.x, oriColor.y, oriColor.z, 0.5f);
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = $"{currentHealth} / {maxHealth}";
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = $"{currentHealth} / {maxHealth}";
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("CurrentHealth");
    }
}
