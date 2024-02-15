using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{

    private TextMesh text;

    private void Awake()
    {
        text = transform.GetChild(0).GetComponent<TextMesh>();
    }

    public void Init(Vector3 value)
    {
        text.text = $"{value.x} , {value.z}";
    }
}
