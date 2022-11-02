using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : Player
{
    public enum BulletProblem
    {
        None,
        NotReach,
        SoFar,
        Obstacle
    }

    public enum BotStrategy
    {
        None,
        MimicOnly,
        AdjustAngle,
        AdjustForce,
        AdjusBoth,
        Dummy,
    }

    public enum BotMovingDirection
    {
        None,
        Left,
        Right
    }



    [Header("BOT")]
    public Vector2 DirectionToTarget;
    public BotMovingDirection Direction;
    public float MovingTime = 3f;
    public float RemainMovingTime;
    public float PreAimedAngle;
    public float AngleModifyingFactor;
    public float ForceScaleFactor = 0.1f;
    public float ForceEpsilon;
    public float AngleEpsilon = 5f;
    public float AimingSpeed = 20f;
    public bool StartAiming;
    public bool StartAdjust;
    public bool FinishedAiming;
    public PathFinder pathFinder;
    public bool pathFound;
    public bool usedPathFinder;

    protected override void Start()
    {
        this.hp = 100;
        this.ownRole = GameManager.GameTurn.Bot;
        this.currentStatus = Status.Idle;
        this.movingState = MovingState.None;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.canMove = true;
        this.RemainMovingTime = this.MovingTime;
        this.Direction = BotMovingDirection.None;
        this.StartAiming = false;
        this.StartAdjust = false;
        this.FinishedAiming = false;

        this.AngleModifyingFactor = 5f;
        this.ForceScaleFactor = 1f;
        this.AngleEpsilon = 5f;
        this.AimingSpeed = 5f;
        this.pathFinder.OnFindPathComplete.AddListener(this.Fire);
        this.pathFound = false;
        this.usedPathFinder = false;


        GameManager.Instance.OnTurnChanged.AddListener(this.OnTurnChange);
        //ForceBar.Instance.OnPowerCompleted.AddListener(this.Fire);
        //ForceBar.Instance.OnPowerCompleted.AddListener(this.lockMovingOnFire);
    }
    public override void UpdateMove()
    {
        this.LookAtPlayer();
        this.Move();
        this.UpdateTankAnim();
        if (this.StartAiming)
            this.Direction = BotMovingDirection.None;
        if (this.Direction == BotMovingDirection.None)
        {
            this.StopMoving();
        }

        if (this.pathFound && !this.FinishedAiming)
        {
            this.PreAimedAngle = this.pathFinder.angle;
            if (this.PreAimedAngle > 90f && this.PreAimedAngle < 180f)
                this.PreAimedAngle = 180f - this.PreAimedAngle;
            this.force = this.pathFinder.force;
            this.SimulateAiming();
        }

        if (this.FinishedAiming && !this.fired)
        {
            this.CalculateForceVector();
            this.SetFireAnim();
            this.fired = true;
            this.currentStatus = Status.Attacking;
            GameManager.Instance.OnPlayerShoot.Invoke();
        }
    }

    private void LookAtPlayer()
    {
        this.DirectionToTarget = this.target.transform.position - this.transform.position;

        if (!this.IsLookingToTarget(this.DirectionToTarget.x))
        {
            this.ReverseFaceDirection();
        }
    }

    private bool IsLookingToTarget(float deltaX)
    {
        if (deltaX >= 0 && this.faceDirection == FaceDirection.LeftRight) return true;
        if (deltaX <= 0 && this.faceDirection == FaceDirection.RightLeft) return true;
        return false;
    }

    protected override void Move()
    {
        this.RemainMovingTime -= Time.deltaTime;
        if (this.RemainMovingTime > 0)
        {
            base.Move();
        }
        else
        {
            this.movingState = MovingState.None;
            this.Direction = BotMovingDirection.None;
            if (!this.pathFound && !this.usedPathFinder)
            {
                this.usedPathFinder = true;
                this.UsePathFinder();
            }
            
            //this.Fire();
        }
    }

    protected override bool MoveRight()
    {
        return this.Direction == BotMovingDirection.Right;
    }

    protected override bool MoveLeft()
    {
        return this.Direction == BotMovingDirection.Left;
    }

    public override void StopMoving()
    {
        base.StopMoving();
        this.Direction = BotMovingDirection.None;
    }

    public void UsePathFinder()
    {
        this.pathFinder.FindPath(Mathf.Atan(Mathf.Abs(this.DirectionToTarget.y / this.DirectionToTarget.x)) * 180f / Mathf.PI);
    }
    public override void Fire()
    {
        if (this.fired) return;
        if (this.ownRole == GameManager.Instance.currentTurn)
        {
            this.CalculateForceVector();

        }
    }
    private void SimulateAiming()
    {
        if (this.FinishedAiming) return;
        if (this.PreAimedAngle > 95f || this.PreAimedAngle < -20f)
        {
            this.FinishedAiming = true;
        }

        if (Mathf.Abs(this.angle - this.PreAimedAngle) < this.AngleEpsilon)
        {
            this.FinishedAiming = true;
        }
        else if (this.angle < this.PreAimedAngle)
        {
            GameManager.Instance.angleRuler.IncreaseAngle(Time.deltaTime * this.AimingSpeed);
            this.angle = GameManager.Instance.angleRuler.curAngle;
        }
        else
        {
            GameManager.Instance.angleRuler.IncreaseAngle(-Time.deltaTime * this.AimingSpeed);
            this.angle = GameManager.Instance.angleRuler.curAngle;
        }
    }


    private void CalculateForceVector()
    {
        if (this.faceDirection == FaceDirection.RightLeft)
        {
            this.angle = 180f - this.angle;
        }
        /*
        this.force = this.target.force + (GameManager.Instance.windSpeed - GameManager.Instance.windSpeedOfLastShot) * GameManager.Instance.windForceScaleFactor;
        if (this.DirectionToTarget.y < 0) this.force *= UnityEngine.Random.Range(0.7f, 1f);
        else this.force *= UnityEngine.Random.Range(1f, 2f);
        */
        this.angle = this.PreAimedAngle;
        if (this.faceDirection == FaceDirection.RightLeft) this.angle = 180f - this.angle;
        this.force = this.pathFinder.force;
        this.forceVector = new Vector2(Mathf.Cos(this.angle * Mathf.PI / 180f), Mathf.Sin(this.angle * Mathf.PI / 180f)) * this.force;
    }

    public override void OnTurnChange()
    {
        if (this.ownRole == GameManager.Instance.currentTurn)
        {
            this.fired = false;
        }

        this.canMove = true;
        this.angle = 0;
        GameManager.Instance.angleRuler.SetAngle(UnityEngine.Random.Range(15f, 80f));
        this.currentStatus = Status.Idle;
        this.StartAiming = false;
        this.StartAdjust = false;
        this.FinishedAiming = false;
        this.Direction = (BotMovingDirection)(UnityEngine.Random.Range(0, 3));
        this.RemainMovingTime = this.MovingTime;
        this.MovingTime = UnityEngine.Random.Range(0.5f, 2f);
        this.pathFound = false;
        this.usedPathFinder = false;
    }

    public override void UpdateHp(int delta)
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
}
