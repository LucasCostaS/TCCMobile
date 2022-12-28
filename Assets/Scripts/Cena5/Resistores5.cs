using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resistores5 : MonoBehaviour
{
    private float resistencia;
    public bool modificado, reduzido;
    private StateController5 controller;
    public bool textoAtiva;
    
    void Start()
    {
        controller = transform.parent.parent.GetComponent<StateController5>();
    }

    void Update()
    {
        
    }

    public float GetResistencia()
    {
        return this.resistencia;
    }

    public void SetResistencia(float resistencia)
    {
        this.resistencia = resistencia;
    }
}


