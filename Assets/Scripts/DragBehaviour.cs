using UnityEngine;

public class DragBehaviour : MonoBehaviour
{
    private bool dragging = false;
    private Vector3 offset;
    private readonly float liftOffsetY = .4f;

    private BoxCollider2D collider;
    private Rigidbody2D rigidbody;

    public BoxCollider2D trashCollider;
    public BoxCollider2D plateCollider;

    private CookBehaviour cookBehaviour;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        cookBehaviour = GetComponent<CookBehaviour>();
    }

    private void Update()
    {
        if (dragging)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rigidbody.MovePosition(new Vector2 (position.x + offset.x, position.y + offset.y));
        }
    }

    private void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = new Vector2(offset.x, offset.y + liftOffsetY);
        SetDragging(true);
    }

    private void OnMouseUp()
    {
        SetDragging(false);
        rigidbody.MovePosition(new Vector2(transform.position.x, transform.position.y - liftOffsetY));

    }

    private void SetDragging(bool value)
    {
        dragging = value;
        cookBehaviour.SetCooking(!value);
    }

    private void LateUpdate()
    {
        if (collider.IsTouching(trashCollider))
        {
            
        }
    }
}
