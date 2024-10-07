using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float scrollSpeed = 5f;

    void Update()
    {
        Vector3 pos = this.transform.position;

        pos.y += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        if (pos.y < 0 ) pos.y = 0;

        this.transform.position = pos;
    }
}
