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

    public Rigidbody2D rigidbody;
    public Collider2D collider;

    protected Bullet currentBullet;
    protected PlayerStatus currentStatus;

    private void Start()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.collider = GetComponent<Collider2D>();
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
