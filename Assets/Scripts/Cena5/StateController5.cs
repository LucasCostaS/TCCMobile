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
  private Vector2[] sombras = new Vector2[5];
  private GameObject red1Direita, red1Esquerda, red2, red3, original, preRed1E, preRed1D, preRed3;
  private bool[] reduzidos = new bool[5];
  private string primeiro;
  private Estado estadoAtivo = Estado.Original;

  public bool spawn = true, reduzir = true, trava = false;
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
    original = Instantiate(circuito);
    sombras = new Vector2[] { new Vector2(sombra1.transform.position.x, sombra1.transform.position.y),
                              new Vector2(sombra2.transform.position.x, sombra2.transform.position.y),
                              new Vector2(sombra3.transform.position.x, sombra3.transform.position.y),
                              new Vector2(sombra4.transform.position.x, sombra4.transform.position.y),
                              new Vector2(sombra5.transform.position.x, sombra5.transform.position.y) };
  }

  private void SetarReducoes()
  {
    red1Direita = circuito.transform.GetChild(2).gameObject;
    red1Esquerda = circuito.transform.GetChild(1).gameObject;
    red2 = circuito.transform.GetChild(3).gameObject;
    red3 = circuito.transform.GetChild(4).gameObject;
  }

  private void SetarResistores()
  {
    if (!evento.dragging)
    {
      int i = 0;

      i = checaPosicao(i, sombras);
      i = checaPosicao(i, sombras);
      i = checaPosicao(i, sombras);
      i = checaPosicao(i, sombras);
      i = checaPosicao(i, sombras);
    }
  }

  private int checaPosicao(int i, Vector2[] sombras)
  {
    RaycastHit2D check = Physics2D.Raycast(sombras[i], Camera.main.transform.forward, Mathf.Infinity, (1 << 6));

    if (reduzidos[i] == false)
    {
      if (check.collider != null)
      {
        r[i] = check.collider.gameObject;
        /*if (!evento.ordemSnap.Exists(x => x.gameObject == r[i]))
        {
          evento.ordemSnap.Add(r[i]);
        }*/
      }
      else
      {
        /*if (evento.ordemSnap.Exists(x => x.gameObject.transform.position == new Vector3 (sombras[i].x, sombras[i].y, 0f)))
        {
          evento.ordemSnap.Remove(evento.ordemSnap.Find(x => x.gameObject.transform.position == new Vector3(sombras[i].x, sombras[i].y, 0f)));
        }*/
        r[i] = null;
      }

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
    if (estadoAtivo == Estado.Esquerda1)
      AnimacaoEsquerda1();

    ChecarReducao1Direita();
    if (estadoAtivo == Estado.Direita1)
      AnimacaoDireita1();

    ChecarReducao2();
    if (estadoAtivo == Estado.Estado2)
      Animacao2();

    ChecarReducao3();
    if (estadoAtivo == Estado.Estado3)
      Animacao3();
  }

  private void Animacao3()
  {
    red2.transform.GetChild(3).transform.gameObject.SetActive(false);
    red2.transform.GetChild(4).transform.gameObject.SetActive(false);
    Vector3 mov = new Vector3(0f, -5.12f / fps, 0f);
    if (red2.transform.GetChild(0).transform.localPosition.x < red3.transform.GetChild(0).transform.localPosition.x)
    {
      red2.transform.GetChild(0).Translate(mov, Space.World);
      red2.transform.GetChild(5).Translate(mov, Space.World);
      red2.transform.GetChild(6).Translate(mov, Space.World);
    }
    else
    {
      red2.transform.GetChild(0).transform.localPosition = red3.transform.GetChild(0).transform.localPosition;
      red2.transform.GetChild(5).transform.gameObject.SetActive(false);
      red2.transform.GetChild(6).transform.gameObject.SetActive(false);
      red2.SetActive(false);
      r[4].SetActive(false);
      red3.SetActive(true);
    }

    if (red3.activeInHierarchy && red3.transform.GetChild(0).GetComponent<Resistores5>().GetResistencia() == 1m)
    {
      vitoria.SetActive(true);
      stock.SetActive(false);
      circuitoUI.SetActive(false);
    }
      
  }

  private void Animacao2()
  {
    spawn = false;
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
      res1.localPosition = new Vector3(res1.localPosition.x, -2.56f, res1.localPosition.z);

    if (res2.localPosition.y > -2.56f)
      res2.Translate((-2.56f / fps), 0f, 0f, Space.World);
    else
      res2.localPosition = new Vector3(res2.localPosition.x, -2.56f, res2.localPosition.z);

    if (res1.localPosition.y == -2.56f)
    {
      res2.gameObject.SetActive(false);
      res1.gameObject.SetActive(false);
    }

    if (!res2.gameObject.activeInHierarchy && !res1.gameObject.activeInHierarchy)
    {
      red1Esquerda.SetActive(false);
      red1Direita.SetActive(false);
      red2.SetActive(true);
      spawn = true;
    }
  }

  private void AnimacaoDireita1()
  {
    trava = true;
    spawn = false;
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
      red1Direita.transform.GetChild(0).gameObject.SetActive(true);
      spawn = true;
      trava = false;
    }
  }

  private void AnimacaoEsquerda1()
  {
    trava = true;
    spawn = false;
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
      red1Esquerda.transform.GetChild(0).gameObject.SetActive(true);
      spawn = true;
      trava = false;
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
    if ((r[0] && r[1]) && (!reduzidos[0] || !reduzidos[1]) && reduzir && estadoAtivo != Estado.Esquerda1)
    {
      if (primeiro == "")
        primeiro = "esquerda";
      reduzidos[0] = true;
      reduzidos[1] = true;
      estadoAtivo = Estado.Esquerda1;
      preRed1E = Instantiate(circuito);
      preRed1E.SetActive(false);
      red1Esquerda.SetActive(true);
      red1Esquerda.transform.GetChild(0).GetComponent<Resistores5>().SetResistencia(1 / ((1 / resistor[0].GetResistencia()) + (1 / resistor[1].GetResistencia())));
      r[0].SetActive(false);
      r[1].SetActive(false);
    }
  }

  private void ChecarReducao1Direita()
  {
    if ((r[2] && r[3]) && (!reduzidos[2] || !reduzidos[3]) && reduzir && estadoAtivo != Estado.Direita1)
    {
      if (primeiro == "")
        primeiro = "direita";
      reduzidos[2] = true;
      reduzidos[3] = true;
      estadoAtivo = Estado.Direita1;
      preRed1D = Instantiate(circuito);
      preRed1D.SetActive(false);
      red1Direita.SetActive(true);
      red1Direita.transform.GetChild(0).GetComponent<Resistores5>().SetResistencia(1 / ((1 / resistor[2].GetResistencia()) + (1 / resistor[3].GetResistencia())));;
      r[2].SetActive(false);
      r[3].SetActive(false);
    }
  }

  private void ChecarReducao2()
  {
    if (reduzidos[0] && reduzidos[1] && reduzidos[2] && reduzidos[3] && !reduzidos[4] && reduzir && estadoAtivo != Estado.Estado2 && !trava)
    {
      estadoAtivo = Estado.Estado2;
      red2.transform.GetChild(0).GetComponent<Resistores5>().SetResistencia(red1Direita.transform.GetChild(0).GetComponent<Resistores5>().GetResistencia() + red1Esquerda.transform.GetChild(0).GetComponent<Resistores5>().GetResistencia());
    }
  }

  private void ChecarReducao3()
  {
    if (red2.activeInHierarchy && r[4] && !reduzidos[4] && reduzir && estadoAtivo != Estado.Estado3)
    {
      reduzidos[4] = true;
      estadoAtivo = Estado.Estado3;

      red3.transform.GetChild(0).GetComponent<Resistores5>().SetResistencia(1 / ((1 / red2.transform.GetChild(0).GetComponent<Resistores5>().GetResistencia()) + (1 / resistor[4].GetResistencia())));
      preRed3 = Instantiate(circuito);
      preRed3.transform.GetChild(3).GetChild(0).GetComponent<Resistores5>().SetResistencia(red2.transform.GetChild(0).GetComponent<Resistores5>().GetResistencia());
      preRed3.SetActive(false);
    }
  }

  public void Desfazer()
  {
    if (estadoAtivo == Estado.Estado3)
    {
      Destroy(circuito);
      circuito = Instantiate(preRed3);
      SetarReducoes();

      red2.transform.GetChild(0).GetComponent<Resistores5>().SetResistencia(preRed3.transform.GetChild(3).GetChild(0).GetComponent<Resistores5>().GetResistencia());

      circuito.SetActive(true);

      Destroy(r[4]);
      SetarResistores();
      reduzidos[4] = false;
      Destroy(preRed3);
      estadoAtivo = Estado.Estado2;
      reduzir = false;
    }
    else if (estadoAtivo == Estado.Estado2)
    {
      Destroy(circuito);
      circuito = Instantiate(original);
      SetarReducoes();
      circuito.SetActive(true);

      Destroy(r[0]);
      Destroy(r[1]);
      Destroy(r[2]);
      Destroy(r[3]);
      SetarResistores();
      reduzidos[0] = false;
      reduzidos[1] = false;
      reduzidos[2] = false;
      reduzidos[3] = false;
      reduzidos[4] = false;
      Destroy(original);
      Destroy(preRed1D);
      Destroy(preRed1E);
      estadoAtivo = Estado.Original;
    }
    else if (estadoAtivo == Estado.Direita1)
    {
      red1Direita.SetActive(false);
      Destroy(circuito);
      circuito = Instantiate(preRed1D);
      SetarReducoes();
      circuito.SetActive(true); 
      Destroy(r[2]);
      Destroy(r[3]);
      reduzidos[2] = false;
      reduzidos[3] = false;

      Destroy(preRed1D);
      estadoAtivo = Estado.Original;
    }
    else if (estadoAtivo == Estado.Esquerda1)
    {
      red1Esquerda.SetActive(false);
      Destroy(circuito);
      circuito = Instantiate(preRed1E);
      SetarReducoes();
      circuito.SetActive(true);
      Destroy(r[0]);
      Destroy(r[1]);
      reduzidos[0] = false;
      reduzidos[1] = false;

      Destroy(preRed1E);
      estadoAtivo = Estado.Original;
    }
  }
}