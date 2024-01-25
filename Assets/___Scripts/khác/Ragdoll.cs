using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private Rigidbody[] ragdollRigidbodies;

    private Animator animator;

    private void Awake()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

        animator = GetComponent<Animator>();

        SetRagdollState(false);
    }

    public void SetRagdollState(bool isActive)
    {
        if (animator != null)
        {
            animator.enabled = !isActive;
        }

        GetComponent<CapsuleCollider>().enabled = !isActive;

        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            if (rb != GetComponent<Rigidbody>())
                rb.isKinematic = !isActive;
        }

        if (isActive)
        {
            Invoke("hide_obj", 5f);
        }

    }
    public void hide_obj()
    {
        gameObject.SetActive(false);
        SetRagdollState(false);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("bullet"))
    //    {
    //        GetComponent<Rigidbody>().isKinematic = true;
    //    }
    //}
}
