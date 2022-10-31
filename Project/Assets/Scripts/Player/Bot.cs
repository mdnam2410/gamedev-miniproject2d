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

    public struct TurnInfo
    {
        public Vector2 BotPos;
        public Vector2 PlayerPos;
        public Vector2 ExplosionPos;
        public float Force;
        public float Angle;
        public float WindSpeed;
        public float TimeExplosion;
    };

    public TurnInfo LastTurnInfo;

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
    public bool FinishedAiming;

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
        this.FinishedAiming = false;

        GameManager.Instance.OnTurnChanged.AddListener(this.OnTurnChange);
        //ForceBar.Instance.OnPowerCompleted.AddListener(this.Fire);
        //ForceBar.Instance.OnPowerCompleted.AddListener(this.lockMovingOnFire);
    }
    public override void UpdateMove()
    {
        this.LookAtPlayer();
        this.Move();
        this.UpdateTankAnim();
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
            this.Fire();
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


    public override void Fire()
    {
        if (this.fired) return;
        if (this.ownRole == GameManager.Instance.currentTurn)
        {
            this.CalculateForceVector();

            if (this.FinishedAiming)
            {
                this.SetFireAnim();
                this.fired = true;
                this.currentStatus = Status.Attacking;
                GameManager.Instance.OnPlayerShoot.Invoke();
            }
        }
    }
    protected override void CalculateForceVector()
    {
        this.SimulateAiming();
        this.SimulateGetForce();
    }
    private void SimulateAiming()
    {
        this.CalculatePreAimedAngle();
        this.SimulateAdjustingAngle();
    }

    private void SimulateAdjustingAngle()
    {
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

    private void CalculatePreAimedAngle()
    {
        if (this.StartAiming) return;

        this.PreAimedAngle = this.target.angle + this.DirectionToTarget.y * this.AngleModifyingFactor;
        this.PreAimedAngle = Mathf.Clamp(this.PreAimedAngle, this.target.angle - 15f, this.target.angle + 15f);
        this.PreAimedAngle = Mathf.Clamp(this.PreAimedAngle, 0f, 90f);
        GameManager.Instance.angleRuler.SetAngle(UnityEngine.Random.Range(0f, 30f));
        this.StartAiming = true;
    }

    private void SimulateGetForce()
    {
        if (!this.FinishedAiming) return;

        if (this.faceDirection == FaceDirection.RightLeft)
        {
            this.angle = 180f - this.angle;
        }

        this.force = this.target.force + (GameManager.Instance.windSpeed - GameManager.Instance.windSpeedOfLastShot) * GameManager.Instance.windForceScaleFactor;
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
        this.currentStatus = Status.Idle;
        this.StartAiming = false;
        this.FinishedAiming = false;
        this.Direction = (BotMovingDirection)(UnityEngine.Random.Range(0, 3));
        this.RemainMovingTime = this.MovingTime;
    }
}
