using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour {

    public float m_MoveSpeed = 10.0f;
    public float m_RotationSpeed = 10.0f;

    private float m_Move;
    private float m_Strafe;

    public CharacterState m_CharacterState;

    private bool m_IsDead;
    private bool m_IsAttacking;
    static Animator anim;

    private Collider m_WeaponCollider;

    // Use this for initialization
    void Start () 
	{
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        m_IsDead = false;
        m_IsAttacking = false;

        Collider[] colliders = this.GetComponentsInChildren<Collider>();
        foreach(Collider collider in colliders)
        {
            if(collider.CompareTag("PlayerWeapon"))
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

        if (m_CharacterState.m_Health <= 0)
        {
            ClearAnim();
            anim.SetBool("isDead", true);
            m_IsDead = true;
            return;
        }
        
        m_Move = Input.GetAxis("Vertical") * m_MoveSpeed * Time.deltaTime;
        m_Strafe = Input.GetAxis("Horizontal") * m_MoveSpeed * Time.deltaTime;

        transform.Translate(m_Strafe, 0, m_Move);

        if (Input.GetButton("Fire1") && ! m_IsAttacking)
        {
            Attack();
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }

        if (m_Move != 0)
        {
            anim.SetBool("isWalking", true);
            anim.SetBool("isIdle", false);
        }
        else
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isIdle", true);
        }

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // clear's all animations
    private void ClearAnim()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isDead", false);
    }

    // char attack
    private void Attack()
    {
        Debug.Log("Player attack begins");
        m_IsAttacking = true;
        ClearAnim();
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
        Debug.Log("Player attack ends");
    }  
}
