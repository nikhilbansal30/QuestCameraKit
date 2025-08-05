using System.Collections.Generic;
using UnityEngine;

public class AlignmentManager : MonoBehaviour
{
    public static AlignmentManager Instance { get; private set; }
    public OVRSpatialAnchor Anchor { get; private set; }

    [SerializeField]
    private GameObject _anchorPrefab;

    private List<Vector3> _QRPointPositions = new List<Vector3>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void AddQRPoint(Vector3 position)
    {
        _QRPointPositions.Add(position);

        if (_QRPointPositions.Count == 2) // we have enough points to spawn Anchor
        {
            var pointA = _QRPointPositions[0];
            var pointB = _QRPointPositions[1];

            Vector3 midpoint = (pointA + pointB) / 2f;

            Vector3 direction = (pointA - pointB).normalized;

            Quaternion rotation = Quaternion.LookRotation(direction);

            //Spawn anchor here
        }
    }

    private void AlignToAnchor()
    {
        
    }
}
