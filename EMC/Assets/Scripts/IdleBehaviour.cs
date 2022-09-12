using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{

	[SerializeField] private float timeUntilBored;
	[SerializeField] private int numberOfIdleAnimations;

	private bool isBored;
	private float idleTime;
	private int idleAnimation;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{ 
		ResetIdle();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!isBored)
		{
			idleTime += Time.deltaTime;

			if (idleTime > timeUntilBored && stateInfo.normalizedTime % 1 > 0.02f)
			{
				isBored = true;
				idleAnimation = Random.Range(1, numberOfIdleAnimations + 1);
				idleAnimation = idleAnimation * 2 - 1;
				animator.SetFloat("IdleAnimation", idleAnimation - 1);
			}
		}

		else if (stateInfo.normalizedTime % 1 > 0.98)
		{
			ResetIdle();
		}
			animator.SetFloat("IdleAnimation", idleAnimation, 0.2f, Time.deltaTime);
	}

	private void ResetIdle()
	{
		if (isBored) idleAnimation --;
		isBored = false;
		idleTime = 0;
	}

}
