using UnityEngine;

public class ConstantRotation : MonoBehaviour
{

    // in 100ths of a degree per update
    public float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back, rotateSpeed / 100);
    }
}
