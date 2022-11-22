using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void OnEnable()
    {

    }

    // Update is called once per frame
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
        InicioDeToque();
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
        if (inicioToque)
        {
            PegarPosicaoNoMundo();

            SetarObjeto();
        }
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

        //if (objeto != null)
        //{
        //    stock = (objeto.transform.parent.name == "Stock");
        //    paiPeca = (objeto.transform.parent.name != "Stock");
        //}


    }

    private void PegarPosicaoNoMundo()
    {
        touchPosWorld = Camera.main.ScreenToWorldPoint(new Vector3(toque.position.x, toque.position.y, 0f));
        touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
    }

    private void FimDeToque()
    {

        if (fimToque)
        {
            if (resistor != null)
            {
                bool reduzido = resistor.GetComponent<Resistores3>().reduzido;

                if (reduzido == false && controller.GetComponent<StateController3>().click == true)
                {
                    resistor.transform.GetChild(0).gameObject.SetActive(true);
                    controller.GetComponent<StateController3>().click = false;
                }

                //GetComponent<SpriteRenderer>().color = Color.yellow;
                if (reduzido == true)
                {
                    resistor.transform.GetChild(1).gameObject.SetActive(true);

                }

                
            }
            if (enunciado.activeSelf)
            {
                DesativarEnunciado();
            }
            resistor = null;

        }
    }
}