using UnityEngine;

/// <summary>
/// A utility that changes the z-angle of rotation on the attached 
/// object's transform at a constant rate
/// </summary>
public class ConstantRotation : MonoBehaviour
{

    /// <summary>
    /// Rotation speed in 100ths of a degree per update
    /// </summary>
    public float rotateSpeed;

    /// <summary>
    /// Rotates the attached gameObject
    /// </summary>
    void Update()
    {
        transform.Rotate(Vector3.back, rotateSpeed / 100);
    }
}
