using UnityEngine;

public class Interactable : MonoBehaviour {

    public float radius =3f;
    public Transform interactionTransform;

    bool isFocused = false;
    bool hasInteracted = false;
    Transform charInteracting;

    public virtual void Interact() {
        // Override
        Debug.Log("Interacting with" + charInteracting.name);
    }

    void Update() {
        if (isFocused) {
            float distance = Vector3.Distance(charInteracting.position, interactionTransform.position);
            if( distance <= radius && !hasInteracted) {
                //set focus
                Interact();
                hasInteracted = true;
            }
        }
    }


    public void IsFocused( Transform charFocus) {
        isFocused = true;
        charInteracting = charFocus;
        hasInteracted = false;
    }

    public void DeFocused() {
        isFocused = false;
        charInteracting = null;
        hasInteracted = false;
    }


     void OnDrawGizmosSelected() {
        if (interactionTransform == null) {
            interactionTransform = transform;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
