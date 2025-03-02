using UnityEngine;

public class SetInactiveAfterSlice : MonoBehaviour
{
    [SerializeField] Collider2D sliceCollider;

    public bool isSlicing = false;

    public void disableCollider()
    {
        sliceCollider.enabled = false;
        //Debug.Log("End slice");
        isSlicing = false; // Reset to false when the animation finishes
    }
}
