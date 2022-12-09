using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
public class EntityFollowGameObject : MonoBehaviour
{
    private EntityManager entityManager;
    private FollowGameObjectComponent followGameObjectComponent;
    private Entity entityThatFollowsGameObject;

    private void OnEnable()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    
    }
    private void Update()
    {
       

        if(entityManager.Exists(entityThatFollowsGameObject))
        {
            followGameObjectComponent.position = transform.position;
            entityManager.SetComponentData<FollowGameObjectComponent>(entityThatFollowsGameObject, followGameObjectComponent);
        }
     
    }
    public void setEntityToFollowGameObject(Entity entity)
    {
        entityThatFollowsGameObject=entity;
        followGameObjectComponent= entityManager.GetComponentData<FollowGameObjectComponent>(entityThatFollowsGameObject);


    }
}
