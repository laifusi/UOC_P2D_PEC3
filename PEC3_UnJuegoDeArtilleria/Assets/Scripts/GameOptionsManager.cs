using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOptionsManager : Singleton<GameOptionsManager>
{
    [SerializeField] Button page1ContinueButton; //Continue Button of page 1
    [SerializeField] Text teamNameText; //Text of the team name label
    [SerializeField] Text teamColorText; //Text of the team color label
    [SerializeField] Text characterText; //Text of the character type label
    [SerializeField] Text characterNameText; //Text of the character name label
    [SerializeField] Button page2ContinueButton; //Continue Button of page 2
    [SerializeField] InputField teamNameInputField; //InputField of the team name
    [SerializeField] InputField characterNameInputField; //InputField of the character name
    [SerializeField] Toggle AIToggle; //Toggle of the AI option
    [SerializeField] Button[] colorsButtons; //Array of color buttons
    [SerializeField] Button[] charactersButtons; //Array of character buttons
    [SerializeField] Image[] backHairImages; //Array of back hair images

    private int numberOfTeams; //number of teams
    private int charactersPerTeam; //number of characters per team

    private bool[] page1Options = new bool[2]; //array of bools that determine if page 1 has been filled
    private bool[] page2Options = new bool[4]; //array of bools that determine if page 2 has been filled
    private int currentTeam; //current team being defined
    private int currentCharacter; //current character being defined
    private ColorBlock nonSelectedColorBlock; //ColorBlock for the colors or the characters not selected
    private Button selectedColorButton; //Button of the selected color that will be deactivated at the end of the team definition

    public Team[] Teams; //teams defined

    /// <summary>
    /// Method to control the number of teams is between 2 and 5,
    /// save the value and check if page 1 has been completed
    /// </summary>
    /// <param name="input">InputField</param>
    public void CheckNumberOfTeams(InputField input)
    {
        int value = int.Parse(input.text);
        numberOfTeams = Mathf.Clamp(value, 2, 5);
        input.text = numberOfTeams.ToString();

        CheckPage1Options(0);
    }

    /// <summary>
    /// Method to control the number of characters per team is bigger that 0,
    /// save the value and check if page 1 has been completed
    /// </summary>
    /// <param name="input">InputField</param>
    public void CheckNumberOfCharacterPerTeam(InputField input)
    {
        int value = int.Parse(input.text);
        charactersPerTeam = value > 0 ? value : 1;
        input.text = charactersPerTeam.ToString();

        CheckPage1Options(1);
    }

    /// <summary>
    /// Method to check if the page has been completed
    /// If it has we initialize the Teams and their Characters
    /// </summary>
    /// <param name="optionsIndex">id of the option that was updated</param>
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

    /// <summary>
    /// Generic method to check if all the options of a page have been set
    /// If they have we activate its continue button
    /// </summary>
    /// <param name="optionsSelected">array of bools to check</param>
    /// <param name="continueButton">button to activate</param>
    /// <returns>true if all options have been set, false if they haven't</returns>
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

    /// <summary>
    /// Method to update the team choosing panel
    /// We change the labels to define the character and team we are currently defining
    /// We reset the character buttons colors
    /// And we set the last two options of the page to false (character name and character type)
    /// And we deactivate the continue button
    /// </summary>
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

    /// <summary>
    /// Method to save the name of the team
    /// </summary>
    /// <param name="input">InputField</param>
    public void SaveTeamName(InputField input)
    {
        Teams[currentTeam].TeamName = input.text;

        CheckPage2Options(0);
    }

    /// <summary>
    /// Method to save the color of the team
    /// We set every other button's normal color to grey
    /// And the selected one to white
    /// </summary>
    /// <param name="color">string that defines which color was chosen</param>
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

    /// <summary>
    /// Method to save the type of character
    /// We set every other button's normal color and the back hair sprites to grey
    /// And the selected one to white
    /// </summary>
    /// <param name="character">string that defines which character was chosen</param>
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

    /// <summary>
    /// Method to save the name of the character
    /// </summary>
    /// <param name="input"></param>
    public void SaveCharacterName(InputField input)
    {
        Teams[currentTeam].Characters[currentCharacter].CharacterName = input.text;

        CheckPage2Options(3);
    }

    /// <summary>
    /// Method to save the AI Toggle
    /// </summary>
    /// <param name="toggle"></param>
    public void SaveAICheck(Toggle toggle)
    {
        Teams[currentTeam].IsAI = toggle.isOn;
    }

    /// <summary>
    /// Method to check if page 2 has been completed
    /// If it has, we check how many characters and teams there are left
    /// If there's characters left, the continue button will update the current character and reset the choosing panel
    /// If there's no character left but there's teams left, the continue button will update the team, reset the character, the team data and the choosing panel
    /// If there's no character and no team left, the continue button will load the game scene
    /// </summary>
    /// <param name="index">id of the last option set</param>
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

    /// <summary>
    /// Method to reset the data of the team
    /// </summary>
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
