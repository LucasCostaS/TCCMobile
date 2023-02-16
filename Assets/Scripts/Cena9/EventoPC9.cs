using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventoPC9 : MonoBehaviour
{
  private EventSystem m_EventSystem;

  private Touch toque;
  private bool inicioToque, fimToque, movimentoToque, tocando;
  private Vector3 touchPosWorld, offset, snapAtual;
  private Vector2 touchPosWorld2D;
  private GameObject resistor;
  private float duracaoToque;
  private StateController9 state;
  private Collider2D colisorLixo;
  public List<GameObject> ordemSnap = new List<GameObject>();

  public GameObject controller, circuitoUI, stock, circuito, vitoria, enunciado, prefab, pai, pecaCriada;
  public bool dragging;

  private void OnEnable()
  {
    m_EventSystem = EventSystem.current;
    state = controller.GetComponent<StateController9>();
    colisorLixo = circuitoUI.transform.GetChild(1).GetComponent<BoxCollider2D>();
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      OnMouseDown();
    }

    if (Input.GetMouseButton(0))
    {
      OnMouseDrag();
    }

    if (Input.GetMouseButtonUp(0))
    {
      OnMouseUp();
    }
  }

  private void OnMouseDown()
  {
    PegarPosicaoNoMundo();
    SetarObjeto();

    if (resistor != null && resistor.CompareTag("Resistor"))
    {
      offset = resistor.transform.position - touchPosWorld;
      snapAtual = resistor.transform.position;
    }

    dragging = true;
    duracaoToque += Time.deltaTime;
  }

  private void OnMouseDrag()
  {
    duracaoToque += Time.deltaTime;
    if (resistor != null && resistor.CompareTag("Resistor"))
    {
      PegarPosicaoNoMundo();
      resistor.GetComponent<Resistores9>().SetPosicao(touchPosWorld + offset);
      state.reduzir = true;
      if (Physics2D.IsTouching(colisorLixo, resistor.GetComponent<BoxCollider2D>()))
        colisorLixo.gameObject.transform.localScale = new Vector3(26f, 26f, 1f);
      else
        colisorLixo.gameObject.transform.localScale = new Vector3(22f, 22f, 1f);//escalaAtual;
    }
  }

  private void OnMouseUp()
  {
    dragging = false;
    if (enunciado.activeSelf)
    {
      DesativarEnunciado();
      resistor = null;
      return;
    }

    if (resistor != null)
    {
      if (duracaoToque < 0.2f)
      {
        if (resistor.CompareTag("Stock"))
        {
          LimparResistencia();
          SpawnPeca();
        }
        else if (resistor.CompareTag("Resistor"))
        {
          MostrarResistencia();
          SnapPraPosicaoCorreta();
        }
        else if (resistor.CompareTag("Reduzido"))
        {
          MostrarResistencia();
        }
      }
      else
      {
        if (resistor.CompareTag("Resistor"))
        {
          if (colisorLixo.IsTouching(resistor.transform.GetComponent<BoxCollider2D>()))
            DestruirObjeto();
          else
            SnapPraPosicaoCorreta();
        }
      }
    }
    else
    {
      if (duracaoToque < 0.2f)
      {
        LimparResistencia();
      }
    }
    resistor = null;
    duracaoToque = 0;
  }


  private void DesativarEnunciado()
  {
    //ARRUMAR
    stock.SetActive(true);
    circuito.SetActive(true);
    circuitoUI.SetActive(true);
    enunciado.SetActive(false);
  }

  private void SetarObjeto()
  {
    //IGNORAR AS PECAS DE SOMBRA
    RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward, Mathf.Infinity, (1 << 6));
    if (hitInformation.collider != null)
    {
      resistor = hitInformation.transform.gameObject;
    }
  }

  private void PegarPosicaoNoMundo()
  {
    touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
  }

  private void DestruirObjeto()
  {
    Destroy(resistor);
    colisorLixo.gameObject.transform.localScale = new Vector3(22f, 22f, 1f);
  }

  private void SnapPraPosicaoCorreta()
  {
    GameObject sombra = resistor.GetComponent<Resistores9>().GetSombra();
    Vector3 pos = resistor.transform.parent.transform.position;
    Quaternion rot = Quaternion.identity;
    RaycastHit2D[] results = new RaycastHit2D[2];
    int raycastHit2D = resistor.transform.GetComponent<BoxCollider2D>()
                                         .Raycast(Camera.main.transform.forward,
                                                  results,
                                                  Mathf.Infinity,
                                                  1 << 6);
    if (sombra != null)
    {
      if (raycastHit2D < 1)
      {
        pos = sombra.transform.position;
        rot = sombra.transform.rotation;
      }
      else
        pos = snapAtual;
    }
    else
    {
      RaycastHit2D hitInformation = Physics2D.Raycast(new Vector2(pos.x, pos.y),
                                                      Camera.main.transform.forward,
                                                      Mathf.Infinity,
                                                      1 << 6);
      if (hitInformation.collider != null)
      {
        pos = snapAtual;
      }
    }

    resistor.transform.position = pos;
    resistor.transform.rotation = rot;
  }

  private void LimparResistencia()
  {
    TMP_Text valor = circuitoUI.transform.GetChild(0).GetComponent<TMP_Text>();
    valor.SetText("");
    circuitoUI.transform.GetChild(0).gameObject.SetActive(false);
  }

  private void MostrarResistencia()
  {
    TMP_Text valor = circuitoUI.transform.GetChild(0).GetComponent<TMP_Text>();
    string texto = (resistor.GetComponent<Resistores9>().GetResistencia().ToString()) + " Ohm";

    valor.SetText(texto);
    //if (snapAtual == resistor.transform.position)
    circuitoUI.transform.GetChild(0).gameObject.SetActive(true);
  }

  private void SpawnPeca()
  {
    if (state.spawn == true)
    {
      pecaCriada = Instantiate(prefab, pai.transform.position, Quaternion.identity, pai.transform);

      pecaCriada.GetComponent<Resistores9>().SetCriador(resistor);

      state.spawn = false;

      pecaCriada = null;
    }
  }
}