using UnityEngine;
using UnityEngine.UI;

public class CharactersAliveTextUpdater : MonoBehaviour
{
    public int TeamID;

    private Text text;

    private void Start()
    {
        PlayerController.OnCharacterDead += CharacterDead;

        text = GetComponent<Text>();
    }

    private void CharacterDead(int teamID, PlayerController character)
    {
        if(teamID == TeamID)
        {
            text.text = GameplayManager.Instance.GetNumberOfAliveCharacters(teamID) + " personajes";
        }
    }

    private void OnDestroy()
    {
        PlayerController.OnCharacterDead -= CharacterDead;
    }
}
