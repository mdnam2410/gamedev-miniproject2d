using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinderMissile : MonoBehaviour
{
    public PathFinder pathFinder;
    public float angle;
    public float force;
    public Rigidbody2D rigid2D;
    public static List<string> ignoreColliders = new List<string> { "Finder", "Buff", "Stair" };
    private void OnCollisionEnter(Collision collision)
    {
        if (ignoreColliders.Contains(collision.gameObject.tag))
            return;
        float distance = (collision.transform.position - pathFinder.target.transform.position).magnitude;
        this.pathFinder.ReceiveResultFromMissile(distance, this.angle, this.force);
        //Debug.Log("Destroy by " + collision.ToString());
        //Debug.Log(pathFinder.count);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ignoreColliders.Contains(collision.gameObject.tag))
            return;

        float distance = (collision.transform.position - pathFinder.target.transform.position).magnitude;
        this.pathFinder.ReceiveResultFromMissile(distance, this.angle, this.force);
        //Debug.Log("Destroy by " + collision.ToString());
        //Debug.Log(pathFinder.count);
        Destroy(this.gameObject);
    }
}
