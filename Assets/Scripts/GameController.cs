using UnityEngine;

public class GameController : MonoBehaviour
{

    public Vector3 rocketStartingPosition;

    public GameObject rocketPrefab;
    public GameObject blackHolePrefab;

    public GameObject rocketGObject;
    public GameObject blackHoleGObject;

    private RocketController rocketController;
    private BlackHoleController blackHoleController;

    public bool inPlay;

    // Start is called before the first frame update
    void Start()
    {
        inPlay = true;

        blackHoleGObject = Instantiate(blackHolePrefab);
        blackHoleController = blackHoleGObject.GetComponent<BlackHoleController>();
        blackHoleController.gameController = this;

        rocketGObject = Instantiate(rocketPrefab, rocketStartingPosition, Quaternion.Euler(0, 0, 0));
        rocketController = rocketGObject.GetComponent<RocketController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Do nothing if not in play
        if (!inPlay) return;

        var force = blackHoleController.GetGravitationalForce(rocketGObject);
        rocketController.ApplyGravitationalForce(force);
    }

    public void ObjectDied(GameObject thing)
    {
        if (thing.tag == "Player")
        {
            GameOver();
        }
        Destroy(thing);
    }

    void GameOver()
    {
        inPlay = false;
        Debug.Log("Game Over!");
    }
}
