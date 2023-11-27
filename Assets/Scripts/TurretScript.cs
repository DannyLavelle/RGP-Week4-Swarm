using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    private Transform currentTarget;
    public float range;
    public float shotCooldownSeconds = 10f;
    private float currentShotCooldown = 0f;
    public Transform turretHead;
    public float turnSpeed = 10f;
    public float damage;
    public float level = 1;
    float levelCycle = 1;
    float Shotspervolly = 1;
    public enum TurretType { Machine,Missile,Melee,Builder,Sniper,Factory}
    public TurretType type;
    private List<GameObject> multipleTargets = new List<GameObject>();

    //values for missile prefabe
    float missileAOE = 3;
    float missileSpeed = 10;
    //end of missile prefab values 
    public GameObject WallPrefab;
    private GameObject BuiltWall;
    public GameObject missilePrefab;
    private WallScript wallScript;
    //values for builder prefabe
    float wallReflect = 0;
    float wallHealth = 300;
    //end of builder prefab values 

    // Start is called before the first frame update
    void Start()
    {
        
        if(type != TurretType.Factory && type != TurretType.Builder)
        {
            InvokeRepeating("updateTarget", 0f, 0.5f);
        }
        else if(type == TurretType.Factory)
        {
            currentTarget = gameObject.transform;
        }
        else if(type == TurretType.Builder)
        {
            Debug.Log("updating Target");
            updateTarget();
        }
        currentShotCooldown = shotCooldownSeconds;
    }

    // Update is called once per frame
    void Update()
    {
      
        currentShotCooldown +=Time.deltaTime;
        //Debug.Log(currentTarget);
        if (currentTarget != null)
        {
            if (currentShotCooldown >= shotCooldownSeconds)
            {
                
                Attack();
                
                //currentShotCooldown = 0;
            }

            if(type != TurretType.Factory )
            {
                Vector3 direction = currentTarget.position - transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                Vector3 rotation = Quaternion.Lerp(turretHead.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
                turretHead.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            }
          
            
            
          
          
             
        }
     
        
    }
    void updateTarget()
    {
        //Debug.Log(type);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (type == TurretType.Machine || type == TurretType.Missile )//looks for closest enemy
        {
           
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;
            foreach (GameObject enemy in enemies)
            {
                float distaceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distaceToEnemy < shortestDistance)
                {
                    shortestDistance = distaceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null && shortestDistance <= range)
            {

                currentTarget = nearestEnemy.transform;
            }
            else
            {
                currentTarget = null;
            }
        }
        else if(type == TurretType.Melee)//Gets every enemy within range
        {
            foreach (GameObject enemy in enemies)
            {
                float distaceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distaceToEnemy <= range)
                {
                    multipleTargets.Add(enemy) ;
                }
            }
        }
        else if(type == TurretType.Builder)
        {
            //Debug.Log("Getting Builder Target");
            GameObject[] Road = GameObject.FindGameObjectsWithTag("Road");
            float shortestDistance = Mathf.Infinity;
            GameObject nearestRoad = null;
            foreach (GameObject Roads in Road)
            {
                //Debug.Log(Roads);
                float distaceToRoad = Vector3.Distance(transform.position, Roads.transform.position);
                if (distaceToRoad < shortestDistance)
                {
                    bool isWallThere = CheckIfWall(Roads.transform);
       
                    if (isWallThere == false )
                    {
                        shortestDistance = distaceToRoad;
                        nearestRoad = Roads;
                    }

                
                }
            }

            if (nearestRoad!= null)
            {

                currentTarget = nearestRoad.transform;
            }
            else
            {
                currentTarget = null;
            }
        }
        else//looks for highest health enemy
        {
            float highestHealth = 0;
            GameObject healthiestEnemy = null;
            foreach(GameObject enemy in enemies)
            {
                EnemyHealth health = enemy.GetComponent<EnemyHealth>();
                if (health.health > highestHealth)
                {
                    highestHealth = health.health;
                    healthiestEnemy = enemy;
                }
            }
            if(healthiestEnemy != null)
            {
                currentTarget = healthiestEnemy.transform;
            }
        }
       
    }
    void Attack()
    {
        EnemyHealth enemy = currentTarget.GetComponent<EnemyHealth>();
        if(type == TurretType.Melee)
        {
            foreach(GameObject enemies in multipleTargets)
            {
                enemy.DecreaseHealth(damage);
            }
            currentShotCooldown = 0;
        }
        else if(type==TurretType.Missile)
        {
            GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            MissileScript missileScript = missile.GetComponent<MissileScript>();
            Debug.Log($"Missile AOE: {missileAOE}, Missile Speed: {missileSpeed}, Damage: {damage}, Current Target: {currentTarget}");

            missileScript.GetStats(missileAOE, missileSpeed, damage, currentTarget);

            currentShotCooldown = 0;
        }
        else if(type ==TurretType.Builder)
        {
            //builder things here
            if (BuiltWall != null)
            {
                wallScript.SetStats(wallReflect, wallHealth);
            }
            else
            {
                //Debug.Log("Building Wall");
                BuiltWall = Instantiate(WallPrefab,currentTarget.position, Quaternion.identity);
                wallScript = BuiltWall.GetComponent<WallScript>();
                wallScript.SetStats(wallReflect, wallHealth);
            }
            currentShotCooldown = 0;
        }
        else if (type == TurretType.Factory)
        {
            for (int i = 0; i < Shotspervolly; i++)
            {
                Currency.amount += damage;
                damage++;
            }
            currentShotCooldown = 0;
        }
        else
        {
            updateTarget();
            for (int i = 0; i < Shotspervolly; i++)
            {
                if (currentTarget == null)
                {
                    updateTarget();
                   if (currentTarget == null) { return; }
                   
                    
                }
                enemy.DecreaseHealth(damage);

            }
            currentShotCooldown = 0;
        }


          
        
       

    }

    public void increaseLevel()
    {
        level++;
        levelCycle++;

        switch (levelCycle)
        {
            case 2:
            ApplyLevelUpEffectStage2();
            break;
            case 3:
            ApplyLevelUpEffectStage3();
            break;
            case 4:
            ApplyLevelUpEffectStage4();
            break;
            case 5:
            ApplyLevelUpEffectStage5();
            levelCycle = 1;
            break;
        }
    }

    void ApplyLevelUpEffectStage2()
    {
        switch (type)
        {
            case TurretType.Machine:
            damage += 10;
            break;
            case TurretType.Missile:
            missileAOE += 1;
            break;
            case TurretType.Melee:
            shotCooldownSeconds= shotCooldownSeconds * 0.9f;                 
            break;
            case TurretType.Builder:
            wallHealth += 10;
            break;
            case TurretType.Sniper:
            damage = damage * 1.2f;
            break;
            case TurretType.Factory:
            damage = damage * 1.5f;
            break;
        }
    }

    void ApplyLevelUpEffectStage3()
    {
        switch (type)
        {
            case TurretType.Machine:
            range += 5;
            break;
            case TurretType.Missile:
            range += 5;
            break;
            case TurretType.Melee:
            damage += 10;
            break;
            case TurretType.Builder:
            wallReflect += 1;
            break;
            case TurretType.Sniper:
            damage = damage * 1.2f;
            break;
            case TurretType.Factory:
            shotCooldownSeconds = shotCooldownSeconds * 0.9f;
            break;
        }
    }

    void ApplyLevelUpEffectStage4()
    {
        switch (type)
        {
            case TurretType.Machine:
            Shotspervolly += 1;
            break;
            case TurretType.Missile:
            shotCooldownSeconds = shotCooldownSeconds * .9f;
            break;
            case TurretType.Melee:
            damage += 10;
            break;
            case TurretType.Builder:
            shotCooldownSeconds = shotCooldownSeconds * .9f;
            break;
            case TurretType.Sniper:
            damage = damage * 1.2f;
            break;
            case TurretType.Factory:
            damage = damage * 1.5f;
            break;
        }
    }

    void ApplyLevelUpEffectStage5()
    {
        switch (type)
        {
            case TurretType.Machine:
            Shotspervolly += 1;
            break;
            case TurretType.Missile:
            damage += 5;
            shotCooldownSeconds = shotCooldownSeconds * .95f;
            break;
            case TurretType.Melee:
            damage += 10;
            break;
            case TurretType.Builder:
            wallHealth = wallHealth * 2f;
            break;
            case TurretType.Sniper:
            damage = damage * 1.5f;
            break;
            case TurretType.Factory:
            Shotspervolly += 1;
            break;
        }
    }

    bool CheckIfWall(Transform target)
    {
        bool isWallThere = false;
        GameObject[] Wall = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject wall in Wall)
        {
            if (Vector3.Distance(transform.position, wall.transform.position) == Vector3.Distance(transform.position, target.position))
            {
                isWallThere = true;
            }
        }
        return isWallThere;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
