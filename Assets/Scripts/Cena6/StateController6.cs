using System;
using UnityEngine;
using System.Collections.Generic;

public class StateController6 : MonoBehaviour
{
  private enum Estado
  {
    Original,
    Estado1,
    Estado3,
    Estado2,
    Estado4Esquerda,
    Estado4Direita
  }

  private float fps = 60f, newFPS;
  private Resistores6[] resistor = new Resistores6[6];
  private GameObject[] r = new GameObject[6];
  private Vector2[] sombras = new Vector2[6];
  private GameObject preRed4Esquerda, preRed4Direita, sombra1, sombra2, sombra3, sombra4, sombra5, sombra6, preRed2, preRed3, preRed1;
  private GameObject red1, red2, red3, red4Esquerda, red4Direita, primeiro;
  private bool[] reduzidos = new bool[6];
  private Estado estadoAtivo = Estado.Original;
  private List<Estado> sequencia = new List<Estado>(6);
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
    primeiro = null;
    sequencia.Add(Estado.Original);
  }

  private void SetarReducoes()
  {
    //red2Direita = circuito.transform.GetChild(3).gameObject;
    red2 = circuito.transform.GetChild(2).gameObject;
    red1 = circuito.transform.GetChild(1).gameObject;
    red3 = circuito.transform.GetChild(3).gameObject;
    red4Esquerda = circuito.transform.GetChild(4).gameObject;
    red4Direita = circuito.transform.GetChild(5).gameObject;
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

    ChecarReducao4Esquerda();
    if (estadoAtivo == Estado.Estado4Esquerda)
      Animacao4Esquerda();

    ChecarReducao4Direita();
    if (estadoAtivo == Estado.Estado4Direita)
      Animacao4Direita();


  }

  private void Animacao4Direita()
  {
    trava = true;
    spawn = false;

    Transform res1 = red3.transform.GetChild(0).transform;
    Transform res2 = red2.transform.GetChild(0).transform;

    if (res1.position.x > res2.position.x)
      res1.Translate(-10.24f / fps, 0f, 0f, Space.World);

    if (res1.position.x <= res2.position.x)
    {
      res1.position = res2.position;
      red4Direita.SetActive(true);
      red3.SetActive(false);
      if (red2.activeInHierarchy)
        red2.SetActive(false);
      else
        red4Esquerda.SetActive(false);

      spawn = true;
      trava = false;

      if (sequencia.Count >= 6 && red4Direita.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia() == 2)
      {
        vitoria.SetActive(true);
        stock.SetActive(false);
        circuitoUI.transform.GetChild(1).gameObject.SetActive(false);
      }
    }
  }

  private void Animacao4Esquerda()
  {
    trava = true;
    spawn = false;

    Transform res1 = red4Esquerda.transform.GetChild(1).transform;
    Transform res2 = red2.transform.GetChild(0).transform;

    if (res1.position.x < res2.position.x)
      res1.Translate(5.12f / fps, 0f, 0f, Space.World);

    if(res1.position.x >= res2.position.x)
    {
      red4Esquerda.transform.GetChild(0).gameObject.SetActive(true);
      red4Esquerda.transform.GetChild(4).gameObject.SetActive(true);
      red4Esquerda.transform.GetChild(5).gameObject.SetActive(true);
      red4Esquerda.transform.GetChild(6).gameObject.SetActive(true);
      red4Esquerda.transform.GetChild(7).gameObject.SetActive(true);
      red4Esquerda.transform.GetChild(8).gameObject.SetActive(true);
      if (!red4Direita.activeInHierarchy)
      {
        red4Esquerda.transform.GetChild(9).gameObject.SetActive(true);
        red4Esquerda.transform.GetChild(10).gameObject.SetActive(true);
        red4Esquerda.transform.GetChild(11).gameObject.SetActive(true);
      }
      red2.SetActive(false);
      res1.gameObject.SetActive(false);
      spawn = true;
      trava = false;

      if (sequencia.Count >= 6 && red4Direita.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia() == 2)
      {
        vitoria.SetActive(true);
        stock.SetActive(false);
        circuitoUI.transform.GetChild(1).gameObject.SetActive(false);
      }
    }
  }

  private void Animacao3()
  {
    trava = true;
    spawn = false;

    Transform res1 = red3.transform.GetChild(1).transform;
    Transform res2 = circuito.transform.GetChild(0).GetChild(5).transform;

    if (res1.position.y > res2.position.y)
      res1.Translate(0f, -10.24f / fps, 0f, Space.World);
    else
      res1.position = new Vector3(res1.position.x, res2.position.y, 0f);

    if (res1.position.x < res2.position.x)
      res1.Translate(5.12f / fps, 0f, 0f, Space.World);
    else
      res1.position = new Vector3(res2.position.x, res1.position.y, 0f);

    if (res1.localRotation.z < res2.localRotation.z)
      res1.Rotate(0f, 0f, 90f / fps, Space.World);
    else
      res1.localRotation.SetEulerAngles(0f, 0f, res2.localRotation.z);

    if (res1.localPosition == res2.localPosition && res1.localRotation.z >= res2.localPosition.z)
    {
      red3.transform.GetChild(0).gameObject.SetActive(true);
      red3.transform.GetChild(2).gameObject.SetActive(true);
      r[5].SetActive(false);
      res1.gameObject.SetActive(false);
      res2.gameObject.SetActive(false);
      spawn = true;
      trava = false;
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
    if (r[2] && reduzidos[1] && reduzidos[4] && !reduzidos[2] && reduzir && estadoAtivo != Estado.Estado2 && !trava)
    {
      estadoAtivo = Estado.Estado2;
      preRed2 = Instantiate(circuito);
      preRed2.SetActive(false);
      preRed2.name = "2";
      reduzidos[2] = true;
      if (primeiro == null)
        primeiro = preRed2;
      preRed2.transform.GetChild(1).GetChild(0).GetComponent<Resistores6>().SetResistencia(circuito.transform.GetChild(1).GetChild(0).GetComponent<Resistores6>().GetResistencia());
      if (reduzidos[3])
        preRed2.transform.GetChild(3).GetChild(0).GetComponent<Resistores6>().SetResistencia(circuito.transform.GetChild(3).GetChild(0).GetComponent<Resistores6>().GetResistencia());
      red2.SetActive(true);
      red2.transform.GetChild(0).GetComponent<Resistores6>().SetResistencia(resistor[2].GetResistencia() + red1.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia());
      red1.transform.GetChild(3).gameObject.SetActive(false);
      r[2].SetActive(false);
      sombra3.SetActive(false);
      sequencia.Add(Estado.Estado2);
    }
  }

  private void ChecarReducao1()
  {
    if (r[1] != null && r[4] != null && !reduzidos[1] && !reduzidos[4] && reduzir && estadoAtivo != Estado.Estado1 && !trava)
    {
      estadoAtivo = Estado.Estado1;
      preRed1 = Instantiate(circuito);
      preRed1.SetActive(false);
      preRed1.name = "1";
      reduzidos[4] = true;
      reduzidos[1] = true;
      circuito.transform.GetChild(0).GetChild(17).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(18).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(20).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(22).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(24).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(25).gameObject.SetActive(false);
      if (reduzidos[3])
        preRed1.transform.GetChild(3).GetChild(0).GetComponent<Resistores6>().SetResistencia(circuito.transform.GetChild(3).GetChild(0).GetComponent<Resistores6>().GetResistencia());
      r[1].SetActive(false);
      r[4].SetActive(false);
      sombra2.gameObject.SetActive(false);
      sombra5.gameObject.SetActive(false);
      red1.SetActive(true);
      red1.transform.GetChild(0).GetComponent<Resistores6>().SetResistencia(1 / (1 / resistor[1].GetResistencia() + 1 / resistor[4].GetResistencia()));
      sequencia.Add(Estado.Estado1);
    }
  }

  private void ChecarReducao3()
  {
    if (r[3] && r[5] && !reduzidos[3] && !reduzidos[5] && reduzir && estadoAtivo != Estado.Estado3 && !trava)
    {
      estadoAtivo = Estado.Estado3;
      preRed3 = Instantiate(circuito);
      preRed3.SetActive(false);
      preRed3.name = "3";
      if (primeiro == null)
        primeiro = preRed3;
      if (reduzidos[2])
        preRed3.transform.GetChild(2).GetChild(0).GetComponent<Resistores6>().SetResistencia(circuito.transform.GetChild(2).GetChild(0).GetComponent<Resistores6>().GetResistencia());
      else if (reduzidos[1])
        preRed3.transform.GetChild(1).GetChild(0).GetComponent<Resistores6>().SetResistencia(circuito.transform.GetChild(1).GetChild(0).GetComponent<Resistores6>().GetResistencia());
      reduzidos[3] = true;
      reduzidos[5] = true;
      r[3].SetActive(false);
      sombra4.gameObject.SetActive(false);
      sombra6.gameObject.SetActive(false);
      red3.SetActive(true);
      red3.transform.GetChild(0).GetComponent<Resistores6>().SetResistencia((resistor[3].GetResistencia() + resistor[5].GetResistencia()));
      sequencia.Add(Estado.Estado3);
    }
  }

  private void ChecarReducao4Esquerda()
  {
    if (r[0] && !reduzidos[0] && reduzidos[2] && reduzir && estadoAtivo != Estado.Estado4Esquerda && !trava)
    {
      estadoAtivo = Estado.Estado4Esquerda;
      preRed4Esquerda = Instantiate(circuito);
      preRed4Esquerda.SetActive(false);
      preRed4Esquerda.name = "4esquerda";
      preRed4Esquerda.transform.GetChild(2).GetChild(0).GetComponent<Resistores6>().SetResistencia(circuito.transform.GetChild(2).GetChild(0).GetComponent<Resistores6>().GetResistencia());   
      if (red4Direita.activeInHierarchy)
        preRed4Esquerda.transform.GetChild(5).GetChild(0).GetComponent<Resistores6>().SetResistencia(circuito.transform.GetChild(5).GetChild(0).GetComponent<Resistores6>().GetResistencia());
      else if (reduzidos[3])
        preRed4Esquerda.transform.GetChild(3).GetChild(0).GetComponent<Resistores6>().SetResistencia(circuito.transform.GetChild(3).GetChild(0).GetComponent<Resistores6>().GetResistencia());
      reduzidos[0] = true;
      r[0].SetActive(false);
      sombra1.gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(11).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(12).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(13).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(14).gameObject.SetActive(false);
      red4Esquerda.SetActive(true);
      if (red4Direita.activeInHierarchy)
        red4Esquerda.transform.GetChild(0).GetComponent<Resistores6>().SetResistencia(1 / ((1 / resistor[0].GetResistencia()) + (1 / red4Direita.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia())));
      else
        red4Esquerda.transform.GetChild(0).GetComponent<Resistores6>().SetResistencia(1 / ((1 / resistor[0].GetResistencia()) + (1 / red2.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia())));
      sequencia.Add(Estado.Estado4Esquerda);
      
    }
  }

  private void ChecarReducao4Direita()
  {
    if (reduzidos[2] && reduzidos[3] && reduzir && estadoAtivo != Estado.Estado4Direita && !trava && red3.activeInHierarchy)
    {
      estadoAtivo = Estado.Estado4Direita;
      preRed4Direita = Instantiate(circuito);
      preRed4Direita.SetActive(false);
      preRed4Direita.name = "4direita";
      preRed4Direita.transform.GetChild(2).GetChild(0).GetComponent<Resistores6>().SetResistencia(circuito.transform.GetChild(2).GetChild(0).GetComponent<Resistores6>().GetResistencia());
      preRed4Direita.transform.GetChild(3).GetChild(0).GetComponent<Resistores6>().SetResistencia(circuito.transform.GetChild(3).GetChild(0).GetComponent<Resistores6>().GetResistencia());
      circuito.transform.GetChild(0).GetChild(19).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(26).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(27).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(28).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(29).gameObject.SetActive(false);
      if (reduzidos[0])
        preRed4Direita.transform.GetChild(4).GetChild(0).GetComponent<Resistores6>().SetResistencia(circuito.transform.GetChild(4).GetChild(0).GetComponent<Resistores6>().GetResistencia());
      red4Direita.SetActive(true);
      if (red4Esquerda.activeInHierarchy)
        red4Direita.transform.GetChild(0).GetComponent<Resistores6>().SetResistencia(1 / ((1 / red4Esquerda.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia()) + (1 / red3.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia())));
      else
        red4Direita.transform.GetChild(0).GetComponent<Resistores6>().SetResistencia(1 / ((1 / red2.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia()) + (1 / red3.transform.GetChild(0).GetComponent<Resistores6>().GetResistencia())));
      sequencia.Add(Estado.Estado4Direita);
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

      reduzidos[1] = false;
      reduzidos[4] = false;

      Destroy(r[4]);
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
      if (primeiro == preRed3)
        primeiro = null;

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
      if (primeiro == preRed2)
        primeiro = null;

      reduzidos[3] = false;
      reduzidos[5] = false;

      Destroy(r[3]);
      Destroy(r[5]);

      SetarResistores();
      SetarReducoes();
      SetarSombras();

      sequencia.RemoveAt(sequencia.Count - 1);

      estadoAtivo = sequencia[sequencia.Count - 1];

      reduzir = false;
    }
    else if (estadoAtivo == Estado.Estado4Esquerda)
    {
      Destroy(circuito);
      circuito = preRed4Esquerda;
      circuito.name = "Pecas";
      circuito.SetActive(true);

      reduzidos[0] = false;

      Destroy(r[0]);

      SetarResistores();
      SetarReducoes();
      SetarSombras();

      sequencia.RemoveAt(sequencia.Count - 1);

      estadoAtivo = sequencia[sequencia.Count - 1];

      Desfazer();

    }
    else if (estadoAtivo == Estado.Estado4Direita)
    {
      Destroy(circuito);
      circuito = preRed4Direita;
      circuito.name = "Pecas";
      circuito.SetActive(true);

      SetarResistores();
      SetarReducoes();
      SetarSombras();

      sequencia.RemoveAt(sequencia.Count - 1);

      estadoAtivo = sequencia[sequencia.Count - 1];

      Desfazer();

    }
  }
}