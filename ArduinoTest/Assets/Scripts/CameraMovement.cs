using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 0.01f, 0, Space.Self);
    }
}
