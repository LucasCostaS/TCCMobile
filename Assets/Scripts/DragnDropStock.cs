using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragnDropStock : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private SpriteRenderer rend;
    private string tipo;
    public GameObject prefab;
    public GameObject pai;

    public GameObject state;
    private StateController2 controlador;
    private int limite;
    private void Start()
    {
        controlador = state.GetComponent<StateController2>();
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


        if (controlador.spawn == true)
        {
            Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity, pai.transform);
            controlador.spawn = false;
        }

    }


}
