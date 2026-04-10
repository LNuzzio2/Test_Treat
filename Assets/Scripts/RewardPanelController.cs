using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// Script responsible for the behavior of the Claim and Reward panels. 
/// It is executed through the completed action (subscription event) upon completion of the tricks.
/// </summary>
public class RewardPanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private DiceExplosionManager diceExplosionManager;
    [Space]
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private GameObject claimPanel;

    [Header("UI Reference")]
    [SerializeField] private Button btn_Claim;
    [SerializeField] private Button btn_FullScreenClaim;


    private void Awake()
    {
        if (btn_Claim != null)
            btn_Claim.onClick.AddListener(OnButtonClicked);

        if (btn_FullScreenClaim != null)
            btn_FullScreenClaim.onClick.AddListener(OnFullScreenTap);
    }

    private void OnButtonClicked()
    {
        rewardPanel.SetActive(false);
        claimPanel.SetActive(true);
    }

    private void OnFullScreenTap()
    {
        claimPanel.SetActive(false);
        diceExplosionManager.StartExplotion();

        playerAnimator.SetTrigger("Jump");
    }

    void OnEnable()
    {
        PlayerUIController.OnAllActionsCompleted += HandleAllActionsCompleted;
    }

    void OnDisable()
    {
        PlayerUIController.OnAllActionsCompleted -= HandleAllActionsCompleted;
    }

    private void HandleAllActionsCompleted()
    {
        StartCoroutine(WaitAndShowReward());
    }

    //Coroutine to ensure the animation finishes before opening the claim panel
    private IEnumerator WaitAndShowReward()
    {
        yield return null;

        while (playerAnimator.IsInTransition(0) || playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Action"))
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        rewardPanel.SetActive(true);
    }
}