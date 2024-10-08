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
    [SerializeField] int spawnCounter;
    [SerializeField] int increaseAmount;
    [Header("Rotation settings")]
    [Tooltip("The next brick is rotate by angleDif amount compare to the last brick")]
    [SerializeField] float angleDif = 5f;
    [SerializeField] Quaternion lastRotation = Quaternion.identity;
    [SerializeField] float rotateSpeed;

    void Start()
    {
        currentAmount = baseAmount;
        currentSpawnPos = baseSpawnPos;
        SpawnBricks();
    }

    public void SpawnBricks()
    {
        // choose and random part group
        brickMaker.SetRandomPartGroup();

        // start setting bricks
        spawnCounter = currentAmount;
        while (spawnCounter > 0)
        {
            // ensure the number of brick doesn't exceed the spawn amount
            int amount = Mathf.Min(spawnCounter, GetRandomAmount());
            spawnCounter -= amount;

            // rotate the brick by a rotateAngle clockwise or counter clockwise
            float rotateAngle = GetRandomRotateDirection();
            float currentAngleDif = rotateAngle > 0 ? -angleDif : angleDif;

            // random number to choose a material style for these bricks
            int num = Random.Range(0, 100);

            List<GameObject> bricks = brickMaker.CreateRandomBrick(amount);
            foreach (GameObject brick in bricks)
            {
                // update brick position and spawn pos
                brick.transform.position = currentSpawnPos;
                currentSpawnPos.y += distance;

                // change brick rotation base on last brick rotation and store that value                
                Quaternion rotation = lastRotation * Quaternion.AngleAxis(currentAngleDif, Vector3.up);
                brick.transform.rotation = rotation;
                lastRotation = rotation;

                brick.transform.parent = transform;

                // start brick rotation
                Brick brickScript = brick.GetComponent<Brick>();
                brickScript.SetRotation(rotateAngle);

                // choose material type base on previous random num
                if (num < 50) brickScript.SetPartsMaterialType1();
                else brickScript.SetPartsMaterialType2();

                brick.SetActive(true);
            }
        }

        SpawnFinishLine();
    }

    void SpawnFinishLine()
    {
        GameObject brick = brickMaker.GetFinishLine();
        brick.transform.position = currentSpawnPos;
        currentSpawnPos.y += distance;
        brick.transform.parent = transform;
    }

    float GetRandomRotateDirection()
    {
        int rand = Random.Range(0, 100); // 50% to rotate clockwise or counter clockwise
        if (rand < 50)
        {
            return rotateSpeed;
        }
        else
        {
            return -rotateSpeed;
        }
    }

    int GetRandomAmount()
    {
        int index = Random.Range(0, spawnAmounts.Count);
        return spawnAmounts[index];
    }

    public void ToNextStage()
    {
        ResetTransformSettings();
        currentAmount += increaseAmount;
        SpawnBricks();
    }

    void ResetTransformSettings()
    {
        currentSpawnPos = baseSpawnPos;
        lastRotation = Quaternion.identity;
    }

    public void Reset()
    {
        brickMaker.Reset();
        currentAmount = baseAmount;
        ResetTransformSettings();
    }
}
