using UnityEngine;

public class Eventos3 : MonoBehaviour
{
  private Touch toque;
  private bool inicioToque, fimToque;
  private bool tocando;
  private Vector3 touchPosWorld;
  private Vector2 touchPosWorld2D;
  private GameObject resistor;
  public GameObject controller;
  public GameObject enunciado;
  public GameObject telaCircuito;
  public GameObject btnDesfazer;
  private GameObject inputTexto;

  private void Update()
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
    if (fimToque)
      FimDeToque();
  }

  private void DesativarEnunciado()
  {
    enunciado.SetActive(false);
    telaCircuito.SetActive(true);
    btnDesfazer.SetActive(true);
  }

  private void InicioDeToque()
  {
    PegarPosicaoNoMundo();

    SetarObjeto();
  }

  private void ReceberToque()
  {
    toque = Input.GetTouch(0);
    inicioToque = (toque.phase == TouchPhase.Began);
    fimToque = (toque.phase == TouchPhase.Ended);
  }

  private void SetarObjeto()
  {
    RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
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
    if (enunciado.activeSelf)
    {
      DesativarEnunciado();
      resistor = null;
      return;
    }

    if (inputTexto != null)
    {
      if (!(inputTexto.transform.parent.GetComponent<Resistores3>().caixaAtiva))
      {
        inputTexto.SetActive(false);
        controller.GetComponent<StateController3>().click = true;
      }
    }

    if (resistor != null)
    {
      bool reduzido = resistor.GetComponent<Resistores3>().reduzido;

      if (reduzido == false && controller.GetComponent<StateController3>().click == true)
      {
        inputTexto = resistor.transform.GetChild(0).gameObject;
        inputTexto.SetActive(true);
        controller.GetComponent<StateController3>().click = false;
      }

      if (reduzido == true)
      {
        inputTexto = resistor.transform.GetChild(1).gameObject;
        inputTexto.SetActive(true);
      }
    }
    resistor = null;
    if (inputTexto != null)
      inputTexto.transform.parent.GetComponent<Resistores3>().caixaAtiva = false;
  }
}