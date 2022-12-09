using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
[GenerateAuthoringComponent]
public struct RotationComponent : IComponentData
{
    public float3 RotationSpeed;
    [Range(0f,1f)]
    public float3 direction;
}
