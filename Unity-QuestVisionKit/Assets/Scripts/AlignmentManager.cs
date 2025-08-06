using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System.Collections;
public class AlignmentManager : NetworkBehaviour
{
    public static AlignmentManager Instance { get; private set; }
    public OVRSpatialAnchor Anchor { get; private set; }



    [SerializeField]
    public GameObject anchorPrefab;

    // [SerializeField]
    // private Transform leftPlacementRoot, rightPlacementRoot;

    // [SerializeField]
    // public Transform offsetForQuest2Controller;

    [SerializeField]
    private Transform player;

    // [SerializeField]
    // private GameObject cubePrefab;

    // [SerializeField]
    // private Transform spawnPoint;

    [SerializeField]
    private Transform playerHands;

    // [SerializeField]
    // private TestingTiles tileManager;

    [SerializeField]
    public UnityEvent onAlign;

    [HideInInspector]
    public GameObject colocationAnchor;

    public bool automaticAlignment = true;

    private GameObject _currentAlignmentAnchor;
    private Coroutine _realignCoroutine;
    // bool doOnce = true;
    // public GameObject terrain;
    // public static Action<GameObject> SetTerrain;
    bool isSpawningAnchor = false;
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

            WaitForAnchorToLocalize(midpoint, rotation);
        }
    }

    private void AlignToAnchor()
    {

    }




    // public void PlaceAnchorAtRoot()
    // {
    //     // if (isSpawningAnchor) return;
    //     isSpawningAnchor = true;
    //     //Calculating centre to position at the centre of left and right controller
    //     Vector3 centre = (leftPlacementRoot.position + rightPlacementRoot.position) / 2;
    //     centre += rightPlacementRoot.transform.forward * offsetForQuest2Controller.transform.localPosition.z; //applying offset to fill the gap between the anchors of client and master client
    //     Vector3 rotationVector = (rightPlacementRoot.position - leftPlacementRoot.position);


    //     if (colocationAnchor)
    //     {
    //         colocationAnchor.GetComponent<OVRSpatialAnchor>().Erase((_, isSuccessful) =>
    //     {
    //         if (isSuccessful)
    //             Destroy(colocationAnchor.gameObject);

    //     });

    //     }

    //     //Aligning x-axis from right to left vector
    //     Quaternion rotation = Quaternion.FromToRotation(Vector3.right, rotationVector);
    //     rotation.eulerAngles = new Vector3(0f, rotation.eulerAngles.y, 0f);

    //     //Rotating anchor of client 180 degrees so that the rotation match with master client
    //     if (!isServer && NetworkClient.isConnected)
    //     {
    //         rotation.eulerAngles = new Vector3(0f, rotation.eulerAngles.y + 180f, 0f);
    //     }

    //     StartCoroutine(WaitForAnchorToLocalize(centre, rotation));

    // }

    // [Server]
    // private void SpawnCube()
    // {
    //     Debug.Log("Yeet");
    //     // if (!isServer) return;
    //     doOnce = false;
    //     // terrain = PhotonNetwork.Instantiate(cubePrefab.name, spawnPoint.position, spawnPoint.rotation);
    //     terrain = Instantiate(cubePrefab, spawnPoint.position, spawnPoint.rotation);
    //     NetworkServer.Spawn(terrain.gameObject);
    //     var mirrorGrabbable = terrain.GetComponent<MirrorThowableObject>();
    //     mirrorGrabbable.TransferOwnershipToLocalPlayer();
    //     SetTerrain?.Invoke(terrain);
    // }






    IEnumerator WaitForAnchorToLocalize(Vector3 centre, Quaternion rotation)
    {



        if (colocationAnchor)
        {
            colocationAnchor.GetComponent<OVRSpatialAnchor>().Erase((_, isSuccessful) =>
        {
            if (isSuccessful)
                Destroy(colocationAnchor.gameObject);

        });

        }


        while (colocationAnchor != null)
            yield return null;

        // yield return new WaitForSeconds(1f);
        colocationAnchor = Instantiate(anchorPrefab, centre, rotation);

        OVRSpatialAnchor spatialAnchor = colocationAnchor.GetComponent<OVRSpatialAnchor>();
        while (!spatialAnchor.PendingCreation)
            yield return null;


        //OnAlignButtonPressed();
        if (automaticAlignment)
        {
            yield return new WaitForSeconds(2f);
            OnAlignButtonPressed();
        }
    }

    public void OnAlignButtonPressed()
    {
        Debug.Log("OnAlignButtonPressed: aligning to anchor");

        Debug.Log(colocationAnchor);

        // if (colocationAnchor)
        //     tileManager.gameObject.transform.parent = colocationAnchor.transform;

        SetAlignmentAnchor(colocationAnchor);
    }

    public void SetAlignmentAnchor(GameObject anchor)
    {
        if (_realignCoroutine != null)
        {
            StopCoroutine(_realignCoroutine);
        }

        if (anchor)
            _realignCoroutine = StartCoroutine(RealignRoutine(anchor));
    }

    private IEnumerator RealignRoutine(GameObject anchor)
    {
        yield return new WaitForSeconds(1f);

        if (_currentAlignmentAnchor != null)
        {
            player.position = Vector3.zero;
            player.eulerAngles = Vector3.zero;

            yield return null;
        }

        var anchorTransform = anchor.transform;

        if (player)
        {
            player.position = anchorTransform.InverseTransformPoint(Vector3.zero);
            player.eulerAngles = new Vector3(0, -anchorTransform.eulerAngles.y, 0);
        }

        if (playerHands)
        {
            playerHands.localPosition = -player.position;
            playerHands.localEulerAngles = -player.eulerAngles;
        }

        _currentAlignmentAnchor = anchor;

        Debug.Log("RealignRoutine: finished alignment!");

        isSpawningAnchor = false;
        onAlign?.Invoke();
    }
}
