using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("1. AFSENDER (0 til 10)")]
    [Tooltip("Troværdighed for Journalist")]
    [Range(0f, 10f)] [SerializeField] private float afsenderJournalist = 7f;
    [Tooltip("Troværdighed for Ekspert")]
    [Range(0f, 10f)] [SerializeField] private float afsenderEkspert = 9f;
    [Tooltip("Troværdighed for Almen person")]
    [Range(0f, 10f)] [SerializeField] private float afsenderAlmenPerson = 4f;
    [Tooltip("Troværdighed for AI (hvis valgt, nulstilles den samlede score altid til 0)")]
    [Range(0f, 10f)] [SerializeField] private float afsenderAI = 0f;
    [Tooltip("Troværdighed for Influencer")]
    [Range(0f, 10f)] [SerializeField] private float afsenderInfluencer = 2f;

    [Header("2. ÆGTHED (0 til 10)")]
    [Tooltip("Troværdighed for AI billede/tekst (hvis valgt, nulstilles den samlede score altid til 0)")]
    [Range(0f, 10f)] [SerializeField] private float aegthedAiBilledeTekst = 0f;
    [Tooltip("Troværdighed for Tid passer ikke")]
    [Range(0f, 10f)] [SerializeField] private float aegthedTidPasserIkke = 2f;
    [Tooltip("Troværdighed for Ægte")]
    [Range(0f, 10f)] [SerializeField] private float aegthedAegte = 10f;

    [Header("3. TENDENS (0 til 10)")]
    [Tooltip("Troværdighed for Bias")]
    [Range(0f, 10f)] [SerializeField] private float tendensBias = 3f;
    [Tooltip("Troværdighed for Objektiv")]
    [Range(0f, 10f)] [SerializeField] private float tendensObjektiv = 9f;
    [Tooltip("Troværdighed for Subjektiv")]
    [Range(0f, 10f)] [SerializeField] private float tendensSubjektiv = 5f;

    [Header("4. TID (0 til 10)")]
    [Tooltip("Troværdighed for Samtidskilde")]
    [Range(0f, 10f)] [SerializeField] private float tidSamtidskilde = 8f;
    [Tooltip("Troværdighed for Efterhåndskilde")]
    [Range(0f, 10f)] [SerializeField] private float tidEfterhaandskilde = 5f;

    [Header("5. AFHÆNGIGHED (0 til 10)")]
    [Tooltip("Troværdighed for Førstehåndskilde")]
    [Range(0f, 10f)] [SerializeField] private float afhaengighedFoerstehaandskilde = 8f;
    [Tooltip("Troværdighed for Andenhåndskilde")]
    [Range(0f, 10f)] [SerializeField] private float afhaengighedAndenhaandskilde = 5f;

    [Header("6. VIDENSNIVEAU (0 til 10)")]
    [Tooltip("Troværdighed for Det akademiske niveau")]
    [Range(0f, 10f)] [SerializeField] private float vidensniveauAkademisk = 10f;
    [Tooltip("Troværdighed for Det faglige niveau")]
    [Range(0f, 10f)] [SerializeField] private float vidensniveauFagligt = 7f;
    [Tooltip("Troværdighed for Det formidlende niveau")]
    [Range(0f, 10f)] [SerializeField] private float vidensniveauFormidlende = 5f;

    // Last calculated trust rating
    public float LastCalculatedTrustRating { get; private set; }

    // Event invoked when a rating is calculated, useful for UI displays or score meters
    public event Action<float, KildeScript> OnTrustRatingCalculated;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    #region Rating Lookup Helpers

    public float GetAfsenderRating(Afsender afsender) => afsender switch
    {
        Afsender.Journalist => afsenderJournalist,
        Afsender.Ekspert => afsenderEkspert,
        Afsender.AlmenPerson => afsenderAlmenPerson,
        Afsender.AI => afsenderAI,
        Afsender.Influencer => afsenderInfluencer,
        _ => 0f
    };

    public float GetAegthedRating(Aegthed aegthed) => aegthed switch
    {
        Aegthed.AiBilledeTekst => aegthedAiBilledeTekst,
        Aegthed.TidPasserIkke => aegthedTidPasserIkke,
        Aegthed.Aegte => aegthedAegte,
        _ => 0f
    };

    public float GetTendensRating(Tendens tendens) => tendens switch
    {
        Tendens.Bias => tendensBias,
        Tendens.Objektiv => tendensObjektiv,
        Tendens.Subjektiv => tendensSubjektiv,
        _ => 0f
    };

    public float GetTidRating(Tid tid) => tid switch
    {
        Tid.Samtidskilde => tidSamtidskilde,
        Tid.Efterhaandskilde => tidEfterhaandskilde,
        _ => 0f
    };

    public float GetAfhaengighedRating(Afhaengighed afhaengighed) => afhaengighed switch
    {
        Afhaengighed.Foerstehaandskilde => afhaengighedFoerstehaandskilde,
        Afhaengighed.Andenhaandskilde => afhaengighedAndenhaandskilde,
        _ => 0f
    };

    public float GetVidensniveauRating(Vidensniveau vidensniveau) => vidensniveau switch
    {
        Vidensniveau.DetAkademiskeNiveau => vidensniveauAkademisk,
        Vidensniveau.DetFagligeNiveau => vidensniveauFagligt,
        Vidensniveau.DetFormidlendeNiveau => vidensniveauFormidlende,
        _ => 0f
    };

    #endregion

    /// <summary>
    /// Calculates the average trustworthiness rating (0 to 10) for the given KildeScript.
    /// If any category contains AI (Afsender.AI or Aegthed.AiBilledeTekst), the rating is immediately 0.0.
    /// </summary>
    public float CalculateTrustRating(KildeScript kilde)
    {
        if (kilde == null)
        {
            Debug.LogWarning("GameManager: Kan ikke beregne troværdighed, da kilde er null.");
            return 0f;
        }

        // Check if any category is AI-related (automatically disqualifies the source)
        bool isAIRelated = kilde.Afsender == Afsender.AI || kilde.Aegthed == Aegthed.AiBilledeTekst;

        if (isAIRelated)
        {
            LastCalculatedTrustRating = 0f;

            Debug.Log($"[Troværdighedsvurdering for: {kilde.gameObject.name}]\n" +
                      $"• AI opdaget! (Afsender: {kilde.Afsender}, Ægthed: {kilde.Aegthed})\n" +
                      $"==> Troværdighed (Trust Rating): 0.0 / 10 (AI-kilder er ikke akademisk pålidelige!)");

            OnTrustRatingCalculated?.Invoke(0f, kilde);
            return 0f;
        }

        float rAfsender = GetAfsenderRating(kilde.Afsender);
        float rAegthed = GetAegthedRating(kilde.Aegthed);
        float rTendens = GetTendensRating(kilde.Tendens);
        float rTid = GetTidRating(kilde.Tid);
        float rAfhaengighed = GetAfhaengighedRating(kilde.Afhaengighed);
        float rVidensniveau = GetVidensniveauRating(kilde.Vidensniveau);

        float total = rAfsender + rAegthed + rTendens + rTid + rAfhaengighed + rVidensniveau;
        float average = Mathf.Clamp(total / 6f, 0f, 10f);

        LastCalculatedTrustRating = average;

        Debug.Log($"[Troværdighedsvurdering for: {kilde.gameObject.name}]\n" +
                  $"• Afsender ({kilde.Afsender}): {rAfsender:F1}\n" +
                  $"• Ægthed ({kilde.Aegthed}): {rAegthed:F1}\n" +
                  $"• Tendens ({kilde.Tendens}): {rTendens:F1}\n" +
                  $"• Tid ({kilde.Tid}): {rTid:F1}\n" +
                  $"• Afhængighed ({kilde.Afhaengighed}): {rAfhaengighed:F1}\n" +
                  $"• Vidensniveau ({kilde.Vidensniveau}): {rVidensniveau:F1}\n" +
                  $"==> Gennemsnitlig troværdighed (Trust Rating): {average:F1} / 10");

        OnTrustRatingCalculated?.Invoke(average, kilde);

        return average;
    }
}

