using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

using Unity.Transforms;
using Unity.Mathematics;

[UpdateAfter(typeof(TransformSystemGroup))]
public class FollowEntity : MonoBehaviour
{
    public  Entity entityToFollow { get; private set; }
    public  EntityManager manager { get; private set; }
  

    private LocalToWorld translation;
    private Rotation rotation;
    private bool startFollowing;

    private void Awake()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }
    private void Start()
    {

        startFollowing = false;
    }
    private void Update()
    {
        
        if(startFollowing)
        {
            if(entityToFollow!= Entity.Null)
            {
            
                
                translation = manager.GetComponentData<LocalToWorld>(entityToFollow);
                rotation = manager.GetComponentData<Rotation>(entityToFollow);
                Vector3 temp = Vector3.zero;
                temp.x= Mathf.Lerp(gameObject.transform.position.x, translation.Position.x, Time.deltaTime*60);
                temp.y= Mathf.Lerp(gameObject.transform.position.y, translation.Position.y, Time.deltaTime* 60);
                temp.z= Mathf.Lerp(gameObject.transform.position.z, translation.Position.z, Time.deltaTime* 60);
                gameObject.transform.position=temp;
                float4 rotValue = new float4(translation.Rotation.value.x, translation.Rotation.value.y, translation.Rotation.value.z, translation.Rotation.value.w);
                
                gameObject.transform.rotation =Quaternion.Slerp(new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w),new Quaternion(rotValue.x,rotValue.y,rotValue.z,rotValue.w),Time.deltaTime*60);
            }
        }
       
        

    }
    
    public void setEntityToFollow(Entity entity)
    {
        entityToFollow=entity;
        startFollowing = true;
    }
    
}
