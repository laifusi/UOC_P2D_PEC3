using UnityEngine;

public class EnemyDetectionTrigger : MonoBehaviour
{
    private AICharacter aiCharacter; //AICharacter component

    /// <summary>
    /// Start method were we define the layer mask the GameObject should have
    /// to ignore characters from the same team
    /// </summary>
    private void Start()
    {
        aiCharacter = transform.parent.GetComponent<AICharacter>();

        string maskName = "";
        switch (aiCharacter.TeamColor)
        {
            case TeamColor.Red:
                maskName = "RedTeam";
                break;
            case TeamColor.Blue:
                maskName = "BlueTeam";
                break;
            case TeamColor.Purple:
                maskName = "PurpleTeam";
                break;
            case TeamColor.Orange:
                maskName = "OrangeTeam";
                break;
            case TeamColor.Green:
                maskName = "GreenTeam";
                break;
        }
        if (maskName != "")
            gameObject.layer = LayerMask.NameToLayer(maskName);
    }

    /// <summary>
    /// OnTriggerEnter method where we detect when an enemy enters our detection trigger
    /// </summary>
    /// <param name="collision">Collider2D that triggered the trigger</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!aiCharacter.IsEnemyNear && collision.CompareTag("Character"))
        {
            aiCharacter.IsEnemyNear = true;
            aiCharacter.EnemyPosition = collision.transform;
        }
    }

    /// <summary>
    /// OnTriggerExit where we detect when an enemy exits our detection trigger
    /// </summary>
    /// <param name="collision">Collider2D that triggered it</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            aiCharacter.IsEnemyNear = false;
            aiCharacter.EnemyPosition = null;
        }
    }
}
