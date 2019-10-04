using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{

    public float massCoefficient;

    public Vector3 GetGravitationalForce(GameObject rocket)
    {
        var r = this.transform.position - rocket.transform.position;
        Debug.DrawLine(this.transform.position, this.transform.position - r);
        r.Normalize();
        r.Scale(massCoefficient / r.sqrMagnitude * Vector3.one);
        return r;
    }
}
