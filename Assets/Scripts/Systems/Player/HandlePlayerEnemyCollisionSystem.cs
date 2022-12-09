using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Physics;
//using Unity.VisualScripting;
//using static UnityEditor.Experimental.GraphView.GraphView;
using Unity.Collections;
//using System.Numerics;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
[UpdateAfter(typeof(StepPhysicsWorld))]
public  partial class HandlePlayerEnemyCollisionSystem : SystemBase
{
    public ComponentDataFromEntity<PlayerTag> players;
    public ComponentDataFromEntity<EnemyTag> enemies;

    public StepPhysicsWorld physicsStep;
    public BuildPhysicsWorld physicsWorld;
    private EndFramePhysicsSystem _endFramePhysicsSystem;
    private EntityManager manager;


    NativeArray<PlayerTag> _players;

    private EntityQuery playerQuery;
    NativeArray<bool> collidedWithPlayer;
    NativeArray<Entity> player,enemy;
    PlayerData playerData;
    EnemyComponent enemyComponent;
    ThrowEnemyComponent  throwEnemyComponent;
    Entity throwEntityColliderEntity;
    protected override void OnCreate()
    {
        base.OnCreate();
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        physicsStep = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<StepPhysicsWorld>();
        physicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();
        _endFramePhysicsSystem = World.GetOrCreateSystem<EndFramePhysicsSystem>();
     
    }
    protected override void OnStartRunning()
    {
        base.OnStartRunning();
        _endFramePhysicsSystem.RegisterPhysicsRuntimeSystemReadWrite();
    }
    
    protected override void OnUpdate()
    {
        collidedWithPlayer = new NativeArray<bool>(1, Allocator.TempJob);
        player = new NativeArray<Entity>(1, Allocator.TempJob);
        enemy = new NativeArray<Entity>(1, Allocator.TempJob);
        throwEntityColliderEntity = GetSingletonEntity<EnemyThrowTriggerTag>();
        throwEnemyComponent = manager.GetComponentData<ThrowEnemyComponent>(throwEntityColliderEntity);

        players = GetComponentDataFromEntity<PlayerTag>(true);
        enemies = GetComponentDataFromEntity<EnemyTag>(true);
        HandlePlayerEnemyCollisionJob handlePlayerEnemyCollisionJob = new HandlePlayerEnemyCollisionJob { PlayerDataGroup = players, enemyGroup = enemies, hasCollided = collidedWithPlayer, player = player, enemy = enemy };
        Dependency = handlePlayerEnemyCollisionJob.Schedule(physicsStep.Simulation, Dependency);
        Dependency.Complete();



        if (handlePlayerEnemyCollisionJob.hasCollided[0] == true )
        {
            playerData = manager.GetComponentData<PlayerData>(handlePlayerEnemyCollisionJob.player[0]);
            if(playerData.isInvinisible)
            {
                collidedWithPlayer.Dispose();
                player.Dispose();
                enemy.Dispose();
                return;
            }
            enemyComponent = manager.GetComponentData<EnemyComponent>(handlePlayerEnemyCollisionJob.enemy[0]);
            //reduce health
            playerData.health -= enemyComponent.damage;
            //check health is <= 0 or not 
            if (playerData.health <= 0)
            {
                playerData.isPlayerDead = true;
            }
            //make player invinsible
            playerData.isInvinisible = true;
            playerData.startTimeToResetInvinsibility = true;
            manager.SetComponentData<PlayerData>(handlePlayerEnemyCollisionJob.player[0], playerData);
         
         
            
           //set enemy component
            enemyComponent.throwEnemy = true;
            manager.SetComponentData<EnemyComponent>(handlePlayerEnemyCollisionJob.enemy[0], enemyComponent);

            //set sphere trigger componenet
            throwEnemyComponent.canThrowEnemyAwayFromPlayer = true;
            manager.SetComponentData<ThrowEnemyComponent>(throwEntityColliderEntity, throwEnemyComponent);

            

        }
        collidedWithPlayer.Dispose();
        player.Dispose();
        enemy.Dispose();

    }


}
public partial struct HandlePlayerEnemyCollisionJob : ITriggerEventsJob
{
    [Unity.Collections.ReadOnly] public ComponentDataFromEntity<PlayerTag> PlayerDataGroup;
    [Unity.Collections.ReadOnly] public ComponentDataFromEntity<EnemyTag> enemyGroup;
   public NativeArray<bool> hasCollided;
    public NativeArray<Entity> player,enemy;
    bool entityAIsPlayer, entityBIsPlayer, entityAIsEnemy, entityBIsEnemy;



    public void Execute(TriggerEvent triggerEvent)
    {
        entityAIsEnemy = false;
        entityBIsEnemy = false;
        entityAIsPlayer = false;
        entityBIsPlayer = false;
        Entity entityA = triggerEvent.EntityA;
        Entity entityB = triggerEvent.EntityB;
    
        entityAIsEnemy = enemyGroup.HasComponent(entityA);
       
        entityBIsEnemy = enemyGroup.HasComponent(entityB);

        entityAIsPlayer = PlayerDataGroup.HasComponent(entityA);
        entityBIsPlayer = PlayerDataGroup.HasComponent(entityB);


        if (entityAIsPlayer || entityBIsPlayer)
        {
            
            if (entityAIsEnemy || entityBIsEnemy)
            {

           
                if (entityAIsEnemy)
                {
                    enemy[0]=triggerEvent.EntityA;
                    player[0]=triggerEvent.EntityB;

                }
                else
                {
                    enemy[0] = triggerEvent.EntityB;
                    player[0] = triggerEvent.EntityA;
                }
                hasCollided[0] = true;
            }
        }

     




    }
}


