using UnityEngine;

[CreateAssetMenu(fileName = "new Projectile", menuName = "MyObjects/Projectile")]
public class ProjectileSO : ScriptableObject
{
    public GameObject Prefab; //Prefab for the projectile
    public float Speed; //speed the projectile goes at
    public float GravityAffection; //gravity affection the projectile has
    public int Damage; //damage the projectile does

    [SerializeField] private Sprite redSprite; //sprite for the red team
    [SerializeField] private Sprite blueSprite; //sprite for the blue team
    [SerializeField] private Sprite purpleSprite; //sprite for the purple team
    [SerializeField] private Sprite orangeSprite; //sprite for the orange team
    [SerializeField] private Sprite greenSprite; //sprite for the green team

    /// <summary>
    /// Method to get the sprite for a given TeamColor
    /// </summary>
    /// <param name="teamColor">TeamColor</param>
    /// <returns>projectile Sprite for the given team</returns>
    public Sprite GetSprite(TeamColor teamColor)
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

    /// <summary>
    /// Method to get the layer mask for a given TeamColor
    /// </summary>
    /// <param name="teamColor">TeamColor</param>
    /// <returns></returns>
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

public enum TeamColor //enum that defines the possible TeamColors
{
    Red, Blue, Purple, Orange, Green
}
