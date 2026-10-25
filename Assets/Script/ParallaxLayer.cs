using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Tooltip("0 = bergerak normal seperti dunia, 1 = ikut kamera (terlihat sangat jauh)")]
    [SerializeField, Range(0f, 1f)] float parallaxFactor = 0.5f;

    Transform cam;
    Vector3 startPos;
    float camStartX;

    void Start()
    {
        cam = Camera.main.transform;
        startPos = transform.position;
        camStartX = cam.position.x;
    }

    void LateUpdate()
    {
        float camDelta = cam.position.x - camStartX;
        transform.position = new Vector3(
            startPos.x + camDelta * parallaxFactor,
            startPos.y,
            startPos.z);
    }
}