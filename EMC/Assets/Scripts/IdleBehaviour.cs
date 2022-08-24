using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{

	[SerializeField] private float timeUntilBored;
	[SerializeField] private int numberOfIdleAnimations;

	private bool isBored;
	private float idleTime;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{ 
		ResetIdle(animator);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!isBored)
		{
			idleTime += Time.deltaTime;

			if(idleTime > timeUntilBored)
			{
				isBored = true;
				int idleAnimation = Random.Range(1, numberOfIdleAnimations + 1);

				animator.SetFloat("IdleAnimation", idleAnimation);
			}
		}

		else if (stateInfo.normalizedTime % 1 > 0.98)
		{
			ResetIdle(animator);
		}
		
	}

	private void ResetIdle(Animator animator)
	{
		isBored = false;
		idleTime = 0;

		animator.SetFloat("IdleAnimation", 0);
	}

}
