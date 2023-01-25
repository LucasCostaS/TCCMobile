using UnityEngine;

public class StateController6 : MonoBehaviour
{
  private enum Estado
  {
    Original,
    Direita2,
    Esquerda2,
    Estado1,
    Estado3
  }

  private float fps = 60f, newFPS;
  private Resistores6[] resistor = new Resistores6[5];
  private GameObject[] r = new GameObject[5];
  private Vector2[] sombras = new Vector2[5];
  private GameObject red2Direita, red2Esquerda, sombra1, sombra2, sombra3, sombra4, sombra5, red1, red3, original, preRed2E, preRed2D;
  private bool[] reduzidos = new bool[5];
  private Estado estadoAtivo = Estado.Original;

  public bool spawn = true, reduzir = true, trava = false;
  public Eventos6 evento;
  public GameObject pecasCriadas, circuitoUI, stock, circuito, vitoria, enunciado;

  private void Start()
  {
    InicializarTelas();
    SetarReducoes();
    SetarSombras();

    for (int i = 0; i < 5; i++)
    {
      reduzidos[i] = false;
    }
    original = Instantiate(circuito);
    sombras = new Vector2[] { new Vector2(sombra1.transform.position.x, sombra1.transform.position.y),
                              new Vector2(sombra2.transform.position.x, sombra2.transform.position.y),
                              new Vector2(sombra3.transform.position.x, sombra3.transform.position.y),
                              new Vector2(sombra4.transform.position.x, sombra4.transform.position.y),
                              new Vector2(sombra5.transform.position.x, sombra5.transform.position.y)};
  }

  private void SetarReducoes()
  {
    red2Direita = circuito.transform.GetChild(3).gameObject;
    red2Esquerda = circuito.transform.GetChild(2).gameObject;
    red1 = circuito.transform.GetChild(1).gameObject;
    red3 = circuito.transform.GetChild(4).gameObject;
  }

  private void SetarSombras()
  {
    sombra1 = circuito.transform.GetChild(0).GetChild(0).gameObject;
    sombra2 = circuito.transform.GetChild(0).GetChild(1).gameObject;
    sombra3 = circuito.transform.GetChild(0).GetChild(2).gameObject;
    sombra4 = circuito.transform.GetChild(0).GetChild(3).gameObject;
    sombra5 = circuito.transform.GetChild(0).GetChild(4).gameObject;
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
        r[i] = check.collider.gameObject;
      else
        r[i] = null;

      if (r[i] != null)
        resistor[i] = r[i].GetComponent<Resistores6>();
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
    ChecarReducao2Esquerda();
    if (estadoAtivo == Estado.Esquerda2 && circuito.transform.GetChild(0).childCount > 24)
      AnimacaoEsquerda();

    ChecarReducao2Direita();
    if (estadoAtivo == Estado.Direita2 && circuito.transform.GetChild(0).childCount > 24)
      AnimacaoDireita();

    ChecarReducao1();
    if (estadoAtivo == Estado.Estado1)
      Animacao1();

    ChecarReducao3();
    if (estadoAtivo == Estado.Estado3)
      Animacao3();
  }

  private void Animacao3()
  {
    red2Direita.transform.GetChild(2).transform.gameObject.SetActive(false);
    red2Direita.transform.GetChild(3).transform.gameObject.SetActive(false);
    red2Esquerda.transform.GetChild(2).transform.gameObject.SetActive(false);
    red2Esquerda.transform.GetChild(3).transform.gameObject.SetActive(false);
    circuito.transform.GetChild(0).GetChild(18).gameObject.SetActive(false);

    if (red2Esquerda.transform.GetChild(0).transform.localPosition.y < 0f)
    {
      red2Esquerda.transform.GetChild(0).Translate(5.12f / fps, 0f, 0f, Space.World);
    }
    else
    {
      red2Esquerda.transform.GetChild(0).transform.localPosition = new Vector3(red2Esquerda.transform.GetChild(0).transform.localPosition.x, 0f, red2Esquerda.transform.GetChild(0).transform.localPosition.z);
      red2Esquerda.SetActive(false);
    }

    if (red2Direita.transform.GetChild(0).transform.localPosition.y > 0f)
    {
      red2Direita.transform.GetChild(0).Translate(-5.12f / fps, 0f, 0f, Space.World);
    }
    else
    {
      red2Direita.transform.GetChild(0).transform.localPosition = new Vector3(red2Direita.transform.GetChild(0).transform.localPosition.x, 0f, red2Direita.transform.GetChild(0).transform.localPosition.z);
      red2Direita.SetActive(false);
    }

    if (!red2Direita.activeInHierarchy && !red2Esquerda.activeInHierarchy)
    {
      red3.SetActive(true);
    }

    if (red3.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia() == 4)
    {
      vitoria.SetActive(true);
      stock.SetActive(false);
      circuitoUI.SetActive(false);
    }
  }

  private void Animacao1()
  {
    trava = true;
    spawn = false;

    Transform res1 = red1.transform.GetChild(0).transform;
    sombra5.gameObject.SetActive(false);
    sombra3.gameObject.SetActive(false);

    if (res1.localRotation.eulerAngles.z < 90f)
      res1.Rotate(0f, 0f, 90f / (fps * 0.75f), Space.World);
    else
      res1.rotation.SetEulerAngles(0f, 0f, 90f);

    if (res1.localPosition.y < -5.12f)
      res1.Translate((5.12f / fps), 0f, 0f, Space.World);

    if (res1.localPosition.x < 7.68f)
      res1.Translate(0f, (-7.68f / fps), 0f, Space.World);

    if (res1.localPosition.x >= sombra5.transform.localPosition.x && res1.localRotation.eulerAngles.z >= 90f)
    {
      res1.position = sombra5.transform.position;
      res1.rotation = sombra5.transform.rotation;
      r[4].gameObject.SetActive(false);
      red1.transform.GetChild(1).gameObject.SetActive(true);
      red1.transform.GetChild(2).gameObject.SetActive(true);
      red1.transform.GetChild(3).gameObject.SetActive(true);
      spawn = true;
      trava = false;
    }
  }

  private void AnimacaoDireita()
  {
    trava = true;
    spawn = false;
    sombra1.gameObject.SetActive(false);
    sombra4.gameObject.SetActive(false);
    circuito.transform.GetChild(0).GetChild(16).gameObject.SetActive(false);

    Transform a = circuito.transform.GetChild(0).GetChild(10).transform;
    Transform b = circuito.transform.GetChild(0).GetChild(24).transform;
    Transform c = circuito.transform.GetChild(0).GetChild(12).transform;

    if (a.localPosition.x < 2.56f)
    {
      a.Translate(0f, (-2.56f / fps), 0f, Space.World);
      b.Translate(0f, (-2.56f / fps), 0f, Space.World);
      c.Translate(0f, (-2.56f / fps), 0f, Space.World);
    }
    else
    {
      a.localPosition = new Vector3(2.56f, a.localPosition.y, a.localPosition.z);
      b.gameObject.SetActive(false);
      c.localPosition = new Vector3(2.56f, c.localPosition.y, c.localPosition.z);
    }

    Transform d = circuito.transform.GetChild(0).GetChild(20).transform;
    Transform e = circuito.transform.GetChild(0).GetChild(25).transform;
    Transform f = circuito.transform.GetChild(0).GetChild(21).transform;

    if (d.localPosition.x > 2.56f)
    {
      d.Translate(0f, (2.56f / fps), 0f, Space.World);
      e.Translate(0f, (2.56f / fps), 0f, Space.World);
      f.Translate(0f, (2.56f / fps), 0f, Space.World);
    }
    else
    {
      d.localPosition = new Vector3(2.56f, d.localPosition.y, d.localPosition.z);
      e.gameObject.SetActive(false);
      f.localPosition = new Vector3(2.56f, f.localPosition.y, f.localPosition.z);
    }

    if (!e.gameObject.activeInHierarchy && !b.gameObject.activeInHierarchy)
    {
      red2Direita.SetActive(true);
      Destroy(circuito.transform.GetChild(0).GetChild(25).gameObject);
      Destroy(circuito.transform.GetChild(0).GetChild(24).gameObject);
      circuito.transform.GetChild(0).GetChild(10).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(12).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(20).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(21).gameObject.SetActive(false);
      spawn = true;
      trava = false;
    }
  }

  private void AnimacaoEsquerda()
  {
    trava = true;
    spawn = false;
    sombra2.gameObject.SetActive(false);

    red1.transform.GetChild(3).gameObject.SetActive(false);

    circuito.transform.GetChild(0).GetChild(23).gameObject.SetActive(false);
    circuito.transform.GetChild(0).GetChild(22).gameObject.SetActive(false);

    Transform a = circuito.transform.GetChild(0).GetChild(13).transform;
    Transform b = circuito.transform.GetChild(0).GetChild(24).transform;
    Transform c = circuito.transform.GetChild(0).GetChild(14).transform;

    if (a.localPosition.x < 2.56f)
    {
      a.Translate(0f, (-2.56f / fps), 0f, Space.World);
      b.Translate(0f, (-2.56f / fps), 0f, Space.World);
      c.Translate(0f, (-2.56f / fps), 0f, Space.World);
    }
    else
    {
      a.localPosition = new Vector3(2.56f, a.localPosition.y, a.localPosition.z);
      b.gameObject.SetActive(false);
      c.localPosition = new Vector3(2.56f, c.localPosition.y, c.localPosition.z);
    }

    Transform d = red1.transform.GetChild(1).transform;
    Transform e = red1.transform.GetChild(0).transform;
    Transform f = red1.transform.GetChild(2).transform;

    if (d.localPosition.x > 2.56f)
    {
      d.Translate(0f, (2.56f / fps), 0f, Space.World);
      e.Translate(0f, (2.56f / fps), 0f, Space.World);
      f.Translate(0f, (2.56f / fps), 0f, Space.World);
    }
    else
    {
      d.localPosition = new Vector3(2.56f, d.localPosition.y, d.localPosition.z);
      e.gameObject.SetActive(false);
      f.localPosition = new Vector3(2.56f, f.localPosition.y, f.localPosition.z);
    }

    if (!e.gameObject.activeInHierarchy && !b.gameObject.activeInHierarchy)
    {
      red2Esquerda.SetActive(true);
      Destroy(circuito.transform.GetChild(0).GetChild(24).gameObject);
      circuito.transform.GetChild(0).GetChild(13).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(14).gameObject.SetActive(false);
      red1.SetActive(false);
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

  private void ChecarReducao2Esquerda()
  {
    if (r[1] && reduzidos[4] && !reduzidos[1] && reduzir && estadoAtivo != Estado.Esquerda2 && !trava)
    {
      reduzidos[1] = true;
      estadoAtivo = Estado.Esquerda2;
      preRed2E = Instantiate(circuito);
      preRed2E.SetActive(false);
      red2Esquerda.transform.GetChild(0).GetComponent<Resistores6>().SetResistencia(1 / ((1 / resistor[1].GetResistencia()) + (1 / red1.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia())));
      r[1].SetActive(false);
      Instantiate(red2Esquerda.transform.GetChild(0), sombra2.transform.position, sombra2.transform.rotation, circuito.transform.GetChild(0).transform);
    }
  }

  private void ChecarReducao2Direita()
  {
    if ((r[0] && r[3]) && (!reduzidos[0] || !reduzidos[3]) && reduzir && estadoAtivo != Estado.Direita2 && !trava)
    {
      reduzidos[0] = true;
      reduzidos[3] = true;
      estadoAtivo = Estado.Direita2;
      preRed2D = Instantiate(circuito);
      preRed2D.SetActive(false);
      // red2Direita.SetActive(true);
      red2Direita.transform.GetChild(0).GetComponent<Resistores6>().SetResistencia(1 / ((1 / resistor[0].GetResistencia()) + (1 / resistor[3].GetResistencia())));
      r[0].SetActive(false);
      r[3].SetActive(false);
      Instantiate(red2Direita.transform.GetChild(0), sombra1.transform.position, sombra1.transform.rotation, circuito.transform.GetChild(0).transform);
      Instantiate(red2Direita.transform.GetChild(0), sombra4.transform.position, sombra4.transform.rotation, circuito.transform.GetChild(0).transform);
    }
  }

  private void ChecarReducao1()
  {
    if (r[2] && r[4] && !reduzidos[2] && !reduzidos[4] && reduzir && estadoAtivo != Estado.Estado1 && !trava)
    {
      estadoAtivo = Estado.Estado1;
      reduzidos[4] = true;
      reduzidos[2] = true;
      r[2].gameObject.SetActive(false);
      red1.gameObject.SetActive(true);
      red1.transform.GetChild(0).GetComponent<Resistores6>().SetResistencia(resistor[2].GetResistencia() + resistor[4].GetResistencia());
    }
  }

  private void ChecarReducao3()
  {
    if (red2Esquerda.activeInHierarchy && red2Direita.activeInHierarchy && !trava && reduzir && estadoAtivo != Estado.Estado3)
    {
      estadoAtivo = Estado.Estado3;

      red3.transform.GetChild(0).GetComponent<Resistores6>().SetResistencia(red2Direita.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia() + red2Esquerda.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia());
    }
  }

  public void Desfazer()
  {
    if (estadoAtivo == Estado.Estado3)
    {
      Destroy(circuito);
      circuito = Instantiate(original);

      reduzidos[0] = false;
      reduzidos[1] = false;
      reduzidos[2] = false;
      reduzidos[3] = false;
      reduzidos[4] = false;

      Destroy(r[0]);
      Destroy(r[1]);
      Destroy(r[3]);
      Destroy(r[2]);
      Destroy(r[4]);

      circuito.SetActive(true);
      Destroy(original);
      Destroy(preRed2D);
      Destroy(preRed2E);

      SetarReducoes();
      SetarSombras();
      estadoAtivo = Estado.Original;
      reduzir = false;
    }
    else if (estadoAtivo == Estado.Estado1)
    {
      Destroy(r[4]);
      Destroy(r[2]);
      reduzidos[2] = false;
      reduzidos[4] = false;

      red1.SetActive(false);

      red1.transform.GetChild(0).transform.position = sombra3.transform.position;
      red1.transform.GetChild(0).transform.rotation = sombra3.transform.rotation;
      red1.transform.GetChild(1).gameObject.SetActive(false);
      red1.transform.GetChild(2).gameObject.SetActive(false);
      red1.transform.GetChild(3).gameObject.SetActive(false);

      sombra5.gameObject.SetActive(true);
      sombra3.gameObject.SetActive(true);

      if (circuito.transform.GetChild(3).gameObject.activeInHierarchy)
        estadoAtivo = Estado.Direita2;
      else
        estadoAtivo = Estado.Original;

      SetarResistores();
      SetarSombras();

    }
    else if (estadoAtivo == Estado.Direita2)
    {
      red2Direita.SetActive(false);
      Destroy(circuito);
      circuito = Instantiate(preRed2D);

      circuito.SetActive(true);
      Destroy(r[0]);
      Destroy(r[3]);
      reduzidos[0] = false;
      reduzidos[3] = false;

      Destroy(preRed2D);
      SetarReducoes();
      SetarSombras();

      if (circuito.transform.GetChild(1).gameObject.activeInHierarchy)
      {
        estadoAtivo = Estado.Estado1;
        Desfazer();
      }
      else
      {
        estadoAtivo = Estado.Original;
      }
    }
    else if (estadoAtivo == Estado.Esquerda2)
    {
      red2Esquerda.SetActive(false);
      Destroy(circuito);
      circuito = Instantiate(preRed2E);

      circuito.SetActive(true);
      Destroy(r[1]);
      reduzidos[1] = false;

      Destroy(preRed2E);
      SetarReducoes();
      SetarSombras();

      if (circuito.transform.GetChild(3).gameObject.activeInHierarchy)
        estadoAtivo = Estado.Direita2;
      else
        estadoAtivo = Estado.Estado1;

      Desfazer();
    }
  }
}