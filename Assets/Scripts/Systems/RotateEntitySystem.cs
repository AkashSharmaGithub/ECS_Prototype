using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
public partial class RotateEntitySystem : SystemBase
{
    public float deltaTime;
    protected override void OnCreate()
    {
        base.OnCreate();
        
    }
    protected override void OnUpdate()
    {
        deltaTime = Time.DeltaTime;
        RotateEntityJob rotateEntityJob = new RotateEntityJob { delta = deltaTime };
        rotateEntityJob.ScheduleParallel().Complete();
    }

    
}

public partial struct RotateEntityJob : IJobEntity
{
    public float delta;
    void Execute(ref Rotation rotation, in RotationComponent rotationData)
    {


        float3 rotationValue = math.normalize(rotationData.direction) * rotationData.RotationSpeed * delta;
        rotation.Value = math.mul(rotation.Value, quaternion.RotateX(math.radians(rotationValue.x)));
        rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(rotationValue.y)));
        rotation.Value = math.mul(rotation.Value, quaternion.RotateZ(math.radians(rotationValue.z)));




    }
}

