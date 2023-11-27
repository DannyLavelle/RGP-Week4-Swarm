using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallScript : MonoBehaviour
{
    float maxHealth;
    float healths;
    float reflect;
    
    public Slider healthSlider;
    public void SetStats(float Reflect, float health)
    {
        reflect = Reflect;
       maxHealth = health;
        healths = maxHealth;
        updateSlider();
        Debug.Log("Refreshed wall");
    }
    public void TakeDamage(GameObject enemy,float amount)
    {

        
        healths -= amount;
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        enemyHealth.DecreaseHealth(reflect * 5f*Time.deltaTime);
        if(healths<= 0)
        {
            Destroy(gameObject);
        }
        Debug.Log("Taking Damage : " + healths +"/" + maxHealth);
        updateSlider();
        
    }
    void updateSlider()
    {
        healthSlider.value = healths / maxHealth;
    }

}
