using UnityEngine;
using UnityEngine.UI;

public class CharactersAliveTextUpdater : MonoBehaviour
{
    public int TeamID; //team id

    private Text text; //Text component

    /// <summary>
    /// Start method to subscribe to the OnCharacterDead Action and get the Text component
    /// </summary>
    private void Start()
    {
        PlayerController.OnCharacterDead += CharacterDead;

        text = GetComponent<Text>();
    }

    /// <summary>
    /// Method to update the characters alive text of a given team
    /// </summary>
    /// <param name="teamID">id of the team the dead character was from</param>
    /// <param name="character">PlayerController of the character</param>
    private void CharacterDead(int teamID, PlayerController character)
    {
        if(teamID == TeamID)
        {
            text.text = GameplayManager.Instance.GetNumberOfAliveCharacters(teamID) + " personajes";
        }
    }

    /// <summary>
    /// OnDestroy method to unsubscribe from the OnCharacterDead Action
    /// </summary>
    private void OnDestroy()
    {
        PlayerController.OnCharacterDead -= CharacterDead;
    }
}
