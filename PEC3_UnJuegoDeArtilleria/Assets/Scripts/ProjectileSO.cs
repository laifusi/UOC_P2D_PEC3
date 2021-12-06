using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Projectile", menuName = "MyObjects/Projectile")]
public class ProjectileSO : ScriptableObject
{
    public GameObject Prefab;
    public float Speed;
    public float GravityAffection;

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
}

public enum TeamColor
{
    Red, Blue, Purple, Orange, Green
}
