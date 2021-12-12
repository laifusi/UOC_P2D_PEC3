using UnityEngine;
using UnityEngine.U2D;

public class Terrain : MonoBehaviour
{
    private SpriteShapeController spriteShape;
    private float distanceBetweenPoints;
    [SerializeField] private int numberOfPoints = 150;
    [SerializeField] private int scale = 1000;
    [SerializeField] private float maxSmoothValue = 2;
    [SerializeField] private float minSmoothValue = 0.1f;

    private void Start()
    {
        spriteShape = GetComponent<SpriteShapeController>();
        distanceBetweenPoints = (float)scale / numberOfPoints;
        spriteShape.spline.SetPosition(0, spriteShape.spline.GetPosition(0) - Vector3.right * scale/2);
        spriteShape.spline.SetPosition(1, spriteShape.spline.GetPosition(1) - Vector3.right * scale/2);
        spriteShape.spline.SetPosition(2, spriteShape.spline.GetPosition(2) + Vector3.right * scale/2);
        spriteShape.spline.SetPosition(3, spriteShape.spline.GetPosition(3) + Vector3.right * scale/2);
        for (int i = 1; i < numberOfPoints; i++)
        {
            spriteShape.spline.InsertPointAt(i + 1, new Vector3(spriteShape.spline.GetPosition(i).x + distanceBetweenPoints, 10*Mathf.PerlinNoise((i - 1) * Random.Range(5f, 15f), 0), 0));
            spriteShape.spline.SetTangentMode(i + 1, ShapeTangentMode.Continuous);
            spriteShape.spline.SetLeftTangent(i + 1, new Vector3(Random.Range(-maxSmoothValue, -minSmoothValue), 0, 0));
            spriteShape.spline.SetRightTangent(i + 1, new Vector3(Random.Range(minSmoothValue, maxSmoothValue), 0, 0));
        }
    }
}
