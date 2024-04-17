using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour, IDamageable
{
    float health;
    public float maxHealth;
    public Slider healthBar;
    public GameObject gameManager;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = gameManager.GetComponent<GameManager>();
        
        health = maxHealth;
        healthBar.maxValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            healthBar.gameObject.SetActive(false);
            gm.enemiesKilled += 1;
        }
    }
}
