using UnityEngine;

public class CookBehaviour : MonoBehaviour
{
    public SpriteRenderer cookedSprite;
    public SpriteRenderer burntSprite;

    public AudioSource sizzleSound;

    public GameObject sizzleObject;

    /// <summary>
    /// Is set by another script
    /// </summary>
    public bool cooking;

    private float doneness;
    private readonly float idealdoneness = 350;
    private readonly float maxdoneness = 700;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupMeat();
    }

    void SetupMeat()
    {
        SetDoneness(0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (cooking)
        {
            SetDoneness(doneness + 1);
        }
    }

    public void SetCooking(bool value)
    {
        cooking = value;
        sizzleSound.volume = (value ? 1 : 0);
        sizzleObject.SetActive(value);
    }

    private void SetDoneness(float value)
    {
        doneness = value;
        cookedSprite.color = new Color(1, 1, 1, Mathf.Min(doneness / idealdoneness, 1));
        burntSprite.color = new Color(1, 1, 1, Mathf.Min((doneness - idealdoneness) / maxdoneness, 1));
    }
}
