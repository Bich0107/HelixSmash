using System.Collections.Generic;
using UnityEngine;

public class BrickMaker : MonoBehaviour
{
    [System.Serializable]
    private class BrickPartGroup
    {
        public List<GameObject> brickParts;

        public GameObject GetRandomPart()
        {
            int index = UnityEngine.Random.Range(0, brickParts.Count);
            return brickParts[index];
        }
    }
    [Header("Prefabs")]
    [SerializeField] GameObject finishLine;
    [Header("Maker settings")]
    [SerializeField] List<BrickPartGroup> brickPartList;
    [SerializeField] BrickPartGroup currentPartGroup;
    [Header("Brick settings")]
    [SerializeField] List<GameObject> spawnBricks;
    [SerializeField] List<float> specialPartRatioList;
    [SerializeField] Material specialPartMaterial;
    [SerializeField] AudioClip breakingSound;

    // select a random part group, each have a number of part with different size
    public void SetRandomPartGroup()
    {
        int index = Random.Range(0, brickPartList.Count);
        currentPartGroup = brickPartList[index];
    }

    public List<GameObject> CreateRandomBrick(int amount)
    {
        GameObject brickPart = GetRandomPart(); // select a random part and create amount number of brick(s) from it

        return CreateBricks(brickPart, amount);
    }

    // create a amount number of bricks with part
    public List<GameObject> CreateBricks(GameObject part, int amount)
    {
        List<GameObject> bricks = new List<GameObject>();
        float specialPartRatio = GetRandomSpecialPartRatio(); // choose a random ratio between normal part and special part

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
            brickScript.Initialize(specialPartRatio, specialPartMaterial, breakingSound);

            brick.SetActive(false);

            // add created brick to return list and spawn list to control later
            bricks.Add(brick);
            spawnBricks.Add(brick);
        }

        return bricks;
    }

    public GameObject GetFinishLine()
    {
        GameObject g = Instantiate(finishLine);
        spawnBricks.Add(g);
        return g;
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

    float GetRandomSpecialPartRatio()
    {
        int index = Random.Range(0, specialPartRatioList.Count);
        return specialPartRatioList[index];
    }
    
    // get the amount of part needed to create a brick from part's name
    int GetPartAmount(string partName)
    {
        string[] nameParts = partName.Split('_');
        int partAmount;
        int.TryParse(nameParts[nameParts.Length - 1], out partAmount);
        return partAmount;
    }

    // each part need to rotate a certain degree to form a brick base on the amount of part needed
    float GetRotateDegree(int partAmount) => 360f / partAmount;

    public void Reset()
    {
        foreach (GameObject brick in spawnBricks)
        {
            if (brick != null)
            {
                Destroy(brick);
            }
        }
    }
}
