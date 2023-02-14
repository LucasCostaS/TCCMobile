using System;
using System.Collections.Generic;
using UnityEngine;

public class StateController10 : MonoBehaviour
{
  private enum Estado
  {
    Original,
    Estado1,
    Estado3,
    Estado2,
    Estado4
  }

  private float fps = 60f, newFPS;
  private Resistores10[] resistor = new Resistores10[5];
  private GameObject[] r = new GameObject[5];
  private Vector2[] posSombra = new Vector2[5];
  private bool[] reduzidos = new bool[5];
  private Estado estadoAtivo = Estado.Original;
  private List<Estado> sequencia = new List<Estado>();
  private List<GameObject> Red = new List<GameObject>();
  private List<GameObject> PreRed = new List<GameObject>();
  private List<GameObject> Sombra = new List<GameObject>();
  public bool spawn = true, reduzir = true, trava = false, parte1 = true;
  public Eventos10 evento;
  public GameObject pecasCriadas, circuitoUI, stock, circuito, vitoria, enunciado;

  private void Start()
  {
    InicializarTelas();
    SetarReducoes();
    SetarSombras();

    for (int i = 0; i < 5; i++)
    {
      reduzidos[i] = false;
      posSombra[i] = new Vector2(Sombra[i].transform.position.x, Sombra[i].transform.position.y);
    }

    sequencia.Add(Estado.Original);
  }

  private void SetarReducoes()
  {
    Red.Clear();
    for (int i = 1; i < 5; i++)
    {
      Red.Add(circuito.transform.GetChild(i).gameObject);
    }
  }

  private void SetarSombras()
  {
    Sombra.Clear();
    for (int i = 0; i < 5; i++)
    {
      Sombra.Add(circuito.transform.GetChild(0).GetChild(i).gameObject);
    }
  }

  private void SetarResistores()
  {
    if (!evento.dragging)
    {
      int i = 0;

      i = checaPosicao(i, posSombra);
      i = checaPosicao(i, posSombra);
      i = checaPosicao(i, posSombra);
      i = checaPosicao(i, posSombra);
      i = checaPosicao(i, posSombra);
    }
  }

  private int checaPosicao(int i, Vector2[] sombras)
  {
    RaycastHit2D check = Physics2D.Raycast(sombras[i], Camera.main.transform.forward, Mathf.Infinity, (1 << 6));

    if (reduzidos[i] == false)
    {
      if (check.collider != null && check.transform.tag != "Reduzido")
        r[i] = check.collider.gameObject;
      else
        r[i] = null;

      if (r[i] != null)
        resistor[i] = r[i].GetComponent<Resistores10>();
      else
        resistor[i] = null;
    }

    return ++i;
  }

  private void VelocidadeAnimacao()
  {
    newFPS = 1.0f / Time.smoothDeltaTime;
    if (newFPS != float.PositiveInfinity)
      fps = Mathf.Lerp(fps, newFPS, 0.005f);
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
    {
      if (parte1)
        Animacao1();
      else
        Animacao1_1();
    }
     

    ChecarReducao2();
    if (estadoAtivo == Estado.Estado2)
      Animacao2();

    ChecarReducao3();
    if (estadoAtivo == Estado.Estado3)
      Animacao3();

    ChecarReducao4();
    if (estadoAtivo == Estado.Estado4)
      Animacao4();
  }

  private void Animacao4()
  {
    trava = true;
    spawn = false;

    Transform res1 = Red[3].transform.GetChild(0).transform;
    Transform res2 = Sombra[0].transform;

    if (res1.localPosition.x < res2.localPosition.x)
      res1.Translate((5.12f / fps), 0f, 0f, Space.World);

    if (res1.localPosition.y < res2.localPosition.y)
      res1.Translate(0f, (5.12f / fps), 0f, Space.World);

    if (res1.localRotation.z < res2.localRotation.z)
      res1.Rotate(0f, 0f, (100 / fps), Space.World);

    if (res1.localPosition.y >= res2.localPosition.y && res1.localPosition.x >= res2.localPosition.x && res1.localRotation.z >= res2.localRotation.z)
    {
      res1.localPosition = res2.localPosition;
      res1.localRotation = res2.localRotation;
      circuito.transform.GetChild(0).GetChild(8).gameObject.SetActive(true);
      Red[3].SetActive(false);

      spawn = true;
      trava = false;
      if (Red[3].transform.GetChild(0).GetComponent<Resistores10>().GetResistencia() == 20m)
      {
        vitoria.SetActive(true);
        stock.SetActive(false);
        circuitoUI.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
      }
    }
  }

  private void Animacao3()
  {
    trava = true;
    spawn = false;

    Transform res1 = Red[2].transform.GetChild(0).transform;
    Transform res2 = Sombra[0].transform;

    if (res1.localPosition.x < res2.localPosition.x)
      res1.Translate((10.24f / fps), 0f, 0f, Space.World);

    if (res1.localPosition.y > res2.localPosition.y)
      res1.Translate(0f, (-5.12f / fps), 0f, Space.World);

    if (res1.localRotation.z > res2.localRotation.z)
      res1.Rotate(0f, 0f, (-45 / fps), Space.World);

    if (res1.localPosition.y <= res2.localPosition.y && res1.localPosition.x >= res2.localPosition.x && res1.localRotation.z <= res2.localRotation.z)
    {
      res1.localPosition = res2.localPosition;
      res1.localRotation = res2.localRotation;
      circuito.transform.GetChild(0).GetChild(7).gameObject.SetActive(false);
      Red[1].SetActive(false);

      spawn = true;
      trava = false;
    }
  }

  private void Animacao1()
  {
    trava = true;
    spawn = false;

    Transform res1 = Red[0].transform.GetChild(3).transform;
    Transform res2 = Red[0].transform.GetChild(4).transform;

    if (res1.localPosition.y < 10.24f)
    {
      res1.Translate(0f, 5.12f / (fps*0.33f), 0f, Space.World);
      res2.Translate(0f, 5.12f / (fps * 0.33f), 0f, Space.World);
    }   

    if (res1.localPosition.y >= 10.24f && res1.localRotation.z > 0f)
    {
      res1.Rotate(0f, 0f, -90 / (fps * 0.33f), Space.World);
      res2.Rotate(0f, 0f, -90 / (fps * 0.33f), Space.World);
    }

    if (res1.localRotation.z <= 0 && res1.localPosition.x < 0f)
    {
      res1.Translate(10.24f / (fps * 0.33f), 0f, 0f, Space.World);
      res2.Translate(-10.24f / (fps * 0.33f), 0f, 0f, Space.World);
    }

    if (res1.localPosition.x >= 0f)
    {
      res2.localPosition = new Vector3(0f, 10.24f, 0f);
      res1.localPosition = res2.localPosition;
      res2.localRotation.eulerAngles.Set(0f, 0f, 0f);
      res1.localRotation = res2.localRotation;

      parte1 = false;
    }
  }

  private void Animacao1_1()
  {
    Transform res1 = Red[0].transform.GetChild(3).transform;
    Transform res2 = Red[0].transform.GetChild(4).transform;
    Transform res3 = Red[0].transform.GetChild(5).transform;
    Transform res4 = Red[0].transform.GetChild(6).transform;

    if (res1.localPosition.x > -5.12f)
    {
      res1.Translate(-5.12f / fps, 0f, 0f, Space.World);
      res3.Translate(-5.12f / fps, 0f, 0f, Space.World);
      res2.Translate(5.12f / fps, 0f, 0f, Space.World);
      res4.Translate(5.12f / fps, 0f, 0f, Space.World);
    }

    if (res1.localPosition.y > 5.12f)
    {
      res1.Translate(0f, (-5.12f / fps), 0f, Space.World);
      res2.Translate(0f, (-5.12f / fps), 0f, Space.World);
    }

    if (res1.localRotation.z > -45f)
    {
      res1.Rotate(0f, 0f, -45f / (fps), Space.World);
      res3.Rotate(0f, 0f, -135f / (fps), Space.World);
      res2.Rotate(0f, 0f, 45f / (fps), Space.World);
      res4.Rotate(0f, 0f, -45f / (fps), Space.World);

    }

    if (res1.localPosition.y <= 5.12f && res1.localPosition.x <= -5.12f && res1.localRotation.z <= -45f)
    {
      res1.gameObject.SetActive(false);
      res2.gameObject.SetActive(false);
      res3.gameObject.SetActive(false);
      res4.gameObject.SetActive(false);

      Red[0].transform.GetChild(0).gameObject.SetActive(true);
      Red[0].transform.GetChild(1).gameObject.SetActive(true);
      Red[0].transform.GetChild(2).gameObject.SetActive(true);
      circuito.transform.GetChild(0).GetChild(6).gameObject.SetActive(true);

      spawn = true;
      trava = false;
    }
  }

  private void Animacao2()
  {
    trava = true;
    spawn = false;

    Transform res1 = Red[1].transform.GetChild(0).transform;
    Transform res2 = Sombra[0].transform;

    if (res1.localPosition.x < res2.localPosition.x)
      res1.Translate((10.24f / fps), 0f, 0f, Space.World);

    if (res1.localPosition.y > res2.localPosition.y)
      res1.Translate(0f, (-10.24f / fps), 0f, Space.World);

    if (res1.localRotation.z < res2.localRotation.z)
      res1.Rotate(0f, 0f, (100 / fps), Space.World);

    if (res1.localPosition.y <= res2.localPosition.y && res1.localPosition.x >= res2.localPosition.x)
    {
      res1.localPosition = res2.localPosition;
      res1.localRotation = res2.localRotation;
      circuito.transform.GetChild(0).GetChild(6).gameObject.SetActive(true);
      Red[0].SetActive(false);

      spawn = true;
      trava = false;
    }
  }

  private void ChecarReducao2()
  {
/*    if (r[2] && !reduzidos[2] && reduzidos[1] && reduzir && estadoAtivo != Estado.Estado2 && !trava)
    {
      estadoAtivo = Estado.Estado2;
      sequencia.Add(Estado.Estado2);

      PreRed.Add(Instantiate(circuito));
      PreRed[PreRed.Count - 1].SetActive(false);
      PreRed[PreRed.Count - 1].name = "2";

      reduzidos[2] = true;
      r[2].SetActive(false);
      Sombra[2].SetActive(false);
      Red[1].SetActive(true);

      Red[1].transform.GetChild(0).GetComponent<Resistores10>().SetResistencia(Red[0].transform.GetChild(0).GetComponent<Resistores10>().GetResistencia() + resistor[2].GetResistencia());

      PreRed[PreRed.Count - 1].transform.GetChild(1).GetChild(0).GetComponent<Resistores10>().SetResistencia(Red[0].transform.GetChild(0).GetComponent<Resistores10>().GetResistencia());
    }*/
  }

  private void ChecarReducao1()
  {
    if (r[0] && r[1] && r[2] && !reduzidos[0] /*&& !reduzidos[1]*/ && reduzir && estadoAtivo != Estado.Estado1 && !trava)
    {
      Debug.Log("Hello World");

      estadoAtivo = Estado.Estado1;
      sequencia.Add(Estado.Estado1);

      PreRed.Add(Instantiate(circuito));
      PreRed[PreRed.Count - 1].SetActive(false);
      PreRed[PreRed.Count - 1].name = "1";

      reduzidos[0] = true;
      //reduzidos[1] = true;
      r[1].SetActive(false);
      r[2].SetActive(false);
      r[0].SetActive(false);
      Sombra[1].SetActive(false);
      Sombra[0].SetActive(false);
      Sombra[2].SetActive(false);
      Red[0].SetActive(true);

      circuito.transform.GetChild(0).GetChild(5).gameObject.SetActive(false);

      //Red[0].transform.GetChild(0).GetComponent<Resistores10>().SetResistencia(1 / (1 / resistor[1].GetComponent<Resistores10>().GetResistencia() + 1 / resistor[0].GetResistencia()));
    }
  }

  private void ChecarReducao3()
  {
/*    if (r[3] && !reduzidos[3] && reduzidos[2] && reduzir && estadoAtivo != Estado.Estado3 && !trava)
    {
      estadoAtivo = Estado.Estado3;
      sequencia.Add(Estado.Estado3);

      PreRed.Add(Instantiate(circuito));
      PreRed[PreRed.Count - 1].SetActive(false);
      PreRed[PreRed.Count - 1].name = "3";

      reduzidos[3] = true;
      r[3].SetActive(false);
      Sombra[3].SetActive(false);
      Red[2].SetActive(true);

      Red[2].transform.GetChild(0).GetComponent<Resistores10>().SetResistencia(1 / (1 / Red[1].transform.GetChild(0).GetComponent<Resistores10>().GetResistencia() + 1 / resistor[3].GetResistencia()));

      PreRed[PreRed.Count - 1].transform.GetChild(2).GetChild(0).GetComponent<Resistores10>().SetResistencia(Red[1].transform.GetChild(0).GetComponent<Resistores10>().GetResistencia());
    }*/
  }

  private void ChecarReducao4()
  {
/*    if (r[4] && !reduzidos[4] && reduzidos[3] && reduzir && estadoAtivo != Estado.Estado4 && !trava)
    {
      estadoAtivo = Estado.Estado4;
      sequencia.Add(Estado.Estado4);

      PreRed.Add(Instantiate(circuito));
      PreRed[PreRed.Count - 1].SetActive(false);
      PreRed[PreRed.Count - 1].name = "4";

      reduzidos[4] = true;
      r[4].SetActive(false);
      Sombra[4].SetActive(false);
      Red[3].SetActive(true);

      Red[3].transform.GetChild(0).GetComponent<Resistores10>().SetResistencia(Red[2].transform.GetChild(0).GetComponent<Resistores10>().GetResistencia() + resistor[4].GetResistencia());

      PreRed[PreRed.Count - 1].transform.GetChild(3).GetChild(0).GetComponent<Resistores10>().SetResistencia(Red[2].transform.GetChild(0).GetComponent<Resistores10>().GetResistencia());
    }*/
  }

  public void Desfazer()
  {
    if (estadoAtivo == Estado.Estado1)
    {
      Destroy(circuito);
      circuito = PreRed[PreRed.Count - 1];
      circuito.name = "Pecas";
      circuito.SetActive(true);
      SetarReducoes();
      PreRed.Remove(circuito);

      reduzidos[1] = false;
      Destroy(r[1]);
      Destroy(r[0]);
      reduzidos[0] = false;

      sequencia.RemoveAt(sequencia.Count - 1);

      estadoAtivo = sequencia[sequencia.Count - 1];

      reduzir = false;

      SetarReducoes();
      SetarSombras();
    }
    else if (estadoAtivo == Estado.Estado2)
    {
      Destroy(circuito);
      circuito = PreRed[PreRed.Count - 1];
      circuito.name = "Pecas";
      circuito.SetActive(true);
      SetarReducoes();
      PreRed.Remove(circuito);

      reduzidos[2] = false;

      Destroy(r[2]);

      sequencia.RemoveAt(sequencia.Count - 1);

      estadoAtivo = sequencia[sequencia.Count - 1];

      reduzir = false;

      SetarResistores();
      SetarSombras();
    }
    else if (estadoAtivo == Estado.Estado3)
    {
      Destroy(circuito);
      circuito = PreRed[PreRed.Count - 1];
      circuito.name = "Pecas";
      circuito.SetActive(true);
      SetarReducoes();
      PreRed.Remove(circuito);

      reduzidos[3] = false;

      Destroy(r[3]);

      sequencia.RemoveAt(sequencia.Count - 1);

      estadoAtivo = sequencia[sequencia.Count - 1];

      reduzir = false;

      SetarResistores();
      SetarSombras();
    }
    else if (estadoAtivo == Estado.Estado4)
    {
      Destroy(circuito);
      circuito = PreRed[PreRed.Count - 1];
      circuito.name = "Pecas";
      circuito.SetActive(true);
      SetarReducoes();
      PreRed.Remove(circuito);

      reduzidos[4] = false;

      Destroy(r[4]);

      sequencia.RemoveAt(sequencia.Count - 1);

      estadoAtivo = sequencia[sequencia.Count - 1];

      reduzir = false;

      SetarResistores();
      SetarSombras();
    }
  }
}