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
  private GameObject red1Direita, red1Esquerda, red2, original;
  private bool[] reduzidos = new bool[5];
  private bool trava = false;
  private Estado estadoAtivo = Estado.Original;

  public bool spawn = true;
  public Eventos5 evento;
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
    if (!evento.dragging)
    {
      GameObject[] sombras = { sombra1, sombra2, sombra3, sombra4, sombra5 };
      int i = 0;

      i = checaPosicao(i, sombras);
      i = checaPosicao(i, sombras);
      i = checaPosicao(i, sombras);
      i = checaPosicao(i, sombras);
      i = checaPosicao(i, sombras);
    }
  }

  private int checaPosicao(int i, GameObject[] sombras)
  {
    RaycastHit2D check = Physics2D.Raycast(new Vector2(sombras[i].transform.position.x, sombras[i].transform.position.y), Camera.main.transform.forward, Mathf.Infinity, (1 << 6));

    if (reduzidos[i] == false)
    {
      if (check.collider != null)
        r[i] = check.collider.gameObject;
      else
        r[i] = null;

      if (r[i] != null)
        resistor[i] = r[i].GetComponent<Resistores5>();
      else
        resistor[i] = null;
    }

    return ++i;
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

    SetarResistores();

    spawn = Physics2D.OverlapCircle(new Vector2(pecasCriadas.transform.localPosition.x, pecasCriadas.transform.localPosition.y), 0.1f, 1 << 6) == null;

    //AnimacaoEsquerda1();
    ChecarReducao1Esquerda();

    ChecarReducao1Direita();

    ChecarReducao2();

    // ChecarReducao3();

    switch (estadoAtivo)
    {
      case Estado.Esquerda1:
        AnimacaoEsquerda1();
        break;

      case Estado.Direita1:
        AnimacaoDireita1();
        break;

      case Estado.Estado2:
        Animacao2();
        break;
    }
  }

  private void Animacao2()
  {
    circuito.transform.GetChild(0).transform.GetChild(15).gameObject.SetActive(false);

    Color alpha;
    if (red1Direita.transform.GetChild(3).GetComponent<SpriteRenderer>().color.a > 0)
    {
      alpha = red1Direita.transform.GetChild(3).GetComponent<SpriteRenderer>().color;
      alpha.a -= 0.03332f;
      red1Direita.transform.GetChild(3).GetComponent<SpriteRenderer>().color = alpha;
      red1Direita.transform.GetChild(6).GetComponent<SpriteRenderer>().color = alpha;
      red1Esquerda.transform.GetChild(3).GetComponent<SpriteRenderer>().color = alpha;
      red1Esquerda.transform.GetChild(6).GetComponent<SpriteRenderer>().color = alpha;
    }
    if (red1Esquerda.transform.GetChild(3).GetComponent<SpriteRenderer>().color.a <= 0)
    {
      red1Direita.transform.GetChild(3).gameObject.SetActive(false);
      red1Direita.transform.GetChild(6).gameObject.SetActive(false);
      red1Esquerda.transform.GetChild(3).gameObject.SetActive(false);
      red1Esquerda.transform.GetChild(6).gameObject.SetActive(false);
    }

    Transform res1 = red1Esquerda.transform.GetChild(0).transform;
    Transform res2 = red1Direita.transform.GetChild(0).transform;

    if (res1.localPosition.y < -2.56f)
      res1.Translate((2.56f / fps), 0f, 0f, Space.World);
    else
      res1.gameObject.SetActive(false);

    if (res2.localPosition.y > -2.56f)
      res2.Translate((-2.56f / fps), 0f, 0f, Space.World);
    else
    {
      res2.gameObject.SetActive(false);
    }
      

    if (!res2.gameObject.activeInHierarchy && !res1.gameObject.activeInHierarchy)
    {
      red2.SetActive(true);
      estadoAtivo = Estado.Original;
    }

    /*circuito.transform.GetChild(0).transform.GetChild(6).gameObject.SetActive(false);
    circuito.transform.GetChild(0).transform.GetChild(7).gameObject.SetActive(false);
    circuito.transform.GetChild(0).transform.GetChild(20).gameObject.SetActive(false);
    circuito.transform.GetChild(0).transform.GetChild(21).gameObject.SetActive(false);
    circuito.transform.GetChild(0).transform.GetChild(8).gameObject.SetActive(false);
    circuito.transform.GetChild(0).transform.GetChild(19).gameObject.SetActive(false);

    Transform a = red1Direita.transform.GetChild(1).transform;
    Transform b = red1Direita.transform.GetChild(2).transform;
    Transform c = red1Direita.transform.GetChild(3).transform;

    if (a.localPosition.x < 0f)
    {
      a.Translate(0f, (-2.56f / fps), 0f, Space.World);
      b.Translate(0f, (-2.56f / fps), 0f, Space.World);
      c.Translate(0f, (-2.56f / fps), 0f, Space.World);
    }
    else
    {
      a.localPosition = new Vector3(0f, a.localPosition.y, a.localPosition.z);
      b.gameObject.SetActive(false);
      c.localPosition = new Vector3(0f, c.localPosition.y, c.localPosition.z);
    }

    Transform d = red1Direita.transform.GetChild(4).transform;
    Transform e = red1Direita.transform.GetChild(5).transform;
    Transform f = red1Direita.transform.GetChild(6).transform;

    if (d.localPosition.x > 0f)
    {
      d.Translate(0f, (2.56f / fps), 0f, Space.World);
      e.Translate(0f, (2.56f / fps), 0f, Space.World);
      f.Translate(0f, (2.56f / fps), 0f, Space.World);
    }
    else
    {
      d.localPosition = new Vector3(0f, d.localPosition.y, d.localPosition.z);
      e.gameObject.SetActive(false);
      f.localPosition = new Vector3(0f, f.localPosition.y, f.localPosition.z);
    }

    if (!e.gameObject.activeInHierarchy && !b.gameObject.activeInHierarchy)
    {
      estadoAtivo = Estado.Original;
      red1Direita.transform.GetChild(0).gameObject.SetActive(true);
    }*/
  }

  private void AnimacaoDireita1()
  {
    circuito.transform.GetChild(0).transform.GetChild(6).gameObject.SetActive(false);
    circuito.transform.GetChild(0).transform.GetChild(7).gameObject.SetActive(false);
    circuito.transform.GetChild(0).transform.GetChild(20).gameObject.SetActive(false);
    circuito.transform.GetChild(0).transform.GetChild(21).gameObject.SetActive(false);
    circuito.transform.GetChild(0).transform.GetChild(8).gameObject.SetActive(false);
    circuito.transform.GetChild(0).transform.GetChild(19).gameObject.SetActive(false);

    Transform a = red1Direita.transform.GetChild(1).transform;
    Transform b = red1Direita.transform.GetChild(2).transform;
    Transform c = red1Direita.transform.GetChild(3).transform;

    if (a.localPosition.x < 0f)
    {
      a.Translate(0f, (-2.56f / fps), 0f, Space.World);
      b.Translate(0f, (-2.56f / fps), 0f, Space.World);
      c.Translate(0f, (-2.56f / fps), 0f, Space.World);
    }
    else
    {
      a.localPosition = new Vector3(0f, a.localPosition.y, a.localPosition.z);
      b.gameObject.SetActive(false);
      c.localPosition = new Vector3(0f, c.localPosition.y, c.localPosition.z);
    }

    Transform d = red1Direita.transform.GetChild(4).transform;
    Transform e = red1Direita.transform.GetChild(5).transform;
    Transform f = red1Direita.transform.GetChild(6).transform;

    if (d.localPosition.x > 0f)
    {
      d.Translate(0f, (2.56f / fps), 0f, Space.World);
      e.Translate(0f, (2.56f / fps), 0f, Space.World);
      f.Translate(0f, (2.56f / fps), 0f, Space.World);
    }
    else
    {
      d.localPosition = new Vector3(0f, d.localPosition.y, d.localPosition.z);
      e.gameObject.SetActive(false);
      f.localPosition = new Vector3(0f, f.localPosition.y, f.localPosition.z);
    }

    if (!e.gameObject.activeInHierarchy && !b.gameObject.activeInHierarchy)
    {
      estadoAtivo = Estado.Original;
      red1Direita.transform.GetChild(0).gameObject.SetActive(true);
    }
  }

  private void AnimacaoEsquerda1()
  {
    circuito.transform.GetChild(0).transform.GetChild(9).gameObject.SetActive(false);
    circuito.transform.GetChild(0).transform.GetChild(11).gameObject.SetActive(false);
    circuito.transform.GetChild(0).transform.GetChild(22).gameObject.SetActive(false);
    circuito.transform.GetChild(0).transform.GetChild(24).gameObject.SetActive(false);
    circuito.transform.GetChild(0).transform.GetChild(10).gameObject.SetActive(false);
    circuito.transform.GetChild(0).transform.GetChild(23).gameObject.SetActive(false);

    Transform a = red1Esquerda.transform.GetChild(1).transform;
    Transform b = red1Esquerda.transform.GetChild(2).transform;
    Transform c = red1Esquerda.transform.GetChild(3).transform;

    if (a.localPosition.x < 0f)
    {
      a.Translate(0f, (-2.56f / fps), 0f, Space.World);
      b.Translate(0f, (-2.56f / fps), 0f, Space.World);
      c.Translate(0f, (-2.56f / fps), 0f, Space.World);
    }
    else
    {
      a.localPosition = new Vector3(0f, a.localPosition.y, a.localPosition.z);
      b.gameObject.SetActive(false);
      c.localPosition = new Vector3(0f, c.localPosition.y, c.localPosition.z);
    }

    Transform d = red1Esquerda.transform.GetChild(4).transform;
    Transform e = red1Esquerda.transform.GetChild(5).transform;
    Transform f = red1Esquerda.transform.GetChild(6).transform;

    if (d.localPosition.x > 0f)
    {
      d.Translate(0f, (2.56f / fps), 0f, Space.World);
      e.Translate(0f, (2.56f / fps), 0f, Space.World);
      f.Translate(0f, (2.56f / fps), 0f, Space.World);
    }
    else
    {
      d.localPosition = new Vector3(0f, d.localPosition.y, d.localPosition.z);
      e.gameObject.SetActive(false);
      f.localPosition = new Vector3(0f, f.localPosition.y, f.localPosition.z);
    }

    if (!e.gameObject.activeInHierarchy && !b.gameObject.activeInHierarchy)
    {
      estadoAtivo = Estado.Original;
      red1Esquerda.transform.GetChild(0).gameObject.SetActive(true);
    }
  }

  private void VelocidadeAnimacao()
  {
    newFPS = 1.0f / Time.smoothDeltaTime;
    if (newFPS != float.PositiveInfinity)
      fps = Mathf.Lerp(fps, newFPS, 0.005f);
  }

  private void ChecarReducao1Esquerda()
  {
    if ((r[0] && r[1]) && (!reduzidos[0] || !reduzidos[1]))
    {
      reduzidos[0] = true;
      reduzidos[1] = true;
      estadoAtivo = Estado.Esquerda1;
      red1Esquerda.SetActive(true);
      red1Esquerda.transform.GetChild(0).GetComponent<Resistores5>().SetResistencia(1 / ((1 / resistor[0].GetResistencia()) + (1 / resistor[1].GetResistencia())));
      r[0].SetActive(false);
      r[1].SetActive(false);
    }
  }

  private void ChecarReducao1Direita()
  {
    if ((r[2] && r[3]) && (!reduzidos[2] || !reduzidos[3]))
    {
      reduzidos[2] = true;
      reduzidos[3] = true;
      estadoAtivo = Estado.Direita1;
      red1Direita.SetActive(true);
      red1Direita.transform.GetChild(0).GetComponent<Resistores5>().SetResistencia(1 / ((1 / resistor[2].GetResistencia()) + (1 / resistor[3].GetResistencia())));
      r[2].SetActive(false);
      r[3].SetActive(false);
    }
  }

  private void ChecarReducao2()
  {
    if (reduzidos[0] && reduzidos[1] && reduzidos[2] && reduzidos[3] && estadoAtivo == Estado.Original)
    {
      estadoAtivo = Estado.Estado2;
      red2.transform.GetChild(0).GetComponent<Resistores5>().SetResistencia(red1Direita.transform.GetChild(0).GetComponent<Resistores5>().GetResistencia() + red1Esquerda.transform.GetChild(0).GetComponent<Resistores5>().GetResistencia());
    }
  }

  private void ChecarReducao3()
  {
    if (reduzidos[0] && reduzidos[1] && reduzidos[2] && reduzidos[3] && r[4] && !reduzidos[4])
    {

    }
  }
}