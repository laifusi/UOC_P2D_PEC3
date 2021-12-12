using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using System.Collections;

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] GameObject characterPrefab; //Prefab of the player controller character
    [SerializeField] GameObject aiCharacterPrefab; //Prefab of the AI character
    [SerializeField] Transform[] initializePositions; //points of instantiation
    [SerializeField] CinemachineTargetGroup cinemachineTargetGroup; //Cinemachine target group
    [SerializeField] float turnTime = 10f; //time that a turn lasts
    [SerializeField] GameObject teamInfoPrefab; //Prefab of the UI team information
    [SerializeField] Transform teamPanel; //Panel the team information should use as its parent
    [SerializeField] Color redColor; //red Color
    [SerializeField] Color blueColor; //blue Color
    [SerializeField] Color purpleColor; //purple Color
    [SerializeField] Color orangeColor; //orange Color
    [SerializeField] Color greenColor; //green Color
    [SerializeField] Text currentTeamText; //Text that shows which team has the turn

    private int currentTeam; //current team
    private Teams[] teams; //array of all teams
    private List<Teams> aliveTeams = new List<Teams>(); //list of the alive teams
    private PlayerController currentCharacterWithTurn; //current PlayerController with turn
    private int currentCharacterIndex; //current character index

    [HideInInspector] public string WinnerTeam; //string of the winning team color
    [HideInInspector] public Color WinnerTeamColor; //Color of the winning team

    public static System.Action OnTurnChange; //Action for when a turn ends

    public struct Teams //struct that defines a team
    {
        public PlayerController[] Characters; //all the characters of the team
        public List<PlayerController> AliveCharacters; //list of alive characters
        public TeamColor TeamColor; //TeamColor
    }

    /// <summary>
    /// Start method where we subscribe to the OnCharacterDead Action,
    /// initialize the characters and the team info
    /// and wait in an attempt to avoid getting the characters to fall off the ground
    /// </summary>
    private void Start()
    {
        base.Start();

        PlayerController.OnCharacterDead += CharacterDead;

        teams = new Teams[GameOptionsManager.Instance.Teams.Length];
        for (int i = 0; i < teams.Length; i++)
        {
            teams[i].Characters = new PlayerController[GameOptionsManager.Instance.Teams[0].Characters.Length];
            teams[i].AliveCharacters = new List<PlayerController>();
        }
        InitializeCharacters();
        InitializeTeamsInfo();

        StartCoroutine(nameof(Wait));
    }

    /// <summary>
    /// Method to initialize the characters defined in the menu
    /// </summary>
    private void InitializeCharacters()
    {
        for (int i = 0; i < GameOptionsManager.Instance.Teams.Length; i++)
        {
            GameObject prefab;
            if (GameOptionsManager.Instance.Teams[i].IsAI)
            {
                prefab = aiCharacterPrefab;
            }
            else
            {
                prefab = characterPrefab;
            }

            teams[i].TeamColor = GameOptionsManager.Instance.Teams[i].TeamColor;

            for (int j = 0; j < GameOptionsManager.Instance.Teams[i].Characters.Length; j++)
            {
                int randomIndex = Random.Range(0, initializePositions.Length);
                Vector3 offset = new Vector3(Random.Range(-10f, 10f), 10, 0);
                GameObject characterGO = Instantiate(prefab, initializePositions[randomIndex].position + offset, Quaternion.identity);
                PlayerController characterPC = characterGO.GetComponent<PlayerController>();
                characterPC.IsAI = GameOptionsManager.Instance.Teams[i].IsAI;
                characterPC.TeamColor = GameOptionsManager.Instance.Teams[i].TeamColor;
                characterPC.CharacterType = GameOptionsManager.Instance.Teams[i].Characters[j].CharacterType;
                characterPC.CharacterName = GameOptionsManager.Instance.Teams[i].Characters[j].CharacterName;
                characterPC.TeamID = i;
                teams[i].Characters[j] = characterPC;
            }
        }

        for (int i = 0; i < teams.Length; i++)
        {
            aliveTeams.Add(teams[i]);
            for (int j = 0; j < teams[i].Characters.Length; j++)
            {
                teams[i].AliveCharacters.Add(teams[i].Characters[j]);
            }
        }
    }

    /// <summary>
    /// Method to initialize the information of the teams defined in the menu
    /// </summary>
    private void InitializeTeamsInfo()
    {
        for (int i = 0; i < teams.Length; i++)
        {
            GameObject teamInfo = Instantiate(teamInfoPrefab, teamPanel);
            Image teamColor = teamInfo.GetComponentInChildren<Image>();
            Text[] textInfo = teamInfo.GetComponentsInChildren<Text>();
            switch (teams[i].TeamColor)
            {
                case TeamColor.Red:
                    teamColor.color = redColor;
                    break;
                case TeamColor.Blue:
                    teamColor.color = blueColor;
                    break;
                case TeamColor.Purple:
                    teamColor.color = purpleColor;
                    break;
                case TeamColor.Orange:
                    teamColor.color = orangeColor;
                    break;
                case TeamColor.Green:
                    teamColor.color = greenColor;
                    break;
            }
            textInfo[0].text = GameOptionsManager.Instance.Teams[i].TeamName;
            textInfo[1].text = teams[i].AliveCharacters.Count.ToString() + " personaje(s)";
            textInfo[1].GetComponent<CharactersAliveTextUpdater>().TeamID = i;
        }
    }

    /// <summary>
    /// Coroutine to deactivate physics for 5 seconds in an attempt to stop the characters from falling off
    /// After the 5 seconds, we Invoke the ChangeTurn method every 10 seconds
    /// </summary>
    private IEnumerator Wait()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
        yield return new WaitForSeconds(5);
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        currentTeam = -1;
        InvokeRepeating(nameof(ChangeTurn), 0, turnTime);
    }

    /// <summary>
    /// Method to change the turn from one team to the next
    /// </summary>
    private void ChangeTurn()
    {
        currentCharacterIndex = 0;
        if(currentCharacterWithTurn != null)
            currentCharacterWithTurn.HasTurn = false;
        currentTeam++;
        if(currentTeam >= aliveTeams.Count)
        {
            currentTeam = 0;
        }

        switch(aliveTeams[currentTeam].TeamColor)
        {
            case TeamColor.Red:
                currentTeamText.text = "ROJO";
                currentTeamText.color = redColor;
                break;
            case TeamColor.Blue:
                currentTeamText.text = "AZUL";
                currentTeamText.color = blueColor;
                break;
            case TeamColor.Purple:
                currentTeamText.text = "MORADO";
                currentTeamText.color = purpleColor;
                break;
            case TeamColor.Orange:
                currentTeamText.text = "NARANJA";
                currentTeamText.color = orangeColor;
                break;
            case TeamColor.Green:
                currentTeamText.text = "VERDE";
                currentTeamText.color = greenColor;
                break;
        }

        currentCharacterIndex = -1;
        ChangeCharacter();
        AICharacter.teamChanged = true;
        OnTurnChange?.Invoke();
    }

    /// <summary>
    /// Method to change which character has the turn
    /// This is used by the ChangeTurn() method but also by the PlayerController and the AICharacter
    /// </summary>
    public void ChangeCharacter()
    {
        if (currentCharacterWithTurn != null)
            currentCharacterWithTurn.HasTurn = false;
        currentCharacterIndex++;
        if(currentCharacterIndex >= aliveTeams[currentTeam].AliveCharacters.Count)
        {
            currentCharacterIndex = 0;
        }
        currentCharacterWithTurn = aliveTeams[currentTeam].AliveCharacters[currentCharacterIndex];
        currentCharacterWithTurn.HasTurn = true;
        cinemachineTargetGroup.m_Targets[0].target = currentCharacterWithTurn?.transform;
    }

    /// <summary>
    /// Method to react to a character's death
    /// We remove it from the alive characters list of the team (defined by id)
    /// If there's no character's alive, we remove the team from the alive teams list
    /// If there's only 1 team left, the game ends and save the winning team information
    /// </summary>
    /// <param name="teamID">id of the team the character was from</param>
    /// <param name="character">PlayerController of the dead character</param>
    private void CharacterDead(int teamID, PlayerController character)
    {
        teams[teamID].AliveCharacters.Remove(character);
        if(teams[teamID].AliveCharacters.Count == 0)
        {
            aliveTeams.Remove(teams[teamID]);
            if(aliveTeams.Count == 1)
            {
                switch(aliveTeams[0].TeamColor)
                {
                    case TeamColor.Red:
                        WinnerTeam = "EQUIPO ROJO";
                        WinnerTeamColor = redColor;
                        break;
                    case TeamColor.Blue:
                        WinnerTeam = "EQUIPO AZUL";
                        WinnerTeamColor = blueColor;
                        break;
                    case TeamColor.Purple:
                        WinnerTeam = "EQUIPO MORADO";
                        WinnerTeamColor = purpleColor;
                        break;
                    case TeamColor.Orange:
                        WinnerTeam = "EQUIPO NARANJA";
                        WinnerTeamColor = orangeColor;
                        break;
                    case TeamColor.Green:
                        WinnerTeam = "EQUIPO VERDE";
                        WinnerTeamColor = greenColor;
                        break;
                }
                CancelInvoke(nameof(ChangeTurn));
                MenuManager.Instance.EndGame();
            }
        }
    }

    /// <summary>
    /// Method that return the number of characters alive from a determined team
    /// </summary>
    /// <param name="teamID">id of the team</param>
    /// <returns>number of alive characters</returns>
    public int GetNumberOfAliveCharacters(int teamID)
    {
        return teams[teamID].AliveCharacters.Count;
    }

    /// <summary>
    /// OnDestroy method to unsubscribe from the OnCharacterDead Action
    /// </summary>
    private void OnDestroy()
    {
        PlayerController.OnCharacterDead -= CharacterDead;
    }
}
