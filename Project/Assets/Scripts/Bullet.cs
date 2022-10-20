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
        Thrown,
        Flying,
        Hit
    }

    public enum CollisionType
    {
        None,
        Environment,
        Target
    }

    public float mass;
    public float radius;
    public float damage;

    public Player owner;
    public Player target;

    public Rigidbody2D rigidbody;
    public Collider2D collider;

    protected BulletStatus currentStatus;
    protected CollisionType currentCollision;
    
    private void Start()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.collider = GetComponent<Collider2D>();
        this.currentStatus = BulletStatus.Hidden;
        this.currentCollision = CollisionType.None;
        this.InitData();
    }

    public virtual void InitData()
    {
        // TODO
    }

    private void Update()
    {
        this.UpdatePosition();
        this.UpdateCollision();
    }

    public virtual void UpdatePosition()
    {
        switch (this.currentStatus)
        {
            case BulletStatus.Hidden:
            case BulletStatus.Hit:
                break;
            case BulletStatus.BeHolding:
                this.HoldByOwner();
                break;
            case BulletStatus.Thrown:
                this.BeThrown();
                break;
            case BulletStatus.Flying:
                this.Fly();
                break;
        }
    }

    protected virtual void HoldByOwner()
    {
        // TODO
    }

    protected virtual void BeThrown()
    {
        // TODO
    }

    protected virtual void Fly()
    {
        // TODO
    }

    protected virtual void UpdateCollision()
    {
        if (this.currentCollision == CollisionType.None) return;

        if (this.currentCollision == CollisionType.Target)
        {
            this.HitTarget();
        }
        else
        {
            this.DestroyByEnvironment();
        }
    }

    protected virtual void HitTarget()
    {
        // TODO
        this.ExecuteSpecialEffect();
        this.gameObject.SetActive(false);
    }

    protected virtual void ExecuteSpecialEffect()
    {
        // TODO
    }

    protected virtual void DestroyByEnvironment()
    {
        // TODO
        this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.Equals(this.owner.gameObject)) return;

        if (collision.gameObject.tag.Equals("Player"))
        {
            this.currentCollision = CollisionType.Target;
        }
        else
        {
            this.currentCollision = CollisionType.Environment;
        }
    }
}
