using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    bool interact = false;
    [SerializeField] Transform shotPoint;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(shotPoint.position, transform.forward, out hit, 2f))
        {
            if(hit.transform.gameObject.layer == 8)
            {
                Debug.Log("Touché");
                if (Input.GetKeyDown(KeyCode.I))
                {
                    Transform target = hit.transform;
                    StartCoroutine(LookToward((new Vector3(target.position.x, transform.position.y, target.position.z)+target.forward*-1), target.rotation));
                }
            }
        }
    }

    IEnumerator LookToward(Vector3 pos, Quaternion rot)
    {
        while((transform.position != pos) || (transform.rotation != rot))
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, 0.01f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 0.7f);
            yield return null;
        }
    }
}
