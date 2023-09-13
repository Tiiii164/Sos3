using UnityEngine;

namespace LlamAcademy.FSM
{
    public class ChaseState : EnemyStateBase
    {
        private Transform Target;

      
        public ChaseState(bool needsExitTime, Enemy Enemy, Transform Target) : base(needsExitTime, Enemy) 
        {
            this.Target = Target;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Agent.enabled = true;
            Agent.isStopped = false;
            Animator.Play("Chase Player");
            Agent.speed = 15;
        }

        public override void OnLogic()
        {
            base.OnLogic();
            if (!RequestedExit)
            {
                //nhớ add cái dự đoán chuyển động của clip này dô https://youtu.be/1Jkg8cKLsC0
                Agent.SetDestination(Target.position);
            }
            else if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                // In case that we were requested to exit, we will continue moving to the last known position prior to transitioning out to idle.
                fsm.StateCanExit();
            }
        }
    }
}
