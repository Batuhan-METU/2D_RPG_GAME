using UnityEditor;
using UnityEngine;

public class Object_Checkpoint : MonoBehaviour, ISaveable
{
    [SerializeField] private string checkpointId;
    [SerializeField] private Transform respawnPoint;

    public bool isActive { get; private set; }
    private Animator anim;
    private AudioSource fireAudioSource;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        fireAudioSource = GetComponent<AudioSource>();
    }

    public string GetCheckpointId() => checkpointId;

    public Vector3 GetPosition() => respawnPoint == null ? transform.position : respawnPoint.position;


    public void ActivateCheckpoint(bool activate)
    {
        isActive = activate;
        anim.SetBool("isActive", activate);

        if (isActive && fireAudioSource.isPlaying == false)
            fireAudioSource.Play();

        if(isActive == false)
            fireAudioSource.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActivateCheckpoint(true);
    }

    public void LoadData(GameData data)
    {
        bool active = data.unlockedCheckpoints.TryGetValue(checkpointId, out active);
        ActivateCheckpoint(active);
    }

    public void SaveData(ref GameData data)
    {
        if (isActive == false)
            return;

        if (data.unlockedCheckpoints.ContainsKey(checkpointId) == false)
            data.unlockedCheckpoints.Add(checkpointId, true);
    }


    private void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(checkpointId))
        {
            checkpointId = System.Guid.NewGuid().ToString();
        }
#endif
    }
}
