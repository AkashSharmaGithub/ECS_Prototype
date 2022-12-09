using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private FollowEntity _followEntity;
    private PlayerData data;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _followEntity= GetComponent<FollowEntity>();
    }
    void Update()
    {
     
        if(_followEntity.entityToFollow!=null)
        {
            data = _followEntity.manager.GetComponentData<PlayerData>(_followEntity.entityToFollow);
        }    
        _animator.SetBool("isMoving", data.moving);
        
    }
}
