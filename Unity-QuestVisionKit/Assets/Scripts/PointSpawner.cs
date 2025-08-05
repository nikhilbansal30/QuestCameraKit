using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _pointPrefab;

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            var marker = MarkerPool.Instance.GetActiveMarker();
            if (marker == null) return;
            Instantiate(_pointPrefab, marker.transform.position, marker.transform.rotation);
            AlignmentManager.Instance.AddQRPoint(marker.transform.position);
        }
    }
}
