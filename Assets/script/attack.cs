using UnityEngine;

public class attack : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float range = 100f;          // Max shoot distance
    public float damage = 10f;         // Damage per shot
    public LayerMask hitLayers;         // Layers to hit (e.g., Enemy, Ground)
    public KeyCode shootKey = KeyCode.Mouse0; // Fire button

    [Header("Visuals")]
    public Transform shootOrigin;      // Where the raycast starts (e.g., gun barrel)
    public float debugLineDuration = 1f; // How long to show the debug line
    public Transform parentobject;
    public bool Holding_object;
    public Transform shootpoint;

    void Update()
    {
            Shoot();

    }

    void Shoot()
    {
        if (shootOrigin == null)
        {
            Debug.LogWarning("No shoot origin assigned!");
            return;
        }

        // Create a ray from the shoot origin, forward
        Ray ray = new Ray(shootOrigin.position, shootOrigin.forward);
        RaycastHit hit;

        // Check if the ray hits something
        if (Physics.Raycast(ray, out hit, range, hitLayers))
        {
            GameObject targetobject = hit.transform.gameObject;
            // Log what was hit
            Debug.Log("Hit: " + targetobject.name);
            if (Input.GetAxisRaw("Fire1") == 1)
            {
                if(!Holding_object)
                {
                    if (targetobject.tag == "Object")
                    {
                        targetobject.transform.SetParent(parentobject);
                        targetobject.GetComponent<Rigidbody>().isKinematic = true;
                        targetobject.transform.localPosition = Vector3.zero;
                        targetobject.transform.localRotation = Quaternion.Euler(0,0,0);
                        Holding_object = true;
                    }
                }
            }

        }
        if (Input.GetAxisRaw("Fire1") == 0)
        {
            if (Holding_object)
            {
                Transform childobject = parentobject.GetChild(0);
                childobject.transform.SetParent(null);
                childobject.GetComponent<Rigidbody>().isKinematic = false;
                childobject.position = shootpoint.position;
                childobject.GetComponent<Rigidbody>().AddForce(shootpoint.forward * 10, ForceMode.Impulse);
                Holding_object = false;
            }
        }

        // Visual debug (shows in Scene view)
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, debugLineDuration);
    }
}