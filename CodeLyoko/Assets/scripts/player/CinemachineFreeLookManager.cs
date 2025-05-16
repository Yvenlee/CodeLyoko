using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CinemachineFreeLookManager : MonoBehaviour
{
    private CinemachineFreeLook _freeLookCamera;

    [SerializeField] private float zoomSpeed = 1.0f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 15f;
    [SerializeField] private float mouseSensitivity = 2f;

    private bool isRotating = false;
    private bool isCursorVisible = false;

    private void Awake()
    {
        _freeLookCamera = GetComponent<CinemachineFreeLook>();
        _freeLookCamera.m_XAxis.m_InputAxisName = "";
        _freeLookCamera.m_YAxis.m_InputAxisName = "";
        ToggleCursorVisibility(isCursorVisible);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }

        // Rotation manuelle
        if (isRotating)
        {
            float mouseX = Input.GetAxisRaw("Mouse X");
            float mouseY = Input.GetAxisRaw("Mouse Y");

            _freeLookCamera.m_XAxis.m_InputAxisValue = mouseX * mouseSensitivity;
            _freeLookCamera.m_YAxis.m_InputAxisValue = -mouseY * mouseSensitivity;
        }
        else
        {
            _freeLookCamera.m_XAxis.m_InputAxisValue = 0f;
            _freeLookCamera.m_YAxis.m_InputAxisValue = 0f;
        }

        // Zoom
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            AdjustZoom(scrollInput);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isCursorVisible = !isCursorVisible;
            ToggleCursorVisibility(isCursorVisible);
        }
    }

    private void AdjustZoom(float scrollInput)
    {
        for (int i = 0; i < _freeLookCamera.m_Orbits.Length; i++)
        {
            _freeLookCamera.m_Orbits[i].m_Radius = Mathf.Clamp(
                _freeLookCamera.m_Orbits[i].m_Radius - scrollInput * zoomSpeed,
                minZoom,
                maxZoom
            );
        }
    }

    private void ToggleCursorVisibility(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
