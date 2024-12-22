using UnityEngine;

public class Attack : MonoBehaviour
{
    // References
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log
            animator.SetTrigger("attack");
        }
    }
}
