using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
[GenerateAuthoringComponent]
public struct PlayerData : IComponentData
{
    
    public bool moving;
    public bool isInvinisible;
    public bool startTimeToResetInvinsibility;
    public float InvinsiblityTime;
    public int health;
    public bool isPlayerDead;
   
 

}
