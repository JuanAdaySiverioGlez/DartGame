using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoDemonio : MonoBehaviour {
    [SerializeField] private Transform[] puntos;
    private Animator animator;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        StartCoroutine(MoverDemonio());
    }
    private IEnumerator MoverDemonio() {
        while (true) {
            yield return new WaitForSeconds(5f);
            for (int i = 0; i < puntos.Length; i++) {
                while (transform.position != puntos[i].position) {
                    Vector3 direction = (puntos[i].position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                    transform.position = Vector3.MoveTowards(transform.position, puntos[i].position, 0.05f);
                    yield return new WaitForSeconds(0.01f);
                }
                yield return new WaitForSeconds(0.01f);
            }
            transform.position = puntos[0].position;
        }
    }
}
