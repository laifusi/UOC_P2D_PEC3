using UnityEngine;

public class EnemyDetectionTrigger : MonoBehaviour
{
    private AICharacter aiCharacter;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Character"))
        {
            aiCharacter.IsEnemyNear = true;
            aiCharacter.EnemyPosition = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            aiCharacter.IsEnemyNear = false;
            aiCharacter.EnemyPosition = null;
        }
    }
}
