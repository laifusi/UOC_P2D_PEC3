using UnityEngine;
using UnityEngine.UI;

public class EndgameMenuManager : MonoBehaviour
{
    [SerializeField] Text winnerTeamText; //Text of the winner team
    [SerializeField] Button menuButton; //Menu Button
    [SerializeField] Button exitButton; //Exit Button

    /// <summary>
    /// Start method where we assign the MenuManager methods to the buttons
    /// and set the winner team information
    /// </summary>
    private void Start()
    {
        menuButton.onClick.AddListener(() => MenuManager.Instance.Menu());
        exitButton.onClick.AddListener(() => MenuManager.Instance.Exit());

        winnerTeamText.text = GameplayManager.Instance.WinnerTeam;
        winnerTeamText.color = GameplayManager.Instance.WinnerTeamColor;
    }
}
