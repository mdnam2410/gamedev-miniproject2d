using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;

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
    public int damage;
    public Transform firePos;
    public Vector3 offsetToFirePos;

    public Player owner;
    public List<GameObject> ownerColliders;

    public Rigidbody2D rbd;
    public Collider2D cld;
    public Animator animator;

    public BulletStatus currentStatus;
    public CollisionType currentCollision;

    public AudioSource firingSound;
    public AudioSource explodingSound;

    // explosion video -> anim "Explode"

    private void Start()
    {
        this.rbd = GetComponent<Rigidbody2D>();
        this.cld = GetComponent<Collider2D>();
        this.animator = GetComponent<Animator>();
        this.currentStatus = BulletStatus.Hidden;
        this.currentCollision = CollisionType.None;
        this.offsetToFirePos = this.transform.position - this.firePos.transform.position;
        this.InitData();
    }

    public virtual void InitData()
    {
        // TODO
    }

    private void Update()
    {
        //this.UpdatePosition();
        this.UpdateRotation();
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

    public virtual void UpdateRotation()
    {
        Vector3 target = Quaternion.Euler(0, 0, 90) * new Vector3(this.rbd.velocity.x, this.rbd.velocity.y);
        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: target);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.deltaTime);
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

        GameManager.Instance.OnBulletDestroyed.Invoke();
    }

    protected virtual void HitTarget()
    {
        this.owner.target.Behit(this.damage);
        this.ExecuteSpecialEffect();
        this.rbd.bodyType = RigidbodyType2D.Static;
        this.cld.enabled = false;
        this.animator.SetTrigger("Destroyed");
        /*
        this.gameObject.SetActive(false);
        this.gameObject.transform.position = this.offsetToFirePos + this.firePos.position;
        */
    }

    protected virtual void ExecuteSpecialEffect()
    {
        // TODO
    }

    protected virtual void DestroyByEnvironment()
    {
        this.rbd.bodyType = RigidbodyType2D.Static;
        this.cld.enabled = false;
        this.animator.SetTrigger("Destroyed");
        /*
        this.gameObject.SetActive(false);
        this.gameObject.transform.position = this.offsetToFirePos + this.firePos.position;
        */
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.ownerColliders.Contains(collision.gameObject)) return;

        if (collision.gameObject.tag.Equals("Player"))
        {
            this.currentCollision = CollisionType.Target;
            Debug.Log("Hit Target!");
        }
        else
        {
            this.currentCollision = CollisionType.Environment;
        }
    }
}
