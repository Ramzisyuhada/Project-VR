using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Snapgrid : MonoBehaviour
{
    public float gridSize = 1f;
    public Color snappedColor = Color.blue; // Warna saat tersnap
    public Color unableToSnapColor = Color.red; // Warna saat tumpang tindih

    private XRGrabInteractable grabInteractable;
    private MeshRenderer meshRenderer;
    private Color originalColor;

    private Vector3 originalPosition;
    private bool isBeingDragged = false;
    private bool isSnapped = false;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEntered.AddListener(OnGrabbed);
        grabInteractable.onSelectExited.AddListener(OnReleased);
        
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (isBeingDragged)
        {
            Vector3 grabbedPosition = transform.position;
            Vector3 snappedPosition = SnapPosition(grabbedPosition);
            transform.position = snappedPosition;

            bool isColliding = IsColliding();

            // Tentukan warna berdasarkan status snapping
            if (isColliding)
            {
                meshRenderer.material.color = unableToSnapColor; // Objek berpotensi tumpang tindih (merah)
                isSnapped = false;
            }
            else
            {
                meshRenderer.material.color = snappedColor; // Objek tersnap (biru)
                isSnapped = true;
            }
        }
    }

    private void OnGrabbed(XRBaseInteractor interactor)
    {
        isBeingDragged = true;
    }

    private void OnReleased(XRBaseInteractor interactor)
    {
        isBeingDragged = false;

        if (isSnapped)
        {
            // Kembalikan warna objek ke warna aslinya setelah tersnap
            meshRenderer.material.color = originalColor;
        }
        else
        {
            // Kembalikan objek ke posisi awal jika tidak berhasil tersnap
            transform.position = originalPosition;
        }
    }

    private Vector3 SnapPosition(Vector3 position)
    {
        // Hitung posisi snapped ke grid
        float snappedX = Mathf.Round(position.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(position.y / gridSize) * gridSize;
        float snappedZ = Mathf.Round(position.z / gridSize) * gridSize;

        Vector3 snappedPosition = new Vector3(snappedX, snappedY, snappedZ);
        RaycastHit hit;

        // Periksa apakah posisi snapped berada di atas objek lain
        if (Physics.Raycast(snappedPosition, Vector3.down, out hit))
        {
            if (hit.collider.gameObject != gameObject)
            {
                return snappedPosition;
            }
        }

        return position;
    }

    private bool IsColliding()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, gridSize / 2f);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                return true;
            }
        }

        return false;
    }
}
