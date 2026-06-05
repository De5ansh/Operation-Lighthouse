using UnityEngine;

public class ScrapItem : MonoBehaviour
{
    public int scrapValue = 1;

    void Start()
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                bool pickUp = inventory.AddScrap(scrapValue);
                if (pickUp)
                {
                    Destroy(gameObject); 
                } 
            }
        }
    }
}