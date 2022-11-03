using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathFinder : MonoBehaviour
{
    public UnityEvent OnFindPathComplete;

    public FinderMissile prefab;

    public Transform startPos;
    public Transform target;

    public float force;
    public float angle;
    public float nearestDistance;
    public int count;
    public bool fired;
    public int angleSample;
    public int forceSample;
    public float maxWaitingTime;
    public float remainingTime;
    public float maxAngle;
    public float minForce;
    public float maxForce;

    private void Start()
    {
        this.target = GameManager.Instance.P1.transform;
    }

    private void Update()
    {
        this.remainingTime -= Time.deltaTime;
        if (this.remainingTime <= 0 && !fired)
        {
            this.fired = true;
            if (GameManager.Instance.P2 is Bot bot)
            {
                bot.pathFound = true;
            }
        }
    }

    public void FindPath(float baseAngle)
    {
        this.remainingTime = this.maxWaitingTime;
        this.count = 0;
        this.fired = false;
        this.nearestDistance = 999999;

        FinderMissile[] missiles = this.GetComponentsInChildren<FinderMissile>();
        for (int i = 0; i < missiles.Length; i++)
        {
            if (missiles[i].gameObject != this.prefab)
                Destroy(missiles[i].gameObject);
        }
        /*
        for (int i = 0; i <= this.angleSample; i++)
        {
            float curAngle = baseAngle + (this.maxAngle - baseAngle) / this.angleSample * i;
            if (GameManager.Instance.P2.faceDirection == Player.FaceDirection.RightLeft)
                curAngle = 180f - curAngle;

            for (int j = 0; j < this.forceSample; j++)
            {
                float curForce = this.minForce + (this.maxForce - this.minForce) / forceSample * j;
                this.FireMissle(curAngle, curForce);
                this.count++;
            }
        }
        */

        for (int i = 0; i <= this.forceSample; i++)
        {
            float curForce = this.minForce + (this.maxForce - this.minForce) / forceSample * i;
            for (int j = 0; j <= this.angleSample; j++)
            {
                float curAngle = baseAngle + (this.maxAngle - baseAngle) / this.angleSample * j;
                if (GameManager.Instance.P2.faceDirection == Player.FaceDirection.RightLeft)
                    curAngle = 180f - curAngle;

                this.FireMissle(curAngle, curForce);
                this.count++;
            }
        }
    }

    public void FireMissle(float angle, float force)
    {
        FinderMissile missile = Instantiate(this.prefab, this.transform);
        missile.transform.position = this.startPos.position;
        missile.pathFinder = this;
        missile.angle = angle;
        missile.force = force;
        missile.gameObject.SetActive(true);
        missile.rigid2D.AddForce(new Vector2(Mathf.Cos(angle * Mathf.PI / 180f), Mathf.Sin(angle * Mathf.PI / 180f)) * force);
    }

    public void ReceiveResultFromMissile(float distance, float angle, float force)
    {
        if (distance < nearestDistance - 0.1)
        {
            this.angle = angle;
            this.force = force;
            this.nearestDistance = distance;
        }
        this.count--;
        if (this.count <= 0 && !this.fired)
        {
            //this.OnFindPathComplete.Invoke();
            if (GameManager.Instance.P2 is Bot bot)
            {
                bot.pathFound = true;
            }
            this.fired = true;
        }
    }
}
