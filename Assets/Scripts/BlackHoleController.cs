using UnityEngine;

public class BlackHoleController : MonoBehaviour
{

    public float massCoefficient;

    public GameController gameController;

    /// <summary>
    /// Returns the gravitational force on a rocket at rocketPosition
    /// </summary>
    /// <param name="rocketPosition">position of the rocket</param>
    /// <returns>standard gravitational force at rocketPosition</returns>
    public Vector3 GetGravitationalForce(Vector3 rocketPosition)
    {
        var r = this.transform.position - rocketPosition;
        Debug.DrawLine(this.transform.position, this.transform.position - r);
        r.Normalize();
        r.Scale(massCoefficient / r.sqrMagnitude * Vector3.one);
        return r;
    }

    /// <summary>
    /// Notifies gameController of everything that enters event
    /// horizon
    /// </summary>
    /// <param name="collider">object that entered event horizon</param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        gameController.ObjectHitBlackHole(collider.gameObject);
    }
}
