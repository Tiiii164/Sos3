using FSM;
using System;
using UnityEngine;

namespace LlamAcademy.FSM
{
    public class RushState : EnemyStateBase
    {
        public RushState(
            bool needsExitTime,
            Enemy Enemy,
            Action<State<EnemyState, StateEvent>> onEnter,
            float ExitTime = 3f) : base(needsExitTime, Enemy, ExitTime, onEnter) { }

        public override void OnEnter()
        {
            Agent.isStopped = true;
            base.OnEnter();
            Animator.Play("Rush Attack");
        }

        public override void OnLogic()
        {
            Agent.Move(2f * Agent.speed * Time.deltaTime * Agent.transform.forward);
            base.OnLogic();
        }
    }
}
