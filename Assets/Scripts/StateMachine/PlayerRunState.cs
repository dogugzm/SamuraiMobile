using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
    }

    public override void CheckSwitchState()
    {
        if (!ctx.IsMovementPressed)
        {
            SwitchState(factory.Idle());

        }
        else if (ctx.IsMovementPressed && !ctx.IsRunPressed )  
        {
            SwitchState(factory.Walk());
        }
       
    }

    public override void EnterState()
    {
        ctx.Animator.SetBool(ctx.IsRunningHash, true);
        ctx.Animator.SetBool(ctx.IsWalkingHash, true);
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

        ctx.AppliedMovement = new Vector3(ctx.CurrentMovementInput.x * ctx.RunMultiplier, ctx.AppliedMovementY, ctx.CurrentMovementInput.x * ctx.RunMultiplier);

    }
}
