using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

public partial class MoveEntityConstantlySystem : SystemBase
{

    protected override void OnUpdate()
    {
        MoveEntityConstantlyJob moveEntityConstantlyJob = new MoveEntityConstantlyJob();
        moveEntityConstantlyJob.Schedule().Complete();
    }
}
public partial struct MoveEntityConstantlyJob : IJobEntity
{
    void Execute(ref PhysicsVelocity velocity, in MovementDataComponent movementData)
    {
        velocity.Linear = math.normalize(movementData.direction) * movementData.speed;
    }
}