using System.Collections.Generic;
using UnityEngine;

public class StateController8 : MonoBehaviour
{
  private enum Estado
  {
    Original,
    Estado1,
    Estado3,
    Estado2,
    Estado4,
    Estado5,
    Estado6,
    Estado7
  }

  private float fps = 60f, newFPS;
  private Resistores8[] resistor = new Resistores8[8];
  private GameObject[] r = new GameObject[8];
  private Vector2[] posSombra = new Vector2[8];
  private bool[] reduzidos = new bool[8];
  private Estado estadoAtivo = Estado.Original;
  private List<Estado> sequencia = new List<Estado>();
  private List<GameObject> Red = new List<GameObject>();
  private List<GameObject> PreRed = new List<GameObject>();
  private List<GameObject> Sombra = new List<GameObject>();
  public bool spawn = true, reduzir = true, trava = false;
  public Eventos8 evento;
  public GameObject pecasCriadas, circuitoUI, stock, circuito, vitoria, enunciado;

  private void Start()
  {
    InicializarTelas();
    SetarReducoes();
    SetarSombras();

    for (int i = 0; i < 8; i++)
    {
      reduzidos[i] = false;
      posSombra[i] = new Vector2(Sombra[i].transform.position.x, Sombra[i].transform.position.y);
    }

    sequencia.Add(Estado.Original);
  }

  private void SetarReducoes()
  {
    Red.Clear();
    for (int i = 1; i < 8; i++)
    {
      Red.Add(circuito.transform.GetChild(i).gameObject);
    }
  }

  private void SetarSombras()
  {
    Sombra.Clear();
    for (int i = 0; i < 8; i++)
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
        resistor[i] = r[i].GetComponent<Resistores8>();
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

    ChecarReducao6();
    if (estadoAtivo == Estado.Estado6)
      Animacao6();

    ChecarReducao7();
    if (estadoAtivo == Estado.Estado7)
      Animacao7();
  }

  private void Animacao7()
  {
    trava = true;
    spawn = false;

    Transform res1 = Red[6].transform.GetChild(0).transform;
    Transform res2 = Red[1].transform.GetChild(0).transform;

    if (res1.localPosition.y < res2.localPosition.y)
      res1.Translate(0f, (10.24f / fps), 0f, Space.World);

    if (res1.localPosition.y >= res2.localPosition.y)
    {
      res1.localPosition = res2.localPosition;
      Red[6].transform.GetChild(1).gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(12).gameObject.SetActive(false);

      spawn = true;
      trava = false; 
      if (Red[6].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia() == 0.3m)
      {
        vitoria.SetActive(true);
        stock.SetActive(false);
        circuitoUI.transform.GetChild(1).gameObject.SetActive(false);
      }
    }
  }

  private void Animacao6()
  {
    trava = true;
    spawn = false;

    Transform res1 = Red[5].transform.GetChild(0).transform;
    Transform res2 = Red[4].transform.GetChild(0).transform;

    if (res1.localPosition.x < res2.localPosition.x)
      res1.Translate((10.24f / fps), 0f, 0f, Space.World);

    if (res1.localPosition.y > res2.localPosition.y)
      res1.Translate(0f, (-5.12f / fps), 0f, Space.World);

    if (res1.localRotation.z > res2.localRotation.z)
      res1.Rotate(0f, 0f, -90f / fps, Space.World);

    if (res1.localPosition.y <= res2.localPosition.y && res1.localPosition.x >= res2.localPosition.x && res1.localRotation.z <= res2.localRotation.z)
    {
      res1.localPosition = res2.localPosition;
      res1.localRotation = res2.localRotation;
      Red[5].transform.GetChild(1).gameObject.SetActive(true);
      Red[4].SetActive(false);

      spawn = true;
      trava = false;
    }
  }

  private void Animacao5()
  {
    trava = true;
    spawn = false;

    Transform res1 = Red[4].transform.GetChild(0).transform;
    Transform res2 = Sombra[4].transform;

    if (res1.localPosition.x < res2.localPosition.x)
      res1.Translate((5.12f / fps), 0f, 0f, Space.World);

    if (res1.localPosition.y < res2.localPosition.y)
      res1.Translate(0f, (2.56f / fps), 0f, Space.World);

    if (res1.localPosition.y >= res2.localPosition.y && res1.localPosition.x >= res2.localPosition.x)
    {
      res1.localPosition = res2.localPosition;
      res2.gameObject.SetActive(false);
      circuito.transform.GetChild(0).GetChild(11).gameObject.SetActive(false);
      r[4].SetActive(false);

      spawn = true;
      trava = false;
    }
  }

  private void Animacao4()
  {
    trava = true;
    spawn = false;

    Transform res1 = Red[3].transform.GetChild(0).transform;
    Transform res2 = Sombra[3].transform;

    if (res1.localPosition.x < res2.localPosition.x)
      res1.Translate((7.68f / fps), 0f, 0f, Space.World);

    if (res1.localPosition.y < res2.localPosition.y)
      res1.Translate(0f, (2.56f / fps), 0f, Space.World);

    if (res1.localPosition.y >= res2.localPosition.y && res1.localPosition.x >= res2.localPosition.x)
    {
      res1.localPosition = res2.localPosition;
      circuito.transform.GetChild(0).GetChild(10).gameObject.SetActive(false);
      res2.gameObject.SetActive(false);
      r[3].SetActive(false);

      spawn = true;
      trava = false;
    }
  }

  private void Animacao3()
  {
    trava = true;
    spawn = false;

    Transform res1 = Red[2].transform.GetChild(0).transform;
    Transform res2;
    if (reduzidos[3])
      res2 = Red[3].transform.GetChild(0).transform;
    else
      res2 = Sombra[6].transform;

    if (res1.localPosition.x < res2.localPosition.x)
      res1.Translate((7.68f / fps), 0f, 0f, Space.World);

    if (res1.localPosition.y < res2.localPosition.y)
      res1.Translate(0f, (2.56f / fps), 0f, Space.World);

    if (res1.localPosition.y >= res2.localPosition.y && res1.localPosition.x >= res2.localPosition.x)
    {
      res1.localPosition = res2.localPosition;
      circuito.transform.GetChild(0).GetChild(9).gameObject.SetActive(false);
      res2.gameObject.SetActive(false);

      if (reduzidos[3])
        Red[3].SetActive(false);
      else
        r[6].SetActive(false);

      spawn = true;
      trava = false;
    }
  }

  private void Animacao1()
  {
    trava = true;
    spawn = false;

    Transform res1 = Red[0].transform.GetChild(0).transform;
    Transform res2;
    if (reduzidos[2])
      res2 = Red[1].transform.GetChild(0).transform;
    else
      res2 = Sombra[1].transform;

    if (res1.localPosition.x < res2.localPosition.x)
      res1.Translate((5.12f / fps), 0f, 0f, Space.World);

    if (res1.localPosition.y > res2.localPosition.y)
      res1.Translate(0f, (-5.12f / fps), 0f, Space.World);

    if (res1.localPosition.y <= res2.localPosition.y && res1.localPosition.x >= res2.localPosition.x)
    {
      res1.localPosition = res2.localPosition;
      res2.gameObject.SetActive(false);

      if (Red[1].activeInHierarchy)
        Red[1].SetActive(false);
      else
      {
        res2.gameObject.SetActive(false);
        r[1].SetActive(false);
      }
      circuito.transform.GetChild(0).GetChild(19).gameObject.SetActive(true);

      spawn = true;
      trava = false;
    }
  }

  private void Animacao2()
  {
    trava = true;
    spawn = false;

    Transform res1 = Red[1].transform.GetChild(0).transform;
    Transform res2 = Sombra[2].transform;

    if (res1.localPosition.x < res2.localPosition.x)
      res1.Translate((5.12f / fps), 0f, 0f, Space.World);

    if (res1.localPosition.y > res2.localPosition.y)
      res1.Translate(0f, (-2.56f / fps), 0f, Space.World);

    if (res1.localPosition.y <= res2.localPosition.y && res1.localPosition.x >= res2.localPosition.x)
    {
      res1.localPosition = res2.localPosition;
      circuito.transform.GetChild(0).GetChild(8).gameObject.SetActive(false);
      if (Red[0].activeInHierarchy)
        Red[0].SetActive(false);
      else
        r[1].SetActive(false);

      r[2].SetActive(false);
      spawn = true;
      trava = false;
    }
  }

  private void ChecarReducao2()
  {
    if (r[2] && r[1] && !reduzidos[2] && reduzir && estadoAtivo != Estado.Estado2 && !trava)
    {
      estadoAtivo = Estado.Estado2;
      sequencia.Add(Estado.Estado2);

      PreRed.Add(Instantiate(circuito));
      PreRed[PreRed.Count - 1].SetActive(false);
      PreRed[PreRed.Count - 1].name = "2";

      reduzidos[2] = true;
      Sombra[1].SetActive(false);
      Sombra[2].SetActive(false);
      Red[1].SetActive(true);

      if (!Red[0].activeInHierarchy)
      {
        r[1].SetActive(false);
        Red[1].transform.GetChild(0).GetComponent<Resistores8>().SetResistencia(1 / (1 / resistor[1].GetResistencia() + 1 / resistor[2].GetResistencia()));
        reduzidos[1] = true;
      }
      else
        Red[1].transform.GetChild(0).GetComponent<Resistores8>().SetResistencia(1 / (1 / Red[0].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia() + 1 / resistor[2].GetResistencia()));

      if (Red[0].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(1).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[0].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[2].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(3).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[2].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[3].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(4).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[3].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[4].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(5).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[4].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[5].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(6).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[5].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
    }
  }

  private void ChecarReducao1()
  {
    if (r[0] && r[1] && !reduzidos[0] && reduzir && estadoAtivo != Estado.Estado1 && !trava)
    {
      estadoAtivo = Estado.Estado1;
      sequencia.Add(Estado.Estado1);

      PreRed.Add(Instantiate(circuito));
      PreRed[PreRed.Count - 1].SetActive(false);
      PreRed[PreRed.Count - 1].name = "1";

      reduzidos[0] = true;
      r[0].SetActive(false);
      Sombra[0].SetActive(false);
      Red[0].SetActive(true);

      if (!Red[1].activeInHierarchy)
      {
        Red[0].transform.GetChild(0).GetComponent<Resistores8>().SetResistencia(1 / (1 / resistor[1].GetResistencia() + 1 / resistor[0].GetResistencia()));
        reduzidos[1] = true;
      }
      else
        Red[0].transform.GetChild(0).GetComponent<Resistores8>().SetResistencia(1 / (1 / Red[1].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia() + 1 / resistor[0].GetResistencia()));

      if (Red[1].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(2).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[1].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[2].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(3).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[2].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[3].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(4).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[3].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[4].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(5).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[4].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[5].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(6).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[5].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
    }
  }

  private void ChecarReducao3()
  {
    if (r[7] && r[6] && !reduzidos[7] && reduzir && estadoAtivo != Estado.Estado3 && !trava)
    {
      estadoAtivo = Estado.Estado3;
      sequencia.Add(Estado.Estado3);

      PreRed.Add(Instantiate(circuito));
      PreRed[PreRed.Count - 1].SetActive(false);
      PreRed[PreRed.Count - 1].name = "3";

      reduzidos[7] = true;
      r[7].SetActive(false);
      Sombra[7].SetActive(false);
      Red[2].SetActive(true);

      if (!Red[3].activeInHierarchy)
      {
        Red[2].transform.GetChild(0).GetComponent<Resistores8>().SetResistencia(1 / (1 / resistor[7].GetResistencia() + 1 / resistor[6].GetResistencia()));
        reduzidos[6] = true;
      }
      else
        Red[2].transform.GetChild(0).GetComponent<Resistores8>().SetResistencia(1 / (1 / Red[3].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia() + 1 / resistor[7].GetResistencia()));

      if (Red[0].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(1).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[0].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[1].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(2).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[1].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[3].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(4).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[3].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[4].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(5).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[4].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[5].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(6).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[5].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
    }
  }

  private void ChecarReducao4()
  {
    if (r[6] && r[3] && !reduzidos[3] && reduzir && estadoAtivo != Estado.Estado4 && !trava)
    {
      estadoAtivo = Estado.Estado4;
      sequencia.Add(Estado.Estado4);

      PreRed.Add(Instantiate(circuito));
      PreRed[PreRed.Count - 1].SetActive(false);
      PreRed[PreRed.Count - 1].name = "4";

      reduzidos[3] = true;
      Sombra[6].SetActive(false);
      Sombra[3].SetActive(false);
      Red[3].SetActive(true);

      if (!Red[2].activeInHierarchy)
      {
        Red[3].transform.GetChild(0).GetComponent<Resistores8>().SetResistencia(1 / (1 / resistor[6].GetResistencia() + 1 / resistor[3].GetResistencia()));
        reduzidos[6] = true;
        r[6].SetActive(false);
      }
      else
      {
        PreRed[PreRed.Count - 1].transform.GetChild(3).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[2].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
        Red[2].SetActive(false);
        Red[3].transform.GetChild(0).GetComponent<Resistores8>().SetResistencia(1 / (1 / Red[2].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia() + 1 / resistor[3].GetResistencia()));
      }

      if (Red[0].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(1).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[0].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[1].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(2).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[1].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[4].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(5).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[4].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[5].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(6).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[5].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
    }
  }

  private void ChecarReducao5()
  {
    if (r[4] && r[5] && !reduzidos[4] && !reduzidos[5] && reduzir && estadoAtivo != Estado.Estado5 && !trava)
    {
      estadoAtivo = Estado.Estado5;
      sequencia.Add(Estado.Estado5);

      PreRed.Add(Instantiate(circuito));
      PreRed[PreRed.Count - 1].SetActive(false);
      PreRed[PreRed.Count - 1].name = "5";

      reduzidos[4] = true;
      reduzidos[5] = true;
      r[5].SetActive(false);
      Sombra[5].SetActive(false);

      Red[4].SetActive(true);
      Red[4].transform.GetChild(0).GetComponent<Resistores8>().SetResistencia(1 / (1 / resistor[4].GetResistencia() + 1 / resistor[5].GetResistencia()));

      if (Red[0].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(1).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[0].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[1].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(2).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[1].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[2].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(3).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[2].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[3].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(4).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[3].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[5].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(6).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[5].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
    }
  }

  private void ChecarReducao7()
  {
    if (Red[5].activeInHierarchy && reduzidos[0] && reduzidos[2] && reduzir && estadoAtivo != Estado.Estado7 && !trava)
    {
      estadoAtivo = Estado.Estado7;
      sequencia.Add(Estado.Estado7);

      PreRed.Add(Instantiate(circuito));
      PreRed[PreRed.Count - 1].SetActive(false);
      PreRed[PreRed.Count - 1].name = "7";

      PreRed[PreRed.Count - 1].transform.GetChild(6).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[5].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      Red[6].SetActive(true);
      Red[5].SetActive(false);
      Red[6].transform.GetChild(0).GetComponent<Resistores8>().SetResistencia(1 / (1 / Red[1].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia() + 1 / Red[5].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia()));

      reduzir = false;
    }
  }

  private void ChecarReducao6()
  {
    if (Red[4].activeInHierarchy && reduzidos[7] && reduzidos[3] && reduzir && estadoAtivo != Estado.Estado6 && !trava)
    {
      estadoAtivo = Estado.Estado6;
      sequencia.Add(Estado.Estado6);

      PreRed.Add(Instantiate(circuito));
      PreRed[PreRed.Count - 1].SetActive(false);
      PreRed[PreRed.Count - 1].name = "6";

      PreRed[PreRed.Count - 1].transform.GetChild(4).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[3].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      Red[5].SetActive(true);
      Red[3].SetActive(false);
      Red[5].transform.GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[3].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia() + Red[4].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());

      if (Red[0].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(1).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[0].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[1].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(2).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[1].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[2].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(3).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[2].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
      if (Red[4].activeInHierarchy)
        PreRed[PreRed.Count - 1].transform.GetChild(5).GetChild(0).GetComponent<Resistores8>().SetResistencia(Red[4].transform.GetChild(0).GetComponent<Resistores8>().GetResistencia());
    }
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

      if (!Red[1].activeInHierarchy)
      {
        reduzidos[1] = false;
        Destroy(r[1]);
      }
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

      if (!Red[0].activeInHierarchy)
      {
        reduzidos[1] = false;
        Destroy(r[1]);
      }

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

      reduzidos[7] = false;

      Destroy(r[7]);

      if (!Red[3].activeInHierarchy)
      {
        reduzidos[6] = false;
        Destroy(r[6]);
      }

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

      reduzidos[3] = false;

      Destroy(r[3]);

      if (!Red[2].activeInHierarchy)
      {
        reduzidos[6] = false;
        Destroy(r[6]);
      }

      sequencia.RemoveAt(sequencia.Count - 1);

      estadoAtivo = sequencia[sequencia.Count - 1];

      reduzir = false;

      SetarResistores();
      SetarSombras();
    }
    else if (estadoAtivo == Estado.Estado5)
    {
      Destroy(circuito);
      circuito = PreRed[PreRed.Count - 1];
      circuito.name = "Pecas";
      circuito.SetActive(true);
      SetarReducoes();
      PreRed.Remove(circuito);

      reduzidos[4] = false;
      Destroy(r[4]);
      reduzidos[5] = false;
      Destroy(r[5]);

      sequencia.RemoveAt(sequencia.Count - 1);

      estadoAtivo = sequencia[sequencia.Count - 1];

      reduzir = false;

      SetarResistores();
      SetarSombras();
    }
    else if (estadoAtivo == Estado.Estado6 || estadoAtivo == Estado.Estado7)
    {
      Destroy(circuito);
      circuito = PreRed[PreRed.Count - 1];
      circuito.name = "Pecas";
      circuito.SetActive(true);
      SetarReducoes();
      PreRed.Remove(circuito);

      sequencia.RemoveAt(sequencia.Count - 1);

      estadoAtivo = sequencia[sequencia.Count - 1];

      reduzir = false;

      SetarResistores();
      SetarSombras();
      Desfazer();
    }
  }
}