using UnityEngine;

public class RotateConstantScript : MonoBehaviour
{

    // in 100ths of a degree per update
    public float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back, rotateSpeed / 100);
    }
}
