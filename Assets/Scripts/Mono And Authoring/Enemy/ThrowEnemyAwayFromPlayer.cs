using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[UpdateAfter(typeof(EnemyPlayerTriggerSystem))]
public class ThrowEnemyAwayFromPlayer : MonoBehaviour
{
    private Rigidbody rb;
    [field: SerializeField] public float force { get; private set; }

    public bool canThrow;
    [SerializeField]
    private float disableCanThrowAfterSeconds;
    float timer;

    private EntityManager manager;
    private Entity enemyEntity;

    public Entity EnemyEntity
    {
        get { return enemyEntity; }
        set
        {
             enemyEntity = value;
        }
    }
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

   

    private void Update()
    {
        if (enemyEntity == Entity.Null)
        {
            Debug.Log("No Enemy");
            return;
        }
        EnemyComponent enemyComponent = manager.GetComponentData<EnemyComponent>(enemyEntity);
      
            canThrow = enemyComponent.throwEnemy;
        if (canThrow && rb.isKinematic && timer<=0)
        {
            rb.isKinematic = false;
            canThrow = false;
            rb.AddForce(-this.transform.forward*force,ForceMode.Impulse);
            timer = disableCanThrowAfterSeconds;
        }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0 && rb.isKinematic == false)
        {
            rb.isKinematic = true;
        }

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.TryGetComponent<PlayerAnimationController>(out PlayerAnimationController controller))
    //    {
    //        if(other.gameObject.TryGetComponent<FollowEntity>(out FollowEntity followEntity))
    //            {
    //            if(canThrow)
    //            { 
    //                return; 
                
    //            }

               
    //                canThrow = true;
                
                
    //            PlayerData playerData = manager.GetComponentData<PlayerData>(followEntity.entityToFollow);
    //            if(playerData.isInvinisible==false)
    //            {
    //                playerData.isInvinisible = true;
    //                playerData.startTimeToResetInvinsibility = true;
    //            }
    //            manager.SetComponentData<PlayerData>(followEntity.entityToFollow,playerData);

    //        }
    //    }
    //}
}
