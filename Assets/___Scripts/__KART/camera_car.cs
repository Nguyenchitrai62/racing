
using UnityEngine;

public class camera_car : MonoBehaviour
{
    public Transform car;
    public float distance = 7.0f;
    public float height = 3.0f;
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;
    public float tiltAngle = 7.0f; // Góc nghiêng của camera khi nhìn về phía xe
    public float speedDistanceMultiplier = 0.15f;


    void FixedUpdate()
    {
        float wantedRotationAngle = car.eulerAngles.y;
        float wantedHeight = car.position.y + height;

        // Xác định vị trí hiện tại của camera
        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        float speedBasedDistance = /*car_ctrl.speed_value **/ speedDistanceMultiplier;
        float finalDistance = distance + speedBasedDistance;

        transform.position = car.position;
        transform.position -= currentRotation * Vector3.forward * finalDistance;

        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        transform.LookAt(car);
        transform.rotation *= Quaternion.Euler(-tiltAngle, 0, 0);
    }


}
