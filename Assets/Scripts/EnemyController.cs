using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : HumanoidController {

    public GameObject m_Player;

    public float m_RotationSpeed = 0.2f;
    public float m_AggroDistance = 10.0f;
    public float m_AggroAngle = 90.0f;

    private Vector3 m_PlayerDirection;
    private float m_PlayerDistance;
    private float m_PlayerAngle;

    private bool m_IsSideStepping = false;
    private int m_SideStepDirection = 0;
    private float m_SideStepDuration = 1.0f;
    private float m_SideStepCooldownDuration = 10.0f;
    private float m_SideStepEnd = 0;
    private float m_SideStepCooldownEnd = 0;

    // Use this for initialization
    protected override void Start()
    {
        m_WeaponColliderName = "EnemyWeapon";
        base.Start();
    }

    // Update is called once per frame
    override protected void Update()
    {
        if (m_IsDead) return;
        if (IsPlayerDead())
        {
            ClearAnim();
            anim.SetBool("isIdle", true);
            return;
        }

        // if killed, do dead stuff
        if (m_CharacterState.m_Health <= 0)
        {
            MakeDead();
            return;
        }

        CalculatePlayerPositioning();

        // run away if dying
        if (m_CharacterState.IsDying())
        {
            AboutFace();
            Walk();
        }

        // if sidestepping, continue to sidestep
        else if (m_IsSideStepping)
        {
            SideStep();
        }

        // if aggro'd, approach and attack
        else if (IsAggroed())
        {
            Face();

            // approach
            if (m_PlayerDirection.magnitude > 4)
            {
                Walk();
            }

            // avoid attacks
            else if (IsPlayerAttacking() && ! IsSideStepOnCooldown())
            {
                StartSideStep();
            }

            // attack
            else if (!m_IsAttacking)
            {
                Attack();
            }
        }
        // otherwise, just stand around
        else
        {
            Idle();
        }
    }    

    private void CalculatePlayerPositioning()
    {
        m_PlayerDirection = m_Player.transform.position - this.transform.position;
        m_PlayerDirection.y = 0;

        m_PlayerDistance = Vector3.Distance(this.transform.position, m_Player.transform.position);
        m_PlayerAngle = Vector3.Angle(m_PlayerDirection, this.transform.forward);
    }

    // returns whether the player is within aggro range and LOS
    private bool IsAggroed()
    {
        return (m_PlayerDistance < m_AggroDistance && m_PlayerAngle < m_AggroAngle);
    }

    // turns the char in the direction of the player
    private void Face()
    {
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(m_PlayerDirection), m_RotationSpeed);
    }

    // turns the char in the opposite direction of the player
    private void AboutFace()
    {
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(-m_PlayerDirection), m_RotationSpeed);
    }

    // char idle
    private void Idle()
    {
        ClearAnim();
        anim.SetBool("isIdle", true);
    }

    // char walk (forward)
    private void Walk()
    {
        ClearAnim();
        anim.SetBool("isWalking", true);
        this.transform.Translate(0, 0, m_MoveSpeed * Time.deltaTime);
    }

    // returns whether the player is in the process of an attack
    private bool IsPlayerAttacking()
    {
        return m_Player.GetComponent<PlayerController>().m_IsAttacking;
    }

    // returns whether the player is dead
    private bool IsPlayerDead()
    {
        return m_Player.GetComponent<PlayerController>().m_IsDead;
    }

    // returns whether sidestep is on cooldown or not
    private bool IsSideStepOnCooldown()
    {
        return m_SideStepCooldownEnd > Time.time;
    }

    // char detects an attack and attempts to strafe around the player
    private void StartSideStep()
    {
        Debug.Log("Starting sidestep");
        m_IsSideStepping = true;
        m_SideStepEnd = Time.time + m_SideStepDuration;
        m_SideStepCooldownEnd = Time.time + m_SideStepCooldownDuration;
        m_SideStepDirection = Random.value > 0.5f ? 1 : -1;
        SideStep();
    }

    // char strafes around char (avoiding attacks)
    private void SideStep()
    {
        if(m_SideStepEnd < Time.time)
        {
            Debug.Log("Ending sidestep");
            m_IsSideStepping = false;
            return;
        }
        ClearAnim();
        anim.SetBool("isWalking", true);
        Face();
        this.transform.Translate(m_MoveSpeed * m_SideStepDirection * 2 * Time.deltaTime, 0, 0);
    }
}
