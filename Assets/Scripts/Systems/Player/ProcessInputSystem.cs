using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;

[UpdateAfter(typeof( EndInitializationEntityCommandBufferSystem))]
public partial class ProcessInputSystem : SystemBase
{
    private PlayerMovement movementInput;
    private float horizontalMovement;
    private float verticalMovement;
    protected override void OnCreate()
    {
        base.OnCreate();
        
    }
    protected override void OnDestroy()
    {
        movementInput.PM.Disable();
        base.OnDestroy();
        
    }
    protected override void OnStartRunning()
    {
        base.OnStartRunning();
        movementInput = new PlayerMovement();
        movementInput.PM.Enable();
    }
    protected override void OnUpdate()
    {

        horizontalMovement = movementInput.PM.Horizontal.ReadValue<float>();
        verticalMovement= movementInput.PM.Vertical.ReadValue<float>();
        InputProcessJob processJob = new InputProcessJob
        {
            hInput = horizontalMovement,
            vInput = verticalMovement
        };
        processJob.Schedule().Complete();

    }


}

public partial struct InputProcessJob : IJobEntity
{
    public float hInput;
    public float vInput;

    void Execute(ref InputComponentData inputComponentData)
    {
        inputComponentData.horizontalInputData = hInput;
        inputComponentData.verticalInputData = vInput;
        inputComponentData.direction = new float3(hInput, 0, vInput);
    }
}




