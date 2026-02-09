using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBehaviour : StateMachineBehaviour
{
    // アニメーション開始時に呼ばれる：Start
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 攻撃中アニメーションの移動静止 
        animator.GetComponent<PlayerManager>().moveSpeed = 0.1f;
    }

    // アニメーション中に呼ばれる：Update
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // アニメーションの遷移終了時に実行される
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 攻撃中アニメーションの移動再開
        animator.GetComponent<PlayerManager>().moveSpeed = 2.5f;
        animator.GetComponent<PlayerManager>().IsAttack = false;
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
