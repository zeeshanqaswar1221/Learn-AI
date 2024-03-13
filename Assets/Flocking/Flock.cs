using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public float speed;
    private bool turning = false;

    private void Start()
    {
        speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
    }

    private void Update()
    {
        Bounds bounds = new Bounds(FlockManager.FM.transform.position, FlockManager.FM.swimLimits * 2);
        turning = !bounds.Contains(transform.position);

        if (turning)
        {
            Vector3 direction = FlockManager.FM.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FlockManager.FM.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);

            if (Random.Range(0, 100) < 10)
                ApplyRules();
        }

        transform.Translate(0f, 0f, speed * Time.deltaTime);
    }

    private void ApplyRules()
    {
        GameObject[] go = FlockManager.FM.spawnedFishes;

        Vector3 averageCenter = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float averageSpeed = 0.01f;
        float dist;
        int groupSize = 0;

        foreach (var item in go)
        {
            if(item != this.gameObject)
            {
                dist = Vector3.Distance(item.transform.position, transform.position);
                if(dist <= FlockManager.FM.neighbourDist)
                {
                    averageCenter += item.transform.position;
                    groupSize++;

                    if(dist < 1.0f) //avoid
                    {
                        vavoid += (transform.position - item.transform.position);
                    }

                    averageSpeed += item.GetComponent<Flock>().speed;
                }
            }
        }

        if(groupSize > 0)
        {
            averageCenter = averageCenter / groupSize + (FlockManager.FM.goalPoint - transform.position);
            averageSpeed = averageSpeed / groupSize;

            averageSpeed = Mathf.Clamp(averageSpeed, 0f, FlockManager.FM.maxSpeed);

            Vector3 direction = (averageCenter + vavoid) - transform.position;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FlockManager.FM.rotationSpeed * Time.deltaTime);
            }
        }

    }

}
