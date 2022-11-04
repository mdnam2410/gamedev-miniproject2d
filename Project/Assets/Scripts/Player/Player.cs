using System;
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
    public int maxHp;
    public float movingSpeed = 5;

    public Transform firePos;
    public Collider2D heroHead;
    public Animator heroHeadAnimator;
    public Animator tankAnimator;
    public GameManager.GameTurn ownTurn;
    public Bullet bullet;
    public Status currentStatus;
    public Rigidbody2D rigid2D;
    public HealthBar healthBar;

    public MovingState movingState;
    public FaceDirection faceDirection;

    public Vector2 forceVector;
    public Player target;
    public float angle = 45f;
    public float force = 200;
    public bool fired = false;
    public bool canMove;
    public Vector2 velo;

    public float verticalVelocity;
    public float verticalJumpingVelocity;
    public float verticalDefaultVelocity;
    public float jumpTimeCooldown;
    public float lockRotate;

    public float shieldBuff = 0;
    public float powerBuff = 0;
    public float speedBuff = 0;

    public WheelJoint2D frontWheel;
    public WheelJoint2D backWheel;
    public JointMotor2D forwardSpeed;
    public JointMotor2D backwardSpeed;
    public JointMotor2D frontStopSpeed;
    public JointMotor2D backStopSpeed;

    public bool verticalMove;
    private float verticalSpeed;
    public float cachedMass;
    public Collider2D collider;
    public Collider2D fWheelCollider;
    public Collider2D bWheelCollider;

    [Header("SPECIAL ABILITIES")]
    public float forceScale = 1;
    public float buffMultiplier = 1;
    public float healingCheat = 0;


    public UnityEvent<Player, int> OnBehit = new UnityEvent<Player, int>();

    protected virtual void Start()
    {
        this.hp = this.maxHp;
        this.currentStatus = Status.Idle;
        this.movingState = MovingState.None;
        this.canMove = true;
        this.lockRotate = 0f;
        this.jumpTimeCooldown = 2f;

        if (this.frontWheel != null)
            this.frontWheel.useMotor = false;
        if (this.backWheel != null)
        this.backWheel.useMotor = false; ;
        this.frontStopSpeed.motorSpeed = 0.001f;
        this.frontStopSpeed.maxMotorTorque = 0.001f;
        this.backStopSpeed.motorSpeed = -0.001f;
        this.backStopSpeed.maxMotorTorque = -0.001f;
        this.verticalSpeed = 4f;
        this.verticalMove = false;
        this.collider = this.transform.GetComponent<Collider2D>();
        this.fWheelCollider = this.frontWheel.GetComponent<Collider2D>();
        this.bWheelCollider = this.backWheel.GetComponent<Collider2D>();


        GameManager.Instance.OnTurnChanged.AddListener(this.OnTurnChange);
        ForceBar.Instance.OnPowerCompleted.AddListener(this.Fire);
        ForceBar.Instance.OnPowerCompleted.AddListener(this.lockMovingOnFire);
    }

    private void Update()
    {
        this.velo = this.rigid2D.velocity;

        if (GameManager.Instance == null) return;

        //this.PreventFalling2();

        if (this.ownRole != GameManager.Instance.currentTurn) return;

        if (GameManager.Instance.currentValidAction == GameManager.ValidAction.All)
        {
            this.UpdateMove();
        }
        else
        {
            this.StopMoving();
            this.tankAnimator.SetBool("Moving", false);
        }
    }

    public virtual void PreventFalling()
    {
        this.lockRotate -= Time.deltaTime;
        if (this.lockRotate > 0)
        {
            this.rigid2D.freezeRotation = true;
        }
        else
        {
            this.rigid2D.freezeRotation = false;
        }

        if (this.transform.localEulerAngles.z > 89f)
        {
            Debug.Log("More than 89 degree");
            this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, 15);
            this.rigid2D.velocity = Vector2.zero;
            this.lockRotate = 1f;
        }
        else if (this.transform.localEulerAngles.z < -89f)
        {
            Debug.Log("Less than -89 degree");
            this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, -15);
            this.rigid2D.velocity = Vector2.zero;
            this.lockRotate = 1f;
        }
        else
        {
            this.rigid2D.freezeRotation = false;
        }
    }

    public virtual void PreventFalling2()
    {
        if (this.transform.localEulerAngles.z > 150f)
        {
            Debug.Log("More than 150 degree");
            this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, 15);
        }

        else if (this.transform.localEulerAngles.z < -150f)
        {
            Debug.Log("Less than -150 degree");
            this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, -15);
        }
    }

    public virtual void StopMoving()
    {
        this.frontWheel.motor = this.frontStopSpeed;
        this.backWheel.motor = this.backStopSpeed;
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
            this.forwardSpeed.motorSpeed = this.movingSpeed;
            this.forwardSpeed.maxMotorTorque = this.movingSpeed * 2;
            this.frontWheel.motor = this.forwardSpeed;
            this.backWheel.motor = this.forwardSpeed;
            this.frontWheel.useMotor = true;
            this.backWheel.useMotor = true;

            this.movingState = MovingState.ToRight;

        }
        else if (this.MoveLeft())
        {
            this.backwardSpeed.motorSpeed = -this.movingSpeed;
            this.backwardSpeed.maxMotorTorque = this.movingSpeed * 2;
            this.frontWheel.motor = this.backwardSpeed;
            this.backWheel.motor = this.backwardSpeed;
            this.frontWheel.useMotor = true;
            this.backWheel.useMotor = true;

            this.movingState = MovingState.ToLeft;
        }
        else
        {
            this.frontWheel.motor = this.frontStopSpeed;
            this.backWheel.motor = this.backStopSpeed;
            this.frontWheel.useMotor = true;
            this.backWheel.useMotor = true;
            this.movingState = MovingState.None;
        }


        if (this.verticalMove && !(this is Bot))
        {
            if (Input.GetKey(KeyCode.E)){
                this.collider.isTrigger = true;
                this.fWheelCollider.isTrigger = true;
                this.bWheelCollider.isTrigger = true;
                this.transform.localEulerAngles = Vector3.zero;
                this.transform.Translate(new Vector3(0, this.verticalSpeed * Time.deltaTime, 0), Space.World);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                this.collider.isTrigger = true;
                this.fWheelCollider.isTrigger = true;
                this.bWheelCollider.isTrigger = true;
                this.transform.localEulerAngles = Vector3.zero;
                this.transform.Translate(new Vector3(0, -this.verticalSpeed * Time.deltaTime, 0), Space.World);
            }
            else
            {
                this.collider.isTrigger = false;
                this.fWheelCollider.isTrigger = false;
                this.bWheelCollider.isTrigger = false;
            }
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
        
        //if (this.velo.x < 0 && this.faceDirection == FaceDirection.RightLeft) return true;
        //else if (this.velo.x > 0 && this.faceDirection == FaceDirection.LeftRight) return true;
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
        this.bullet.AddDamageBuff((int)this.powerBuff);
        this.bullet.currentCollision = Bullet.CollisionType.None;
        this.bullet.currentStatus = Bullet.BulletStatus.Flying;
        this.bullet.gameObject.transform.rotation = Quaternion.identity;
        this.bullet.gameObject.transform.localScale = this.bullet.cachedScale;
        this.bullet.gameObject.SetActive(true);

        if (this.forceScale < 1f)
        {
            this.forceScale = 1f;
        }
        this.bullet.rbd.AddForce(this.forceVector * this.forceScale + new Vector2(GameManager.Instance.windSpeed * GameManager.Instance.windForceScaleFactor, 0));
        GameManager.Instance.SaveWindSpeed(); // for bot using
        
        //this.bullet.PlayFiringSound();
        
        GameManager.Instance.cameraController.FocusBullet(this.bullet.gameObject);
    }

    public virtual void Behit(int damage)
    {
        if (shieldBuff != 0)
        {
            damage = (int)(damage * 100 / (100 + shieldBuff));
            shieldBuff = 0;
        }
        
        this.hp -= damage;
        OnBehit.Invoke(this, damage);
        this.healthBar.SetHealth(this.hp);
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
            this.UpdateHp((int)this.healingCheat);
        }

        this.canMove = true;
        this.currentStatus = Status.Idle;
        GameManager.Instance.angleRuler.SetAngle(15f);

        // anniele
        this.ResetBuffs();
    }

    private void ResetBuffs()
    {
        // Only reset speed and power
        // Shield reset later
        // HP no need reset
        this.movingSpeed -= this.speedBuff;
        this.speedBuff = 0;
        this.powerBuff = 0;
    }

    protected void lockMovingOnFire() => this.canMove = false;
    protected void unlockMove() => this.canMove = true;

    protected virtual void UpdateWinLoseStatus()
    {
        // TODO
    }

    public virtual void SetHealthBar(HealthBar healthBar)
    {
        this.healthBar = healthBar;
        this.healthBar.SetMaxHealth(hp);
    }

    public virtual void UpdateHp(int delta)
    {
        int newHp = hp + delta;

        if (newHp > maxHp)
        {
            hp = maxHp;
        }
        else if (newHp < 0)
        {
            hp = 0;
        }
        else
        {
            hp = newHp; 
        }

        this.healthBar.SetHealth(hp);
    }

    public void AddBuff(BuffData buff)
    {
        if (this.buffMultiplier < 1f)
            this.buffMultiplier = 1f;
        switch (buff.buffType)
        {
            case BuffType.Speed:
                speedBuff += buff.buffValue * this.buffMultiplier;
                this.movingSpeed += buff.buffValue * this.buffMultiplier;
                break;

            case BuffType.Power:
                powerBuff += (buff.buffValue * this.bullet.damage) * this.buffMultiplier;
                break;

            case BuffType.Shield:
                shieldBuff += buff.buffValue * this.buffMultiplier;
                break;

            case BuffType.Health:
                UpdateHp((int)(buff.buffValue * this.buffMultiplier));
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Stair"))
        {
            this.verticalMove = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Stair"))
        {
            this.verticalMove = false;
        }
    }
}
