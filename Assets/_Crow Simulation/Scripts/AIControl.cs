using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour {

    public GameObject goal;
    NavMeshAgent agent;
    private float speed;

    void Start() {

        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(goal.transform.position);
    }


    void Update() {
    }
}