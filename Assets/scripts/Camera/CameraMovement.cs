using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    private Player player;
    public float smooth = 1.5f;
    private Transform playerTransform;
    private Vector3 relCameraPos;
    private float relCameraPosMag;
    private Vector3 newPos;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerTransform = player.transform;
        relCameraPos = transform.position - playerTransform.position;
        relCameraPosMag = relCameraPos.magnitude - 0.5f;

    }

    void FixedUpdate()
    {
        Vector3 standardPos = playerTransform.position + relCameraPos;
        Vector3 abovePos = playerTransform.position + Vector3.up * relCameraPosMag;
        Vector3[] checkPoints = new Vector3[5];
        checkPoints[0] = standardPos;
        checkPoints[1] = Vector3.Lerp(standardPos, abovePos, 0.25f);
        checkPoints[2] = Vector3.Lerp(standardPos, abovePos, 0.5f);
        checkPoints[3] = Vector3.Lerp(standardPos, abovePos, 0.75f);
        checkPoints[4] = abovePos;

        for (int i = 0; i < checkPoints.Length; i++)
        {
            if (ViewingPosCheck(checkPoints[i]))
            {
                break;
            }
        }

        transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.deltaTime);
        SmoothLookAt();
    }

    bool ViewingPosCheck(Vector3 checkPos)
    {
        RaycastHit hit;
        if (Physics.Raycast(checkPos, playerTransform.position - checkPos, out hit, relCameraPosMag))
        {
            if (hit.transform != playerTransform)
            {
                return false;
            }
        }
        newPos = checkPos;
        return true;
    }

    void SmoothLookAt()
    {
        // Create a vector from the camera towards the player.
        Vector3 relPlayerPosition = playerTransform.position - transform.position;

        // Create a rotation based on the relative position of the player being the forward vector.
        Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);

        // Lerp the camera's rotation between it's current rotation and the rotation that looks at the player.
        transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, smooth * Time.deltaTime);
    }
}
