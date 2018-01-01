using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanoidController : MonoBehaviour
{    
    public float m_MoveSpeed = 10.0f;
    public CharacterState m_CharacterState;
    public AudioClip m_WeaponSwingSound;
    public AudioClip m_DeathSound;

    protected float m_Move;
    protected float m_Strafe;
    public bool m_IsDead;
    public bool m_IsAttacking;

    protected Animator anim;
    protected AudioSource m_AudioSource;
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

        m_AudioSource = this.GetComponent<AudioSource>();

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
        m_IsAttacking = true;
        ClearAnim();
        anim.SetBool("isAttacking", true);
        StartCoroutine(EnableWeaponCollider());
        StartCoroutine(PlayWeaponSound());
        StartCoroutine(DisableWeaponCollider());
        StartCoroutine(ReenableAttacking());
    }

    protected IEnumerator EnableWeaponCollider()
    {
        yield return new WaitForSeconds(.7f);
         m_WeaponCollider.enabled = true;
    }

    protected IEnumerator DisableWeaponCollider()
    {
        yield return new WaitForSeconds(1.1f);
        m_WeaponCollider.enabled = false;
    }

    protected IEnumerator PlayWeaponSound()
    {
        yield return new WaitForSeconds(.9f);
        m_AudioSource.PlayOneShot(m_WeaponSwingSound);
    }

    protected IEnumerator ReenableAttacking()
    {
        yield return new WaitForSeconds(3.0f);
        m_IsAttacking = false;
    }

    // kill and cleanup enemy
    protected void MakeDead()
    {
        ClearAnim();
        anim.SetBool("isDead", true);
        m_AudioSource.PlayOneShot(m_DeathSound);

        // disable all colliders
        foreach (Collider c in m_AllColliders)
        {
            c.enabled = false;
        }
        m_IsDead = true;
    }
}
