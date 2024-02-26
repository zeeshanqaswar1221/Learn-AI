using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace LearnAI.FSM
{
    public class AIBrain : MonoBehaviour
    {
        public GameObject player;
        
        NavMeshAgent agent;
        Animator animator;
        State m_CurrentState;


        private void Awake()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            m_CurrentState = new IdleState(gameObject, agent, animator, player);
        }

        private void Update()
        {
            m_CurrentState = m_CurrentState.Process();
        }

    }
}