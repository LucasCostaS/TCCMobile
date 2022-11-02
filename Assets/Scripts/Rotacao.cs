using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotacao : MonoBehaviour
{

    private SpriteRenderer rend;

    private void OnMouseOver()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.color = Color.yellow;
        // Se botão direito do mouse pressionado, rotaciona para direita
        if (Input.GetMouseButtonDown(0))
        {
            transform.Rotate(0.0f, 0.0f, -45.0f, Space.Self);
        }
        // Se botão direito do mouse pressionado, rotaciona para esquerda
        else if (Input.GetMouseButtonDown(1))
        {

            transform.Rotate(0.0f, 0.0f, 45.0f, Space.Self);
        }
    }

    private void OnMouseExit()
    {
        rend.color = Color.black;
    }

}
