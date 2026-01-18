using UnityEngine;
public class InputController : MonoBehaviour
{
    public enum InputPhase
    {
        Down,
        Hold,
        Up
    }

    public event System.Action<Collider2D, InputPhase> OnInput;
    [SerializeField] private Camera cam;

    private void Awake()
    {
        if (!cam) cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) FireEvent(InputPhase.Down, Input.mousePosition);
        if (Input.GetMouseButton(0)) FireEvent(InputPhase.Hold, Input.mousePosition);
        if (Input.GetMouseButtonUp(0)) FireEvent(InputPhase.Up, Input.mousePosition);
    }

    private void FireEvent(InputPhase phase, Vector2 screenPos)
    {
        Vector3 world = cam.ScreenToWorldPoint(screenPos);
        Vector2 world2 = new Vector2(world.x, world.y);
        RaycastHit2D hit = Physics2D.Raycast(world2, Vector2.zero);

        if (hit.collider == null) return;
        OnInput?.Invoke(hit.collider, phase);
    }
}
