using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Eventos2 : MonoBehaviour
{

    EventSystem m_EventSystem;
    private Touch touch;
    Vector3 touchPosWorld;
    private Vector3 offset;
    private bool dragging;
    private float duracaoToque;
    private GameObject objeto = null;
    private string tipoToque;
    Vector2 touchPosWorld2D;
    public GameObject state, prefab;
    private GameObject pai;
    private StateController2 controlador;
    private DragnDropStock Stock;
    private Variables var;

    void OnEnable()
    {
        m_EventSystem = EventSystem.current;
        controlador = state.GetComponent<StateController2>();
    }

    void Update()
    {
        RecebeToque();
    }

    private void RecebeToque()
    {
        if (Input.touchCount > 0)
        {
            duracaoToque += Time.deltaTime;
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchPosWorld = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

                SetarObjeto();

                if (objeto.transform.parent.name != "Stock")
                {
                touchPosWorld = new Vector3(touch.position.x, touch.position.y, 0f);
                touchPosWorld = Camera.main.ScreenToWorldPoint(touchPosWorld);
                offset = objeto.transform.position - touchPosWorld;
                dragging = true;
                }
                
            }

            if (dragging && touch.phase == TouchPhase.Moved)
            {
                touchPosWorld = new Vector3(touch.position.x, touch.position.y, 0f);
                touchPosWorld = Camera.main.ScreenToWorldPoint(touchPosWorld);
                objeto.transform.position = touchPosWorld + offset;
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                dragging = false;
                if (duracaoToque < 0.2f && objeto.transform.parent.name == "Stock")
                {
                    SpawnPeca();
                    return;
                }

                if (duracaoToque < 0.2f && objeto.transform.parent.transform.parent.name == "pecas")
                {
                    Rotacao();
                }

                duracaoToque = 0.0f;
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
            Stock = objeto.GetComponent<DragnDropStock>();
            prefab = Stock.prefab;
            pai = Stock.pai;
            Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity, pai.transform);
            controlador.spawn = false;
        }
    }
}
