using UnityEngine;

public class StateController6 : MonoBehaviour
{
  private enum Estado
  {
    Original,
    Direita2,
    Esquerda2,
    Estado1,
    Estado3,
    Estado2
  }

  private float fps = 60f, newFPS;
  private Resistores6[] resistor = new Resistores6[6];
  private GameObject[] r = new GameObject[6];
  private Vector2[] sombras = new Vector2[6];
  private GameObject red2Direita, red2, sombra1, sombra2, sombra3, sombra4, sombra5, sombra6, red1, red3, original, preRed2, preRed2D, preRed1;
  private bool[] reduzidos = new bool[6];
  private Estado estadoAtivo = Estado.Original;

  public bool spawn = true, reduzir = true, trava = false;
  public Eventos6 evento;
  public GameObject pecasCriadas, circuitoUI, stock, circuito, vitoria, enunciado;

  private void Start()
  {
    InicializarTelas();
    SetarReducoes();
    SetarSombras();

    for (int i = 0; i < 6; i++)
    {
      reduzidos[i] = false;
    }
    sombras = new Vector2[] { new Vector2(sombra1.transform.position.x, sombra1.transform.position.y),
                              new Vector2(sombra2.transform.position.x, sombra2.transform.position.y),
                              new Vector2(sombra3.transform.position.x, sombra3.transform.position.y),
                              new Vector2(sombra4.transform.position.x, sombra4.transform.position.y),
                              new Vector2(sombra5.transform.position.x, sombra5.transform.position.y),
                              new Vector2(sombra6.transform.position.x, sombra6.transform.position.y)};
  }

  private void SetarReducoes()
  {
    red2Direita = circuito.transform.GetChild(3).gameObject;
    red2 = circuito.transform.GetChild(2).gameObject;
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
    sombra6 = circuito.transform.GetChild(0).GetChild(5).gameObject;
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

    ChecarReducao1();
    if (estadoAtivo == Estado.Estado1)
      Animacao1();

    ChecarReducao2();
    if (estadoAtivo == Estado.Estado2)
      Animacao2();

    ChecarReducao3();
    if (estadoAtivo == Estado.Estado3)
      Animacao3();
  }

  private void Animacao3()
  {
    red2Direita.transform.GetChild(2).transform.gameObject.SetActive(false);
    red2Direita.transform.GetChild(3).transform.gameObject.SetActive(false);
    red2.transform.GetChild(2).transform.gameObject.SetActive(false);
    red2.transform.GetChild(3).transform.gameObject.SetActive(false);
    circuito.transform.GetChild(0).GetChild(18).gameObject.SetActive(false);

    if (red2.transform.GetChild(0).transform.localPosition.y < 0f)
    {
      red2.transform.GetChild(0).Translate(5.12f / fps, 0f, 0f, Space.World);
    }
    else
    {
      red2.transform.GetChild(0).transform.localPosition = new Vector3(red2.transform.GetChild(0).transform.localPosition.x, 0f, red2.transform.GetChild(0).transform.localPosition.z);
      red2.SetActive(false);
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

    if (!red2Direita.activeInHierarchy && !red2.activeInHierarchy)
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

    Transform res1 = red1.transform.GetChild(1).transform;
    Transform res2 = red1.transform.GetChild(2).transform;

    if (res1.localPosition.x < red1.transform.GetChild(0).localPosition.x)
      res1.Translate((5.12f / fps), 0f, 0f, Space.World);

    if (res2.localPosition.x > red1.transform.GetChild(0).localPosition.x)
      res2.Translate((-5.12f / fps), 0f, 0f, Space.World);

    if (res1.localPosition.x >= red1.transform.GetChild(0).localPosition.x && res2.localPosition.x <= red1.transform.GetChild(0).localPosition.x)
    {
      res1.localPosition = red1.transform.GetChild(0).localPosition;
      res2.localPosition = red1.transform.GetChild(0).localPosition;
      res1.gameObject.SetActive(false);
      res2.gameObject.SetActive(false);
      red1.transform.GetChild(0).gameObject.SetActive(true);
      red1.transform.GetChild(3).gameObject.SetActive(true);
      red1.transform.GetChild(4).gameObject.SetActive(true);
      red1.transform.GetChild(5).gameObject.SetActive(true);
      red1.transform.GetChild(6).gameObject.SetActive(true);
      spawn = true;
      trava = false;
    }
  }

  private void Animacao2()
  {
    trava = true;
    spawn = false;

    Transform res1 = red1.transform.GetChild(0).transform;
    Transform res2 = red2.transform.GetChild(1).transform;

    if (res1.localPosition.y < red2.transform.GetChild(0).localPosition.y)
      res1.Translate(0f, (5.12f / fps), 0f, Space.World);

    if (res2.localPosition.y > red2.transform.GetChild(0).localPosition.y)
      res2.Translate(0f, (-5.12f / fps), 0f, Space.World);

    if (res1.localPosition.y >= red2.transform.GetChild(0).localPosition.y && res2.localPosition.y <= red2.transform.GetChild(0).localPosition.y)
    {
      res1.localPosition = red2.transform.GetChild(0).localPosition;
      res2.localPosition = red2.transform.GetChild(0).localPosition;
      res2.gameObject.SetActive(false);
      red2.transform.GetChild(0).gameObject.SetActive(true);
      red2.transform.GetChild(2).gameObject.SetActive(true);
      red2.transform.GetChild(3).gameObject.SetActive(true);
      red2.transform.GetChild(4).gameObject.SetActive(true);
      red2.transform.GetChild(5).gameObject.SetActive(true);
      red2.transform.GetChild(6).gameObject.SetActive(true);
      red2.transform.GetChild(7).gameObject.SetActive(true);
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

  private void ChecarReducao2()
  {
    if (r[2] && reduzidos[1] && reduzidos[4] && !reduzidos[2] &&reduzir && estadoAtivo != Estado.Estado2 && !trava)
    {
      reduzidos[2] = true;
      reduzidos[3] = true;
      estadoAtivo = Estado.Estado2;
      preRed2 = Instantiate(circuito);
      preRed2.SetActive(false);
      red2.SetActive(true);
      red2.transform.GetChild(0).GetComponent<Resistores6>().SetResistencia(resistor[2].GetResistencia() + red1.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia());
      red1.transform.GetChild(3).gameObject.SetActive(false);
      r[2].SetActive(false);
      sombra3.SetActive(false);
    }
  }

  private void ChecarReducao1()
  {
    if (r[1] != null && r[4] != null && !reduzidos[1] && !reduzidos[4] && reduzir && estadoAtivo != Estado.Estado1 && !trava)
    {
      estadoAtivo = Estado.Estado1;
      preRed1 = Instantiate(circuito);
      preRed1.SetActive(false);
      reduzidos[4] = true;
      reduzidos[1] = true;
      circuito.transform.GetChild(0).GetChild(17).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(18).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(20).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(22).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(24).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(25).gameObject.SetActive(false);
      r[1].SetActive(false);
      r[4].SetActive(false);
      sombra2.gameObject.SetActive(false);
      sombra5.gameObject.SetActive(false);
      red1.SetActive(true);
      red1.transform.GetChild(0).GetComponent<Resistores6>().SetResistencia(1 / (1 / resistor[1].GetResistencia() + 1 / resistor[4].GetResistencia()));
    }
  }

  private void ChecarReducao3()
  {
    if (red2.activeInHierarchy && red2Direita.activeInHierarchy && !trava && reduzir && estadoAtivo != Estado.Estado3)
    {
      estadoAtivo = Estado.Estado3;

      red3.transform.GetChild(0).GetComponent<Resistores6>().SetResistencia(red2Direita.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia() + red2.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia());
    }
  }

  public void Desfazer()
  {
    if (estadoAtivo == Estado.Estado1)
    {
      decimal aux = 0;
      if (reduzidos[5] || reduzidos[3])
        aux = red3.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia();

      Destroy(circuito);
      circuito = Instantiate(preRed1);
      circuito.SetActive(true);
      Destroy(preRed1);

      reduzidos[1] = false;
      reduzidos[4] = false;

      Destroy(r[4]);
      Destroy(r[1]);

      SetarResistores();
      SetarReducoes();
      SetarSombras();

      if (red3.activeInHierarchy && aux != 0)
      {
        red3.transform.GetChild(0).GetComponent<Resistores6>().SetResistencia(aux);
        estadoAtivo = Estado.Estado3;
      }
      else
      {
        estadoAtivo = Estado.Original;
      }
      reduzir = false;
    }
    else if (estadoAtivo == Estado.Estado2)
    {
      Debug.Log("Hello World");

      decimal aux = 0;
      if (reduzidos[5] || reduzidos[3])
        aux = red3.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia();
      preRed2.transform.GetChild(1).GetChild(0).GetComponent<Resistores6>().SetResistencia(red1.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia());

      Destroy(circuito);
      circuito = Instantiate(preRed2);
      circuito.transform.GetChild(1).GetChild(0).GetComponent<Resistores6>().SetResistencia(preRed2.transform.GetChild(1).GetChild(0).GetComponent<Resistores6>().GetResistencia());
      circuito.SetActive(true);
      Destroy(preRed2);

      reduzidos[2] = false;

      Destroy(r[2]);

      SetarResistores();
      SetarReducoes();
      SetarSombras();

      if (aux != 0)
      {
        red3.transform.GetChild(0).GetComponent<Resistores6>().SetResistencia(aux);
        estadoAtivo = Estado.Estado3;
      }
      else
      {
        estadoAtivo = Estado.Estado1;
      }
      reduzir = false;
    }
    /*    if (estadoAtivo == Estado.Estado3)
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
          Destroy(preRed2);

          SetarReducoes();
          SetarSombras();
          estadoAtivo = Estado.Original;
          reduzir = false;
        }*/

    /*    else if (estadoAtivo == Estado.Direita2)
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
        }*/
    /*    else if (estadoAtivo == Estado.Esquerda2)
        {
          red2.SetActive(false);
          Destroy(circuito);
          circuito = Instantiate(preRed2);

          circuito.SetActive(true);
          Destroy(r[1]);
          reduzidos[1] = false;

          Destroy(preRed2);
          SetarReducoes();
          SetarSombras();

          if (circuito.transform.GetChild(3).gameObject.activeInHierarchy)
            estadoAtivo = Estado.Direita2;
          else
            estadoAtivo = Estado.Estado1;

          Desfazer();
        }*/
  }
}