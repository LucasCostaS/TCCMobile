using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Resistores5 : MonoBehaviour
{
  private float resistencia;
  private StateController5 controller;
  public bool textoAtiva;
  private GameObject criador;
  private GameObject sombra;

  void Start()
  {
    SetResistencia(float.Parse(criador.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().GetParsedText().Substring(0, 2)));
  }

  void Update()
  {
    sombra = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y), new Vector2(0.5f, 0.5f), 0f, (1 << 7)).gameObject;
  }

  public float GetResistencia()
  {
    return this.resistencia;
  }

  public void SetResistencia(float resistencia)
  {
    this.resistencia = resistencia;
  }

  public GameObject GetCriador()
  {
    return this.criador;
  }

  public void SetCriador(GameObject criador)
  {
    this.criador = criador;
  }

  public GameObject GetSombra()
  {
    return this.sombra;
  }

  public void SetSombra(GameObject sombra)
  {
    this.sombra = sombra;
  }
}


