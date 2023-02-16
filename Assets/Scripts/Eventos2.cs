using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Eventos2 : MonoBehaviour
{
  private EventSystem m_EventSystem;
  private Touch toque;
  private Vector3 touchPosWorld;
  private Vector3 offset;
  private bool dragging, tocando, inicioToque, fimToque, movimentoToque, stock, paiPeca;
  private float duracaoToque;
  private GameObject objeto = null;
  private string tipoToque;
  private Vector2 touchPosWorld2D;
  private GameObject pai, prefab;
  private StateController2 controlador;
  private DragnDropStock dragnDrop;
  private PosicaoSnap posicaoSnap;
  private Collider2D colisorLixo;
  public GameObject state, lixo;
  private Vector3 escalaAtual;
  private bool trava = false;
  private Vector3 posSnap = new Vector3();
  private Vector3 posReserva = new Vector3();
  private float[] gradeX = new float[4];
  private float[] gradeY = new float[4];
  private float posX;
  private float posY;
  private bool snap = true;
  private int ocupacao;
  private Vector2 lugar = new Vector2();

  private void OnEnable()
  {
    m_EventSystem = EventSystem.current;
    controlador = state.GetComponent<StateController2>();
    posicaoSnap = controlador.pecas.GetComponent<PosicaoSnap>();
    colisorLixo = lixo.GetComponent<BoxCollider2D>();

    escalaAtual = new Vector3(lixo.transform.localScale.x, lixo.transform.localScale.y, 1f);
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
      InicioDeToque();
    if (Input.GetMouseButton(0))
      ToqueDrag();
    if (Input.GetMouseButtonUp(0))
      FimDeToque();
  }

  private void InicioDeToque()
  {

    duracaoToque += Time.deltaTime;
    PegarPosicaoNoMundo();

      SetarObjeto();

      if (!stock)
      {
        PegarPosicaoNoMundo();
        if (objeto != null)
          offset = objeto.transform.position - touchPosWorld;
        dragging = true;
      }
    
  }

  private void FimDeToque()
  {

      gradeX = posicaoSnap.gradeX;
      gradeY = posicaoSnap.gradeY;
      dragging = false;

      if (objeto != null)
      {
        if (duracaoToque < 0.1f && stock)
          SpawnPeca();

        if (duracaoToque < 0.1f && paiPeca)
          Rotacao();

        if (colisorLixo.IsTouching(objeto.GetComponent<BoxCollider2D>()))
          DestruirObjeto();

        if (paiPeca)
        {
          float distancia = (Math.Abs((gradeX[0] - gradeX[1])) / 2);

          for (int i = 0; i < 4; i++)
          {
            if (objeto.transform.position.x >= (gradeX[i] - distancia) && objeto.transform.position.x < (gradeX[i] + distancia))
            {
              posX = gradeX[i];
              snap = true;
            }

            if (objeto.transform.position.x > (gradeX[3] + distancia))
              snap = false;

            if (objeto.transform.position.y >= (gradeY[i] - distancia) && objeto.transform.position.y < gradeY[i] + distancia)
              posY = gradeY[i];
          }

          posSnap.Set(posX, posY, 0);

          if (snap == true)
          {
            lugar.Set(posX, posY);
            Collider2D[] resultado = Physics2D.OverlapCircleAll(lugar, 0.2f);
            ocupacao = resultado.Length;

            PosicionarNaGrade();
          }
        
      }
      posX = 0;
      posY = 0;
    }
    duracaoToque = 0.0f;
    objeto = null;
  }

  private void PosicionarNaGrade()
  {
    if (ocupacao > 1)
    {
      if (objeto.transform.position.y >= lixo.transform.position.y - (colisorLixo.bounds.size.y / 2))
      {
        posReserva.Set(gradeX[3] + (Math.Abs(gradeX[0] - gradeX[1]) * 1.1f), objeto.transform.position.y - colisorLixo.bounds.size.y, 0);
        objeto.transform.position = posReserva;
      }
      else
      {
        posReserva.Set(gradeX[3] + (Math.Abs(gradeX[0] - gradeX[1]) * 1.1f), objeto.transform.position.y, 0);
        objeto.transform.position = posReserva;
      }
    }
    else
    {
      objeto.transform.localPosition = posSnap;
    }
  }

  private void ToqueDrag()
  {
    duracaoToque += Time.deltaTime;
    if (dragging && objeto != null)
    {
      PegarPosicaoNoMundo();
      objeto.transform.position = touchPosWorld + offset;
      if (Physics2D.IsTouching(colisorLixo, objeto.GetComponent<BoxCollider2D>()))
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
      }
    }
  }

  private void SetarObjeto()
  {
    RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
    if (hitInformation.collider != null)
    {
      objeto = hitInformation.transform.gameObject;
    }

    if (objeto != null)
    {
      stock = (objeto.transform.parent.name == "Stock");
      paiPeca = (objeto.transform.parent.name != "Stock");
    }
  }

  private void Rotacao()
  {
    objeto.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);
  }

  private void SpawnPeca()
  {
    if (controlador.spawn == true)
    {
      dragnDrop = objeto.GetComponent<DragnDropStock>();
      prefab = dragnDrop.prefab;
      pai = dragnDrop.pai;
      Instantiate(prefab, new Vector3(0f, 0, 0), Quaternion.identity, pai.transform);
      controlador.spawn = false;
    }
  }

  private void PegarPosicaoNoMundo()
  {
    touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
  }

  private void DestruirObjeto()
  {
    Destroy(objeto.transform.parent.gameObject);
    lixo.transform.localScale = new Vector3(0.667f, 0.667f, 1f);
  }
}