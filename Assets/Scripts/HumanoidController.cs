using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanoidController : MonoBehaviour
{    
    public float m_MoveSpeed = 10.0f;
    public CharacterState m_CharacterState;

    protected float m_Move;
    protected float m_Strafe;
    protected bool m_IsDead;
    protected bool m_IsAttacking;

    protected Animator anim;
    protected AudioSource swingAudioSource;
    protected Collider m_WeaponCollider;
    protected string m_WeaponColliderName = "Weapon";

    private Collider[] m_AllColliders;

    // Use this for initialization
    virtual protected void Start()
    {
        anim = GetComponent<Animator>();
        m_IsDead = false;
        m_IsAttacking = false;

        m_AllColliders = this.GetComponentsInChildren<Collider>();
        foreach (Collider collider in m_AllColliders)
        {
            if (collider.CompareTag(m_WeaponColliderName))
            {
                m_WeaponCollider = collider;
                break;
            }
        }

        swingAudioSource = this.GetComponent<AudioSource>();

        m_WeaponCollider.enabled = false;
    }

    // Update is called once per frame (leave this for parent classes to populate)
    virtual protected void Update()
    {
        
    }

    // clear all animations
    protected void ClearAnim()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isDead", false);
    }

    // char attack
    protected void Attack()
    {
        Debug.Log("Player attack begins");
        m_IsAttacking = true;
        ClearAnim();
        anim.SetBool("isAttacking", true);
        StartCoroutine(EnableWeaponCollider());
        StartCoroutine(PlayWeaponSound());
        StartCoroutine(DisableWeaponCollider());
    }

    protected IEnumerator EnableWeaponCollider()
    {
        yield return new WaitForSeconds(.6f);
        Debug.Log("Collider's enabled");
        m_WeaponCollider.enabled = true;
    }

    protected IEnumerator DisableWeaponCollider()
    {
        yield return new WaitForSeconds(1.2f);
        Debug.Log("Collider's disabled");
        m_WeaponCollider.enabled = false;
        m_IsAttacking = false;
        Debug.Log("Player attack ends");
    }

    protected IEnumerator PlayWeaponSound()
    {
        yield return new WaitForSeconds(.8f);
        swingAudioSource.Play();
    }

    // kill and cleanup enemy
    protected void MakeDead()
    {
        ClearAnim();
        anim.SetBool("isDead", true);

        // disable all colliders
        foreach (Collider c in m_AllColliders)
        {
            c.enabled = false;
        }
        m_IsDead = true;
    }
}
