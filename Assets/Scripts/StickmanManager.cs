using UnityEngine;
using DG.Tweening;

public class StickmanManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("red") && other.transform.parent.childCount > 0) 
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        switch (other.tag)
        {
            case "red":
                if (other.transform.parent.childCount > 0)
                {
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                   
                   
                    
                }

                break;

            case "ramp":
                
                transform.DOJump(transform.position, 1f, 1, 1f).SetEase(Ease.Flash).OnComplete(PlayerManager.PlayerManagerInstance.FormatStickman);
               
                break;

            
        }
    }
}

