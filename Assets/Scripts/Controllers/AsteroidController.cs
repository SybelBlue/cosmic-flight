using UnityEngine;

/// <summary>
/// A class that defines the unique behaviors of an asteroid prefab
/// </summary>
public class AsteroidController : MonoBehaviour
{
    public GameController gameController;
    public FlagController flagController;

    /// <summary>
    /// sets up a random fast speed with a random parity
    /// </summary>
    private void Start()
    {
        GetComponent<ConstantRotation>().rotateSpeed = Random.Range(30f / 3, 40f / 3) * 3 * (Random.value > 0.5f ? -1 : 1);
    }

    /// <summary>
    /// Reports collisions with player when they are not returning to the start
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !collision.gameObject.GetComponent<RocketController>().isSafe)
        {
            gameController.RocketLandOn(gameObject);
        }
    }

    /// <summary>
    /// Fires the flag raising animation
    /// </summary>
    internal void RaiseFlag()
    {
        flagController.SetRaised(true);
    }

    /// <summary>
    /// Fires the flag lowering animation
    /// </summary>
    internal void LowerFlag()
    {
        flagController.SetRaised(false);
    }
}
