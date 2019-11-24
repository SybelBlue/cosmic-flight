using UnityEngine;

public class PersistBetweenScenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);   
    }
}
