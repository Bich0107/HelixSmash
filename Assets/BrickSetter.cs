using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickSetter : MonoBehaviour
{
    [SerializeField] BrickMaker brickMaker;

    [Header("Spawn settings")]
    [SerializeField] Vector3 baseSpawnPos;
    [SerializeField] Vector3 currentSpawnPos;
    [Tooltip("List of possible number of brick that will be spawn for a type of brick")]
    [SerializeField] List<int> spawnAmounts;
    [SerializeField] float distance = 1f;
    [SerializeField] int baseAmount = 50;
    [SerializeField] int currentAmount;
    [Header("Rotation settings")]
    [Tooltip("The next brick is rotate by angleDif amount compare to the last brick")]
    [SerializeField] float angleDif = 5f;
    [SerializeField] Quaternion lastRotation = Quaternion.identity;
    [SerializeField] float rotateSpeed;

    void Start()
    {
        brickMaker.SetRandomPartGroup();

        currentAmount = baseAmount;
        currentSpawnPos = baseSpawnPos;
        SpawnBricks();
    }

    public void SpawnBricks()
    {
        while (currentAmount > 0)
        {
            // ensure the number of brick doesn't exceed the spawn amount
            int amount = Mathf.Min(currentAmount, GetRandomAmount()); 
            currentAmount -= amount;

            float rotateAngle = GetRandomRotateDirection();

            List<GameObject> bricks = brickMaker.CreateRandomBrick(amount);
            foreach (GameObject brick in bricks)
            {
                brick.transform.position = currentSpawnPos;
                currentSpawnPos.y += distance;

                // change brick rotation base on last brick rotation and store that value                
                Quaternion rotation = lastRotation *  Quaternion.AngleAxis(angleDif, Vector3.up);
                brick.transform.rotation = rotation;
                lastRotation = rotation;
            
                brick.transform.parent = transform;
                brick.GetComponent<Brick>().SetRotation(rotateAngle);

                brick.SetActive(true);
            }
        }
    }

    float GetRandomRotateDirection() {
        int rand = Random.Range(0, 100);
        if (rand < 50) {
            return rotateSpeed;
        } else {
            return -rotateSpeed;
        }
    }

    int GetRandomAmount()
    {
        int index = Random.Range(0, spawnAmounts.Count);
        return spawnAmounts[index];
    }

    public void Reset() {
        currentAmount = baseAmount;
        currentSpawnPos = baseSpawnPos;
        lastRotation = Quaternion.identity;
    }
}
