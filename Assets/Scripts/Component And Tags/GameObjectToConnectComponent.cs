using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;


[GenerateAuthoringComponent]
public class GameObjectToConnectComponent : IComponentData
{
    public GameObject GameObjectToConnect;
}
