using UnityEngine;

public class PlanetController : MonoBehaviour
{

    public GameController gameController;
    public GameObject asteroidToReplace;

    private void Update()
    {
        if (asteroidToReplace == null) return;

        if (transform.localScale.x > 0.95f)
        {
            transform.localScale = new Vector3(1, 1);
            Destroy(asteroidToReplace);
            return;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1), 0.15f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameController.RocketLandOn(gameObject);
        }
    }

    /// <summary>
    /// Sets up this planet to start replacing asteroid
    /// Called by GameController.Terraform
    /// </summary>
    /// <param name="asteroid">the asteroid GameObject to replace</param>
    internal void Replace(GameObject asteroid)
    {
        asteroidToReplace = asteroid;
        transform.localScale = new Vector3(0.1f, 0.1f);
    }
}
