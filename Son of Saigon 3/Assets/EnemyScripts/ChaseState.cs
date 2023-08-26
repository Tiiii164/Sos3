using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ChaseState : StateMachineBehaviour
{
    //[SerializeField] GameObject player;
    [SerializeField] float chaseSpeed;
    //AudioClip ChaseThemeAudioClip;
    NavMeshAgent navMeshAgent;
    Transform player;
    //AudioSource audioSource;
    //AudioClip audioClip;
    CodeMonkey.HealthSystemCM.EnemyNavMesh enemyNavMesh;
    //EnemyNavMesh enemyNavMesh;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyNavMesh = animator.GetComponent<CodeMonkey.HealthSystemCM.EnemyNavMesh>();
        //audioClip = animator.GetComponent<AudioClip>();
        //audioSource = animator.GetComponent<AudioSource>();
        navMeshAgent = animator.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = chaseSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //chase sound
        //audioSource.PlayOneShot(enemyNavMesh.ChaseThemeAudioClip);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance > 15)
        {
            animator.SetBool("Chasing", false);
        }
        if (distance < 2)
        {
            animator.SetBool("Attacking", true);
        }
        navMeshAgent.SetDestination(player.position);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent.SetDestination(animator.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
         //Implement code that processes and affects root motion
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
         //Implement code that sets up animation IK (inverse kinematics)
    }
}
