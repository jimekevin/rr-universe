using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public CharacterController controller;
    public float[] zoomDecelerationFactors;
    public GameObject[] zoomOuterTriggers;
    public GameObject[] zoomInnerTriggers;

    // Start is called before the first frame update
    void Start()
    {
        var allGamepads = Gamepad.all;
        var gamepad = Gamepad.current;
        //Camera.main.enabled = false;
        Camera.main.enabled = true;
    }

    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            return; // No gamepad connected.
        }

        float velocity = moveSpeed;

        for (int i = 0; i < zoomInnerTriggers.Length; ++i)
        {
            var zoomInnerCollider = zoomInnerTriggers[i].GetComponent<SphereCollider>();
            var zoomOuterCollider = zoomOuterTriggers[i].GetComponent<SphereCollider>();
            // not completely correct but better than nothing
            var minDistance = zoomInnerCollider.radius * zoomInnerCollider.transform.lossyScale.x;
            var maxDistance = zoomOuterCollider.radius * zoomOuterCollider.transform.lossyScale.x;
            var distance = Vector3.Distance(zoomOuterCollider.bounds.center, Camera.main.transform.position);
            //var playerCollider = controller.GetComponent<Collider>();
            //var colliding = zoomCollider.bounds.Intersects(playerCollider.bounds);
            var isInside = zoomOuterCollider.bounds.Contains(Camera.main.transform.position);
            if (isInside && distance <= maxDistance)
            {
                //Debug.Log(maxDistance);
                //Debug.Log(distance / maxDistance);
                //Debug.Log(zoomCollider.bounds.extents.magnitude);
                //Debug.Log(zoomCollider.transform.lossyScale * zoomCollider.radius);
                var factor = Mathf.Pow(distance / maxDistance, 2);
                var minFactor = Mathf.Pow(minDistance / maxDistance, 2);
                factor = Mathf.Clamp(factor, minFactor, 1.0f) * zoomDecelerationFactors[i];
                velocity *= factor;

                // scale: 1 -> 0.05
                //world.transform.localScale = initialWorld.transform.localScale * factor;
                //break;
            }
        }
        
        // Adjust camera accordingly
        var rs = gamepad.rightStick.ReadValue();
        var rotateValue = new Vector3(rs.y, rs.x * -1, 0);
        Camera.main.transform.eulerAngles -= rotateValue;

        // Move the player in camera direction
        var ls = gamepad.leftStick.ReadValue();
        controller.transform.position += Camera.main.transform.forward * ls.y * velocity * Time.deltaTime + Camera.main.transform.right * ls.x * velocity * Time.deltaTime;
    }
}
