using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;
using Unity.Mathematics;
[UpdateAfter(typeof(ProcessInputSystem))]
public partial class MovementSystem : SystemBase
{
    private EntityQuery PlayerQuery,playerPhysicsQuery;
    protected override void OnCreate()
    {
        base.OnCreate();
        PlayerQuery = GetEntityQuery(ComponentType.ReadWrite<Translation>(), ComponentType.ReadOnly<MovementData>(), ComponentType.ReadOnly<InputComponentData>(), ComponentType.ReadOnly<PlayerTag>());
     
       

    }
    protected override void OnUpdate()
    {
        PlayerQuery = GetEntityQuery(ComponentType.ReadWrite<Translation>(), ComponentType.ReadWrite<PlayerData>(), ComponentType.ReadOnly<MovementData>(), ComponentType.ReadOnly<InputComponentData>(), ComponentType.ReadOnly<PlayerTag>());
       MoveByPhysicsJob moveJob = new MoveByPhysicsJob { deltaTime = Time.DeltaTime };
        moveJob.ScheduleParallel().Complete();
       
    }

   
}
public partial struct MoveByPhysicsJob : IJobEntity
{
    public float deltaTime;

    void Execute(ref Rotation rotation, ref PhysicsVelocity velocity, ref PlayerData playerData, in MovementData movementData, in InputComponentData inputComponentData)
    {

        if (!inputComponentData.direction.Equals(float3.zero))
        {
            playerData.moving = true;
            velocity.Linear = inputComponentData.direction * movementData.MoveSpeed;

            // velocity.Angular = inputComponentData.direction * movementData.RotationSpeed;
            Quaternion targetRotation = Quaternion.LookRotation(inputComponentData.direction, math.up());
            rotation.Value = math.slerp(rotation.Value, targetRotation, movementData.RotationSpeed);
        }
        else

        {
            playerData.moving = false;
            velocity.Linear = float3.zero;
        }
    }
}