using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerRole
    {
        P1,
        P2,
        Bot
    }
    public enum PlayerStatus
    {
        Idle,
        Aiming,
        Throw,
        BeHit,
        Win,
        Lose
    }

    public PlayerRole role;
    public int hp;
    public Transform firePos;
    public Collider2D heroHead;
    public Collider2D tank;
    public Animator heroHeadAnimator;
    public Animator tankAnimator;
    public GameManager.GameTurn ownTurn;
    public Bullet bullet;
    public PlayerStatus currentStatus;
    public Rigidbody2D rigid2D;

    public float movingSpeed = 10;
    public bool isMoving;

    public Vector2 fireForce;

    private void Start()
    {
        this.hp = 100;
        this.currentStatus = PlayerStatus.Idle;
        this.isMoving = false;
        this.fireForce = new Vector2(300, 350);
        this.rigid2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        this.CheckInOwnTurn();
        this.UpdateMove();
        this.UpdateAttack();
        this.UpdateBehit();
        this.UpdateWinLoseStatus();
    }

    public virtual void CheckInOwnTurn()
    {
        // TODO
    }

    public virtual void UpdateMove()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if (this.rigid2D.velocity.magnitude < 3)
                this.rigid2D.AddForce(new Vector2(2, 1f));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (this.rigid2D.velocity.magnitude < 3)
                this.rigid2D.AddForce(new Vector2(-2, 1f));
        }
    }

    public virtual void UpdateAttack()
    {
        if (this.bullet.gameObject.activeInHierarchy) return;

        this.Aim();
        this.GetPower();
        this.FireBullet();
    }

    private void FireBullet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.ApplyWindForce();
            this.tankAnimator.SetBool("DemoFire", false);
            this.tankAnimator.SetTrigger("Fire");
            this.heroHeadAnimator.SetTrigger("Fire");
        }
    }

    protected virtual void Aim()
    {
        // TODO
    }

    protected virtual void GetPower()
    {
        // TODO
    }

    protected virtual void ApplyWindForce()
    {
        // TODO
    }

    public virtual void ThrowBullet()
    {
        // this method called by StateMachineBehavior
        this.bullet.currentCollision = Bullet.CollisionType.None;
        this.bullet.currentStatus = Bullet.BulletStatus.Flying;
        this.bullet.gameObject.transform.rotation = Quaternion.identity;
        this.bullet.gameObject.SetActive(true);
        this.bullet.rbd.AddForce(this.fireForce + new Vector2(GameManager.instance.windSpeed * GameManager.instance.windForceScaleFactor, 0));
    }

    public virtual void UpdateBehit()
    {
        // TODO
    }

    public virtual void UpdateWinLoseStatus()
    {
        // TODO
    }

}
