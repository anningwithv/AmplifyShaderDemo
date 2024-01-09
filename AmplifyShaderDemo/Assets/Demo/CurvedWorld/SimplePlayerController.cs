using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
  #region Inspector

  [SerializeField]
  private Rigidbody physics = default;

  [SerializeField]
  private float speed = default;
    [SerializeField]
    private float rotateSpeed = default;
   #endregion


    #region Fields

    private float _force;

  private float _direction;

    #endregion


    #region MonoBehaviour

    private void Awake()
    {
        Camera.main.cullingMatrix = Matrix4x4.Ortho(-100, 100, -100, 100, 0.001f, 100) *
                        Camera.main.worldToCameraMatrix;
    }

    private void Update ()
  {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * speed * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.D))
        {
            transform.localEulerAngles += new Vector3(0, rotateSpeed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.localEulerAngles += new Vector3(0, -rotateSpeed * Time.deltaTime, 0);
        }
    }

  private void FixedUpdate ()
  {

  }

  private void OnTriggerEnter (Collider other)
  {

  }

  private void OnTriggerExit (Collider other)
  {

  }

  #endregion
}