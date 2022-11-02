using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragnDrop : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private SpriteRenderer rend;
    private float[] gradeX = new float[4];
    private float[] gradeY = new float[4];
    private Vector2 lugar = new Vector2();
    private Collider2D colisorLixo;
    private Collider2D[] aux;
    private GameObject lixo;
    private int ocupacao;
    private Vector3 posSnap = new Vector3();
    private Vector3 posReserva = new Vector3();
    private Vector3 escalaAtual = new Vector3();
    private float posX;
    private float posY;
    private bool snap = true;

    private bool trava = false;

    private GameObject pecas;
    private PosicaoSnap temp;

    void Start()
    {
        pecas = gameObject.transform.parent.parent.gameObject;
        temp = pecas.GetComponent<PosicaoSnap>();
        aux = Physics2D.OverlapBoxAll(new Vector2(0f, 0f), new Vector2(100f, 100f), 0f);
        foreach (var col in aux)
        {
            if (col.gameObject.tag == "Lixo")
            {
                colisorLixo = col;
                lixo = colisorLixo.gameObject;
                escalaAtual = new Vector3(lixo.transform.localScale.x, lixo.transform.localScale.y, 1f);
            }

        }
    }

    private void OnMouseOver()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.color = Color.yellow;

    }

    private void OnMouseExit()
    {
        rend.color = Color.black;
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
    }

    private void OnMouseDrag()
    {
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
        transform.position = cursorPosition;
        if (Physics2D.IsTouching(colisorLixo, gameObject.GetComponent<BoxCollider2D>()))
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


    private void OnMouseUp()
    {
        gradeX = temp.gradeX;
        gradeY = temp.gradeY;

        if (colisorLixo.IsTouching(gameObject.GetComponent<BoxCollider2D>()))
        {
            Destroy(gameObject.transform.parent.gameObject);
            lixo.transform.localScale = new Vector3(0.667f, 0.667f, 1f);
        }


        for (int i = 0; i < 4; i++)
        {
            if (transform.position.x >= (gradeX[i] - (Math.Abs((gradeX[0] - gradeX[1])) / 2)) && transform.position.x < (gradeX[i] + (Math.Abs((gradeX[0] - gradeX[1])) / 2)))
            {
                posX = gradeX[i];
                snap = true;
            }
            if (transform.position.x > gradeX[3] + (Math.Abs((gradeX[0] - gradeX[1])) / 2))
            {
                snap = false;
            }

            if (transform.position.y >= gradeY[i] - (Math.Abs((gradeY[0] - gradeY[1])) / 2) && transform.position.y < gradeY[i] + (Math.Abs((gradeY[0] - gradeY[1])) / 2))
            {
                posY = gradeY[i];
            }

        }
        posSnap.Set(posX, posY, 0);

        if (snap == true)
        {
            lugar.Set(posX, posY);
            Collider2D[] resultado = Physics2D.OverlapCircleAll(lugar, 2f);
            ocupacao = resultado.Length;

            if (ocupacao > 1)
            {
                UnityEngine.Debug.Log(transform.position.y >= lixo.transform.position.y - (colisorLixo.bounds.size.y / 2));
                if (transform.position.y >= lixo.transform.position.y - (colisorLixo.bounds.size.y / 2))
                {
                    posReserva.Set(gradeX[3] + (Math.Abs(gradeX[0] - gradeX[1]) * 1.1f), transform.position.y - colisorLixo.bounds.size.y, 0);
                    transform.position = posReserva;
                }
                else
                {
                    posReserva.Set(gradeX[3] + (Math.Abs(gradeX[0] - gradeX[1]) * 1.1f), transform.position.y, 0);
                    transform.position = posReserva;
                }
            }
            else
            {
                transform.position = posSnap;
            }

            posX = 0;
            posY = 0;
        }

    }
}
