using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController7 : MonoBehaviour
{
  private enum Estado
  {
    Original,
    Estado1,
    Estado3,
    Estado2,
    Estado4,
    Estado5
  }

  private float fps = 60f, newFPS;
  private Resistores7[] resistor = new Resistores7[6];
  private GameObject[] r = new GameObject[6];
  private Vector2[] sombras = new Vector2[6];
  private GameObject preRed4, preRed5, sombra1, sombra2, sombra3, sombra4, sombra5, sombra6, preRed2, preRed3, preRed1;
  private GameObject red1, red2, red3, red4, red5;
  private bool[] reduzidos = new bool[6];
  private Estado estadoAtivo = Estado.Original;
  private List<Estado> sequencia = new List<Estado>(6);
  public bool spawn = true, reduzir = true, trava = false;
  public EventoPC7 evento;
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

    sequencia.Add(Estado.Original);
  }

  private void SetarReducoes()
  {
    //red2Direita = circuito.transform.GetChild(3).gameObject;
    red2 = circuito.transform.GetChild(2).gameObject;
    red1 = circuito.transform.GetChild(1).gameObject;
    red3 = circuito.transform.GetChild(3).gameObject;
    red4 = circuito.transform.GetChild(4).gameObject;
    red5 = circuito.transform.GetChild(5).gameObject;
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
        resistor[i] = r[i].GetComponent<Resistores7>();
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

    ChecarReducao4();
    if (estadoAtivo == Estado.Estado4)
      Animacao4();

    ChecarReducao5();
    if (estadoAtivo == Estado.Estado5)
      Animacao5();

  }

  private void Animacao5()
  {
    trava = true;
    spawn = false;

    Transform res1 = red5.transform.GetChild(0).transform;
    Transform res2 = red2.transform.GetChild(0).transform; if (res1.localPosition.x > res2.localPosition.x)
      res1.Translate((-5.12f / fps), 0f, 0f, Space.World);

    if (res1.localPosition.y > res2.localPosition.y)
      res1.Translate(0f, (-10.24f / fps), 0f, Space.World);

    if (res1.rotation.z < res2.rotation.z)
      res1.Rotate(0f, 0f, (90f / fps), Space.World);

    if (res1.rotation.z >= res2.rotation.z && res1.localPosition.y <= res2.localPosition.y && res1.localPosition.x <= res2.localPosition.x)
    {
      res1.localPosition = res2.localPosition;
      res1.rotation = res2.rotation;
      red4.SetActive(false);
      red5.transform.GetChild(2).gameObject.SetActive(true);
      spawn = true;
      trava = false;
      if (red5.transform.GetChild(0).GetComponent<Resistores7>().GetResistencia() == 15.97m)
      {
        vitoria.SetActive(true);
        stock.SetActive(false);
        circuitoUI.transform.GetChild(1).gameObject.SetActive(false);
      }
    }  
  }

  private void Animacao4()
  {
    trava = true;
    spawn = false;

    Transform res1 = red4.transform.GetChild(0).transform;
    Transform res2 = red3.transform.GetChild(0).transform;

    if (res1.position.x > res2.position.x)
      res1.Translate(-5.12f / fps, 0f, 0f, Space.World);

    if (res1.rotation.z < res2.rotation.z)
      res1.Rotate(0f, 0f, (90f + 53.39f) / fps, Space.World);

    if (res1.position.x <= res2.position.x && res1.rotation.z >= res2.rotation.z)
    {
      res1.localPosition = res2.localPosition;
      res1.rotation = res2.rotation;

      circuito.transform.GetChild(0).GetChild(18).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(19).gameObject.SetActive(false);

      red3.SetActive(false);
      spawn = true;
      trava = false;
    }
  }

  private void Animacao3()
  {
    trava = true;
    spawn = false;

    Transform res1 = red3.transform.GetChild(0).transform;
    Transform res2 = red2.transform.GetChild(0).transform;

    if (res1.localPosition.x > res2.localPosition.x)
      res1.Translate((-5.12f / fps), 0f, 0f, Space.World);

    if (res1.localPosition.y < res2.localPosition.y)
      res1.Translate(0f, (10.24f / fps), 0f, Space.World);

    if (res1.rotation.z < res2.rotation.z)
      res1.Rotate(0f, 0f, (90f / fps), Space.World);

    if (res1.rotation.z >= res2.rotation.z && res1.localPosition.y >= res2.localPosition.y && res1.localPosition.x <= res2.localPosition.x)
    {
      res1.localPosition = res2.localPosition;
      res1.rotation = res2.rotation;
      r[2].SetActive(false);
      red3.transform.GetChild(1).gameObject.SetActive(true);
      res2.gameObject.SetActive(false);
      spawn = true;
      trava = false;
    }
  }

  private void Animacao1()
  {
    trava = true;
    spawn = false;

    Transform res1 = red1.transform.GetChild(0).transform;
    Transform res2 = circuito.transform.GetChild(0).GetChild(0).transform;

    if (res1.localPosition.x > res2.localPosition.x)
      res1.Translate((-5.12f / fps), 0f, 0f, Space.World);

    if (res1.localPosition.y > res2.localPosition.y)
      res1.Translate(0f, (-10.24f / fps), 0f, Space.World);

    if (res1.rotation.z < res2.rotation.z)
      res1.Rotate(0f, 0f, (90f / fps), Space.World);

    if (res1.rotation.z >= res2.rotation.z && res1.localPosition.y <= res2.localPosition.y && res1.localPosition.x <= res2.localPosition.x)
    {
      res1.localPosition = res2.localPosition;
      res1.rotation = res2.rotation;
      res2.gameObject.SetActive(false);
      red1.transform.GetChild(1).gameObject.SetActive(true);
      r[0].SetActive(false);
      spawn = true;
      trava = false;
    }
  }

  private void Animacao2()
  {
    trava = true;
    spawn = false;

    Transform res1 = red1.transform.GetChild(0).transform;
    Transform res2 = red2.transform.GetChild(0).transform;

    if (res1.localPosition.x < res2.localPosition.x)
      res1.Translate((10.24f / fps), 0f, 0f, Space.World);

    if (res1.localPosition.x >= res2.localPosition.x)
    {
      res1.localPosition = res2.localPosition;
      red2.SetActive(true);
      circuito.transform.GetChild(0).GetChild(6).gameObject.SetActive(false);
      red1.SetActive(false);
      r[2].SetActive(false);
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
    if (r[2] && reduzidos[1] && !reduzidos[2] && reduzir && estadoAtivo != Estado.Estado2 && !trava)
    {
      estadoAtivo = Estado.Estado2;
      preRed2 = Instantiate(circuito);
      preRed2.SetActive(false);
      preRed2.name = "2";
      reduzidos[2] = true;
      preRed2.transform.GetChild(1).GetChild(0).GetComponent<Resistores7>().SetResistencia(circuito.transform.GetChild(1).GetChild(0).GetComponent<Resistores7>().GetResistencia());
      red2.transform.GetChild(0).GetComponent<Resistores7>().SetResistencia(1/(1/resistor[2].GetResistencia() + 1/red1.transform.GetChild(0).GetComponent<Resistores7>().GetResistencia()));
      sombra3.SetActive(false);
      sequencia.Add(Estado.Estado2);
    }
  }

  private void ChecarReducao1()
  {
    if (r[0] && r[1] && !reduzidos[0] && !reduzidos[1] && reduzir && estadoAtivo != Estado.Estado1 && !trava)
    {
      estadoAtivo = Estado.Estado1;
      preRed1 = Instantiate(circuito);
      preRed1.SetActive(false);
      preRed1.name = "1";
      reduzidos[0] = true;
      reduzidos[1] = true;
      r[1].SetActive(false);
      sombra1.gameObject.SetActive(false);
      sombra2.gameObject.SetActive(false);
      red1.SetActive(true);
      red1.transform.GetChild(0).GetComponent<Resistores7>().SetResistencia(resistor[1].GetResistencia() + resistor[0].GetResistencia());
      sequencia.Add(Estado.Estado1);
    }
  }

  private void ChecarReducao3()
  {
    if (r[3] && !reduzidos[3] && reduzidos[2] && reduzir && estadoAtivo != Estado.Estado3 && !trava)
    {
      estadoAtivo = Estado.Estado3;
      preRed3 = Instantiate(circuito);
      preRed3.SetActive(false);
      preRed3.name = "3";
      preRed3.transform.GetChild(2).GetChild(0).GetComponent<Resistores7>().SetResistencia(circuito.transform.GetChild(2).GetChild(0).GetComponent<Resistores7>().GetResistencia());
      reduzidos[3] = true;
      r[3].SetActive(false);
      sombra4.gameObject.SetActive(false);
      red3.SetActive(true);
      red3.transform.GetChild(0).GetComponent<Resistores7>().SetResistencia(resistor[3].GetResistencia() + red2.transform.GetChild(0).GetComponent<Resistores7>().GetResistencia());
      sequencia.Add(Estado.Estado3);
    }
  }

  private void ChecarReducao4()
  {
    if (r[5] && !reduzidos[5] && reduzidos[3] && reduzir && estadoAtivo != Estado.Estado4 && !trava)
    {
      estadoAtivo = Estado.Estado4;
      preRed4 = Instantiate(circuito);
      preRed4.SetActive(false);
      preRed4.name = "4";
      preRed4.transform.GetChild(3).GetChild(0).GetComponent<Resistores7>().SetResistencia(circuito.transform.GetChild(3).GetChild(0).GetComponent<Resistores7>().GetResistencia());
      reduzidos[5] = true;
      r[5].SetActive(false);
      sombra6.gameObject.SetActive(false);
      red4.SetActive(true);
      red4.transform.GetChild(0).GetComponent<Resistores7>().SetResistencia(1 / ((1 / resistor[5].GetResistencia()) + (1 / red3.transform.GetChild(0).GetComponent<Resistores7>().GetResistencia())));
      sequencia.Add(Estado.Estado4);

    }
  }

  private void ChecarReducao5()
  {
    if (r[4] && !reduzidos[4] && reduzidos[5] && reduzir && estadoAtivo != Estado.Estado5 && !trava)
    {
      estadoAtivo = Estado.Estado5;
      preRed5 = Instantiate(circuito);
      preRed5.SetActive(false);
      preRed5.name = "5";
      reduzidos[4] = true;
      r[4].SetActive(false);
      preRed5.transform.GetChild(4).GetChild(0).GetComponent<Resistores7>().SetResistencia(circuito.transform.GetChild(4).GetChild(0).GetComponent<Resistores7>().GetResistencia());
      sombra5.gameObject.SetActive(false);
      red5.SetActive(true);
      red5.transform.GetChild(0).GetComponent<Resistores7>().SetResistencia(red4.transform.GetChild(0).GetComponent<Resistores7>().GetResistencia() + r[4].GetComponent<Resistores7>().GetResistencia());
      sequencia.Add(Estado.Estado5);
    }
  }

  public void Desfazer()
  {
    if (estadoAtivo == Estado.Estado1)
    {

      Destroy(circuito);
      circuito = preRed1;
      circuito.name = "Pecas";
      circuito.SetActive(true);

      reduzidos[0] = false;
      reduzidos[1] = false;

      Destroy(r[0]);
      Destroy(r[1]);

      SetarResistores();
      SetarReducoes();
      SetarSombras();

      sequencia.RemoveAt(sequencia.Count - 1);

      estadoAtivo = sequencia[sequencia.Count - 1];

      reduzir = false;
    }
    else if (estadoAtivo == Estado.Estado2)
    {

      Destroy(circuito);
      circuito = preRed2;
      circuito.name = "Pecas";
      circuito.SetActive(true);

      reduzidos[2] = false;

      Destroy(r[2]);

      SetarResistores();
      SetarReducoes();
      SetarSombras();

      sequencia.RemoveAt(sequencia.Count - 1);

      estadoAtivo = sequencia[sequencia.Count - 1];

      reduzir = false;
    }
    else if (estadoAtivo == Estado.Estado3)
    {
      Destroy(circuito);
      circuito = preRed3;
      circuito.name = "Pecas";
      circuito.SetActive(true);

      reduzidos[3] = false;

      Destroy(r[3]);

      SetarResistores();
      SetarReducoes();
      SetarSombras();

      sequencia.RemoveAt(sequencia.Count - 1);

      estadoAtivo = sequencia[sequencia.Count - 1];

      reduzir = false;
    }
    else if (estadoAtivo == Estado.Estado4)
    {
      Destroy(circuito);
      circuito = preRed4;
      circuito.name = "Pecas";
      circuito.SetActive(true);

      reduzidos[5] = false;

      Destroy(r[5]);

      SetarResistores();
      SetarReducoes();
      SetarSombras();

      sequencia.RemoveAt(sequencia.Count - 1);

      estadoAtivo = sequencia[sequencia.Count - 1];
    }
    else if (estadoAtivo == Estado.Estado5)
    {
      Destroy(circuito);
      circuito = preRed5;
      circuito.name = "Pecas";
      circuito.SetActive(true);

      reduzidos[4] = false;

      Destroy(r[4]);

      SetarResistores();
      SetarReducoes();
      SetarSombras();

      sequencia.RemoveAt(sequencia.Count - 1);

      estadoAtivo = sequencia[sequencia.Count - 1];
    }
  }
}
