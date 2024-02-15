using UnityEditor.ProjectWindowCallback;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

namespace LearnAI.FSM
{
    public class State
    {
        public enum STATE
        {
            IDLE,
            PATROL,
            PURSUE,
            ATTACK,
            SLEEP
        };
    
        public enum EVENT
        {
            ENTER,
            UPDATE,
            EXIT
        };

        protected STATE name;
    
        protected EVENT stage;
        protected NavMeshAgent agent;
    
        // References
        protected GameObject brain;
        protected Animator animator;
        protected GameObject player;
        protected State nextState;
    
        // Properties
        protected float visualAngle = 30f;
        protected float shootDist = 7f;
        protected float visualDist = 10f;
    
        public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, GameObject _player)
        {
            brain = _npc;
            agent = _agent;
            animator = _anim;
            player = _player;
    
            stage = EVENT.ENTER;
        }
    
        public virtual void Enter()
        {
            stage = EVENT.UPDATE;
        }
    
        public virtual void Update()
        {
            stage = EVENT.UPDATE;
        }
    
        public virtual void Exit()
        {
            stage = EVENT.EXIT;
        }
    
        public State Process()
        {
            if (stage == EVENT.ENTER) Enter();
            if(stage == EVENT.UPDATE) Update();
            if (stage == EVENT.EXIT)
            {
                Exit();
                return nextState;
            }
    
            return this;
        }
    
        public bool CanSeePlayer()
        {
            var direction = player.transform.position - brain.transform.position;
            var angle = Vector3.Angle(brain.transform.position, direction);


            if (direction.magnitude < visualDist)
            {
                if (angle <= visualAngle)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanAttack()
        {
            var direction = player.transform.position - brain.transform.position;

            if (direction.magnitude < shootDist)
            {
                return true;
            }

            return false;
        }


    }
}

