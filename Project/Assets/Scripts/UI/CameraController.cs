using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraStatus
{
    Start,
    Normal,
    FocusBullet,
    FocusPlayer,
}

public class CameraController : MonoBehaviour
{
    public float scrollSpeed = 15;

    public float maxX;
    public float minX;
    public float maxY;
    public float minY;

    public float zoomMultiplier = 1;
    public float zoomSpeed = 30;

    public CameraStatus status = CameraStatus.Normal;

    public GameObject focusedObject = null;
    public Vector3 focusDistance;
    public Vector3 playerDelta;
    public Vector3 canvasDelta;
    public float remainingTime;
    public Camera camera;

    private void Start()
    {
        GameManager.Instance.OnBulletDestroyed.AddListener(this.EndFocusBullet);
        canvasDelta = MainCanvas.instance.transform.position - transform.position;
    }

    public float zoomMultiplier = 1;
    public float zoomSpeed = 30;

    public Camera camera;

    // Update is called once per frame
    void Update()
    {
        if (status == CameraStatus.FocusBullet)
        {
            if (focusedObject)
            {
                Vector3 tmp = focusedObject.transform.position + focusDistance;
                Vector3 newPos = GetValidPosition(tmp);
                transform.position = newPos;
                MainCanvas.instance.transform.position = newPos + canvasDelta;
            }
        }
        else if (status == CameraStatus.FocusPlayer)
        {
            if (remainingTime > 0)
            {
                float deltaTime = Time.deltaTime;
                MoveCamera(playerDelta, deltaTime);
                remainingTime -= deltaTime;
            }
            else
            {
                status = CameraStatus.Normal;
            }
        }
        else if (status == CameraStatus.Normal)
        {
            if (Input.mousePosition.x >= Screen.width * 0.95)
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
        
    }

    public void MoveCamera(Vector3 vector, float delta)
    {
        transform.Translate(vector * delta, Space.World);
        MainCanvas.instance.transform.Translate(vector * delta, Space.World);
    }

    public void FocusBullet(GameObject bullet)
    {
        focusedObject = bullet;
        focusDistance = transform.position - bullet.transform.position;
        status = CameraStatus.FocusBullet;
    }

    public void FocusPlayer(GameObject player)
    {
        Vector3 pos = player.transform.position;
        Vector3 newPos = GetValidPosition(new Vector3(pos.x, pos.y, -10));
        playerDelta = newPos - transform.position;
        remainingTime = 1;

        status = CameraStatus.FocusPlayer;
    }

    Vector3 GetValidPosition(Vector3 pos)
    {
        float newX = pos.x;
        if (pos.x > maxX)
        {
            newX = maxX;
        }
        else if (pos.x < minX)
        {
            newX = minX;
        }

        float newY = pos.y;
        if (pos.y > maxY)
        {
            newY = maxY;
        }
        else if (pos.y < minY)
        {
            newY = minY;
        }

        return new Vector3(newX, newY, pos.z);
    }

    public void FocusAtPos(Vector3 pos)
    {
        camera.orthographicSize = camera.orthographicSize / 2;
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }
    void EndFocusBullet()
    {
        status = CameraStatus.Normal;
    }
}
