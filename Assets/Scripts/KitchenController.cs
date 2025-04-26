using System.Collections;
using UnityEngine;

public class KitchenController : MonoBehaviour
{

    public class Order
    {
        public float doneness;
        public float timeWillingToWait;

        public Order(float doneness, float timeWillingToWait)
        {
            this.doneness = doneness;
            this.timeWillingToWait = timeWillingToWait;
        }

        public override string ToString()
        {
            return "Doneness: " + doneness + " | Time Willing To Wait: " + timeWillingToWait;
        }
    }

    public delegate void OrderAdded(Order order);
    public static event OrderAdded OnOrderAdded;

    public delegate void OrderRemoved(Order order);
    public static event OrderRemoved OnOrderRemoved;

    public delegate void OrderWaitTimeExceeded(Order order);
    public static event OrderWaitTimeExceeded OnOrderWaitTimeExceeded;

    private Order currentOrder = null;

    public float minimumOrderTime = 2.5f;
    public float maximumOrderTime = 5f;

    public float minimumWaitTime = 20f;
    public float maximumWaitTime = 40f;

    public float innerTimer = 0;


    private void Start()
    {
        StartCoroutine(OrderUpdater());
        StartCoroutine(OrderWaitTimer());

        CookBehaviour.OnFoodBurned += CookBehaviour_OnFoodBurned;
        DragBehaviour.OnFullfillOrder += DragBehaviour_OnFullfillOrder;
    }

    private bool DragBehaviour_OnFullfillOrder(float doneness)
    {
        bool inRange = currentOrder.doneness - 50 < doneness && currentOrder.doneness + 50 > doneness;//Is it within range?
        if (inRange)
        {
            if (OnOrderRemoved != null) OnOrderRemoved.Invoke(currentOrder);
            currentOrder = null;
        }
        return inRange; 
    }

    private void CookBehaviour_OnFoodBurned()
    {
        //ToDo: Idk
    }

    IEnumerator OrderWaitTimer()
    {
        while(true)
        {
            innerTimer = 0;
            yield return new WaitUntil(() => currentOrder != null);
            Order lastTrackedOrder = currentOrder;
            Debug.Log(lastTrackedOrder.ToString());
            while (currentOrder != null && innerTimer < currentOrder.timeWillingToWait)
            {
                innerTimer += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }

            if (innerTimer >= lastTrackedOrder.timeWillingToWait)
            {
                if (OnOrderWaitTimeExceeded != null) OnOrderWaitTimeExceeded.Invoke(lastTrackedOrder);
            }
        }
    }

    IEnumerator OrderUpdater()
    {
        while(true)
        {
            yield return new WaitUntil(() => currentOrder == null);
            float waitTime = Random.Range(minimumOrderTime, maximumOrderTime);
            yield return new WaitForSeconds(waitTime);
            Order order = new Order(Random.Range(300, 500), Random.Range(minimumWaitTime, maximumWaitTime));
            currentOrder = order;
            if (OnOrderAdded != null) OnOrderAdded.Invoke(currentOrder);
        }
    }


}
