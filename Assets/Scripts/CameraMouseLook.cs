using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseLook : MonoBehaviour {

    public float m_Sensitivity = 5.0f;
    public float m_Smoothing = 2.0f;

    private Vector2 m_MouseLook;
    private Vector2 m_SmoothV;

    GameObject character;

	// Use this for initialization
	void Start () 
	{
        character = this.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () 
	{

        if(character.GetComponent<CharacterState>().m_Health <= 0)
        {
            return;
        }

        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(m_Sensitivity * m_Smoothing, m_Sensitivity * m_Smoothing));

        m_SmoothV.x = Mathf.Lerp(m_SmoothV.x, md.x, 1f / m_Smoothing);
        m_SmoothV.y = Mathf.Lerp(m_SmoothV.y, md.y, 1f / m_Smoothing);
        m_MouseLook += m_SmoothV;

        this.transform.localRotation = Quaternion.AngleAxis(-m_MouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(m_MouseLook.x, character.transform.up);
    }
}
