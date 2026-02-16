using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float radius = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private InteractPromt prompt;
    private Collider[] buffer = new Collider[32];
    private IInteractable focused;

    private void Update()
    {
        IInteractable nearest = FindNearestInteractable();
        UpdateFocus(nearest);
        if (focused != null && Input.GetKeyDown(KeyCode.X))
        {
            if (focused.CanInteract()) focused.Interact();
        }
    }

    private IInteractable FindNearestInteractable()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, radius, buffer, interactableLayer, QueryTriggerInteraction.Collide);
        IInteractable nearest = null;
        float bestDistSq = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            Collider col = buffer[i];
            if (col == null) continue;
            IInteractable interactable = col.GetComponentInParent<IInteractable>();
            if (interactable == null) continue;
            if (!interactable.CanInteract()) continue;
            float distSq = (col.transform.position - transform.position).sqrMagnitude;
            if (distSq < bestDistSq)
            {
                bestDistSq = distSq;
                nearest = interactable;
            }
        }
        return nearest;
    }

    private void UpdateFocus(IInteractable nearest)
    {
        if(ReferenceEquals(focused, nearest)) return;
        focused?.OnFocusLost();
        focused = nearest;
      //  focused?.OnFocusGained();
        if (focused != null)
        {
            focused.OnFocusGained();
           prompt.Show(focused);
        }else
        {
            prompt.Hide();
        }
        
    }
}
