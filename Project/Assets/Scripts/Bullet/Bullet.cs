using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public static List<string> ignoreTags = new List<string> { "Buff" };
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
    public float radius = 2;
    public int damage;
    public float partialDamageRate = 0.5f;
    public int damageBuff = 0;
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
    public Vector3 cachedScale;

    // explosion video -> anim "Explode"

    private void Start()
    {
        this.rbd = GetComponent<Rigidbody2D>();
        this.cld = GetComponent<Collider2D>();
        this.animator = GetComponent<Animator>();
        this.currentStatus = BulletStatus.Hidden;
        this.currentCollision = CollisionType.None;
        this.offsetToFirePos = this.transform.position - this.firePos.transform.position;
        this.cachedScale = this.transform.localScale;
        this.InitData();
    }

    public virtual void InitData()
    {
        // TODO
    }

    private void Update()
    {
        this.UpdateRotation();
    }

    public virtual void UpdatePosition()
    {
        // TODO
    }

    public virtual void UpdateRotation()
    {
        Vector3 target = Quaternion.Euler(0, 0, 90) * new Vector3(this.rbd.velocity.x, this.rbd.velocity.y);
        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: target);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.ownerColliders.Contains(collision.gameObject)) return;
        if (Bullet.ignoreTags.Contains(collision.gameObject.tag)) return;

        this.PlayExplodingSound();

        if (collision.gameObject.tag.Equals("Player"))
        {
            this.HitTarget();
            Debug.Log("Hit target");
        }
        else
        {
            Debug.Log("Bullet destroyed by " + collision.gameObject.ToString());
            Collider2D[] collidersAround = Physics2D.OverlapCircleAll(this.transform.position, this.radius);

            for (int i = 0; i < collidersAround.Length; i++)
            {
                if (TryHittingTargetAround(collidersAround[i]))
                {
                    break;
                }
            }
        }

        this.rbd.bodyType = RigidbodyType2D.Static;
        this.cld.enabled = false;
        this.animator.SetTrigger("Destroyed");
        
        GameManager.Instance.OnBulletDestroyed.Invoke();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.ownerColliders.Contains(collision.gameObject)) return;
        if (Bullet.ignoreTags.Contains(collision.gameObject.tag)) return;

        this.PlayExplodingSound();

        if (collision.gameObject.tag.Equals("Player"))
        {
            this.HitTarget();
            Debug.Log("Hit target");
        }
        else
        {
            Debug.Log("Bullet destroyed by " + collision.gameObject.ToString());
            Collider2D[] collidersAround = Physics2D.OverlapCircleAll(this.transform.position, this.radius);

            for (int i = 0; i < collidersAround.Length; i++)
            {
                if (TryHittingTargetAround(collidersAround[i]))
                {
                    break;
                }
            }
        }

        this.rbd.bodyType = RigidbodyType2D.Static;
        this.cld.enabled = false;
        this.animator.SetTrigger("Destroyed");
        
        GameManager.Instance.OnBulletDestroyed.Invoke();
    }

    protected virtual bool TryHittingTargetAround(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (!this.ownerColliders.Contains(collision.gameObject))
            {
                this.HitTargetPartially();
                return true;
            }
        }
        return false;
    }

    protected virtual void HitTarget()
    {
        if (this.damageBuff != 0)
        {
            this.owner.target.Behit(this.damage + this.damageBuff);
            this.damageBuff = 0;
        }
        else
        {
            this.owner.target.Behit(this.damage);
        }

        
        this.ExecuteSpecialEffect();
    }

    protected virtual void HitTargetPartially()
    {
        if (this.damageBuff != 0)
        {
            this.owner.target.Behit((int)((this.damage + this.damageBuff) * this.partialDamageRate));
            this.damageBuff = 0;
        }
        else
        {
            this.owner.target.Behit((int)(this.damage * this.partialDamageRate));
        }

        Debug.Log("Hit target partially");
    }

    protected virtual void ExecuteSpecialEffect()
    {
        // TODO
    }

    protected virtual void DestroyByEnvironment()
    {
    }

    public void PlayFiringSound()
    {
        if (!SoundManager.Instance.Enabled)
            return;

        if (this.firingSound == null) return;
        if (this.firingSound.isPlaying)
        {
            this.firingSound.Stop();
        }
        this.firingSound.Play();
    }

    public void PlayExplodingSound()
    {
        if (!SoundManager.Instance.Enabled)
            return;

        if (this.explodingSound == null) return;
        if (this.explodingSound.isPlaying)
        {
            this.explodingSound.Stop();
        }
        this.explodingSound.Play();
    }

    public void AddDamageBuff(int buff)
    {
        this.damageBuff = buff;
    }
}
