using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickMaker : MonoBehaviour
{
    [Serializable]
    private class BrickPartGroup
    {
        public List<GameObject> brickParts;

        public GameObject GetRandomPart()
        {
            int index = UnityEngine.Random.Range(0, brickParts.Count);
            return brickParts[index];
        }
    }

    [Header("Maker settings")]
    [SerializeField] List<BrickPartGroup> brickPartList;
    [SerializeField] BrickPartGroup currentPartGroup;
    [Header("Brick settings")]
    [SerializeField] float specialPartRatio;
    [SerializeField] Material specialPartMaterial;

    public void SetRandomPartGroup()
    {
        int index = UnityEngine.Random.Range(0, brickPartList.Count);
        currentPartGroup = brickPartList[index];
    }

    public List<GameObject> CreateRandomBrick(int amount)
    {
        GameObject brickPart = GetRandomPart();

        return CreateBricks(brickPart, amount);
    }

    public List<GameObject> CreateBricks(GameObject part, int amount)
    {
        List<GameObject> bricks = new List<GameObject>();

        for (int i = 0; i < amount; i++)
        {
            GameObject brick = new GameObject("Brick");

            // create parts and make them children of brick
            int partAmount = GetPartAmount(part.name);
            float rotateDegree = GetRotateDegree(partAmount);
            for (int j = 0; j < partAmount; j++)
            {
                Quaternion rotation = Quaternion.AngleAxis(rotateDegree * j, Vector3.up);

                GameObject instance = Instantiate(part, Vector3.zero, rotation, brick.transform);
                instance.AddComponent<BrickPart>();
            }

            // initialize brick
            Brick brickScript = brick.AddComponent<Brick>();
            brickScript.Initialize(specialPartRatio, specialPartMaterial);

            brick.SetActive(false);

            bricks.Add(brick);
        }

        return bricks;
    }

    GameObject GetRandomPart()
    {
        if (currentPartGroup == null)
        {
            Debug.Log("current part group is null");
            return null;
        }
        return currentPartGroup.GetRandomPart();
    }

    int GetPartAmount(string partName)
    {
        string[] nameParts = partName.Split('_');
        int partAmount;
        int.TryParse(nameParts[nameParts.Length - 1], out partAmount);
        return partAmount;
    }

    float GetRotateDegree(int partAmount) => 360f / partAmount;
}
