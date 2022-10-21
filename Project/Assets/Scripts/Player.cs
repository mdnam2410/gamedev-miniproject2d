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

    private void Start()
    {
        this.hp = 100;
        this.currentStatus = PlayerStatus.Idle;
    }

    private void Update()
    {
        this.CheckInOwnTurn();
        this.UpdateAttack();
        this.UpdateBehit();
        this.UpdateWinLoseStatus();
    }

    public virtual void CheckInOwnTurn()
    {
        // TODO
    }

    public virtual void UpdateAttack()
    {
        // TODO
        this.GetDirection();
        this.GetPower();
        if (this.tankAnimator.GetBool("DemoFire"))
        {
            if (this.bullet.gameObject.activeInHierarchy) return;

            this.tankAnimator.SetBool("DemoFire", false);
            this.tankAnimator.SetTrigger("Fire");
            this.heroHeadAnimator.SetTrigger("Fire");
            //this.ThrowBullet();
        }
    }

    protected virtual void GetDirection()
    {
    }

    protected virtual void GetPower()
    {
    }

    public virtual void ThrowBullet()
    {
        this.bullet.currentCollision = Bullet.CollisionType.None;
        this.bullet.currentStatus = Bullet.BulletStatus.Flying;
        this.bullet.gameObject.transform.rotation = Quaternion.identity;
        this.bullet.gameObject.SetActive(true);
        this.bullet.rbd.AddForce(new Vector2(300, 400));
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
