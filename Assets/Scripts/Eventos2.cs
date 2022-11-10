using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Eventos2 : MonoBehaviour
{

    EventSystem m_EventSystem;
    private Touch touch;
    Vector3 touchPosWorld;
    private float duracaoToque;
    private GameObject objeto = null;
    Vector2 touchPosWorld2D;

    void OnEnable()
    {

        m_EventSystem = EventSystem.current;
    }



    void Update()
    {
        RecebeToque();
        SetarObjeto();
        Rotacao();

    }


    private void RecebeToque()
    {
        if (Input.touchCount > 0)
        {
            duracaoToque += Time.deltaTime;
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended && duracaoToque < 0.2f)
            {
                touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
            }
            else
                duracaoToque = 0.0f;
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
        if (objeto)
            if (objeto.transform.parent.transform.parent.name == "pecas")
                objeto.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);
        objeto = null;
        duracaoToque = 0.0f;
        touchPosWorld2D = Vector2.negativeInfinity;
    }

}
