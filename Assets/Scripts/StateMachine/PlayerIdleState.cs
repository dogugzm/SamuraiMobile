using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {


    }

    public override void CheckSwitchState()
    {
        if (ctx.IsMovementPressed && ctx.IsRunPressed)
        {
            SwitchState(factory.Run());
        }
        else if (ctx.IsMovementPressed)
        {
            SwitchState(factory.Walk()); 
        }
    }

    public override void EnterState()
    {
        ctx.Animator.SetBool(ctx.IsWalkingHash, false);
        ctx.Animator.SetBool(ctx.IsRunningHash, false);

        ctx.CurrentMovement = new Vector3(0, ctx.CurrentMovementY, 0);
        ctx.AppliedMovement = new Vector3(0, ctx.AppliedMovementY, 0);

       


    }

    public override void ExitState()
    {
       
    }

    public override void InitializeSubState()
    {
       
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }
}
