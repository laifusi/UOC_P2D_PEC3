using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon", menuName = "MyObjects/Weapon")]
public class WeaponSO : ScriptableObject
{
    public GameObject Prefab;
    public ProjectileSO Projectile;

    [SerializeField] private Sprite redSprite;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite purpleSprite;
    [SerializeField] private Sprite orangeSprite;
    [SerializeField] private Sprite greenSprite;

    public Sprite SetSprite(TeamColor teamColor)
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
