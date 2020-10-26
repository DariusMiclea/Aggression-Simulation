using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject food, AgrCreature, FrndCreature;
    float foodX, foodZ;
    public GameObject spawnPoint;
    public GameObject[] foodPos = new GameObject[25];
    public int agrCreatureNum = 0, frndCreatureNum = 0;
    public Text daysText, agNum, frNum, speed;
    float timePassed = 0;
    int dayCount = 0, currentSpeed = 1;
    public struct SpawnPoint
    {
        public Vector3 position;
        public GameObject creature;
    }
    public SpawnPoint[] spawnPoints = new SpawnPoint[50];
    // Start is called before the first frame update
    void Start()
    {
        foodSpawner();
        generateSpawnPoints();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timePassed += Time.deltaTime;
        if(timePassed >= 10.0f)
        {
            foodDestroyer();
            foodSpawner();
            
            timePassed = 0.0f;
            dayCount++;
            Debug.Log(dayCount + 1);

        }
        UiUpdate();
        
    }
    void foodSpawner()
    {
        
        for(int i = 0; i < 25; i++)
        {
            foodX = Random.Range(-3f, 3f);
            foodZ = Random.Range(-3f, 3f);
            

            foodPos[i] = Instantiate(food, new Vector3(foodX, food.transform.position.y, foodZ), Quaternion.identity);
          
        }
        
    }
    void foodDestroyer()
    {
        for (int i = 0; i < 25; i++)
        {
            Destroy(foodPos[i]);

        }
    }
    void generateSpawnPoints()
    {
        int i = 1;
        float x = 0;
        float z = -6.0f;
        //the first half of the array belongs to the friendly creatures
        spawnPoints[0].position = new Vector3(x, FrndCreature.transform.position.y, z + 0.3f);
        spawnPoints[0].creature = FrndCreature;
        frndCreatureNum++;

        //the 2nd half of the array belongs to the aggressive creatures
        spawnPoints[25].position = new Vector3(x, AgrCreature.transform.position.y, -z - 0.3f);
        spawnPoints[25].creature = AgrCreature;
        agrCreatureNum++;

        while (i < 25)
        {
            x += 0.44f;
            z += 0.01f + ( ((i+25)/2) * ((i+25)/2) ) / 950.0f;
            spawnPoints[i].position = new Vector3(x, AgrCreature.transform.position.y, z);
            spawnPoints[i+1].position = new Vector3(-x, AgrCreature.transform.position.y, z);
            i += 2; 
        }
        x = 0;
        z = 6.0f;
        i = 26;
        while(i < 50)
        {
            x += 0.44f;
            z -= 0.01f + (i/2 * i/2) / 950.0f;
            spawnPoints[i].position = new Vector3(x, AgrCreature.transform.position.y, z);
            spawnPoints[i + 1].position = new Vector3(-x, AgrCreature.transform.position.y, z);
            i += 2;
        }
    }
    void DayStart()
    {
        foodSpawner();
    }
    
    void UiUpdate()
    {
        daysText.text = "DAY: " + (dayCount+1).ToString();
        frNum.text = ": " + frndCreatureNum.ToString();
        agNum.text = ": " + agrCreatureNum.ToString();
        speed.text = "SPEED: x" + currentSpeed.ToString();
    }
    public void SpeedUp()
    {
        currentSpeed++;
        Time.timeScale = currentSpeed;
    }
    public void SpeedDown()
    {
        if (currentSpeed > 0)
        {
            currentSpeed--;
        }
        
        if(currentSpeed == 0)
        {
           speed.text = "SPEED: x" + currentSpeed.ToString();
        }
        Time.timeScale = currentSpeed;
    }
    public void ResetSimulation()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Simulation");
    }
}
