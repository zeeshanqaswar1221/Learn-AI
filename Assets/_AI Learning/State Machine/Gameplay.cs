using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Gameplay : MonoBehaviour
{
    public Transform followObject;

    public float walkSpeed = 5f;
    public float rotateSpeed = 5f;

    public float stopLimit = 3f;

    public enum State{Idle, Rotate, Walk}
    public State currentState;

    private Vector3 direction;

    private float distance;
    private float angleBetween;

    private void Awake() {
        currentState = State.Idle;
    }

    private void Update() {
        
        direction = followObject.position - transform.position;
        direction = direction.normalized;

        distance = Vector3.Distance(followObject.position, transform.position);
        angleBetween = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

        switch(currentState){

            case State.Rotate:
            RotateState();
            break;

            case State.Walk:
            WalkState();
            break;

            case State.Idle:
            IdleState();
            break;
        }
    }

    private void IdleState(){

        if (Mathf.Abs(angleBetween) > 1 || distance > stopLimit)
        {
            currentState = State.Rotate;
        }
    }

    private void RotateState(){

        if (Mathf.Abs(angleBetween) < 1)
        {
            currentState = State.Walk;
        }

        float factor = -1f * CrossProduct(direction, transform.forward).y;
        transform.Rotate(factor * Vector3.up * rotateSpeed * Time.deltaTime);

    }

    private void WalkState()
    {
        if (distance <= stopLimit)
        {
            currentState = State.Idle;
            return;
        }
        else if( Mathf.Abs(angleBetween) > 1){
            currentState = State.Rotate;
            return;
        }

        transform.Translate(transform.forward * walkSpeed * Time.deltaTime, Space.World);
    }

    

    Vector3 CrossProduct(Vector3 v, Vector3 w){
        Vector3 temp = new Vector3();
        temp.x = v.y * w.z - v.z * w.y;
        temp.y = v.z * w.x - v.x * w.z;
        temp.z = v.x * w.y - v.y * w.x;

        return temp;
    }
}
