using UnityEngine;

/// <summary>
/// A class that defines the unique behaviors of the black hole
/// </summary>
public class BlackHoleController : MonoBehaviour
{

    /// <summary>
    /// Constant that scales the effect of gravity
    /// </summary>
    public float massCoefficient;

    /// <summary>
    /// A reference to the GameController that is running the level
    /// </summary>
    public GameController gameController;

    /// <summary>
    /// Returns the gravitational force on a rocket at rocketPosition
    /// </summary>
    /// <param name="rocketPosition">position of the rocket</param>
    /// <returns>standard gravitational force at rocketPosition</returns>
    public Vector3 GetGravitationalForce(Vector3 rocketPosition)
    {
        Vector3 blackHolePosition = this.transform.position;
        Vector3 r = blackHolePosition - rocketPosition;
        Debug.DrawLine(blackHolePosition, rocketPosition);
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
