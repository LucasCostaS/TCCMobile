using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Eventos2 : MonoBehaviour
{

    EventSystem m_EventSystem;
    private Touch toque;
    Vector3 touchPosWorld;
    private Vector3 offset;
    private bool dragging, tocando, inicioToque, fimToque, movimentoToque, stock, pecas;
    private float duracaoToque;
    private GameObject objeto = null;
    private string tipoToque;
    Vector2 touchPosWorld2D;
    public GameObject state, prefab;
    private GameObject pai;
    private StateController2 controlador;
    private DragnDropStock dragnDrop;
    private Variables var;

    void OnEnable()
    {
        m_EventSystem = EventSystem.current;
        controlador = state.GetComponent<StateController2>();
        
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

    private void ReceberToque()
    {
        toque = Input.GetTouch(0);
        inicioToque = (toque.phase == TouchPhase.Began);
        fimToque = (toque.phase == TouchPhase.Ended || toque.phase == TouchPhase.Canceled);
        movimentoToque = (toque.phase == TouchPhase.Moved);
    }

    private void AcoesDoToque()
    {
        duracaoToque += Time.deltaTime;

        InicioDeToque();

        ToqueDrag();

        FimDeToque();
    }

    private void InicioDeToque()
    {
        if (inicioToque)
        {
            PegarPosicaoNoMundo();

            SetarObjeto();

            if (!stock)
            {
                PegarPosicaoNoMundo();
                offset = objeto.transform.position - touchPosWorld;
                dragging = true;
            }
        }
    }

    private void FimDeToque()
    {
        if (fimToque)
        {
            dragging = false;
            if (duracaoToque < 0.2f && stock)
            {
                SpawnPeca();
                return;
            }

            if (duracaoToque < 0.2f && pecas)
            {
                Rotacao();
            }
            duracaoToque = 0.0f;
        }
    }

    private void ToqueDrag()
    {
        if (dragging && movimentoToque)
        {
            PegarPosicaoNoMundo();
            objeto.transform.position = touchPosWorld + offset;
        }
    }
    private void SetarObjeto()
    {

        RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
        if (hitInformation.collider != null)
        {
            objeto = hitInformation.transform.gameObject;
        }
        stock = (objeto.transform.parent.name == "Stock");
        pecas = (objeto.transform.parent.transform.parent.name == "pecas");
    }

    private void Rotacao()
    {

        objeto.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);
        objeto = null;
        duracaoToque = 0.0f;
    }

    private void SpawnPeca()
    {
        if (controlador.spawn == true)
        {
            dragnDrop = objeto.GetComponent<DragnDropStock>();
            prefab = dragnDrop.prefab;
            pai = dragnDrop.pai;
            Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity, pai.transform);
            controlador.spawn = false;
        }
    }

    private void PegarPosicaoNoMundo()
    {
        touchPosWorld = Camera.main.ScreenToWorldPoint(new Vector3(toque.position.x, toque.position.y, 0f));
        touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
    }
}
