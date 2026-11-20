using System.Collections;
using UnityEngine;

public class Compass : MonoBehaviour
{
    [SerializeField] GameObject guideParticles;
    [SerializeField] float collectDistance = 3f;
    [SerializeField] GameObject purifiedOrb;
    
    OrbsBehaviour[] orbs;
    Transform closestOrb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        orbs = FindObjectsByType<OrbsBehaviour>(FindObjectsSortMode.None);
        closestOrb = orbs[0].transform;
        StartCoroutine(updateClosesOrb());
    }
    // Update is called once per frame
    void Update()
    {
        if (closestOrb == null) {
            guideParticles.SetActive(false);
            return;
        }
        else
        {
            guideParticles.SetActive(true);
        }
        Vector3 adjustedHeight = closestOrb.position;
        adjustedHeight.y = transform.position.y;
        transform.LookAt(adjustedHeight);
        if (Vector3.Distance(transform.position, closestOrb.position) <= collectDistance)
        {
            closestOrb.GetComponent<OrbsBehaviour>().Absorb(transform);
        }
    }

    IEnumerator updateClosesOrb()
    {
        int nullCounter = 0;
        while (nullCounter < orbs.Length)
        {
            nullCounter = 0;
            foreach (OrbsBehaviour orb in orbs)
            {
                if(orb == null)
                {
                    nullCounter++;
                    continue;
                }
                if(closestOrb == null)
                {
                    closestOrb = orb.transform;
                    continue;
                }
                if (Vector3.Distance(closestOrb.position, transform.position) > Vector3.Distance(orb.transform.position, transform.position))
                {
                    closestOrb = orb.transform;
                }
            }
            yield return new WaitForSeconds(3f);
        }
        Debug.Log("Todos los orbes obtenidos");
        Instantiate(purifiedOrb, transform.position, Quaternion.identity);
        FindAnyObjectByType<EnemyGenerator>().gameObject.SetActive(false);
        foreach (BasicEnemyMovement enemy in FindObjectsByType<BasicEnemyMovement>(FindObjectsSortMode.None))
        {
            enemy.Exorcite();
        }
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(transform.position, collectDistance);
    }
}
