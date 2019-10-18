using UnityEngine;

public class ExitPlanetController : MonoBehaviour
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
