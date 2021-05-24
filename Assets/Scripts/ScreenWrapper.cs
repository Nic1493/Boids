using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    Camera mainCam;
    Vector2 camSize;

    void Awake()
    {
        mainCam = Camera.main;
        camSize.y = mainCam.orthographicSize;
        camSize.x = camSize.y * mainCam.aspect;
    }

    void OnBecameInvisible()
    {
        if (Mathf.Abs(transform.position.x) > camSize.x)
        {
            Vector2 newPos = transform.position;
            newPos.x *= -1;
            transform.position = newPos;
        }

        if (Mathf.Abs(transform.position.y) > camSize.y)
        {
            Vector2 newPos = transform.position;
            newPos.y *= -1;
            transform.position = newPos;
        }
    }
}