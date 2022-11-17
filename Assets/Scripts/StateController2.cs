using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController2 : MonoBehaviour
{

    public bool spawn;
    private Vector2 lugar = new Vector2(0.0f, 0.0f);
    private Collider2D vet, vet1;
    private float[] gradeX = new float[4];
    private float[] gradeY = new float[4];
    private Vector2[] posicao = new Vector2[16];
    private bool[] posicaoCorreta = new bool[16];
    public GameObject vitoria, sombra, pecas, stock;

    void Start()
    {
        spawn = true;
    }

    void Update()
    {

        setarSnap();

        vet = Physics2D.OverlapCircle(lugar, 0.1f);
        spawn = (vet == null);

        checarPosicao();

        checarVitoria();

    }

    public void setarSnap()
    {
        for (int i = 0; i < gradeY.Length; i++)
        {
            gradeX[i] = sombra.transform.GetChild(i).transform.position.x;
            gradeY[i] = sombra.transform.GetChild(4 * i).transform.position.y;
        }
        for (int i = 0; i < gradeY.Length; i++)
        {
            for (int j = 0; j < gradeY.Length; j++)
            {
                posicao[j + (4 * i)] = new Vector2(gradeX[j], gradeY[gradeY.Length - (1 + i)]);
            }
        }
    }

    private void checarPosicao()
    {
        for (int i = 0; i < posicaoCorreta.Length; i++)
        {
            vet1 = Physics2D.OverlapCircle(posicao[i], 0f);

            switch (i)
            {
                case 0:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaLPrefab" && Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 90);
                    break;
                case 1:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaRetaPrefab" && (Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 0 || Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 180));
                    break;
                case 2:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaTPrefab" && Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 180);
                    break;
                case 3:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaLPrefab" && Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 180);
                    break;
                case 4:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaRetaPrefab" && (Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 90 || Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 270));
                    break;
                case 5:
                    posicaoCorreta[i] = (vet1 == null);
                    break;
                case 6:
                     posicaoCorreta[i] = (vet1 != null && vet1.tag == "ResistorPrefab" && Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 90);                  
                    break;
                case 7:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaRetaPrefab" && (Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 90 || Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 270));
                    break;
                case 8:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaTPrefab" && Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 90);
                    break;
                case 9:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "FontePrefab" && Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 0);
                    break;
                case 10:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaLPrefab" && Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 270);
                    break;
                case 11:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaRetaPrefab" && (Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 90 || Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 270));
                    break;
                case 12:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaLPrefab" && Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 0);
                    break;
                case 13:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "ResistorPrefab" && Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 0);
                    break;
                case 14:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaRetaPrefab" && (Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 0 || Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 180));
                    break;
                case 15:
                    posicaoCorreta[i] = (vet1 != null && vet1.tag == "LinhaLPrefab" && Math.Ceiling(vet1.gameObject.transform.eulerAngles.z) == 270);
                    break;
                default:
                    break;
            }

        }
    }

    private void checarVitoria()
    {
        int cont = 0;
        for (int i = 0; i < posicaoCorreta.Length; i++)
        {
            if (posicaoCorreta[i] == true)
                cont += 1;

        }
        if (cont >= 16)
        {
            vitoria.SetActive(true);
            sombra.SetActive(false);
            stock.SetActive(false);
            int k = pecas.transform.childCount;
            for (int i = 0; i < k; i++)
            {
                Destroy(pecas.transform.GetChild(i).GetChild(0).GetComponent<DragnDrop>());
                pecas.transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().color = Color.yellow;
                pecas.transform.GetChild(i).GetChild(0).localScale = new Vector3(0.5f, 0.5f, 0f);
                pecas.transform.GetChild(i).GetChild(0).localPosition = new Vector3(pecas.transform.GetChild(i).GetChild(0).localPosition.x * 0.5f, pecas.transform.GetChild(i).GetChild(0).localPosition.y * 0.5f, 0f);

            }

            spawn = false;
        }
    }

}
