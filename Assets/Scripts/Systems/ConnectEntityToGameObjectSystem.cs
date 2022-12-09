using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
[UpdateAfter(typeof(EndInitializationEntityCommandBufferSystem))]
public  partial class ConnectEntityToGameObjectSystem : SystemBase
{
    
    protected override void OnUpdate()
    {
       
    }
    protected override void OnStartRunning()
    {
        ConnectEntityToGameObjectJob connectEntityToGameObjectJob = new ConnectEntityToGameObjectJob();
        connectEntityToGameObjectJob.Run();
    }
    

}
public partial struct ConnectEntityToGameObjectJob : IJobEntity
{

    void Execute(GameObjectToConnectComponent gameObjectToConnectComponent, in Entity entity)
    {
        if (gameObjectToConnectComponent.GameObjectToConnect.TryGetComponent<FollowEntity>(out FollowEntity follow))
        {
            follow.setEntityToFollow(entity);
        }
    }
}
