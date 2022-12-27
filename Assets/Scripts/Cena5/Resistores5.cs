using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resistores5 : MonoBehaviour
{
    private float resistencia;
    public bool modificado, reduzido;
    public GameObject controller;
    public bool caixaAtiva;
    void Start()
    {
        
    }

    // Update is called once per frame
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


