using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public ObjectPool<Enemy> pool { get; private set; }
    private NavMeshAgent agent;
    private Transform player;

    
    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            return;
        }
        if(agent == null)
        {
            return ;
        }
        
       agent.destination = player.position;
    }
    public void setPool(ObjectPool<Enemy> pool)
    { 
        this.pool = pool; 
    }
    public void setPosition(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;
    }
    public void release()
    {
        pool.Release(this);
    }
    public void setObjectToAttack(Transform objectToDestroy)
    {
        player = objectToDestroy;
    }
}

