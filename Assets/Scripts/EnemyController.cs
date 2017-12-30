using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : HumanoidController {

    public Transform m_Player;

    public float m_RotationSpeed = 0.2f;
    public float m_AggroDistance = 10.0f;
    public float m_AggroAngle = 90.0f;

    private Vector3 m_PlayerDirection;
    private float m_PlayerDistance;
    private float m_PlayerAngle;

    // Use this for initialization
    protected override void Start()
    {
        m_WeaponColliderName = "EnemyWeapon";
        base.Start();
    }

    // Update is called once per frame
    override protected void Update () 
	{
        if (m_IsDead) return;

        // if killed, do dead stuff
        if (m_CharacterState.m_Health <= 0)
        {
            MakeDead();
            return;            
        }

        CalculatePlayerPositioning();

        // run away if dying
        if(m_CharacterState.IsDying())
        {
            AboutFace();
            Walk();
        }

        // if aggro'd, approach and attack
        else if (IsAggroed())
        {            
            Face();            

            if (m_PlayerDirection.magnitude > 4)
            {
                Walk();
            }
            else if(!m_IsAttacking)
            {
                Debug.Log("enemy begin attack");
                Attack();
            }
        }
        // otherwise, just stand around
        else
        {
            Debug.Log("enemy idle");
            Idle();
        }
    }

    private void CalculatePlayerPositioning()
    {
        m_PlayerDirection = m_Player.position - this.transform.position;
        m_PlayerDirection.y = 0;

        m_PlayerDistance = Vector3.Distance(this.transform.position, m_Player.position);
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
}
