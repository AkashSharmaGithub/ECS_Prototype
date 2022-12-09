using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ConnectEntityWithGameObject : MonoBehaviour, IConvertGameObjectToEntity
{
    public GameObject visual;
    private EntityManager manager;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        if (visual.TryGetComponent<FollowEntity>(out FollowEntity follower))
        {
            follower.setEntityToFollow(entity);
   
        }

    }
    private void Start()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
    }




}