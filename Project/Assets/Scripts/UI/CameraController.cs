using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraStatus
{
    Start,
    Normal,
    BulletFlying
}

public class CameraController : MonoBehaviour
{
    public float scrollSpeed = 15;
    public float maxX;
    public float minX;

    public float zoomMultiplier = 1;
    public float zoomSpeed = 30;

    public Camera camera;

    // Update is called once per frame
    void Update()
    {

        if (Input.mousePosition.x >= Screen.width * 0.95)
        {
            float delta = Time.deltaTime * scrollSpeed;
            if (transform.position.x + delta < maxX)
            {
                transform.Translate(Vector3.right * delta, Space.World);
                MainCanvas.instance.transform.Translate(Vector3.right * delta, Space.World);
            }
            
        }
        else if (Input.mousePosition.x <= Screen.width * 0.05)
        {
            float delta = Time.deltaTime * scrollSpeed;
            if (transform.position.x - delta > minX)
            {
                transform.Translate(Vector3.left * delta, Space.World);
                MainCanvas.instance.transform.Translate(Vector3.left * delta, Space.World);
            }
        }
    }

    public void FocusAtPos(Vector3 pos)
    {
        camera.orthographicSize = camera.orthographicSize / 2;
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }
}
