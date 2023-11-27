using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed;
    private Transform currentTarget;
    private int wavepointIndex = 0;
    GameObject WallCollision;
    float attackTimer;
    WallScript wall;
    // Start is called before the first frame update
    void Start()
    {
        currentTarget = Waypoints.points[0];
    }

    // Update is called once per frame
    void Update()
    {
        
        if (WallCollision == null)
        {
            Vector3 direction = currentTarget.position - transform.position;
            transform.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, currentTarget.position) <= .2f)
            {
                getnextWaypoint();
            }
        }
        else
        {
            Debug.Log("Enemy Attack");
            wall.TakeDamage(gameObject, 10f * Time.deltaTime);
            attackTimer = 0;
            //attackTimer += Time.deltaTime;
            //    if (attackTimer >= 1)
            //{
               
            //}
            
           
       
        }
     
        
    }

    void getnextWaypoint()
    {
        if (wavepointIndex >= Waypoints.points.Length-1)
        {
            BuildManager.instance.takeHit();
            Destroy(gameObject);
        }
        else
        {
            wavepointIndex++;
            currentTarget = Waypoints.points[wavepointIndex];
        }
      
    }
    private void OnTriggerEnter(Collider other)
    {
        if (WallCollision == null)
        {
            Debug.Log("Trigger entered!");
            WallCollision = other.gameObject;
            Debug.Log(WallCollision);
            wall = WallCollision.GetComponent<WallScript>();
        }
      
    }
    public void setMoveSpeed(float speedmult)
    {
        moveSpeed *= speedmult;
    }
}
