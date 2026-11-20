using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class OrbsBehaviour : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float speed = 1f;
    [Header("Animation")]
    [SerializeField] float animationDuration = 2f;
    [SerializeField] AnimationCurve movementCurve;
    [SerializeField] AnimationCurve scaleCurve;

    NavMeshAgent agent;


    int iCurrentPatrollPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        iCurrentPatrollPoint = Random.Range(0, patrolPoints.Length);
        setNextDestination(patrolPoints[iCurrentPatrollPoint].position);
        agent.speed = speed;

    }

    Vector3 omitHeigth = new Vector3 (1, 0, 1);

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Vector3.Scale(patrolPoints[iCurrentPatrollPoint].position , omitHeigth), Vector3.Scale(transform.position, omitHeigth)) <0.5)
        {
            iCurrentPatrollPoint = (iCurrentPatrollPoint + 1) % patrolPoints.Length;
            setNextDestination(patrolPoints[iCurrentPatrollPoint].position);
        }
    }

    private void setNextDestination(Vector3 dest) {
        NavMeshHit hit;
        NavMesh.SamplePosition(dest, out hit, 5f, NavMesh.AllAreas);
        agent.SetDestination(hit.position);
    }
    Coroutine corSave = null;
    public void Absorb(Transform tr)
    {
        if (corSave != null) { return; }
        corSave = StartCoroutine(absorvCoroutine(tr));
    }
    IEnumerator absorvCoroutine(Transform tr)
    {
        agent.enabled = false;
        foreach (Transform child in transform)
        {
            StartCoroutine(scaleElement(child));
        }

        Vector3 startPosition = transform.position;
        float time = 0;
        while(time < animationDuration)
        {
            time += Time.deltaTime;
            yield return null;
            transform.position = Vector3.Lerp(startPosition, tr.position, movementCurve.Evaluate(time/animationDuration));

        }
        Destroy(gameObject);
        

    }

    IEnumerator scaleElement(Transform element)
    {
        Vector3 startScale = element.localScale;

        float time = 0;
        while (time < animationDuration)
        {
            time += Time.deltaTime;
            yield return null;
            element.localScale = Vector3.Lerp(startScale, Vector3.zero, movementCurve.Evaluate(time / animationDuration));
        }
    }
}
