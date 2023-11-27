using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    
    public float maxHealth;
    public float health;
    public Slider healthSlider;
    public bool canSplit;
    public GameObject splitProduct;
    public int splitNumber;
   
    public float dropAmount = 35;
    void Start()
    {
        health = maxHealth; 
    }
    public void setHealth(float healthMulti,float dropAmountIncrease)
    {
        maxHealth *= healthMulti;
        health = maxHealth;
        dropAmount+=dropAmountIncrease;
    }
    public void IncreaseHealth(float amount)
    {
        health += amount;
        if (health >maxHealth)
        {
            health = maxHealth;
        }
        updateSlider();
    }
    public void DecreaseHealth(float amount)
    {

        health -= amount;
        if (health <= 0)
        {
            Death();
        }
        updateSlider();
    }
    void updateSlider()
    {
        healthSlider.value = health / maxHealth;
    }
    void Death()
    {
        Currency.amount += dropAmount;
        if(canSplit)
        {
            float offset = 0.2f;
            for(int i = 0; i < splitNumber; i++)
            {
                Vector3 spawnPosition = transform.position + new Vector3(i * offset, 0, i * offset);
                Debug.Log("Split");
                Instantiate(splitProduct,spawnPosition,Quaternion.identity);
            }
        }
        //Drops and things here
        Destroy(gameObject);
    }
}
