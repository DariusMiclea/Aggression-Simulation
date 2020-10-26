using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject creature1, creature2;
    public GameManager gameManager;
    GameObject gameManagerObject;
    
    void Start()
    {
        gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        if(creature1 == creature2)
        {
            creature2 = null;
        }
        
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Friendly" || collision.gameObject.tag == "Aggressive")
        {
            int creatureTarget = collision.gameObject.GetComponent<Creature>().foodIndex;
            if(gameObject == gameManager.foodPos[creatureTarget])
            {
                if (creature1 == null)
                {
                    creature1 = collision.gameObject;
                    creature1.GetComponent<Creature>().colidedWithFood = true;
                }
                else if (creature2 == null)
                {
                    creature2 = collision.gameObject;
                    creature2.GetComponent<Creature>().colidedWithFood = true;
                }
                else
                {
                    collision.gameObject.GetComponent<Creature>().foodIndex = Random.Range(0, 25);
                }
            }
            
        }
        
    }
    
    void Interaction()
    {
        //creature1.GetComponent<Creature>().hunger = 100;
        if(creature1.tag == "Friendly" && creature2.tag == "Friendly")
        {
            creature1.GetComponent<Creature>().hunger = 50;
            creature2.GetComponent<Creature>().hunger = 50;
        }
        else if (creature1.tag == "Friendly" && creature2.tag == "Aggressive")
        {
            creature1.GetComponent<Creature>().hunger = 1;
            creature2.GetComponent<Creature>().hunger = 99;
        }
        else if(creature1.tag == "Aggressive" && creature2.tag == "Aggressive)")
        {
            creature1.GetComponent<Creature>().hunger = 30;
            creature2.GetComponent<Creature>().hunger = 30;
        }
    }
    public void CheckInteraction()
    {
        if(creature1 && !creature2)
        {
            creature1.GetComponent<Creature>().hunger = 100;
        }
        else if(creature2 && !creature1)
        {
            creature2.GetComponent<Creature>().hunger = 100;
        }
        else if(creature1 && creature2)
        {
            Interaction();
        }
    }
    
}
