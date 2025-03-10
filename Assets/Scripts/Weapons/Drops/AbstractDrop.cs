using UnityEngine;

public abstract class AbstractDrop : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        GetPickedUp(collision.collider.GetComponentInParent<PlayerController>());
    }

    private void OnTriggerEnter(Collider other)
    { 
        GetPickedUp(other.GetComponentInParent<PlayerController>());
    }

    bool GetPickedUp(PlayerController player)
    {
        if (!player) return false;

        if (ApplyDrop(player))
        {
            Destroy(this.gameObject);
        }

        return true;
    }

    protected abstract bool ApplyDrop(PlayerController player);
}
