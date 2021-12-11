using UnityEngine;
using UnityEngine.UI;

public class CharacterNameText : MonoBehaviour
{
    [SerializeField] PlayerController character;

    private void Start()
    {
        character.OnNameAdded += WriteName;
        GetComponent<Text>().text = character.CharacterName;
    }

    private void WriteName(string name)
    {
        GetComponent<Text>().text = name;
        Debug.Log(name);
    }

    private void OnDestroy()
    {
        character.OnNameAdded -= WriteName;
    }
}
