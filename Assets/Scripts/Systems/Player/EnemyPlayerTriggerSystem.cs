using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Collections;
using Unity.Physics.Systems;
//using static UnityEditor.Experimental.GraphView.GraphView;
using System.Numerics;
using Unity.Physics.Extensions;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using JetBrains.Annotations;

[UpdateAfter(typeof(HandlePlayerEnemyCollisionSystem))]
public partial class EnemyPlayerTriggerSystem : SystemBase
{
    public StepPhysicsWorld physicsStep;
    public BuildPhysicsWorld physicsWorld;
    private EndFramePhysicsSystem _endFramePhysicsSystem;
    private EntityManager manager;


    private NativeArray<bool> collidedWithPlayer;
    private NativeArray<Entity> enemy;
    private ComponentDataFromEntity<EnemyThrowTriggerTag> playerColliderGroup;
    private ComponentDataFromEntity<EnemyTag> enemies;


    private NativeList<Entity> enemiesWithinRadius;

    private Entity playerColliderEntity;
    ThrowEnemyComponent throwEnemyComponent;

    protected override void OnCreate()
    {
        base.OnCreate();
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        physicsStep = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<StepPhysicsWorld>();
        physicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();
        _endFramePhysicsSystem = World.GetOrCreateSystem<EndFramePhysicsSystem>();
    }
    protected override void OnUpdate()
    {

        playerColliderEntity = GetSingletonEntity<EnemyThrowTriggerTag>();
        if(playerColliderEntity == Entity.Null)
        {
          //  Debug.Log("No trigger Component");
        }
        throwEnemyComponent = manager.GetComponentData<ThrowEnemyComponent>(playerColliderEntity);


        //if (throwEnemyComponent.canThrowEnemyAwayFromPlayer)
        //{
        //   // Debug.Log("Entered to clause where canThrowENemy Become true");
        //    collidedWithPlayer = new NativeArray<bool>(1, Allocator.TempJob);
        //    enemy = new NativeArray<Entity>(1, Allocator.TempJob);
        //    playerColliderGroup = GetComponentDataFromEntity<EnemyThrowTriggerTag>(true);
        //    enemies = GetComponentDataFromEntity<EnemyTag>(true);

        //    EnemyPlayerTriggerJob addForceToEnemiesInPlayerTriggerJob = new EnemyPlayerTriggerJob { playerColliderGroup = playerColliderGroup, enemyGroup = enemies, hasCollided = collidedWithPlayer, enemy = enemy };
        //    Dependency = addForceToEnemiesInPlayerTriggerJob.Schedule(physicsStep.Simulation, Dependency);
        //    Dependency.Complete();
        //    if (addForceToEnemiesInPlayerTriggerJob.hasCollided[0] == true)
        //    {
        //       // Debug.Log("Enemy Collide with player Trigger");
        //        var enemyComponent = manager.GetComponentData<EnemyComponent>(addForceToEnemiesInPlayerTriggerJob.enemy[0]);
        //        enemyComponent.throwEnemy = true;
        //        //activate enemy throw trigger sphere
        //        manager.SetComponentData<EnemyComponent>(addForceToEnemiesInPlayerTriggerJob.enemy[0], enemyComponent);

           
        //    }
        //    collidedWithPlayer.Dispose();
        //    enemy.Dispose();

        //}

        if (throwEnemyComponent.canThrowEnemyAwayFromPlayer)
        {
            Translation playerTranslation = manager.GetComponentData<Translation>(playerColliderEntity);
            enemiesWithinRadius = new NativeList<Entity>(Allocator.TempJob);
            EntityQuery query=GetEntityQuery(ComponentType.ReadOnly<Translation>(),ComponentType.ReadOnly<EnemyTag>());
            FindEntitiesWithinRadiusJob findEntitiesWithinRadiusJob = new FindEntitiesWithinRadiusJob {entities=enemiesWithinRadius, radius=throwEnemyComponent.radius, startEntityTranslation= playerTranslation };
            findEntitiesWithinRadiusJob.Schedule(query).Complete();
          
            for (int i = 0; i < findEntitiesWithinRadiusJob.entities.Length; i++)
            {
                
                var enemyComponent = manager.GetComponentData<EnemyComponent>(findEntitiesWithinRadiusJob.entities[i]);
                if(enemyComponent.throwEnemy)
                {
                    continue;
                }
                enemyComponent.throwEnemy = true;
                manager.SetComponentData<EnemyComponent>(findEntitiesWithinRadiusJob.entities[i], enemyComponent);
            }
            enemiesWithinRadius.Dispose();
        }
    }


}
public partial struct EnemyPlayerTriggerJob : ITriggerEventsJob
{
    [Unity.Collections.ReadOnly] public ComponentDataFromEntity<EnemyTag> enemyGroup;
    [Unity.Collections.ReadOnly] public ComponentDataFromEntity<EnemyThrowTriggerTag> playerColliderGroup;



    public NativeArray<bool> hasCollided;
    public NativeArray<Entity> enemy;
    bool entityAIsPlayerCollider, entityBIsPlayerCollider, entityAIsEnemy, entityBIsEnemy;
    public void Execute(TriggerEvent triggerEvent)
    {
        entityAIsEnemy = false;
        entityBIsEnemy = false;
        entityAIsPlayerCollider = false;
        entityBIsPlayerCollider = false;
        Entity entityA = triggerEvent.EntityA;
        Entity entityB = triggerEvent.EntityB;

        entityAIsEnemy = enemyGroup.HasComponent(entityA);

        entityBIsEnemy = enemyGroup.HasComponent(entityB);

        entityAIsPlayerCollider = playerColliderGroup.HasComponent(entityA);
        entityBIsPlayerCollider = playerColliderGroup.HasComponent(entityB);

        if(entityAIsPlayerCollider==false && entityBIsPlayerCollider==false)
        {
           // Debug.Log("player trigger not found");
        }
        //if (entityAIsPlayerCollider || entityBIsPlayerCollider)
        //{
            
            if (entityAIsEnemy || entityBIsEnemy)
            {
                
                if (entityAIsEnemy)
                {
                    enemy[0] = triggerEvent.EntityA;


                }
                else
                {
                    enemy[0] = triggerEvent.EntityB;

                }
                hasCollided[0] = true;
            }
        //}




    }
}


public partial struct FindEntitiesWithinRadiusJob: IJobEntity
{
    public Translation startEntityTranslation;
    public NativeList<Entity> entities;
    public float radius;
    void Execute(in Translation translation,in Entity entity)
    {
        if(math.abs(math.distance(startEntityTranslation.Value,translation.Value))<= radius)
        {
            entities.Add(entity);
        }
    }
}
//public partial struct enableForceToEnemies : IJobEntity
//{
//    public NativeList<EnemyComponent> Enemies;
 
   

//    public void Execute()
//    {
//        for(int i=0;i<Enemies.Length;i++)
//        {

//        }
//    }
//}