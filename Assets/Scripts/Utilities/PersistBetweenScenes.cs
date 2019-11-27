using System.Collections.Generic;
using UnityEngine;

public class PersistBetweenScenes : MonoBehaviour
{

    public bool stayUnique = true;

    private static List<string> uniqueObjects = new List<string>();

    void Start()
    {
        if (stayUnique && uniqueObjects.Contains(gameObject.name))
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);   

        if (stayUnique)
        {
            uniqueObjects.Add(gameObject.name);
        }
    }
}
