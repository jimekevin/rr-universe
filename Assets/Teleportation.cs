using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public Transform player;
    public Transform receiver;

    private bool playerIsOverlapping = false;

    // Update is called once per frame
    void Update()
    {
        if (playerIsOverlapping)
        {
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            // Player moved accross the portal, let's teleport him
            if (dotProduct < 0f)
            {
                float rotationDiff = Quaternion.Angle(transform.rotation, receiver.rotation);
                rotationDiff += 180;
                player.Rotate(Vector3.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                player.position = receiver.position + positionOffset;

                playerIsOverlapping = false;
            }
        }
        Debug.Log(playerIsOverlapping);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision enter");
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collision stay");
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Collision exit");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter");
        if (other.tag == "Player")
        {
            playerIsOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger EXIT");
        if (other.tag == "Player")
        {
            playerIsOverlapping = false;
        }
    }
}
