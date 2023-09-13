using FSM;
using System;
using UnityEngine;
using UnityEngine.Pool;

namespace LlamAcademy.FSM
{
    public class AttackState : EnemyStateBase
    {
        private Smash Prefab;
        private ObjectPool<Smash> Pool;

        public AttackState(
            bool needsExitTime,
            Enemy Enemy,
            Smash Prefab,
            Action<State<EnemyState, StateEvent>> onEnter,
            float ExitTime = 1.2f) : base(needsExitTime, Enemy, ExitTime, onEnter)
        {
            this.Prefab = Prefab;
            Pool = new(CreateObject, GetObject, ReleaseObject);
        }

        private Smash CreateObject()
        {
            return GameObject.Instantiate(Prefab);
        }

        private void GetObject(Smash Instance)
        {
            Instance.transform.forward = Enemy.transform.forward;
            Instance.transform.position = Enemy.transform.position + Enemy.transform.forward + Vector3.up * 1.5f;
            Instance.gameObject.SetActive(true);
        }

        private void ReleaseObject(Smash Instance)
        {
            Instance.gameObject.SetActive(false);
        }

        public override void OnEnter()
        {
            Agent.isStopped = true;
            base.OnEnter();
            Animator.Play("Melee Attack");
            Pool.Get();
        }
    }
}
