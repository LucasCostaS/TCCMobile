using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Botoes : MonoBehaviour
{

    public GameObject menuInicial, menuFases;
    public void BotaoJogar()
    {
        menuInicial.SetActive(false);
        menuFases.SetActive(true);
    }

    public void Fase1()
    {
        SceneManager.LoadScene(1);
    }

    public void Fase2()
    {
        SceneManager.LoadScene(2);
    }

    public void Fase3()
    {
        SceneManager.LoadScene(3);
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void BotaoSair()
    {
        Application.Quit();
    }

    public int Teste()
    {
        return 1;
    }


}
