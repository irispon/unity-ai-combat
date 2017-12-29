using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour {

    public float m_MoveSpeed = 10.0f;
    public float m_RotationSpeed = 100.0f;

    private float m_Move;
    private float m_Strafe;

    public CharacterState m_CharacterState;

    private bool m_IsDead;
    static Animator anim;


    // Use this for initialization
    void Start () 
	{
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        m_IsDead = false;
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

        if (Input.GetButton("Fire1"))
        {
            anim.SetBool("isAttacking", true);
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
    }

    // char walk
    private void Walk()
    {
        ClearAnim();
        anim.SetBool("isWalking", true);
        this.transform.Translate(m_Strafe, 0.0f, m_Move);
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
        anim.SetBool("isDead", false);
    }
}
