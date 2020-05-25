using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Rigidbody playerRigidBody;
    public float jumpForce;
    private int points;
    private bool isOnGround;


    // For spawning a platform
    public Vector3 positionRelativeToPlatform;
    public GameObject currentPlatform;
    public List<GameObject> platformList;
    public bool newPlatformInstantiated;
    public GameObject pointToken;

    void SpawnPlatform(int platformIndex, Vector3 platformPosition) {
        GameObject newPlatform = Instantiate(platformList[platformIndex], platformPosition, new Quaternion(0, 0, 0, 0));
        newPlatform.tag = "PlatformNew";
        newPlatformInstantiated = true;
        Vector3 tokenPosition = newPlatform.transform.position + new Vector3(0, 1, 2);
        Instantiate(pointToken, tokenPosition, new Quaternion(0, 0, 0, 0));
    }

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        points = 0;
        isOnGround = true;
        positionRelativeToPlatform = new Vector3(0, 0, 0);
        newPlatformInstantiated = false;
    }

    void Update() {
        positionRelativeToPlatform = currentPlatform.transform.InverseTransformPoint(transform.position);
        if (positionRelativeToPlatform.z > 0 && !newPlatformInstantiated) {
            int platformIndex = (int)Mathf.Round(Random.Range(-0.5f, 3.49f));
            Bounds newPlatformBounds = platformList[platformIndex].GetComponent<MeshRenderer>().bounds;
            Bounds currentPlatformBounds = currentPlatform.GetComponent<MeshRenderer>().bounds;
            SpawnPlatform(platformIndex, transform.position + new Vector3(0, 0, newPlatformBounds.extents.z * 2 + currentPlatformBounds.extents.z));
        }
    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float moveJump = 0.0f;

        if(Input.GetKey(KeyCode.Space) && isOnGround) {
            moveJump = 20 * jumpForce;
            isOnGround = false;
        }

        Vector3 movement = new Vector3(moveHorizontal, moveJump, moveVertical);
        playerRigidBody.AddForce(movement * speed);
    }

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag.StartsWith("Platform")) {
            if(other.gameObject.tag != currentPlatform.tag) {
                currentPlatform = other.gameObject;
                currentPlatform.tag = "PlatformCurrent";
                newPlatformInstantiated = false;
            }
            isOnGround = true;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Piece")){
            ++ points;
            GameObject particles = other.gameObject.GetComponent<TokenController>().particles;
            GameObject newParticles = Instantiate(particles, other.gameObject.transform.position, other.gameObject.transform.rotation);
            Destroy(other.gameObject);
            SendMessageUpwards("cleanUpObject", newParticles);
        }
    }
}
