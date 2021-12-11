using UnityEngine;
using UnityEngine.UI;

public class HealthScrollbar : MonoBehaviour
{
    [SerializeField] PlayerController character;
    [SerializeField] Color redColor;
    [SerializeField] Color blueColor;
    [SerializeField] Color purpleColor;
    [SerializeField] Color orangeColor;
    [SerializeField] Color greenColor;

    private Scrollbar scrollbar;

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

    private void HealthChange(int health)
    {
        scrollbar.size = health / 100f;
    }

    private void OnDestroy()
    {
        character.OnHealthChanged -= HealthChange;
    }
}
