using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
public partial class DisablePlayerInvinsibilitySystem : SystemBase
{
    private Entity player;
    private EntityManager entityManager;
    private PlayerData playerData;
    private float timer;
    protected override void OnCreate()
    {
        base.OnCreate();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

    }
    protected override void OnUpdate()
    {
        player = GetSingletonEntity<PlayerTag>();
        if(player==Entity.Null)
        {
                return;
        }
        playerData = entityManager.GetComponentData<PlayerData>(player);
        if(playerData.startTimeToResetInvinsibility)
        {
            playerData.startTimeToResetInvinsibility = false;
            timer = playerData.InvinsiblityTime;
            entityManager.SetComponentData<PlayerData>(player, playerData);


        }
        if(timer>0)
        {
            timer -= Time.DeltaTime;

        }
        if(timer<0)
        {
            timer = 0;
            playerData.isInvinisible = false;
            ThrowEnemyComponent throwEnemyComponent = entityManager.GetComponentData<ThrowEnemyComponent>(player);
            throwEnemyComponent.canThrowEnemyAwayFromPlayer = false;
            entityManager.SetComponentData<PlayerData>(player, playerData);
            entityManager.SetComponentData<ThrowEnemyComponent>(player, throwEnemyComponent);
        }

    }
}


