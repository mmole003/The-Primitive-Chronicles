using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState activeState;
    

    public void Initialise()
    {
        ChangeState(new PatrolState());
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(activeState != null)
        {
            activeState.Perform();
        }
    }

    public void ChangeState(BaseState newState)
    {
        //check activeState is not null
        if (activeState != null)
        {
            activeState.Exit();
        }
        //change to a new state
        activeState = newState;

        //fail-safe null check for newState
        if (newState != null)
        {
            //setup new state.
            activeState.stateMachine = this;
            activeState.enemy = GetComponent<Enemy>();
            activeState.Enter();

        }
    }
}
