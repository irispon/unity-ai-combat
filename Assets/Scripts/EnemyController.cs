using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    public Transform m_Player;
    public CharacterState m_CharacterState;
    public Collider[] colliders;

    public float m_RotationSpeed = 0.2f;
    public float m_MoveSpeed = 5.0f;
    public float m_AggroDistance = 10.0f;
    public float m_AggroAngle = 90.0f;    

    private Vector3 m_PlayerDirection;
    private float m_PlayerDistance;
    private float m_PlayerAngle;

    private bool m_IsDead;

    static Animator anim;

    // Use this for initialization
    void Start () 
	{
        anim = GetComponent<Animator>();
        m_IsDead = false;
        colliders = this.GetComponentsInChildren<Collider>();
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (m_IsDead) return;

        if (m_CharacterState.m_Health <= 0)
        {
            MakeDead();
            return;            
        }
        calculatePlayerPositioning();

        if (IsAggroed())
        {            
            Face();            

            if (m_PlayerDirection.magnitude > 4)
            {
                Walk();
            }
            else
            {                
                Attack();
            }

        } else
        {
            Idle();
        }
    }

    private void calculatePlayerPositioning()
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

    // turns the char in the direction fo the player
    private void Face()
    {
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(m_PlayerDirection), m_RotationSpeed);
    }

    // walks forward
    private void Walk()
    {
        ClearAnim();
        anim.SetBool("isWalking", true);
        this.transform.Translate(0, 0, m_MoveSpeed * Time.deltaTime);
    }

    // char idle
    private void Idle()
    {
        ClearAnim();
        anim.SetBool("isIdle", true);
    }

    // char attack
    private void Attack()
    {
        ClearAnim();
        anim.SetBool("isAttacking", true);
    }

    // clear's all animations
    private void ClearAnim()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
    }

    // kill and cleanup enemy
    private void MakeDead()
    {
        ClearAnim();
        anim.SetBool("isDead", true);
        
        // disable all colliders
        foreach(Collider c in colliders)
        {
            Debug.Log("Disable collider: " + c.ToString());
            c.enabled = false;
        }
        m_IsDead = true;
    }
}
