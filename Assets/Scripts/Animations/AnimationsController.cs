using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MovementController;

public class AnimationsController : MonoBehaviour
{
    private Animator animator;
    private string currentState;
    //Animation states
    const string PLAYER_IDLE = "Idle";
    const string PLAYER_START_CLIMBING_LADDER = "StartClimbingLadder";
    const string PLAYER_CLIMBING_LADDER = "ClimbingLadder";
    const string PLAYER_HANGING_IDLE1 = "HangingIdle1";
    const string PLAYER_HANGING_IDLE2 = "HangingIdle2";
    //Bool states
    private bool startedClimbingLadder = false;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        
        animator.Play(newState);
        currentState = newState;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && startedClimbingLadder == false && MovementController.movementInstance.isOnLadder)
        {
            ChangeAnimationState(PLAYER_START_CLIMBING_LADDER);
            float startClimbingLadderDelay = animator.GetCurrentAnimatorStateInfo(0).length;
            //StartCoroutine(StartClimbingLadderDelay(startClimbingLadderDelay));
        }
        if (MovementController.movementInstance.isOnLadder && startedClimbingLadder == false)
        {
            if(Input.GetKey(KeyCode.W))
            {
                ChangeAnimationState(PLAYER_CLIMBING_LADDER);
            }
            else
            {
                ChangeAnimationState(PLAYER_HANGING_IDLE1);
            }
        }
        if(MovementController.movementInstance.isOnLadder == false)
        {
            ChangeAnimationState(PLAYER_IDLE);
        }
    }
    private IEnumerator StartClimbingLadderDelay(float delay)
    {
        startedClimbingLadder = true;
        MovementController.movementInstance.canMove = false;
        yield return new WaitForSeconds(delay);
        MovementController.movementInstance.canMove = true;
        startedClimbingLadder = false;
    }
}
