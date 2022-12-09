using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;

public partial class FollowParentEntitySystem : SystemBase
{

    EntityManager entityManager;
    protected override void OnCreate()
    {
        
        base.OnCreate();
       

    }
    protected override void OnUpdate()
    {
        FollowParentEntityJob job = new FollowParentEntityJob {manager= World.DefaultGameObjectInjectionWorld.EntityManager };
        job.Run();
    
    }
}



public partial struct FollowParentEntityJob : IJobEntity
{

    [ReadOnly]
    public EntityManager manager;

    public LocalToWorld parentTranslation;
    public Parent parent;
    void Execute(Entity child, ref Translation translation, in FollowParentComponent followParentTag)
    {

        if (followParentTag.parent != Entity.Null)
        {

            parentTranslation = manager.GetComponentData<LocalToWorld>(followParentTag.parent);
            translation.Value = new float3(followParentTag.parentTranslation.Value.x, followParentTag.parentTranslation.Value.y, followParentTag.parentTranslation.Value.z);


        }

    }
}
