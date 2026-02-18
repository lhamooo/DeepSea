using UnityEngine;

public class WhaleBehaviour : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.TransformDirection(new Vector3(0, 0, 0.01f));
    }
}
