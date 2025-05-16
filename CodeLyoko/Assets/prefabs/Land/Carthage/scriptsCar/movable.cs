using System.Collections;
using UnityEngine;

public class Movable : MonoBehaviour
{
    [Header("Movement Settings")]
    public float distance = 5f;
    public float speed = 2f;
    public float waitTime = 1f;

    [Header("Movement Mode")]
    public bool moveHorizontally = true;

    private Vector3 startPosition;
    private bool movingForward = true;

    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(MoveObject());
    }

    IEnumerator MoveObject()
    {
        while (true)
        {
            Vector3 direction = moveHorizontally ? transform.right : transform.up;
            Vector3 targetPosition = startPosition + direction * (movingForward ? distance : 0);

            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);
            movingForward = !movingForward;

            if (!movingForward)
            {
                targetPosition = startPosition;
                while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                    yield return null;
                }
                yield return new WaitForSeconds(waitTime);
            }
        }
    }
}
