using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class FollowGameObjectSystem : SystemBase
{

    protected override void OnUpdate()
    {
       
        FollowGameObjectJob followGameObjectJob = new FollowGameObjectJob();
        followGameObjectJob.ScheduleParallel().Complete();
    }
}
public partial struct FollowGameObjectJob : IJobEntity
{
    public float3 position;

    void Execute(ref Translation translation, in FollowGameObjectComponent followGameObjectComponent)
    {



        translation.Value = followGameObjectComponent.position;



    }
}