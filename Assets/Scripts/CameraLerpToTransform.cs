using UnityEngine;

public class CameraLerpToTransform : MonoBehaviour
{
    public Transform camTarget;
    public float trackingSpeed;
    public float mixX;
    public float mixY;
    public float maxX;
    public float maxY;

    void FixedUpdate()
    {
        if (camTarget != null)
        {
            Vector2 newPos = Vector2.Lerp(transform.position, camTarget.transform.position, Time.deltaTime * trackingSpeed);
            Vector3 camPosition = new Vector3(newPos.x, newPos.y, -10f);
            Vector3 v3 = camPosition;

            float clampX = Mathf.Clamp(v3.x, mixX, maxX);
            float clampY = Mathf.Clamp(v3.y, mixY, maxY);

            transform.position = new Vector3(clampX, clampY, -10f);
        }
    }
}
