using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletStatus
    {
        Hidden,
        BeHolding,
        Flying,
        Hit
    }

    public float mass;
    public float radius;
    public float damage;

    public Rigidbody2D rigidbody;
    public Collider2D collider;

    protected BulletStatus currentStatus;
    protected Player owner;
    protected Player target;

    private void Start()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.collider = GetComponent<Collider2D>();
        this.currentStatus = BulletStatus.Hidden;
        this.InitData();
    }

    public virtual void InitData()
    {
        // TODO
    }

    private void Update()
    {
        this.UpdatePosition();
        this.UpdateHittingTarget();
        this.UpdateActiveStatus();
    }

    public virtual void UpdatePosition()
    {

    }

    public virtual void UpdateHittingTarget()
    {
    }

    public virtual void UpdateActiveStatus()
    {

    }

    public virtual void BeThrown(Vector2 position, float force, float angle)
    {
        // TODO
    }

    protected virtual void HitPlayer(Player player)
    {
        // TODO
    }

    protected virtual void ExecuteSpecialEffect()
    {
        // TODO
    }
}
