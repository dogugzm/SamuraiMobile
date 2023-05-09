using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
    }

    public override void CheckSwitchState()
    {
        if (!ctx.IsMovementPressed)
        {
            SwitchState(factory.Idle());
        }
        else if (ctx.IsMovementPressed && ctx.IsRunPressed)
        {
            SwitchState(factory.Run());
        }
    }

    public override void EnterState()
    {
        ctx.Animator.SetBool(ctx.IsWalkingHash, true);
        ctx.Animator.SetBool(ctx.IsRunningHash, false);
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

        ctx.AppliedMovement = new Vector3(ctx.CurrentMovementInput.x,ctx.AppliedMovementY, ctx.CurrentMovementInput.x);

        
    }
}
