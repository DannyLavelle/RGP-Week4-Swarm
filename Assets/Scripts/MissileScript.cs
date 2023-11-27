using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    Transform Target;
    float speed;
    float AOE;
    float Damage;
    float timer;
    public void GetStats(float area, float flyspeed, float damage,Transform target)
    {
        AOE = area;
        speed =flyspeed;
        Damage = damage;
        Target = target;

    }
    private void Update()
    {

        timer += Time.deltaTime;
        if(timer >= 5)
        {
            Explode();
        }
        if (Target != null)
        {
            Vector3 direction = Target.position - transform.position;
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, Target.position) <= .2f)
            {
                Explode();
            }
        }
 
    }

    void Explode()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
       foreach(GameObject enemy in enemies)
        {
            float distaceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distaceToEnemy <= AOE)
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                enemyHealth.DecreaseHealth(Damage);
            }
        }
       Destroy(gameObject);
    }

}
