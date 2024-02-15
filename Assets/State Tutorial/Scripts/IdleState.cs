using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace LearnAI.FSM
{
    
    public class IdleState : State
    {
        public IdleState(GameObject _npc, NavMeshAgent _agent, Animator _anim, GameObject _player) : base(_npc,_agent, _anim, _player)
        {
            name = STATE.IDLE;
        }

        public override void Enter()
        {
            animator.SetTrigger("isIdle");
            base.Enter();
        }

        public override void Update()
        {
            if (CanSeePlayer())
            {
                nextState = new Pursue(brain, agent, animator, player);
                stage = EVENT.EXIT;
            }

            if (Random.Range(0,100) < 10)
            {
                nextState = new PatrolState(brain, agent, animator,player);
                stage = EVENT.EXIT;
            }
        }

        public override void Exit()
        {
            animator.ResetTrigger("isIdle");
            base.Exit();
        }
    }
    
    public class PatrolState : State
    {
        private int m_CurrentIndex = 0;

        public PatrolState(GameObject _npc, NavMeshAgent _agent, Animator _anim, GameObject _player) : base(_npc,_agent, _anim, _player)
        {
            name = STATE.PATROL;
            agent.speed = 2;
            agent.isStopped = false;
        }

        public override void Enter()
        {
            base.Enter();
            animator.SetTrigger("isWalking");
        }

        public override void Update()
        {
            if (CanSeePlayer())
            {
                nextState = new Pursue(brain, agent, animator, player);
                stage = EVENT.EXIT;
            }

            if (agent.remainingDistance < 1)
            {
                if (m_CurrentIndex >= GameEnvironment.Instance.CheckPoints.Count - 1)
                    m_CurrentIndex = 0;
                else
                    m_CurrentIndex++;


                agent.SetDestination(GameEnvironment.Instance.CheckPoints[m_CurrentIndex].transform.position);
            }
        }

        public override void Exit()
        {
            animator.ResetTrigger("isWalking");
            base.Exit();
        }
    }

    public class Pursue : State
    {
        private int m_CurrentIndex = 0;

        public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, GameObject _player) : base(_npc, _agent, _anim, _player)
        {
            name = STATE.PURSUE;
            agent.speed = 5;
            agent.isStopped = false;
        }

        public override void Enter()
        {
            base.Enter();
            animator.SetTrigger("isRunning");
            agent.SetDestination(player.transform.position);
        }

        public override void Update()
        {
            if (agent.hasPath)
            {
                if (CanAttack())
                {
                    nextState = new Attack(brain, agent, animator, player);
                    stage = EVENT.EXIT;
                }
                
                if (!CanSeePlayer())
                {
                    nextState = new PatrolState(brain, agent, animator, player);
                    stage = EVENT.EXIT;
                }
            }
        }

        public override void Exit()
        {
            animator.ResetTrigger("isRunning");
            base.Exit();
        }
    }

    public class Attack : State
    {
        AudioSource shootSound;
        float rotationSpeed = 5f;

        public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, GameObject _player) : base(_npc, _agent, _anim, _player)
        {
            name = STATE.ATTACK;
            shootSound = _npc.GetComponent<AudioSource>();
        }

        public override void Enter()
        {
            base.Enter();
            animator.SetTrigger("isShooting");
            agent.isStopped = true;
            shootSound.Play();
        }

        public override void Update()
        {
            Vector3 dir = player.transform.position - brain.transform.position;
            dir.Normalize();
            dir.y = 0f;

            brain.transform.rotation = Quaternion.Slerp(brain.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed);

            if (!CanSeePlayer())
            {
                nextState = new IdleState(brain, agent, animator, player);
                stage = EVENT.EXIT;
            }
        }

        public override void Exit()
        {
            animator.ResetTrigger("isShooting");
            base.Exit();
        }
    }
}

