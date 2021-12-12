using UnityEngine;
using UnityEngine.UI;

public class HealthScrollbar : MonoBehaviour
{
    [SerializeField] PlayerController character; //PlayerController of the character
    [SerializeField] Color redColor; //Color red
    [SerializeField] Color blueColor; //Color blue
    [SerializeField] Color purpleColor; //Color purple
    [SerializeField] Color orangeColor; //Color orange
    [SerializeField] Color greenColor; //Color green

    private Scrollbar scrollbar; //Scrollbar component

    /// <summary>
    /// Start method where we instantiate the color of the scrollbar
    /// and subscribe to the OnHealthChanged Action of out character
    /// </summary>
    private void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
        ColorBlock colors = scrollbar.colors;
        switch(character.TeamColor)
        {
            case TeamColor.Red:
                colors.disabledColor = redColor;
                break;
            case TeamColor.Blue:
                colors.disabledColor = blueColor;
                break;
            case TeamColor.Purple:
                colors.disabledColor = purpleColor;
                break;
            case TeamColor.Orange:
                colors.disabledColor = orangeColor;
                break;
            case TeamColor.Green:
                colors.disabledColor = greenColor;
                break;
        }
        scrollbar.colors = colors;

        character.OnHealthChanged += HealthChange;
    }

    /// <summary>
    /// Method to react to the health change
    /// We set the size of the bar to the given health
    /// assuming the maximum health is 100
    /// </summary>
    /// <param name="health">current health</param>
    private void HealthChange(int health)
    {
        scrollbar.size = health / 100f;
    }

    /// <summary>
    /// OnDestroy method where we unsubscribe from the OnHealthChanged Action of out character
    /// </summary>
    private void OnDestroy()
    {
        character.OnHealthChanged -= HealthChange;
    }
}
