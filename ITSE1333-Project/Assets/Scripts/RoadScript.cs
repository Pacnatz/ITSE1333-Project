using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class RoadScript : MonoBehaviour
{
    [SerializeField]
    private GameObject roadPrefab;
    [SerializeField]
    private SplineContainer splineObject;

    [SerializeField]
    private int splineIndex;
    [SerializeField]
    [Range(0f, 1f)]
    private float time;
    [SerializeField]
    private float resolution;

    [SerializeField]
    private float oilSpawnChance;
    [SerializeField]
    private GameObject oilSpillPrefab;

    private void Start()
    {

        for (int i = 0; i < resolution; i++)
        {
            float progress = i / resolution;
            // Get the position on the spline
            Vector3 position = splineObject.EvaluatePosition(progress);

            // Get the tangent (direction) on the spline
            Vector3 tangent = splineObject.EvaluateTangent(progress);

            Instantiate(roadPrefab, position, Quaternion.LookRotation(tangent), transform);

            float oilChance = Random.Range(0, 100);
            if (oilChance <= oilSpawnChance)
            {
                GameObject spawnedOil = Instantiate(oilSpillPrefab, position, Quaternion.LookRotation(tangent), transform);
                spawnedOil.transform.localPosition += new Vector3(Random.Range(-5f, 5f), 0, 0);
            }
        }
        /*
        _progress += speed * Time.deltaTime;
        _progress = Mathf.Clamp01(_progress); // Ensure progress stays between 0 and 1

        // Get the position on the spline
        Vector3 position = spline.EvaluatePosition(_progress);

        // Get the tangent (direction) on the spline
        Vector3 tangent = spline.EvaluateTangent(_progress);

        // Set the object's position
        transform.position = position;

        // Rotate the object to face the tangent
        transform.rotation = Quaternion.LookRotation(tangent);
        */
    }

}
