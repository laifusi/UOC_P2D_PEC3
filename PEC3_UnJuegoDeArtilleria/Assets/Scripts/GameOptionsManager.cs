using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOptionsManager : Singleton<GameOptionsManager>
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
    [SerializeField] Button[] colorsButtons;
    [SerializeField] Button[] charactersButtons;
    [SerializeField] Image[] backHairImages;

    int numberOfTeams;
    int charactersPerTeam;

    private bool[] page1Options = new bool[2];
    private bool[] page2Options = new bool[4];
    private int currentTeam;
    private int currentCharacter;
    private ColorBlock nonSelectedColorBlock;
    private ColorBlock selectableColorBlock;
    private Button selectedColorButton;

    public Team[] Teams;

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
            Teams = new Team[numberOfTeams];

            for (int i = 0; i < Teams.Length; i++)
            {
                Teams[i] = new Team();
                Teams[i].Characters = new Character[charactersPerTeam];
                for(int j = 0; j < Teams[i].Characters.Length; j++)
                {
                    Teams[i].Characters[j] = new Character();
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
        characterNameInputField.text = "";

        for (int i = 0; i < charactersButtons.Length; i++)
        {
            nonSelectedColorBlock = charactersButtons[i].colors;
            nonSelectedColorBlock.normalColor = Color.white;
            charactersButtons[i].colors = nonSelectedColorBlock;
            if(i < backHairImages.Length)
            {
                backHairImages[i].color = Color.white;
            }
        }

        page2Options[2] = false;
        page2Options[3] = false;

        page2ContinueButton.interactable = false;
    }

    public void SaveTeamName(InputField input)
    {
        Teams[currentTeam].TeamName = input.text;

        CheckPage2Options(0);
    }

    public void SaveTeamColor(string color)
    {
        selectedColorButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        if (color == "Red")
        {
            Teams[currentTeam].TeamColor = TeamColor.Red;
        }
        else if (color == "Blue")
        {
            Teams[currentTeam].TeamColor = TeamColor.Blue;
        }
        else if (color == "Purple")
        {
            Teams[currentTeam].TeamColor = TeamColor.Purple;
        }
        else if (color == "Orange")
        {
            Teams[currentTeam].TeamColor = TeamColor.Orange;
        }
        else if (color == "Green")
        {
            Teams[currentTeam].TeamColor = TeamColor.Green;
        }

        for(int i = 0; i < colorsButtons.Length; i++)
        {
            if(selectedColorButton != colorsButtons[i])
            {
                nonSelectedColorBlock = colorsButtons[i].colors;
                nonSelectedColorBlock.normalColor = Color.grey;
                colorsButtons[i].colors = nonSelectedColorBlock;
            }
            else
            {
                nonSelectedColorBlock = colorsButtons[i].colors;
                nonSelectedColorBlock.normalColor = Color.white;
                colorsButtons[i].colors = nonSelectedColorBlock;
            }
        }

        CheckPage2Options(1);
    }

    public void SaveCharacter(string character)
    {
        Button selectedCharacterButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        if (character == "FemaleAdventurer")
            Teams[currentTeam].Characters[currentCharacter].CharacterType = CharacterType.FemaleAdventurer;
        else if (character == "FemalePerson")
            Teams[currentTeam].Characters[currentCharacter].CharacterType = CharacterType.FemalePerson;
        else if (character == "MaleAdventurer")
            Teams[currentTeam].Characters[currentCharacter].CharacterType = CharacterType.MaleAdventurer;
        else if (character == "MalePerson")
            Teams[currentTeam].Characters[currentCharacter].CharacterType = CharacterType.MalePerson;
        else if (character == "Robot")
            Teams[currentTeam].Characters[currentCharacter].CharacterType = CharacterType.Robot;
        else if (character == "Zombie")
            Teams[currentTeam].Characters[currentCharacter].CharacterType = CharacterType.Zombie;

        for (int i = 0; i < charactersButtons.Length; i++)
        {
            if (selectedCharacterButton != charactersButtons[i])
            {
                nonSelectedColorBlock = charactersButtons[i].colors;
                nonSelectedColorBlock.normalColor = Color.grey;
                charactersButtons[i].colors = nonSelectedColorBlock;
                if (i < backHairImages.Length)
                {
                    backHairImages[i].color = Color.grey;
                }
            }
            else
            {
                nonSelectedColorBlock = charactersButtons[i].colors;
                nonSelectedColorBlock.normalColor = Color.white;
                charactersButtons[i].colors = nonSelectedColorBlock;
                if (i < backHairImages.Length)
                {
                    backHairImages[i].color = Color.white;
                }
            }
        }

        CheckPage2Options(2);
    }

    public void SaveCharacterName(InputField input)
    {
        Teams[currentTeam].Characters[currentCharacter].CharacterName = input.text;

        CheckPage2Options(3);
    }

    public void SaveAICheck(Toggle toggle)
    {
        Teams[currentTeam].IsAI = toggle.isOn;
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
                    page2ContinueButton.onClick.AddListener(ResetTeamData);
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

    private void ResetTeamData()
    {
        teamNameInputField.text = "";
        selectedColorButton.interactable = false;
        for (int i = 0; i < colorsButtons.Length; i++)
        {
            if (colorsButtons[i].interactable)
            {
                nonSelectedColorBlock = colorsButtons[i].colors;
                nonSelectedColorBlock.normalColor = Color.white;
                colorsButtons[i].colors = nonSelectedColorBlock;
            }
        }

        page2Options[0] = false;
        page2Options[1] = false;
    }
}
