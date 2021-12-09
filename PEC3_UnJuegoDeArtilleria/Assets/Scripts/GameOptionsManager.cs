using UnityEngine;
using UnityEngine.UI;

public class GameOptionsManager : MonoBehaviour
{
    [SerializeField] Button page1ContinueButton;
    [SerializeField] Text teamNameText;
    [SerializeField] Text teamColorText;
    [SerializeField] Text characterText;
    [SerializeField] Text characterNameText;
    [SerializeField] Button page2ContinueButton;
    [SerializeField] InputField teamNameInputField;
    [SerializeField] InputField characterNameInputField;
    [SerializeField] Toggle AIToggle;

    int numberOfTeams;
    int charactersPerTeam;
    Team[] teams;

    private bool[] page1Options = new bool[2];
    private bool[] page2Options = new bool[4];
    private int currentTeam;
    private int currentCharacter;

    public void CheckNumberOfTeams(InputField input)
    {
        int value = int.Parse(input.text);
        numberOfTeams = Mathf.Clamp(value, 2, 5);
        input.text = numberOfTeams.ToString();

        CheckPage1Options(0);
    }

    public void CheckNumberOfCharacterPerTeam(InputField input)
    {
        int value = int.Parse(input.text);
        charactersPerTeam = value > 0 ? value : 1;
        input.text = charactersPerTeam.ToString();

        CheckPage1Options(1);
    }

    private void CheckPage1Options(int optionsIndex)
    {
        page1Options[optionsIndex] = true;
        bool allOptionsSelected = CheckAllOptionsSelected(page1Options, page1ContinueButton);
        if(allOptionsSelected)
        {
            teams = new Team[numberOfTeams];

            for (int i = 0; i < teams.Length; i++)
            {
                teams[i] = new Team();
                teams[i].Characters = new Character[charactersPerTeam];
                for(int j = 0; j < teams[i].Characters.Length; j++)
                {
                    teams[i].Characters[j] = new Character();
                }
            }
        }
    }

    private bool CheckAllOptionsSelected(bool[] optionsSelected, Button continueButton)
    {
        for(int i = 0; i < optionsSelected.Length; i++)
        {
            if(!optionsSelected[i])
            {
                return false;
            }
        }

        continueButton.interactable = true;
        return true;
    }

    public void UpdateTeamChoosingPanel()
    {
        int teamNumber = currentTeam + 1;
        int characterNumber = currentCharacter + 1;
        teamNameText.text = "Nombre del equipo (" + teamNumber + "/" + numberOfTeams + "):";
        teamColorText.text = "Color del equipo (" + teamNumber + "/" + numberOfTeams + "):";
        characterText.text = "Personaje (" + characterNumber + "/" + charactersPerTeam + "):";
        characterNameText.text = "Nombre del personaje (" + characterNumber + "/" + charactersPerTeam + "):";
        teamNameInputField.text = "";
        characterNameInputField.text = "";
        page2ContinueButton.interactable = false;
    }

    public void SaveTeamName(InputField input)
    {
        teams[currentTeam].TeamName = input.text;

        CheckPage2Options(0);
    }

    public void SaveTeamColor(string color)
    {
        if(color == "Red")
            teams[currentTeam].TeamColor = TeamColor.Red;
        else if (color == "Blue")
            teams[currentTeam].TeamColor = TeamColor.Blue;
        else if (color == "Purple")
            teams[currentTeam].TeamColor = TeamColor.Purple;
        else if (color == "Orange")
            teams[currentTeam].TeamColor = TeamColor.Orange;
        else if (color == "Green")
            teams[currentTeam].TeamColor = TeamColor.Green;

        CheckPage2Options(1);
    }

    public void SaveCharacter(string character)
    {
        if (character == "FemaleAdventurer")
            teams[currentTeam].Characters[currentCharacter].CharacterType = CharacterType.FemaleAdventurer;
        else if (character == "FemalePerson")
            teams[currentTeam].Characters[currentCharacter].CharacterType = CharacterType.FemalePerson;
        else if (character == "MaleAdventurer")
            teams[currentTeam].Characters[currentCharacter].CharacterType = CharacterType.MaleAdventurer;
        else if (character == "MalePerson")
            teams[currentTeam].Characters[currentCharacter].CharacterType = CharacterType.MalePerson;
        else if (character == "Robot")
            teams[currentTeam].Characters[currentCharacter].CharacterType = CharacterType.Robot;
        else if (character == "Zombie")
            teams[currentTeam].Characters[currentCharacter].CharacterType = CharacterType.Zombie;

        CheckPage2Options(2);
    }

    public void SaveCharacterName(InputField input)
    {
        teams[currentTeam].Characters[currentCharacter].CharacterName = input.text;

        CheckPage2Options(3);
    }

    public void SaveAICheck(Toggle toggle)
    {
        teams[currentTeam].IsAI = toggle.isOn;
    }

    private void CheckPage2Options(int index)
    {
        page2Options[index] = true;
        bool allOptionsSelected = CheckAllOptionsSelected(page2Options, page2ContinueButton);
        if(allOptionsSelected)
        {
            if (currentTeam + 1 < numberOfTeams)
            {
                if (currentCharacter + 1 < charactersPerTeam)
                {
                    page2ContinueButton.onClick.RemoveAllListeners();
                    page2ContinueButton.onClick.AddListener(() => currentCharacter++);
                    page2ContinueButton.onClick.AddListener(UpdateTeamChoosingPanel);
                }
                else
                {
                    page2ContinueButton.onClick.RemoveAllListeners();
                    page2ContinueButton.onClick.AddListener(() => currentTeam++);
                    page2ContinueButton.onClick.AddListener(() => currentCharacter = 0);
                    page2ContinueButton.onClick.AddListener(() => AIToggle.isOn = false);
                    page2ContinueButton.onClick.AddListener(UpdateTeamChoosingPanel);
                }
            }
            else
            {
                if (currentCharacter + 1 < charactersPerTeam)
                {
                    page2ContinueButton.onClick.RemoveAllListeners();
                    page2ContinueButton.onClick.AddListener(() => currentCharacter++);
                    page2ContinueButton.onClick.AddListener(UpdateTeamChoosingPanel);
                }
                else
                {
                    page2ContinueButton.onClick.RemoveAllListeners();
                    page2ContinueButton.onClick.AddListener(() => MenuManager.Instance.Play());
                }
            }
        }
    }
}
