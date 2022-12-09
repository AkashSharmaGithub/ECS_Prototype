using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
[GenerateAuthoringComponent]
public struct ThrowEnemyComponent : IComponentData
{
    public float force;
    public bool canThrowEnemyAwayFromPlayer;
    public float radius;
}
