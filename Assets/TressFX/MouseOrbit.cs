using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit")]
public class MouseOrbit : MonoBehaviour
{
    public Transform target ;
    public float distance = 10.0f;

    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    private float x = 0.0f;
    private float y = 0.0f;

    public float scaleSpeed = 500f;

    public Vector3 lookPivit = new Vector3(0,0,0);
    private Rigidbody m_RigidBody;

    void Start () {

        if (target == null)
        {
            target = new GameObject().transform;
            target.hideFlags = HideFlags.HideAndDontSave;
            target.position = Vector3.zero;
        }

        var angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // If camera has Rigidbody
        m_RigidBody = GetComponent<Rigidbody>();
        // Make the rigid body not change rotation
        if (m_RigidBody)
            m_RigidBody.freezeRotation = true;
        doOrbiting();
    }

    void Update () {
        if (Input.GetMouseButton(2) || Input.GetKey(KeyCode.O))
        {
            doOrbiting();
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            this.distance += scaleSpeed * Time.deltaTime;
            doOrbiting();
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            this.distance -= scaleSpeed * Time.deltaTime;
            doOrbiting();
        }
    }

    void doOrbiting()
    {
        if (target) {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            
            y = ClampAngle(y, yMinLimit, yMaxLimit);
                
            var rotation = Quaternion.Euler(y, x, 0);
            var position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position + lookPivit;
            
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    static float ClampAngle (float angle, float min, float max ) {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp (angle, min, max);
    }
}