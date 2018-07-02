using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsCanvasController : MonoBehaviour {

    [SerializeField] Slider healthBar;
    [SerializeField] Slider expBar;
    [SerializeField] Text levelNumber;

    PlayerController player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.OnLevelChanged += OnLevelChanged;
        player.OnExpChanged += OnExpChanged;
        player.OnHealthChanged += OnHealthChanged;
    }

    void OnLevelChanged(int level)
    {
        levelNumber.text = level.ToString();
    }

    void OnExpChanged(int exp, int maxExp)
    {
        expBar.maxValue = maxExp;
        expBar.value = exp;
    }

    void OnHealthChanged(int health, int maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
    }
}
