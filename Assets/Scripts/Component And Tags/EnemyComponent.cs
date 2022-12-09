using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
[GenerateAuthoringComponent]
public struct EnemyComponent : IComponentData
{
    public int damage;
    public bool throwEnemy;

}
