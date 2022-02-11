using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public PlayerController playerHealth;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = playerHealth.lives;
        healthBar.value = playerHealth.lives;
    }

    public void SetHealth(int hp)
    {
        healthBar.value = hp;
    }
}
