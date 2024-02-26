using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform target;

    Drive targetDrive;
    Vector3 targetPoint;

    public GameObject[] hidingPlaces;

    private void Awake()
    {
        hidingPlaces = GameObject.FindGameObjectsWithTag("hide");
    }

    private void Start()
    {
        targetDrive = target.GetComponent<Drive>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Flee()
    {
        Vector3 direction = transform.position - target.position;
        agent.SetDestination(transform.position + direction);
    }

    private void Pursuit()
    {
        Vector3 dirVec = target.position - transform.position;

        float relativeHeading = Vector3.Angle(transform.forward, transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(transform.forward, transform.TransformVector(dirVec));

        if (toTarget > 90f && relativeHeading < 20 || targetDrive.speed < 0.01f)
        {
            agent.SetDestination(target.position);
            return;
        }

        float point = dirVec.magnitude/ (targetDrive.currentSpeed + agent.speed);
        targetPoint = target.position + (target.forward * point);
        agent.SetDestination(targetPoint);
    }

    private void Evade()
    {
        Vector3 dirVec = -1 * (target.position - transform.position);

        float point = dirVec.magnitude / (targetDrive.currentSpeed + agent.speed);
        print(point);
        targetPoint = transform.position + (dirVec.normalized * point);
        agent.SetDestination(targetPoint);
    }

    Vector3 wanderTarget = Vector3.zero;
    private void Wander()
    {
        float distance = 20f;
        float radius = 10f;
        float jitter = 1f;

        wanderTarget = new Vector3(Random.Range(-1f, 1f) * jitter, 0f, Random.Range(-1f, 1f) * jitter);
        wanderTarget.Normalize();
        wanderTarget *= radius;

        wanderTarget.z += distance;
        Vector3 toLocalSpace = transform.InverseTransformVector(wanderTarget);

        agent.SetDestination(toLocalSpace);

    }

    private void Hide()
    {
        Vector3 hidingSpot = Vector3.zero;
        float dist = Mathf.Infinity;

        foreach (GameObject t in hidingPlaces)
        {
            Vector3 dir = t.transform.position - target.position;
            Vector3 tempPoint = t.transform.position + dir.normalized * 5f;

            if (Vector3.Distance(tempPoint,transform.position)  < dist)
            {
                dist = Vector3.Distance(tempPoint, transform.position);
                hidingSpot = tempPoint;
            }
        }

        agent.SetDestination(hidingSpot);
    }

    private void Update()
    {
        //Flee();
        //Pursuit();
        //Evade();
        //Wander();
        Hide();

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPoint, 0.5f);
    }



}
