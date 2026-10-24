using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float parallaxFactor = 0.5f;
    [SerializeField] bool infiniteHorizontal = true;

    Transform cam;
    float startX, length;

    void Start()
    {
        cam = Camera.main.transform;
        startX = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        float travelled = cam.position.x * parallaxFactor;
        float fromStart = cam.position.x * (1f - parallaxFactor);

        transform.position = new Vector3(startX + travelled, transform.position.y, transform.position.z);

        if (!infiniteHorizontal) return;

        if (fromStart > startX + length) startX += length;
        else if (fromStart < startX - length) startX -= length;
    }
}