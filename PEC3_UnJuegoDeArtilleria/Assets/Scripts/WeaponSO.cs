using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon", menuName = "MyObjects/Weapon")]
public class WeaponSO : ScriptableObject
{
    public GameObject Prefab; //weapon prefab
    public ProjectileSO Projectile; //type of projectile the weapon shoots

    [SerializeField] private Sprite redSprite; //sprite for the red team
    [SerializeField] private Sprite blueSprite; //sprite for the blue team
    [SerializeField] private Sprite purpleSprite; //sprite for the purple team
    [SerializeField] private Sprite orangeSprite; //sprite for the orange team
    [SerializeField] private Sprite greenSprite; //sprite for the green team

    /// <summary>
    /// Method to get the sprite for a given TeamColor
    /// </summary>
    /// <param name="teamColor">TeamColor</param>
    /// <returns>weapon Sprite for the given team</returns>
    public Sprite GetSprite(TeamColor teamColor)
    {
        Sprite sprite = null;
        switch (teamColor)
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
}
