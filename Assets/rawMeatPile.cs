using UnityEngine;

public class rawMeatPile : MonoBehaviour
{
    public meatObjectPool meatObjectPool;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        GameObject meat = meatObjectPool.SharedInstance.GetPooledObject();
        if (meat != null)
        {
/*            meat.transform.position = meat.transform.position;
            meat.transform.rotation = meat.transform.rotation;*/
            meat.SetActive(true);
            meat.GetComponent<DragBehaviour>().shadowObject.SetActive(true);
        }
    }
}
