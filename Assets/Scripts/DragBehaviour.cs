using UnityEngine;
using System.Collections;

public class DragBehaviour : MonoBehaviour
{
    private bool dragging = false;
    private Vector3 offset;
    private readonly float liftOffsetY = .4f;
    private readonly float shadowOffsetY = .5f;

    private BoxCollider2D collider;
    private Rigidbody2D rigidbody;

    public BoxCollider2D trashCollider;
    public CircleCollider2D plateCollider;

    public GameObject shadowObject;

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

        shadowObject.transform.position = new Vector3(rigidbody.transform.position.x, (transform.position.y - shadowOffsetY) + (dragging ? -liftOffsetY : 0), shadowObject.transform.position.z);
    }

    private void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = new Vector2(offset.x, offset.y + liftOffsetY);
        shadowObject.transform.position.Set(0, 5, 0);
        SetDragging(true);
    }

    private void OnMouseUp()
    {
        SetDragging(false);
        rigidbody.position = new Vector2(transform.position.x, transform.position.y - liftOffsetY);

        if (collider.IsTouching(trashCollider))
        {
            resetMeat();
        }
        else if (collider.IsTouching(plateCollider))
        {
            resetMeat();
        }
    }

    private void resetMeat()
    {
        rigidbody.MovePosition(new Vector2(-7.13f, 0));
        StartCoroutine(meatReset());
    }

    private void SetDragging(bool value)
    {
        dragging = value;
        cookBehaviour.SetCooking(!value);
    }

    private void LateUpdate()
    {

    }

    IEnumerator meatReset()
    {
        yield return new WaitForFixedUpdate();
        gameObject.SetActive(false);
        cookBehaviour.SetCooking(false);
        cookBehaviour.SetDoneness(0);
        shadowObject.SetActive(false);
    }
    
}
