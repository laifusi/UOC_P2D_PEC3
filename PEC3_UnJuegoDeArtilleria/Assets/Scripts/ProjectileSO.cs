using UnityEngine;

[CreateAssetMenu(fileName = "new Projectile", menuName = "MyObjects/Projectile")]
public class ProjectileSO : ScriptableObject
{
    public GameObject Prefab;
    public float Speed;
    public float GravityAffection;
    public int Damage;

    [SerializeField] private Sprite redSprite;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite purpleSprite;
    [SerializeField] private Sprite orangeSprite;
    [SerializeField] private Sprite greenSprite;

    public Sprite SetSprite(TeamColor teamColor)
    {
        Sprite sprite = null;
        switch(teamColor)
        {
            case TeamColor.Red:
                sprite = redSprite;
                break;
            case TeamColor.Blue:
                sprite = blueSprite;
                break;
            case TeamColor.Purple:
                sprite = purpleSprite;
                break;
            case TeamColor.Orange:
                sprite = orangeSprite;
                break;
            case TeamColor.Green:
                sprite = greenSprite;
                break;
        }

        return sprite;
    }

    public int GetLayerMask(TeamColor teamColor)
    {
        string maskName = "";
        switch (teamColor)
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
        return LayerMask.NameToLayer(maskName);
    }
}

public enum TeamColor
{
    Red, Blue, Purple, Orange, Green
}
