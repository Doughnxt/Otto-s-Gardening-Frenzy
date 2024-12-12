using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class HarvestPlant : MonoBehaviour
{
    private bool isColliding = false; // Variable to check if collision is happening
    [SerializeField] private GameObject actionsText; // Actions text
    [SerializeField] private GameObject objectGlow; // Glow
    public bool harvested; // Boolean for if the plant has been harvested
    private Points gamePoints; // Points script
    [SerializeField] private float pointValue = 100; // The plant's point value
    private PlantPot pot; // Plant pot script

    // Start is called before the first frame update
    void Start()
    {
        harvested = false; // Sets harvested to false
        // Disables text and glow
        actionsText.SetActive(false);
        objectGlow.SetActive(false);
        gamePoints = FindObjectOfType<Points>(); // Sets variable to the object found in the scene
        pot = GetComponentInParent<PlantPot>(); // Sets variable to the parent object
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding) // Checks if player is colliding
        {
            if (!harvested) // Checks if the plant hasn't been harvested
            {
                // Enables the action text and glow
                actionsText.SetActive(true); 
                objectGlow.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Space)) // Checks if the space key is pressed
                {
                    gamePoints.points += pointValue; // Adds points to the point total
                    // Resets the plant pot by setting the following booleans to false
                    pot.growing = false;
                    pot.planted = false;
                    pot.canWater = false;
                    pot.isWatering = false;
                    pot.atWateringPosition = false;
                    // Sets harvesting to true disable harvesting
                    harvested = true;
                }
            }
            else
            {
                // Enables action text and glow
                actionsText.SetActive(false);
                objectGlow.SetActive(false);
            }
        }
    }

    // Called when a 2D collider enters this game object's collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Checks if the collision was the player
        {
            isColliding = true; // Player is colliding
        }
    }

    // Called when a 2D collider leaves this game object's collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Checks if the collision was the player
        {
            // Disables text and glow
            actionsText.SetActive(false);
            objectGlow.SetActive(false);
            isColliding = false; // Player isn't colliding

        }
    }
}
