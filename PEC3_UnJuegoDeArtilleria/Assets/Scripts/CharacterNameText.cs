using UnityEngine;
using UnityEngine.UI;

public class CharacterNameText : MonoBehaviour
{
    [SerializeField] PlayerController character; //PlayerController of the parent character

    /// <summary>
    /// Start method to set the character's name
    /// </summary>
    private void Start()
    {
        GetComponent<Text>().text = character.CharacterName;
    }
}
