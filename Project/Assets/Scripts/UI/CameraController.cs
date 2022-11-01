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

    public CameraStatus status = CameraStatus.Normal;

    public GameObject focusedObject = null;

    public Camera camera;

    // Update is called once per frame
    void Update()
    {
        if (status == CameraStatus.BulletFlying)
        {
            Bullet bullet = focusedObject.GetComponent<Bullet>();
            Vector2 velo = bullet.rbd.velocity;
            Vector3 vector = new Vector3(velo.x, velo.y, -10);
            float delta = Time.deltaTime * scrollSpeed;

            Debug.Log(transform.position.ToString());
            Debug.Log(focusedObject.transform.position.ToString());

            MoveCamera(vector, delta);
        }

        else if (Input.mousePosition.x >= Screen.width * 0.95)
        {
            float delta = Time.deltaTime * scrollSpeed;
            if (transform.position.x + delta < maxX)
            {
                MoveCamera(Vector3.right, delta);
            }
        }
        else if (Input.mousePosition.x <= Screen.width * 0.05)
        {
            float delta = Time.deltaTime * scrollSpeed;
            if (transform.position.x - delta > minX)
            {
                MoveCamera(Vector3.left, delta);
            }
        }
    }

    public void MoveCamera(Vector3 vector, float delta)
    {
        transform.Translate(vector * delta, Space.World);
        MainCanvas.instance.transform.Translate(vector * delta, Space.World);
    }

    public void FocusBullet(GameObject bullet)
    {
        focusedObject = bullet;
        status = CameraStatus.BulletFlying;
    }

    public void FocusAtPos(Vector3 pos)
    {
        camera.orthographicSize = camera.orthographicSize / 2;
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }
}
