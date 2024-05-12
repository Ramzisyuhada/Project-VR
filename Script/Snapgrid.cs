using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Snapgrid : MonoBehaviour
{
    public float gridSize = 1f; // Ukuran grid untuk snapping
    public Material snappedMaterial; // Material yang akan digunakan saat objek tersnap
    public Material unableToSnapMaterial; // Material yang akan digunakan saat objek tidak dapat tersnap

    private XRGrabInteractable grabInteractable;
    private MeshRenderer meshRenderer;
    private Color originalColor; // Warna asli objek

    private Vector3 originalPosition;
    private bool isBeingDragged = false;
    private bool isSnapped = false;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEntered.AddListener(OnGrabbed);
        grabInteractable.onSelectExited.AddListener(OnReleased);

        meshRenderer = GetComponent<MeshRenderer>();
        originalPosition = transform.position; // Simpan posisi awal objek
        originalColor = meshRenderer.material.color; // Simpan warna asli objek

    }

    private void Update()
    {
        if (isBeingDragged)
        {
            Vector3 grabbedPosition = transform.position;
            Vector3 snappedPosition = SnapPosition(grabbedPosition);
            transform.position = snappedPosition;

            // Periksa apakah objek bersentuhan dengan objek lain
            bool isColliding = IsColliding();

            // Tentukan warna berdasarkan status snapping
            meshRenderer.material = isColliding ? unableToSnapMaterial : snappedMaterial;

            isSnapped = !isColliding; // Tandai objek sebagai tersnap jika tidak bersentuhan dengan objek lain
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

        // Periksa apakah posisi snapped berada di atas objek lain
        Vector3 snappedPosition = new Vector3(snappedX, snappedY, snappedZ);
        RaycastHit hit;
        if (Physics.Raycast(snappedPosition, Vector3.down, out hit))
        {
            if (hit.collider.gameObject != gameObject) // Hindari collision dengan diri sendiri
            {
                return snappedPosition; // Return posisi snapped jika berada di atas objek lain
            }
        }

        return position; // Kembalikan posisi awal jika tidak berada di atas objek lain
    }

    private bool IsColliding()
    {
        // Periksa collision menggunakan collider di sekitar objek dengan radius setengah dari ukuran grid
        Collider[] colliders = Physics.OverlapSphere(transform.position, gridSize / 2f);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject) // Hindari collision dengan diri sendiri
            {
                return true; // Ada collision dengan objek lain
            }
        }

        return false; // Tidak ada collision dengan objek lain
    }
}
