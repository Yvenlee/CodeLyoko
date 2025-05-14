using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGameOver : MonoBehaviour
{
    public Canvas CanvaGameOver; // Le canvas qui sera affiché/masqué
    private Button[] buttons; // Tableau contenant tous les boutons

    void Start()
    {
        CanvaGameOver.gameObject.SetActive(false); // Assure que le panneau est caché au début

        // Initialisation dynamique du tableau de boutons
        buttons = CanvaGameOver.GetComponentsInChildren<Button>();

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
        if (clickedButton.name == "BtnRetry") // Vérifie si le bouton 'Retry' est cliqué
        {
            Debug.Log("Chargement du jeu...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recharge la scène actuelle
            CanvaGameOver.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        else if (clickedButton.name == "BtnQuitter") // Vérifie si le bouton 'Quit' est cliqué
        {
            Debug.Log("Fermeture du jeu...");
            SceneManager.LoadScene("Menu"); // Ferme l'application
        }
    }
}
