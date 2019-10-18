using UnityEngine;

public class PlanetController : MonoBehaviour
{

    public GameController gameController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameController.LandOnPlanet(gameObject);
        }
    }
}
