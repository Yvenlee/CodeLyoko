using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLaunchScript : MonoBehaviour
{
    public GameObject panelMenu;
    public GameObject panelCredit;
    private Button[] buttons;

    void Start()
    {
        if (GetComponent<AudioSource>())
            GetComponent<AudioSource>().Play();
        panelCredit.SetActive(false);

        buttons = panelMenu.GetComponentsInChildren<Button>();

        if (buttons.Length == 0)
        {
            Debug.LogError("Aucun bouton trouvé dans le panel !");
        }
        else
        {
            foreach (Button btn in buttons)
            {
                btn.onClick.AddListener(() => ButtonSelected(btn));
            }
        }
    }

    void ButtonSelected(Button clickedButton)
    {
        Debug.Log("Vous avez cliqué sur " + clickedButton.name);
        if (clickedButton.name == "BtnJouer")
        {
            Debug.Log("Chargement du jeu...");
            SceneManager.LoadScene("SampleScene");
            Time.timeScale = 1;
        }
        else if (clickedButton.name == "BtnCredit")
        {
            Debug.Log("Affichage des crédits...");
            panelCredit.SetActive(!panelCredit.activeSelf);
        }
        else if (clickedButton.name == "BtnQuitter")
        {
            Debug.Log("Fermeture du jeu...");
            Application.Quit();
        }
    }
}
