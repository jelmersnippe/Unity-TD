using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class DamagePopup : MonoBehaviour
{
    TextMeshPro textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        Destroy(gameObject, 2f);
    }

    private void Update()
    {
        transform.position += new Vector3(0, 1 * Time.deltaTime);
    }

    public void setDamage(int damage)
    {
        textMesh.text = damage.ToString();
    }
}
