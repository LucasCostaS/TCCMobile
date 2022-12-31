using TMPro;
using UnityEngine;

public class Resistores5 : MonoBehaviour
{
  private float resistencia;
  private StateController5 controller;
  public bool textoAtiva;
  private GameObject criador;
  private GameObject sombra;

  private void Start()
  {
    SetResistencia(float.Parse(criador.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().GetParsedText().Substring(0, 2)));
  }

  private void Update()
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