using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to make the object this script is attached to persist between scene loads
/// Also contains the option to keep the attached object unique within DontDestroyOnLoad
/// </summary>
public class PersistBetweenScenes : MonoBehaviour
{
    /// <summary>
    /// When true, only one of these objects will persist between scene loads
    /// </summary>
    public bool stayUnique = true;

    /// <summary>
    /// The list of object namees to keep unique
    /// </summary>
    private static List<string> uniqueObjects = new List<string>();

    /// <summary>
    /// Either makes this object persist between scens or 
    /// destroys it if it is marked as unique and the object's name
    /// is already in the list of unique objects
    /// </summary>
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
