using UnityEngine;

public class PersistBetweenScenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // GameObject.FindGameObjectsWithTag("Music and Effects")
        DontDestroyOnLoad(gameObject);   
    }
}
