using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private bool isColliding = false; // Variable to check if collision is happening

    [SerializeField] private Sprite defaultSprite; // Default sprite
    [SerializeField] private Sprite highlightedSprite; // Highlighted sprite
    private SpriteRenderer sRenderer; // Sprite renderer variable
    [SerializeField] private GameObject actionsText; // Actions text

    [SerializeField] private GameObject inventoryFullText; // Inventory full text
    [SerializeField] private float displayTime = 1f; // Display time for the inventory full text
    private Inventory inventory; // Inventory object variable
    [SerializeField] private float waterMoveRate = 0.03f;


    // Start is called before the first frame update
    void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>(); // Sets variable to the attached sprite renderer component
        inventory = FindObjectOfType<Inventory>(); // Sets inventory variable to the object found in the scene
        inventoryFullText.SetActive(false); // Disables text
        actionsText.SetActive(false); // Disables text
    }

    // Update is called once per frame
    void Update()
    {   
        if (isColliding) // Checks if the player is still in the collider
        {
            if (Input.GetKey(KeyCode.Space)) // Checks if the space key is held down
            {
                CollectWater(); // Collects water
            }
        }
    }

    // Called when a 2D collider enters this game object's collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Checks if the collision was the player
        {
            sRenderer.sprite = highlightedSprite; // Changes to the highlighted sprite
            actionsText.SetActive(true); // Displays text showing availible actions
            isColliding = true;
        }
    }

    // Called when a 2D collider leaves this game object's collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Checks if the collision was the player
        {
            sRenderer.sprite = defaultSprite; // Changes to the regular sprite
            actionsText.SetActive(false); // Disables text showing availible actions
            isColliding = false;
        }
    }

    // This method adds water to the player's inventory when called.
    private void CollectWater()
    {
        if (inventory.waterAmount < inventory.maxWaterAmount) // Checks if there's space in the inventory
        {
            inventory.waterAmount += waterMoveRate * Time.deltaTime; // Adds water to the inventory based on the rate defined earlier
        }
        else // If there's no space left in the inventory
        {
            inventory.waterAmount = inventory.maxWaterAmount;
            StartCoroutine(DisplayInventoryFullText()); // Calls the function DisplayInventoryFullText
        }
    }



// This function displays the inventory full text for a short amount of time when called
private IEnumerator DisplayInventoryFullText()
{
    inventoryFullText.SetActive(true); // Displays text
    yield return new WaitForSeconds(displayTime); // Waits for display time
    inventoryFullText.SetActive(false); // Disables text
}
}

