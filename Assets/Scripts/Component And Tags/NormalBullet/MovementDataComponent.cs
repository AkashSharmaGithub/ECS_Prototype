
using Unity.Entities;
using Unity.Mathematics;
[GenerateAuthoringComponent]
public class MovementDataComponent : IComponentData
{
    public float speed;
    public float3 direction;
}
