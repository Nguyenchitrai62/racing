using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class ctrl_virtual_camera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float shiftTime = 3.0f; // Thời gian dịch chuyển camera
    public Vector3 startPosition = new Vector3(0, 0, -10); // Vị trí ban đầu phía sau bullet
    public Vector3 endPosition = new Vector3(-20, 10, -10); // Vị trí cuối cùng bên trái bullet

    private void OnEnable()
    {
        ShiftCamera();
    }

    void ShiftCamera()
    {
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_FollowOffset = startPosition;

        // Sử dụng DOTween để di chuyển
        DOTween.To(() => transposer.m_FollowOffset,
                   x => transposer.m_FollowOffset = x,
                   endPosition,
                   shiftTime);
    }
}
