using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Resistores3 : MonoBehaviour
{

    public float resistencia = 1;
    public bool modificado, reduzido;
    public GameObject controller;
    public bool caixaAtiva;
    void Start()
    {
        modificado = false;
        reduzido = false;
        caixaAtiva = false;
    }
    private void Update()
    {
        transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<TMP_Text>().SetText("Resistencia: " + GetComponent<Resistores3>().resistencia.ToString());
    }
    public void setarResistencia(string texto)
    {
        resistencia = float.Parse(texto);
        transform.GetChild(0).gameObject.SetActive(false);
        modificado = true;
        controller.GetComponent<StateController3>().click = true;
    }
    public void CaixaAtiva()
    {
        caixaAtiva = true;
        
    }
}
