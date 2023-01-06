using System;
using TMPro;
using UnityEngine;

public class Resistores5 : MonoBehaviour
{
  private Decimal resistencia;
  private GameObject criador, sombra;

  private void Start()
  {
    if (this.tag == "Resistor")
      SetResistencia(decimal.Parse(criador.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().GetParsedText().Substring(0, 2)));
  }

  private void Update()
  {
  }

  public decimal GetResistencia()
  {
    return this.resistencia;
  }

  public void SetResistencia(decimal resistencia)
  {
    this.resistencia = Math.Round(resistencia, 2, MidpointRounding.ToEven);
  }

  public void SetPosicao(Vector2 posicao)
  {
    transform.position = posicao;
    Collider2D col = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y),
                                      0.5f,
                                      1 << 7);
    if (col != null)
    {
      SetSombra(Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y),
                                      0.5f,
                                      1 << 7).gameObject);
    }
    else
      SetSombra(null);
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