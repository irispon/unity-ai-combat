using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitDetector : MonoBehaviour {

    public string opponent;

    private CharacterState m_CharacterState;
    private int m_Damage = 50;

    public void Start()
    {
        m_CharacterState = this.transform.root.GetComponent<CharacterState>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT: " + other.tag + " -> " + this.tag);

        if(other.CompareTag(opponent + "Weapon"))
        {
            m_CharacterState.TakeDamage(m_Damage);
        }
    }
}
