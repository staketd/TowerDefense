using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GravitationalMovement : MonoBehaviour {
    [SerializeField] 
    private float m_Mass;

    [SerializeField] 
    private Vector3 m_Start;

    [SerializeField]
    private Vector3 m_MPosition;

    [SerializeField]
    private double m_MMass;
    
    [SerializeField] 
    private Vector3 m_Velocity;

    private const float m_G = 6.67408e-11f;

    private const float TOLERANCE = 0.8f;

    void Start() {
        transform.position = m_Start;
        
    }

    void Update() {
    }

    private void FixedUpdate() {
        Vector3 pathVector = m_MPosition - transform.position;
        float distance = pathVector.magnitude;
        if (distance < TOLERANCE) {
            return;
        }
        Vector3 acceleration = (float)(m_G * m_MMass / distance / distance ) * pathVector.normalized;

        Vector3 delta = m_Velocity * Time.fixedDeltaTime + acceleration * (Time.fixedDeltaTime * Time.fixedDeltaTime) / 2;
        m_Velocity = m_Velocity + acceleration * Time.fixedDeltaTime;
        transform.Translate(delta);
    }
}