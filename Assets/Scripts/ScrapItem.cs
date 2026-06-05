using UnityEngine;

public class ScrapItem : MonoBehaviour
{
    public int scrapValue = 1;

    void Start()
    {
        // Force the scale to be exactly 0.3 on the beach
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f); 
    }

    private void OnTriggerEnter(Collider other)
    {
        // The player's Rigidbody will trigger this smoothly!
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                bool pickUp = inventory.AddScrap(scrapValue);
                Debug.Log($"{pickUp}");
                if (pickUp)
                {
                    Destroy(gameObject); 
                } 
            }
        }
    }
}