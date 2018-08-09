using UnityEngine;

public class RotationScript : MonoBehaviour
{
    public float rotationsPerMinute = 640f;

    void Update()
    {
        transform.Rotate(0, 0, -rotationsPerMinute * Time.deltaTime);
    }
}
