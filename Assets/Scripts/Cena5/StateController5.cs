using System;
using UnityEngine;

public class StateController5 : MonoBehaviour
{
  private enum Estado
  {
    Original,
    Direita1,
    Esquerda1,
    Estado2,
    Estado3
  }

  private float fps = 60f, newFPS;
  private Resistores5[] resistor = new Resistores5[5];
  private GameObject[] r = new GameObject[5];
  private GameObject red1Direita, red1Esquerda, red2;
  private bool[] reduzidos = new bool[5];
  private bool trava = false;
  private Estado estadoAtivo = Estado.Original;

  public bool spawn = true;
  public GameObject pecasCriadas, sombra1, sombra2, sombra3, sombra4, sombra5, circuitoUI, stock, circuito, vitoria, enunciado;

  private void Start()
  {
    InicializarTelas();
    SetarReducoes();

    for (int i = 0; i < 5; i++)
    {
      reduzidos[i] = false;
    }
  }

  private void SetarReducoes()
  {
    red1Direita = circuito.transform.GetChild(2).gameObject;
    red1Esquerda = circuito.transform.GetChild(1).gameObject;
    red2 = circuito.transform.GetChild(3).gameObject;
  }

  private void SetarResistores()
  {
    GameObject[] sombras = { sombra1, sombra2, sombra3, sombra4, sombra5 };

    for (int i = 0; i < 5; i++)
    {
      Collider2D check = Physics2D.OverlapCircle(new Vector2(sombras[i].transform.localPosition.x, sombras[i].transform.localPosition.y),
                                  0.1f,
                                 (1 << 6));

      if (reduzidos[i] == false)
      {
        if (check != null)
          r[i] = check.gameObject;
        else
          r[i] = null;

        if (!r[i])
          resistor[i] = r[i].GetComponent<Resistores5>();
        else
          resistor[i] = null;
      }
    }
    Debug.Log("r0 " + r[0]);
    Debug.Log("r1 " + r[1]);
  }

  private void InicializarTelas()
  {
    stock.SetActive(false);
    circuito.SetActive(false);
    vitoria.SetActive(false);
    circuitoUI.SetActive(false);
    enunciado.SetActive(true);
  }

  private void Update()
  {
    
    VelocidadeAnimacao();

    //SetarResistores();

    spawn = Physics2D.OverlapCircle(new Vector2(pecasCriadas.transform.localPosition.x, pecasCriadas.transform.localPosition.y), 0.1f, 1 << 6) == null;

    AnimacaoEsquerda1();
    //ChecarReducao1Esquerda();

    // ChecarReducao1Direita();

    // ChecarReducao2();

    // ChecarReducao3();

    switch (estadoAtivo)
    {
      case Estado.Esquerda1:
        AnimacaoEsquerda1();
        break;
    }
  }

  private void AnimacaoEsquerda1()
  {
    Debug.Log(red1Esquerda.transform.GetChild(1).transform.position.x != 0f);
    if (red1Esquerda.transform.GetChild(1).transform.position.x != 0f)
      red1Esquerda.transform.GetChild(1).transform.Translate((2.56f / fps), 0f, 0f, Space.Self);
  }

  private void VelocidadeAnimacao()
  {
    newFPS = 1.0f / Time.smoothDeltaTime;
    if (newFPS != float.PositiveInfinity)
      fps = Mathf.Lerp(fps, newFPS, 0.005f);
  }

  private void ChecarReducao1Esquerda()
  {
    /* Debug.Log(r[0]);
     Debug.Log(r[1]);
     Debug.Log(reduzidos[0]);
     Debug.Log(reduzidos[1]);
     Console.ReadLine();*/

    //reduzidos[0], reduzidos[1]
    if ((r[0] && r[1]) && (!reduzidos[0] || !reduzidos[1]))
    {
      reduzidos[0] = true;
      reduzidos[1] = true;
      estadoAtivo = Estado.Esquerda1;
      red1Esquerda.SetActive(true);
      red1Esquerda.transform.GetChild(0).GetComponent<Resistores5>().SetResistencia(resistor[0].GetResistencia() + resistor[1].GetResistencia());
    }
  }

  private void ChecarReducao1Direita()
  {
    throw new NotImplementedException();
  }

  private void ChecarReducao2()
  {
    throw new NotImplementedException();
  }

  private void ChecarReducao3()
  {
    throw new NotImplementedException();
  }
}