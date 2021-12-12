using UnityEngine;
using UnityEngine.UI;

public class EndgameMenuManager : MonoBehaviour
{
    [SerializeField] Text winnerTeamText;
    [SerializeField] Button menuButton;
    [SerializeField] Button exitButton;

    private void Start()
    {
        menuButton.onClick.AddListener(() => MenuManager.Instance.Menu());
        exitButton.onClick.AddListener(() => MenuManager.Instance.Exit());

        winnerTeamText.text = GameplayManager.Instance.WinnerTeam;
        winnerTeamText.color = GameplayManager.Instance.WinnerTeamColor;
    }
}
