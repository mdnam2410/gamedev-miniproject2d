using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public enum Status
    {
        Idle,
        Moving,
        Aiming,
        Attacking,
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
    public Status currentStatus;
    public Rigidbody2D rigid2D;

    public float movingSpeed = 5;
    public MovingState movingState;
    public FaceDirection faceDirection;

    public Vector2 forceVector;
    public Player target;
    public float angle = 45f;
    public float force = 200;
    public bool fired = false;
    public bool canMove;
    public Vector2 velo;

    protected virtual void Start()
    {
        this.hp = 100;
        this.currentStatus = Status.Idle;
        this.movingState = MovingState.None;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.canMove = true;

        GameManager.Instance.OnTurnChanged.AddListener(this.OnTurnChange);
        ForceBar.Instance.OnPowerCompleted.AddListener(this.Fire);
        ForceBar.Instance.OnPowerCompleted.AddListener(this.lockMovingOnFire);
    }

    private void Update()
    {
        this.velo = this.rigid2D.velocity;
        if (GameManager.Instance == null) return;
        if (this.ownRole != GameManager.Instance.currentTurn) return;

        if (GameManager.Instance.currentValidAction == GameManager.ValidAction.All)
        {
            this.UpdateMove();
        }
        else
        {
            this.tankAnimator.SetBool("Moving", false);
        }
    }

    public virtual void UpdateMove()
    {
        this.UpdateFaceDirection();
        this.Move();
        this.UpdateTankAnim();
    }

    public virtual void UpdateFaceDirection()
    {
        if (Input.GetMouseButtonDown(1))
        {
            this.ReverseFaceDirection();
        }
    }

    public virtual void ReverseFaceDirection()
    {
        this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        this.faceDirection = (FaceDirection)(1 - this.faceDirection);
    }

    protected virtual void Move()
    {
        if (!this.canMove)
        {
            this.movingState = MovingState.None;
            return;
        }

        if (this.MoveRight())
        {
            if (this.rigid2D.velocity.magnitude < 1)
                //this.rigid2D.AddForce(new Vector2(10, 0.6f));
                this.rigid2D.velocity = new Vector2(this.rigid2D.velocity.x + this.movingSpeed * Time.deltaTime, .1f);

            this.movingState = MovingState.ToRight;

        }
        else if (this.MoveLeft())
        {
            if (this.rigid2D.velocity.magnitude < 1)
                //this.rigid2D.AddForce(new Vector2(-10, 0.6f));
                this.rigid2D.velocity = new Vector2(this.rigid2D.velocity.x - this.movingSpeed * Time.deltaTime, .1f);

            this.movingState = MovingState.ToLeft;
        }
        else
        {
            this.movingState = MovingState.None;
        }
    }

    protected virtual bool MoveLeft()
    {
        return Input.GetKey(KeyCode.A);
    }

    protected virtual bool MoveRight() {
        return Input.GetKey(KeyCode.D);
    }

    protected virtual void UpdateTankAnim()
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

    public virtual void Fire()
    {
        if (this.fired) return;
        if (this.ownRole == GameManager.Instance.currentTurn)
        {
            this.CalculateForceVector();
            this.SetFireAnim();
            this.fired = true;
            this.currentStatus = Status.Attacking;
            GameManager.Instance.OnPlayerShoot.Invoke();
        }
    }

    protected void SetFireAnim()
    {
        this.tankAnimator.SetBool("DemoFire", false);
        this.tankAnimator.SetTrigger("Fire");
        this.heroHeadAnimator.SetTrigger("Fire");
    }

    protected virtual void CalculateForceVector()
    {
        this.angle = GameManager.Instance.angleRuler.curAngle;

        if (this.faceDirection == FaceDirection.RightLeft)
        {
            this.angle = 180f - this.angle;
        }

        this.force = ForceBar.Instance.getForce();
        this.forceVector = new Vector2(Mathf.Cos(this.angle * Mathf.PI / 180f), Mathf.Sin(this.angle * Mathf.PI / 180f)) * this.force;
    }

    public virtual void ThrowBullet()
    {
        // this method called by StateMachineBehavior
        this.bullet.currentCollision = Bullet.CollisionType.None;
        this.bullet.currentStatus = Bullet.BulletStatus.Flying;
        this.bullet.gameObject.transform.rotation = Quaternion.identity;
        this.bullet.gameObject.SetActive(true);
        this.bullet.rbd.AddForce(this.forceVector + new Vector2(GameManager.Instance.windSpeed * GameManager.Instance.windForceScaleFactor, 0));
    }

    public virtual void Behit(int damage)
    {
        this.hp -= damage;
        this.heroHeadAnimator.SetTrigger("Behit");
        if (this.hp <= 0)
        {
            this.hp = 0;
            this.currentStatus = Status.Lose;
            GameManager.Instance.PlayerDefeated(this);
        }
        else
        {
            Debug.Log(this.gameObject.name + ": " + this.hp.ToString());
        }
    }

    public virtual void OnTurnChange()
    {
        if (this.ownRole == GameManager.Instance.currentTurn)
        {
            this.fired = false;
        }

        this.canMove = true;
        this.currentStatus = Status.Idle;
    }

    protected void lockMovingOnFire() => this.canMove = false;
    protected void unlockMove() => this.canMove = true;

    protected virtual void UpdateWinLoseStatus()
    {
        // TODO
    }

}
