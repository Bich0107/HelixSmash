using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] float YdistanceToPlayer;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Ball ball = other.GetComponent<Ball>();
            ball.Finish();

            Vector3 pos = ball.transform.position;
            pos.y = transform.position.y + YdistanceToPlayer;
            ball.transform.position = pos;

            Debug.Log("finish");
            SceneManager.LoadScene(0);
        }
    }
}
