using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{    public enum PlayerStatus
    {
        Idle,
        Aiming,
        Throw,
        BeHit,
        Win,
        Lose
    }

    public enum FaceDirection
    {
        LeftRight,
        RightLeft
    }

    public enum MovingState
    {
        None,
        ToRight,
        ToLeft
    }

    public GameManager.GameTurn ownRole;
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
    public MovingState movingState;
    public FaceDirection faceDirection;

    public Vector2 fireForce;
    public Player target;

    private void Start()
    {
        this.hp = 100;
        this.currentStatus = PlayerStatus.Idle;
        this.movingState = MovingState.None;
        this.faceDirection = FaceDirection.LeftRight;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.InitFireForce();

    }

    private void InitFireForce()
    {
        this.fireForce = new Vector2(300, 350);
        if (this.target != null)
        {
            float delta = this.target.transform.position.x - this.transform.position.x;
            if (delta < 0)
            {
                this.fireForce = new Vector2(-300, 350);
            }
        }
    }

    private void Update()
    {
        if (GameManager.instance != null && this.ownRole == GameManager.instance.currentTurn)
        {
            this.CheckInOwnTurn();
            this.UpdateMove();
            this.UpdateAttack();
            this.UpdateBehit();
            this.UpdateWinLoseStatus();
        }
    }

    public virtual void CheckInOwnTurn()
    {
        // TODO
    }

    public virtual void UpdateMove()
    {
        this.UpdateFaceDirection();
        this.MovingByKey();
        this.UpdateTankMovingAnimation();

    }

    private void UpdateFaceDirection()
    {
        if (Input.GetMouseButtonDown(1))
        {
            this.ReverseDirectionByInput();
        }
    }

    private void ReverseDirectionByInput()
    {
        this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        this.faceDirection = (FaceDirection)(1 - this.faceDirection);
    }
    private void MovingByKey()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if (this.rigid2D.velocity.magnitude < 1)
                this.rigid2D.AddForce(new Vector2(1, 0.6f));

            this.movingState = MovingState.ToRight;
            
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (this.rigid2D.velocity.magnitude < 1)
                this.rigid2D.AddForce(new Vector2(-1, 0.6f));

            this.movingState = MovingState.ToLeft;
        }
        else
        {
            this.movingState = MovingState.None;
        }
    }

    private void UpdateTankMovingAnimation()
    {
        if (this.movingState != MovingState.None)
        {
            this.tankAnimator.SetBool("Moving", true);
            if (this.isMovingToward())
            {
                this.tankAnimator.SetBool("Toward", true);
            }
            else
            {
                this.tankAnimator.SetBool("Toward", false);
            }
        }
        else
        {
            this.tankAnimator.SetBool("Moving", false);
        }
    }

    private bool isMovingToward()
    {
        if (this.movingState == MovingState.ToLeft && this.faceDirection == FaceDirection.RightLeft) return true;
        if (this.movingState == MovingState.ToRight && this.faceDirection == FaceDirection.LeftRight) return true;
        return false;
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
