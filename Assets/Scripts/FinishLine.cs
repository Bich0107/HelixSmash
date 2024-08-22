using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] PlayText playText;
    [SerializeField] float YDistanceToPlayer = 1.1f;
    [SerializeField] float delay = 2f;

    void Start() {
        playText = FindObjectOfType<PlayText>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Ball ball = other.GetComponent<Ball>();
            ball.Finish();

            Vector3 pos = ball.transform.position;
            pos.y = transform.position.y + YDistanceToPlayer;
            ball.transform.position = pos;

            StartCoroutine(CR_LoadNewState());
        }
    }

    IEnumerator CR_LoadNewState()
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.ToNextStage();
        playText.Show();
        Destroy(gameObject);
    }
}
