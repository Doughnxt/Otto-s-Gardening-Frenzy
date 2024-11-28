using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeds : MonoBehaviour
{
    private enum seedType { sunflower, tomato, carrot, lettuce } // Seed types
    [SerializeField] seedType seed; // Variable for seed type

    private bool isColliding = false; // Variable to check if collision is happening

    [SerializeField] private Sprite defaultSprite; // Default sprite
    [SerializeField] private Sprite highlightedSprite; // Highlighted sprite
    private SpriteRenderer sRenderer; // Sprite renderer variable
    [SerializeField] private GameObject actionsText; // Actions text

    [SerializeField] private GameObject inventoryFullText; // Inventory full text
    [SerializeField] private float displayTime = 1f; // Display time for the inventory full text
    private Inventory inventory; // Inventory object variable


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
            if (Input.GetKeyDown(KeyCode.Space)) // Checks if the space key is pressed
            {
                MoveSeeds(true); // Collects seeds
            }

            else if (Input.GetKeyDown(KeyCode.LeftShift)) // Checks if the shift key is pressed
            {
                MoveSeeds(false); // Removes seeds
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

    // This method moves seeds either into or out of your inventory, using the boolean taking as a parameter to determine if the player is
    // either taking or removing seeds.
    private void MoveSeeds(bool taking)
    {
        if (taking) // Checks if taking is true
        {
            if (inventory.items.Count < inventory.maxSeedCarryAmount) // Checks if there's space in the inventory
            {
                inventory.items.Add(seed.ToString()); // Adds the seed to the inventory
            }
            else // If there's no space left in the inventory
            {
                StartCoroutine(DisplayInventoryFullText()); // Calls the function DisplayInventoryFullText
            }
        }

        else // If taking is false
        {
            inventory.items.Remove(seed.ToString()); // Removes a seed from the inventory
        }
    }

    // This function displays the inventory full text for a shot amount of time when called
    private IEnumerator DisplayInventoryFullText()
    {
        inventoryFullText.SetActive(true); // Displays text
        yield return new WaitForSeconds(displayTime); // Waits for display time
        inventoryFullText.SetActive(false); // Disables text
    }
}
