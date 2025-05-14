using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGameOver : MonoBehaviour
{
    public Canvas CanvaGameOver; // Le canvas qui sera affich�/masqu�
    private Button[] buttons; // Tableau contenant tous les boutons

    void Start()
    {
        CanvaGameOver.gameObject.SetActive(false); // Assure que le panneau est cach� au d�but

        // Initialisation dynamique du tableau de boutons
        buttons = CanvaGameOver.GetComponentsInChildren<Button>();

        if (buttons.Length == 0)
        {
            Debug.LogError("Aucun bouton trouv� dans le panel !");
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
        Debug.Log("Vous avez cliqu� sur " + clickedButton.name);
        if (clickedButton.name == "BtnRetry") // V�rifie si le bouton 'Retry' est cliqu�
        {
            Debug.Log("Chargement du jeu...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recharge la sc�ne actuelle
            CanvaGameOver.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        else if (clickedButton.name == "BtnQuitter") // V�rifie si le bouton 'Quit' est cliqu�
        {
            Debug.Log("Fermeture du jeu...");
            SceneManager.LoadScene("Menu"); // Ferme l'application
        }
    }
}
