
public abstract class PlayerBaseState 
{
    protected bool isRootState = false;
    protected PlayerStateMachine ctx;
    protected PlayerStateFactory factory;
    protected PlayerBaseState currentSubState;
    protected PlayerBaseState currentSuperState;


    protected PlayerBaseState(PlayerStateMachine ctx, PlayerStateFactory factory)
    {
        this.ctx = ctx;
        this.factory = factory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    public abstract void InitializeSubState();


    public void UpdateStates() 
    { 
        UpdateState();
        if (currentSubState != null)
        {
            currentSubState.UpdateStates();
        }

    }
    protected void SwitchState(PlayerBaseState newState) 
    {
        ExitState();
        newState.EnterState();
        if (!isRootState)
        {
            ctx.CurrentState = newState;

        }
        else if (currentSuperState!=null)
        {
            currentSuperState.SetSubState(newState);
        }

    
    }
    protected void SetSuperState(PlayerBaseState newSuperState) 
    {
        currentSuperState = newSuperState;    
    
    }
    protected void SetSubState(PlayerBaseState newSubState) 
    {
        currentSubState = newSubState;
        newSubState.SetSuperState(this);
    
    
    }



}
