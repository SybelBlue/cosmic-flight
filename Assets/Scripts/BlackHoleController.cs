using UnityEngine;

public class BlackHoleController : MonoBehaviour
{

    public float massCoefficient;

    public GameController gameController;

    public Vector3 GetGravitationalForce(GameObject rocket)
    {
        var r = this.transform.position - rocket.transform.position;
        Debug.DrawLine(this.transform.position, this.transform.position - r);
        r.Normalize();
        r.Scale(massCoefficient / r.sqrMagnitude * Vector3.one);
        return r;
    }

    // Destroy everything that enters the trigger
    void OnTriggerEnter2D(Collider2D collider)
    {
        gameController.ObjectDied(collider.gameObject);
    }
}
