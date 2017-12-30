using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterState : MonoBehaviour {

    public int m_Health;
    public Slider m_HealthSlider;

    public int m_StartingHealth = 100;
    public int m_DyingThreshold = 20;
    
    public void Awake()
    {
        m_Health = m_StartingHealth;
    }

    public void TakeDamage(int damage)
    {
        m_Health -= damage;
        UpdateHealthSlider();
    }

    public bool IsDying()
    {
        return m_Health <= m_DyingThreshold;
    }

    private void UpdateHealthSlider()
    {
        m_HealthSlider.value = m_Health;
    }
}
