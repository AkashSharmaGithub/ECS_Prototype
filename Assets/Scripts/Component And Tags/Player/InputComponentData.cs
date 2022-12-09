using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
[GenerateAuthoringComponent]
public struct InputComponentData :IComponentData
{
    public float horizontalInputData;
    public float verticalInputData;
    
    [HideInInspector]public float3 direction;
    public void setDirection(float3 dir)
    {
        direction = dir;
    }
    public float3 getDirection()
    {
        return direction;
    }
}
