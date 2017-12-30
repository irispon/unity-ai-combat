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
    private bool m_IsAttacking;

    static Animator anim;

    private CharacterState characterState;
    private Collider m_WeaponCollider;

    // Use this for initialization
    void Start () 
	{
        anim = GetComponent<Animator>();
        characterState = this.GetComponentInParent<CharacterState>();

        m_IsDead = false;
        m_IsAttacking = false;

        Collider[] colliders = this.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("EnemyWeapon"))
            {
                m_WeaponCollider = collider;
                break;
            }
        }
        m_WeaponCollider.enabled = false;
    }
	
	// Update is called once per frame
	void Update () 
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
            else
            {
                //Attack();
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

    // clear's all animations
    private void ClearAnim()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
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

    // char attack
    private void Attack()
    {

        m_IsAttacking = true;
        anim.SetBool("isAttacking", true);
        StartCoroutine(EnableWeaponCollider());
        StartCoroutine(DisableWeaponCollider());
    }

    private IEnumerator EnableWeaponCollider()
    {
        yield return new WaitForSeconds(.6f);
        Debug.Log("Collider's enabled");
        m_WeaponCollider.enabled = true;
    }

    private IEnumerator DisableWeaponCollider()
    {
        yield return new WaitForSeconds(1.2f);
        Debug.Log("Collider's disabled");
        m_WeaponCollider.enabled = false;
        m_IsAttacking = false;
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
