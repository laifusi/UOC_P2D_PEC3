using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using System.Collections;

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] GameObject characterPrefab;
    [SerializeField] GameObject aiCharacterPrefab;
    [SerializeField] Transform[] initializePositions;
    [SerializeField] CinemachineTargetGroup cinemachineTargetGroup;
    [SerializeField] float turnTime = 10f;
    [SerializeField] GameObject teamInfoPrefab;
    [SerializeField] Transform teamPanel;
    [SerializeField] Color redColor;
    [SerializeField] Color blueColor;
    [SerializeField] Color purpleColor;
    [SerializeField] Color orangeColor;
    [SerializeField] Color greenColor;
    [SerializeField] Text currentTeamText;

    private int currentTeam;
    private Teams[] teams;
    private List<Teams> aliveTeams = new List<Teams>();
    private PlayerController currentCharacterWithTurn;
    private int currentCharacterIndex;

    [HideInInspector] public string WinnerTeam;
    [HideInInspector] public Color WinnerTeamColor;

    public static System.Action OnTurnChange;

    [System.Serializable]
    public struct Teams
    {
        public PlayerController[] Characters;
        public List<PlayerController> AliveCharacters;
        public TeamColor TeamColor;
    }

    private void Start()
    {
        base.Start();

        PlayerController.OnCharacterDead += CharacterDead;

        teams = new Teams[GameOptionsManager.Instance.Teams.Length];
        for(int i = 0; i< teams.Length; i++)
        {
            teams[i].Characters = new PlayerController[GameOptionsManager.Instance.Teams[0].Characters.Length];
            teams[i].AliveCharacters = new List<PlayerController>();
        }
        InitializeCharacters();
        for(int i = 0; i < teams.Length; i++)
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
        currentTeam = -1;

        StartCoroutine(nameof(Wait));
    }

    private IEnumerator Wait()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
        yield return new WaitForSeconds(5);
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        InvokeRepeating(nameof(ChangeTurn), 0, turnTime);
    }

    private void InitializeCharacters()
    {
        for(int i = 0; i < GameOptionsManager.Instance.Teams.Length; i++)
        {
            GameObject prefab;
            if(GameOptionsManager.Instance.Teams[i].IsAI)
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
                characterPC.OnNameAdded?.Invoke(GameOptionsManager.Instance.Teams[i].Characters[j].CharacterName);
                characterPC.CharacterName = GameOptionsManager.Instance.Teams[i].Characters[j].CharacterName;
                characterPC.TeamID = i;
                teams[i].Characters[j] = characterPC;
            }
        }

        for(int i = 0; i < teams.Length; i++)
        {
            aliveTeams.Add(teams[i]);
            for(int j = 0; j < teams[i].Characters.Length; j++)
            {
                teams[i].AliveCharacters.Add(teams[i].Characters[j]);
            }
        }
    }

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

    public int GetNumberOfAliveCharacters(int teamID)
    {
        return teams[teamID].AliveCharacters.Count;
    }

    private void OnDestroy()
    {
        PlayerController.OnCharacterDead -= CharacterDead;
    }
}
