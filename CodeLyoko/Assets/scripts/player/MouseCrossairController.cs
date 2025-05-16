using UnityEngine;

public class MouseCrosshairController : MonoBehaviour
{
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private float sensitivity = 1f;

    private Vector2 currentPosition;

    void Start()
    {
        currentPosition = Vector2.zero;
        if (crosshair != null)
            crosshair.anchoredPosition = currentPosition;
    }

    void Update()
    {
        if (crosshair == null || canvasRect == null) return;

        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * sensitivity;
        currentPosition += mouseDelta;

        Vector2 canvasSize = canvasRect.sizeDelta;
        Vector2 halfCanvas = canvasSize / 2f;
        Vector2 minPos = -halfCanvas;
        Vector2 maxPos = halfCanvas;

        currentPosition.x = Mathf.Clamp(currentPosition.x, minPos.x, maxPos.x);
        currentPosition.y = Mathf.Clamp(currentPosition.y, minPos.y, maxPos.y);

        crosshair.anchoredPosition = currentPosition;
    }

    public void ResetCrosshair()
    {
        currentPosition = Vector2.zero;
        if (crosshair != null)
            crosshair.anchoredPosition = currentPosition;
    }
}
