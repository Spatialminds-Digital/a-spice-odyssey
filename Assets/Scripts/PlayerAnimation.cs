using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Animator animator;

    int running=Animator.StringToHash("running");
    
    void Update()
    {
        if(!animator || !playerMovement) return;

        animator.SetBool(running, Math.Abs(playerMovement.currentVelocity) > 0);
    }
}
