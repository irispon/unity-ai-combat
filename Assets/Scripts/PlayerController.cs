using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : HumanoidController {

    // Use this for initialization
    override protected void Start () 
	{
        Cursor.lockState = CursorLockMode.Locked;
        m_WeaponColliderName = "PlayerWeapon";
        base.Start();
	}
	
	// Update is called once per frame
	override protected void Update ()
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
}
