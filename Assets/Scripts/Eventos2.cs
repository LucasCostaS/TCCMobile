using System.Collections;
using System.Collections.Generic;
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

    void OnEnable()
    {
        m_EventSystem = EventSystem.current;
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
                //Ray ray = Camera.main.ScreenPointToRay(pos);
                //RaycastHit hit;

                //if (Physics.Raycast(ray, out hit))
                //{
                SetarObjeto();
                    if (objeto.transform.parent.name != "Stock")
                    {
                    // toDrag = objeto.transform;
                    // dist = hit.transform.position.z - Camera.main.transform.position.z;
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

            if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
            {
                dragging = false;
                if (duracaoToque < 0.2f)
                {
                    Rotacao();
                    //touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    //touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
                    //tipoToque = "Toque";

                }
                else
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
       
            if (objeto.transform.parent.transform.parent.name == "pecas")
                objeto.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);
        objeto = null;
        duracaoToque = 0.0f;
    }

}
/*void Update()
    {
        
        Vector3 v3;
 
        if (Input.touchCount != 1)
        {
            dragging = false;
            return;
        }
 
        Touch touch = Input.touches[0];
        Vector3 pos = touch.position;
 
        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit;
 
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "cube")
                {
                    toDrag = hit.transform;
                    dist = hit.transform.position.z - Camera.main.transform.position.z;
                    v3 = new Vector3(pos.x, pos.y, dist);
                    v3 = Camera.main.ScreenToWorldPoint(v3);
                    offset = toDrag.position - v3;
                    dragging = true;
                }
            }
        }
 
        if (dragging && touch.phase == TouchPhase.Moved)
        {
            v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            v3 = Camera.main.ScreenToWorldPoint(v3);
            toDrag.position = v3 + offset;
        }
 
        if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            dragging = false;
        }
    }*/