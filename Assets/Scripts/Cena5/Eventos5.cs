using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Eventos5 : MonoBehaviour
{
  private EventSystem m_EventSystem;
  private Touch toque;
  private bool inicioToque, fimToque;
  private bool movimentoToque;
  private bool tocando;
  private Vector3 touchPosWorld;
  private Vector3 offset;
  private Vector2 touchPosWorld2D;
  private GameObject resistor;
  public GameObject controller;
  public GameObject enunciado;
  public GameObject telaCircuito;
  public GameObject btnDesfazer;
  private GameObject objeto;
  private GameObject inputTexto;
  private float duracaoToque;
  private GameObject stock;
  private bool dragging;
  public GameObject prefab;
  public GameObject pai;
  public GameObject pecaCriada;
  private StateController5 state;

  void OnEnable()
  {
    m_EventSystem = EventSystem.current;
    state = controller.GetComponent<StateController5>();
  }

  void Update()
  {
    tocando = (Input.touchCount > 0);
    if (tocando)
    {
      ReceberToque();
      AcoesDoToque();
    }
  }

  private void AcoesDoToque()
  {
    if (inicioToque)
      InicioDeToque();
    if (movimentoToque && (resistor != null))
      ToqueDrag();
    if (fimToque)
      FimDeToque();
  }

  private void ToqueDrag()
  {
    if (dragging && (resistor.tag == "Resistor"))
    {
      PegarPosicaoNoMundo();
      resistor.transform.position = touchPosWorld + offset;
      /*if (Physics2D.IsTouching(colisorLixo, objeto.GetComponent<BoxCollider2D>()))
      {
        if (trava == false)
        {
          escalaAtual = new Vector3(lixo.transform.localScale.x, lixo.transform.localScale.y, 1f);
          lixo.transform.localScale = new Vector3(lixo.transform.localScale.x * 1.2f, lixo.transform.localScale.y * 1.2f, 1f);
          trava = true;
        }


      }
      else
      {
        lixo.transform.localScale = escalaAtual;
        escalaAtual = new Vector3(lixo.transform.localScale.x, lixo.transform.localScale.y, 1f);
        trava = false;
      }*/
    }
  }

  private void DesativarEnunciado()
  {
    //ARRUMAR
    enunciado.SetActive(false);
    telaCircuito.SetActive(true);
    btnDesfazer.SetActive(true);
    stock.SetActive(true);
  }

  private void InicioDeToque()
  {

    PegarPosicaoNoMundo();

    SetarObjeto();

    if (resistor != null)
      offset = resistor.transform.position - touchPosWorld;
    dragging = true;
  }

  private void ReceberToque()
  {
    toque = Input.GetTouch(0);
    inicioToque = (toque.phase == TouchPhase.Began);
    fimToque = (toque.phase == TouchPhase.Ended || toque.phase == TouchPhase.Canceled);
    movimentoToque = (toque.phase == TouchPhase.Moved);
    duracaoToque += Time.deltaTime;
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
    touchPosWorld = Camera.main.ScreenToWorldPoint(new Vector3(toque.position.x, toque.position.y, 0f));
    touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
  }

  private void FimDeToque()
  {

    dragging = false;
    if (enunciado.activeSelf)
    {
      DesativarEnunciado();
      resistor = null;
      return;
    }

    /* if (inputtexto != null)
     {
         if (!(inputtexto.transform.parent.getcomponent<resistores3>().caixaativa)){
             inputtexto.setactive(false);
             controller.getcomponent<statecontroller3>().click = true;
         }
     }*/

    if (resistor != null)
    {
      if (duracaoToque < 0.2f && (resistor.tag == "Stock"))
        SpawnPeca();

      /*if (duracaoToque < 0.2f && paiPeca)
          Rotacao();

      if (colisorLixo.IsTouching(objeto.GetComponent<BoxCollider2D>()))
          DestruirObjeto();*/

      //bool reduzido = resistor.GetComponent<Resistores3>().reduzido;

      /*if (reduzido == false && controller.GetComponent<StateController3>().click == true)
      {

          inputTexto = resistor.transform.GetChild(0).gameObject;
          inputTexto.SetActive(true);
          controller.GetComponent<StateController3>().click = false;
      }*/

      /*if (reduzido == true)
      {
          inputTexto = resistor.transform.GetChild(1).gameObject;
          inputTexto.SetActive(true);

      }*/
    }
    resistor = null;
    duracaoToque = 0;
    /*if (inputTexto != null)
        inputTexto.transform.parent.GetComponent<Resistores3>().caixaAtiva = false;*/

  }

  private void SpawnPeca()
  {
    if (state.spawn == true)
    {
      pecaCriada = Instantiate(prefab, pai.transform.position, Quaternion.identity, pai.transform);

      pecaCriada.GetComponent<Resistores5>().SetCriador(resistor);

      state.spawn = false;
    }
  }

}
