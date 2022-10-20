using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerStatus
    {
        Idle,
        Aiming,
        Throw,
        BeHit,
        Win,
        Lose
    }

    public int hp;
    public Transform holdPos;
    public Transform throwPos;
    public GameManager.GameTurn ownTurn;

    protected Rigidbody2D rigidbody;
    protected Collider2D collider;
    protected Animator animator;

    protected Bullet currentBullet;
    protected PlayerStatus currentStatus;

    private void Start()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.collider = GetComponent<Collider2D>();
        this.animator = GetComponent<Animator>();

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
        this.ThrowBullet();
    }

    protected virtual void GetDirection()
    {
    }

    protected virtual void GetPower()
    {
    }

    public virtual void ThrowBullet()
    {
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
