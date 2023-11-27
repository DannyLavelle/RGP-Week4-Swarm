using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    float timer;
     float spawnLoops;
    float gamestage;
    private GameObject Spawn;

    public GameObject[] Enemies;
    public GameObject Boss;





    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 5)
        {
            spawnLoops++;
            gamestage++;
            int index = Random.Range(0, Enemies.Length);
            Spawn =Instantiate(Enemies[index]);
            EnemyHealth health = Spawn.GetComponent<EnemyHealth>();
            EnemyMovement move = Spawn.GetComponent<EnemyMovement>();
            move.setMoveSpeed(Mathf.Pow(1.03f, spawnLoops));
            health.setHealth(Mathf.Pow(1.03f, spawnLoops),spawnLoops);
            timer = 0;
        }
        if (gamestage == 5)
        {
            Spawn =Instantiate(Boss);
            EnemyHealth health = Spawn.GetComponent<EnemyHealth>();
            EnemyMovement move = Spawn.GetComponent<EnemyMovement>();
            move.setMoveSpeed(Mathf.Pow(1.03f, spawnLoops));
            health.setHealth(Mathf.Pow(1.03f, spawnLoops),spawnLoops);
            Instantiate(gameObject);
            gamestage =0;
        }
    }
}
