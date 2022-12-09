using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
[GenerateAuthoringComponent]
public struct FollowParentComponent : IComponentData
{
    public Entity parent;
    public Translation parentTranslation;
}
