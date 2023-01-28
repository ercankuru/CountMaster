using UnityEngine;
using DG.Tweening;

public class Fine : MonoBehaviour
{

    public Transform stickman; // Çarpacak olan stickman'in transform'i
    public Transform wall; // Çarpýlacak olan duvarýn transform'i
    [SerializeField] private BoxCollider boxCollider;

    private void Start()
    {
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("all"))
        {
            GetComponent<BoxCollider>().isTrigger = false;
        }

    }
}