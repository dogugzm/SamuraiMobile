using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        isRootState = true;
        InitializeSubState();
    }

    public override void CheckSwitchState()
    {
        
    }

    public override void EnterState()
    {
        Debug.Log("Hello grounded");

        ctx.CurrentMovementY = -0.05f;
        ctx.AppliedMovementY = -0.05f;

    }

    public override void ExitState()
    {
        
    }

    public override void InitializeSubState()
    {
        if (!ctx.IsMovementPressed && !ctx.IsRunPressed)
        {
            SetSubState(factory.Idle());
        }
        else if (ctx.IsMovementPressed && !ctx.IsRunPressed)
        {
            SetSubState(factory.Walk());
        }
        else
        {
            SetSubState(factory.Run());
        }
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }
}
