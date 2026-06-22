using System.Linq;
using UnityEngine;

public class RandomAudioPlayer : MonoBehaviour
{
    [SerializeField] private float _timer = 0;
    [SerializeField] private float _CountTo = 5;
    [SerializeField] RangeAttribute range = new RangeAttribute(11, 30);

    [SerializeField] private AudioClip[] _audioClips;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _CountTo = Random.Range(range.min, range.max);
    }

    private void OnEnable()
    {
        if(_audioSource == null || _audioClips.Length == 0)
        {
            Debug.Log("Audio Source not found or Audio Clips not inserted");
            this.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _CountTo)
        {
            ResetCounterAndPlayClip();
        }
    }

    private void ResetCounterAndPlayClip()
    {
        _timer = 0;
        _CountTo = Random.Range(range.min, range.max);
        int RandomClipIndex = Random.Range(0, _audioClips.Length);

        if (_audioClips[RandomClipIndex] != null)
        {
            _audioSource.PlayOneShot(_audioClips[RandomClipIndex]);
        }
    }

    
}
