using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public GameController gameController;
    public GameObject flag;

    private void Start()
    {
        // sets up a random fast speed with a random parity
        GetComponent<RotateConstantScript>().rotateSpeed = Random.Range(30f / 3, 40f / 3) * 3 * (Random.value > 0.5f ? -1 : 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !collision.gameObject.GetComponent<RocketController>().isSafe)
        {
            gameController.RocketLandOn(gameObject);
        }
    }

    internal void RaiseFlag()
    {
        Debug.Log("Raised flag on " + this);
    }

    internal void LowerFlag()
    {
        Debug.Log("Lowered flag on " + this);
    }
}
