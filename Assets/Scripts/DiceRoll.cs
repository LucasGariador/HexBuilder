using UnityEngine;
using TMPro;

public class DiceRoll : MonoBehaviour
{
    public Rigidbody rb;
    public float forceMultiplier = 5f; // Ajusta seg�n necesites
    public Transform rollPoint; // Un punto donde lanzar el dado
    private bool rolling = false;
    [SerializeField]
    private TextMeshProUGUI resultText; // Texto para mostrar el resultado


    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    public void RollDice()
    {
        resultText.text = ""; // Limpia el texto antes de lanzar el dado
        if (rolling) return; // Evita lanzar m�ltiples veces seguidas
        rolling = true;

        // Resetea la posici�n del dado
        transform.position = rollPoint.position;
        transform.rotation = Random.rotation; // Rotaci�n aleatoria

        // Aplica fuerza aleatoria
        Vector3 force = new Vector3(
            Random.Range(-1f, 1f),
            1f,
            Random.Range(-1f, 1f)
        ).normalized * forceMultiplier;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(force, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * forceMultiplier, ForceMode.Impulse);

        // Espera un poco y luego calcula el resultado
        Invoke("CheckResult", 3f);
    }

    private void CheckResult()
    {
        rolling = false;
        int result = GetDiceResult();
        resultText.text = result.ToString();
        Debug.Log("Resultado: " + result);
    }

    private int GetDiceResult()
    {
        return Random.Range(1, 21); 
    }
}

