using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
[UpdateBefore(typeof(TransformSystemGroup))]
public partial class FollowPlayerSystem : SystemBase
{
    EntityManager entityManager;
    Translation playerTransform;
    float3 playerPosition;
    PlayerData playerData;
    Entity player;
    protected override void OnCreate()
    {
        base.OnCreate();
        //get player entity transform
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
    }
    protected override void OnUpdate()
    {
        player = GetSingletonEntity<PlayerTag>();
        if(player == null)
        {
           
        }
        if(player!=null)
        {
            playerTransform = entityManager.GetComponentData<Translation>(player);
            playerPosition = playerTransform.Value;
   
            playerData = entityManager.GetComponentData<PlayerData>(player);
            MoveBulletSpawnerWithPlayerJob moveBulletSpawnerWithPlayerJob = new MoveBulletSpawnerWithPlayerJob { playerPosition = playerPosition,FollowEntityData=playerData };
            moveBulletSpawnerWithPlayerJob.ScheduleParallel().Complete();
        }
        
            
    }
}
public partial struct MoveBulletSpawnerWithPlayerJob : IJobEntity
{
    public float3 playerPosition;
    public PlayerData FollowEntityData;

    public void Execute(ref Translation spawnerPosition, in OffsetFromPlayerData offsetFromPlayer)
    {
 
        //if(FollowEntityData.moving)
        spawnerPosition.Value = playerPosition + offsetFromPlayer.offset;

    }
}

