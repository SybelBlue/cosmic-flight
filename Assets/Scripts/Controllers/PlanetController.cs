using UnityEngine;

/// <summary>
/// A class that defines the unique behaviors of the planet prefab
/// </summary>
public class PlanetController : MonoBehaviour
{

    /// <summary>
    /// The GameController instance of the current level
    /// </summary>
    public GameController gameController;

    /// <summary>
    /// The asteroid this planet is currently replacing,
    /// null if no such planet
    /// </summary>
    public GameObject asteroidToReplace;

    /// <summary>
    /// Runs the asteroid replacement animation
    /// </summary>
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

    /// <summary>
    /// Reports to the gameController of the player lands on this
    /// </summary>
    /// <param name="collision"></param>
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
