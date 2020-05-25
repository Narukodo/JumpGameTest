using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        // Instantiate(prefabList[0], player.transform.position + new Vector3(0, 0, 30), new Quaternion(0, 0, 0, 0));
    }

    void cleanUpObject(GameObject gameObject) {
        Destroy(gameObject, 3);
    }
}
