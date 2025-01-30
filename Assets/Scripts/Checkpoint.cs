using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool active = false;
    [SerializeField] private bool playerInRange = false;
    [SerializeField] private ParticleSystem particles;

    public UnityEvent OnActivated;
    public UnityEvent OnDeactivated;

    public void Activate()
    {
        PlayerScript.Instance.TriggerRest();
        PlayerScript.Instance.starterAssetsInputs.action = false;
        CheckpointUI.Instance.exitButton.onClick.AddListener(Deactivate);
        OnActivated.Invoke();
        CheckpointUI.Instance.Show();
    }

    public void Deactivate()
    {
        PlayerScript.Instance.TriggerNotRest();
        PlayerScript.Instance.starterAssetsInputs.action = false;
        CheckpointUI.Instance.exitButton.onClick.RemoveListener(Deactivate);
        OnDeactivated.Invoke();
        CheckpointUI.Instance.Hide();
    }

    private void Update()
    {
        if (!playerInRange) return;
        if (PlayerScript.Instance.starterAssetsInputs.action && !active && !PlayerScript.Instance.isSitting)
        {
            Activate();
        }
        else if (PlayerScript.Instance.starterAssetsInputs.action && active && PlayerScript.Instance.isSitting)
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            particles.Play();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            particles.Stop();
        }
    }
}
