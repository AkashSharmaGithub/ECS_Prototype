using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

public partial class AttackNearestEnemySystem : SystemBase
{
    protected override void OnUpdate()
    {
        
    }

   
}
public partial struct FindNearestEnemyJob : IJobEntity
{
    public Translation unitPosition;
    //divide map into quadrant
    //add entities to quadrant with no isAlreadyAimedTag
    //sort entities
    //add first entity to isAlreadyAttack list
    //

    public NativeArray<Entity> entityToAttack;
    public NativeArray<Translation> entityToAttackPosition;
    public float distance;
    void Execute(in Entity entity, in Translation translation)
    {

        if (distance == 0 || math.distance(unitPosition.Value, translation.Value) < math.distance(unitPosition.Value, entityToAttackPosition[0].Value))
        {
            entityToAttack[0] = entity;
            entityToAttackPosition[0] = translation;


        }

    }
}