using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// General script for the general behavior of the player (dog)
/// </summary>
public class PlayerUIController : MonoBehaviour
{
    public static event Action OnAllActionsCompleted;

    [Header("Player Reference")]
    [SerializeField] private Animator playerController;

    [Header("UI Reference")]
    [SerializeField] private Button btn_Roll;
    [SerializeField] private Button btn_Spin;
    [SerializeField] private Button btn_Bark;
    [SerializeField] private Button btn_Restart;
    [Space]
    [SerializeField] private TextMeshProUGUI buttonSign;

    [Header("Reward Conditions")]
    private HashSet<string> _completedActions = new HashSet<string>(); //We use HashSet to control/restrict the firing of the trigger parameters
    private const int TOTAL_ACTIONS_NEEDED = 3;

    [Header("Idle Animations")]
    [SerializeField] private List<string> randomIdleTriggers = new List<string>();
    [SerializeField] private float idleThreshold = 5f;
    private Coroutine _idleCoroutine;

    [Header("Dog Customization")]
    [SerializeField] private Button btn_ChangeColor;
    [SerializeField] private SkinnedMeshRenderer dogSkinnedRenderer;
    private Material _cachedBodyMaterial; 
    [SerializeField] private List<Color> skinColors = new List<Color>();
    private int _currentColorIndex = 0;

    private void Awake()
    {
        if (dogSkinnedRenderer != null && dogSkinnedRenderer.materials.Length > 1)
        {
            _cachedBodyMaterial = dogSkinnedRenderer.materials[1];
        }
    }

    void Start()
    {
        if (btn_Roll != null)
            btn_Roll.onClick.AddListener(() => OnButtonClicked("Roll", btn_Roll));

        if (btn_Spin != null)
            btn_Spin.onClick.AddListener(() => OnButtonClicked("Spin", btn_Spin));

        if (btn_Bark != null)
            btn_Bark.onClick.AddListener(() => OnButtonClicked("Bark", btn_Bark));

        if (btn_Restart != null) btn_Restart.onClick.AddListener(RestartGame);
        if (btn_ChangeColor != null) btn_ChangeColor.onClick.AddListener(CycleDogColor);

        _idleCoroutine = StartCoroutine(IdleDetectionRoutine());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    //Function to control the animations that are triggered
    private void OnButtonClicked(string trigger, Button button)
    {
        //Condition for not playing two animations at the same time
        if (playerController.IsInTransition(0) || IsPerformingAction())
        {
            buttonSign.text = "Bluster is already doing a trick!";
            StartCoroutine(FeedbackRoutine(2f));

            return;
        }

        playerController.SetTrigger(trigger);

        //We check if the trigger already existed
        if (_completedActions.Add(trigger))
        {
            //If we complete it, we call the action of the panel.
            if (_completedActions.Count >= TOTAL_ACTIONS_NEEDED)
            {
                OnAllActionsCompleted?.Invoke();
            }
        }

        button.interactable = false;
    }

    //Function to check if any animation with the Action tag is playing
    private bool IsPerformingAction()
    {
        AnimatorStateInfo stateInfo = playerController.GetCurrentAnimatorStateInfo(0);

        return stateInfo.IsTag("Action") && stateInfo.normalizedTime < 1.0f;
    }

    //Function for the warning Sign
    private IEnumerator FeedbackRoutine(float duration)
    {
        buttonSign.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        buttonSign.gameObject.SetActive(false);
    }

    //Function to trigger the idle animation randomly
    private IEnumerator IdleDetectionRoutine()
    {
        float timer = 0f;

        while (true)
        {
            // If the dog is "playing or the panel is open
            if (IsPerformingAction() || playerController.IsInTransition(0))
            {
                timer = 0f;
            }
            else
            {
                timer += Time.deltaTime;
            }

            if (timer >= idleThreshold)
            {
                TriggerRandomIdle();
                timer = 0f; // Resetear tras disparar
            }

            yield return null;
        }
    }

    //Function to retrieve a random string from the list
    private void TriggerRandomIdle()
    {
        if (randomIdleTriggers.Count == 0) return;

        string randomTrigger = randomIdleTriggers[UnityEngine.Random.Range(0, randomIdleTriggers.Count)];
        playerController.SetTrigger(randomTrigger);
    }

    //Function to change the base color of the dog's material
    public void CycleDogColor()
    {
        if (_cachedBodyMaterial == null || skinColors.Count == 0) return;

        _currentColorIndex = (_currentColorIndex + 1) % skinColors.Count;
        Color nextColor = skinColors[_currentColorIndex];

        //We need to collect the alpha because the material is transparent.
        float originalAlpha = _cachedBodyMaterial.GetColor("_BaseColor").a;

        Color finalColor = new Color(nextColor.r, nextColor.g, nextColor.b, originalAlpha);

        _cachedBodyMaterial.SetColor("_BaseColor", finalColor);
    }

    //Basic function to reset the behavior of the buttons and the dog animator
    public void RestartGame()
    {
        _completedActions.Clear();

        btn_Roll.interactable = true;
        btn_Spin.interactable = true;
        btn_Bark.interactable = true;

        buttonSign.text = "All tricks restored!";
        StartCoroutine(FeedbackRoutine(2f));

        playerController.Rebind();

        if (_idleCoroutine != null) StopCoroutine(_idleCoroutine);
        _idleCoroutine = StartCoroutine(IdleDetectionRoutine());

        Debug.Log("Game State Reset.");
    }
}