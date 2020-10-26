using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    // Start is called before the first frame update
    public int hunger = 0;
    public GameManager gameManager;
    GameObject gameManagerObject;
    public float speed = 0.5f;
    bool init = true;
    public bool colidedWithFood = false;
    GameObject food;
    Vector3 startPosition;
    float distance, timePassed = 0;
    public int foodIndex;
    
    bool retry = false;


    void Start()
    {
        gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
        startPosition = transform.position;
        speed = 1.5f;
        foodIndex = Random.Range(0,25);
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
       timePassed += Time.deltaTime;
       if(timePassed != 0 && init == true)//inceputul primei zile
        {
            init = false;
            DayStart();
        }
       else if(timePassed >= 10.0f)
        {
            foodIndex = Random.Range(0, 25);
            DayEnd();
            timePassed = 0.0f;
        }
       else if(timePassed >= 7.0f && colidedWithFood == false && retry == false)
        {
            foodIndex = Random.Range(0, 25);
            retry = true;
            
        }
        FindFood(foodIndex);
    }
    public void FindFood(int foodIndex)
    {
        if(colidedWithFood == false)
        {
            food = gameManager.foodPos[foodIndex];
            Vector3 destination = food.transform.position + new Vector3(0f, 0f, 0f);
            distance = Vector3.Distance(transform.position, food.transform.position);
            transform.position = Vector3.Lerp(transform.position, 
                destination, (Time.deltaTime * speed)/distance);
            Vector3 direction = new Vector3(food.transform.position.x, transform.position.y, food.transform.position.z) - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;
            
        }
        
       
    }
    void Reproduce()
    {
        if (gameObject.tag == "Friendly")
        {
            int i;

            //prioretizam prima jumatate a vectorului daca creatura e pasnica
            for (i = 0; i < 25; i++)
            {
                if (gameManager.spawnPoints[i].creature == null)
                {
                    gameManager.spawnPoints[i].creature = gameObject;
                    Instantiate(gameManager.spawnPoints[i].creature,
                            gameManager.spawnPoints[i].position, Quaternion.identity);
                    gameManager.frndCreatureNum++;
                    break;
                }
            }
                if (i == 25)
                {
                    for (i = 25; i < 50; i++)
                    {
                        if (gameManager.spawnPoints[i].creature == null)
                        {
                            gameManager.spawnPoints[i].creature = gameObject;
                            Instantiate(gameManager.spawnPoints[i].creature,
                                gameManager.spawnPoints[i].position, Quaternion.identity);
                            gameManager.frndCreatureNum++;
                            break;
                        }
                    }
                }
            
        }
            if (gameObject.tag == "Aggressive")
            {
                int i;
                //prioretizam a 2-a jumatate a vectorului daca creatura e agresiva
                for (i = 25; i < 50; i++)
                {
                    if (gameManager.spawnPoints[i].creature == null)
                    {
                        gameManager.spawnPoints[i].creature = gameObject;
                        Instantiate(gameManager.spawnPoints[i].creature,
                            gameManager.spawnPoints[i].position, Quaternion.identity);
                        gameManager.agrCreatureNum++;
                        break;
                    }
                }
                    if (i == 50)
                    {
                        for (i = 0; i < 25; i++)
                        {
                            if (gameManager.spawnPoints[i].creature == null)
                            {
                                gameManager.spawnPoints[i].creature = gameObject;
                                Instantiate(gameManager.spawnPoints[i].creature,
                                    gameManager.spawnPoints[i].position, Quaternion.identity);
                                gameManager.agrCreatureNum++;
                                break;
                            }
                        }
                    }
                
            }


        
    }
    void DestroyMe()
    {
        for (int i = 0; i < 50; i++)
        {
            if (gameObject == gameManager.spawnPoints[i].creature)
            {
                gameManager.spawnPoints[i].creature = null;
                
                break;
            }
        }
        if (gameObject.tag == "Aggressive")
        {
            gameManager.agrCreatureNum--;
        }
        else if (gameObject.tag == "Friendly")
        {
            gameManager.frndCreatureNum--;
        }
        Destroy(gameObject);

    }
   
    public void DayStart()
    {
        food = gameManager.foodPos[foodIndex];
        colidedWithFood = false;
        transform.position = startPosition;
        hunger = 0;
    }
    public void DayEnd()
    {
        if(hunger == 0)
        {
            food.GetComponent<Food>().CheckInteraction();
        }
        if(hunger == 0)
        {
            DestroyMe();
        }
        else if(hunger == 100)
        {
            Reproduce();
        }
        else if(hunger == 75)
        {
            if (Random.value <= 0.5f)
            {
                Reproduce();
            }
        }
        else if(hunger == 25)
        {
            if(Random.value <= 0.5f)
            {
                DestroyMe();
            }
        }
        DayStart();
    }
}
