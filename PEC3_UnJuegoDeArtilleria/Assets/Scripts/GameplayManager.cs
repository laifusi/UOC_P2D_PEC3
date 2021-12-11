using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;
using System.Collections;

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] GameObject characterPrefab;
    [SerializeField] GameObject aiCharacterPrefab;
    [SerializeField] Transform[] initializePositions;
    [SerializeField] CinemachineTargetGroup cinemachineTargetGroup;
    [SerializeField] float turnTime = 10f;

    private int currentTeam;
    private Teams[] teams;
    private List<Teams> aliveTeams = new List<Teams>();
    private PlayerController currentCharacterWithTurn;
    private int currentCharacterIndex;

    private void Start()
    {
        teams = new Teams[GameOptionsManager.Instance.Teams.Length];
        for(int i = 0; i< teams.Length; i++)
        {
            teams[i].Characters = new PlayerController[GameOptionsManager.Instance.Teams[0].Characters.Length];
            teams[i].AliveCharacters = new List<PlayerController>();
        }
        InitializeCharacters();
        currentTeam = -1;
        InvokeRepeating(nameof(ChangeTurn), 0, turnTime);
        StartCoroutine(nameof(Wait));
    }

    private IEnumerator Wait()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
        yield return new WaitForSeconds(5);
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
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

            for(int j = 0; j < GameOptionsManager.Instance.Teams[i].Characters.Length; j++)
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

        currentCharacterIndex = -1;
        ChangeCharacter();
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
        cinemachineTargetGroup.m_Targets[0].target = currentCharacterWithTurn.transform;
    }

    public struct Teams
    {
        public PlayerController[] Characters;
        public List<PlayerController> AliveCharacters;
    }
}
