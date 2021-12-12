using UnityEngine;
using UnityEngine.U2D;

public class Terrain : MonoBehaviour
{
    private SpriteShapeController spriteShape; //SpriteShape that defines the terrain
    private float distanceBetweenPoints; //distance between sprite shape points
    [SerializeField] private int numberOfPoints = 150; //number of points we want
    [SerializeField] private int scale = 1000; //scale of the terrain
    [SerializeField] private float maxSmoothValue = 2; //maximum smooth value
    [SerializeField] private float minSmoothValue = 0.1f; //minimum smooth value

    /// <summary>
    /// Start method where we procedurally initialize a random terrain defined by a sprite shape with a given scale and a number of points
    /// We define the distance needed between points and define the borders
    /// For each point needed we insert it with a random height using PerlinNoise and set a random smooth value for each side
    /// </summary>
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
