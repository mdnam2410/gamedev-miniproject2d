using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyed : StateMachineBehaviour
{
    public float normalizedTimeDestroyed;
    public float normalizedTimePlaySound;
    private Bullet bullet;
    private bool destroyed;
    private bool played;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.bullet = animator.gameObject.GetComponent<Bullet>();
        this.destroyed = false;
        this.played = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > this.normalizedTimePlaySound && !this.played)
        {
            //this.bullet.PlayExplodingSound();
            this.played = true;
        }
        if (stateInfo.normalizedTime > this.normalizedTimeDestroyed && !this.destroyed)
        {
            this.bullet.gameObject.transform.position = this.bullet.offsetToFirePos + this.bullet.firePos.position;
            this.bullet.gameObject.SetActive(false);
            this.bullet.cld.enabled = true;
            this.bullet.rbd.bodyType = RigidbodyType2D.Dynamic;
            this.destroyed = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
