using System;
using UnityEngine;

public class SourceCalculator : MonoBehaviour
{
    public static SourceCalculator Instance { get; private set; }

    [Header("1. AFSENDER (0 til 100)")]
    [Tooltip("Troværdighed for Journalist")]
    [Range(0f, 100f)] [SerializeField] private float afsenderJournalist = 70f;
    [Tooltip("Troværdighed for Ekspert")]
    [Range(0f, 100f)] [SerializeField] private float afsenderEkspert = 90f;
    [Tooltip("Troværdighed for Almen person")]
    [Range(0f, 100f)] [SerializeField] private float afsenderAlmenPerson = 40f;
    [Tooltip("Troværdighed for AI (hvis valgt, nulstilles den samlede score altid til 0)")]
    [Range(0f, 100f)] [SerializeField] private float afsenderAI = 0f;
    [Tooltip("Troværdighed for Influencer")]
    [Range(0f, 100f)] [SerializeField] private float afsenderInfluencer = 20f;

    [Header("2. ÆGTHED (0 til 100)")]
    [Tooltip("Troværdighed for AI billede/tekst (hvis valgt, nulstilles den samlede score altid til 0)")]
    [Range(0f, 100f)] [SerializeField] private float aegthedAiBilledeTekst = 0f;
    [Tooltip("Troværdighed for Tid passer ikke (hvis valgt, nulstilles den samlede score altid til 0)")]
    [Range(0f, 100f)] [SerializeField] private float aegthedTidPasserIkke = 0f;
    [Tooltip("Troværdighed for Ægte")]
    [Range(0f, 100f)] [SerializeField] private float aegthedAegte = 100f;

    [Header("3. TENDENS (0 til 100)")]
    [Tooltip("Troværdighed for Bias")]
    [Range(0f, 100f)] [SerializeField] private float tendensBias = 30f;
    [Tooltip("Troværdighed for Objektiv")]
    [Range(0f, 100f)] [SerializeField] private float tendensObjektiv = 90f;
    [Tooltip("Troværdighed for Subjektiv")]
    [Range(0f, 100f)] [SerializeField] private float tendensSubjektiv = 50f;

    [Header("4. TID (0 til 100)")]
    [Tooltip("Troværdighed for Samtidskilde")]
    [Range(0f, 100f)] [SerializeField] private float tidSamtidskilde = 80f;
    [Tooltip("Troværdighed for Efterhåndskilde")]
    [Range(0f, 100f)] [SerializeField] private float tidEfterhaandskilde = 50f;

    [Header("5. AFHÆNGIGHED (0 til 100)")]
    [Tooltip("Troværdighed for Førstehåndskilde")]
    [Range(0f, 100f)] [SerializeField] private float afhaengighedFoerstehaandskilde = 80f;
    [Tooltip("Troværdighed for Andenhåndskilde")]
    [Range(0f, 100f)] [SerializeField] private float afhaengighedAndenhaandskilde = 50f;

    [Header("6. VIDENSNIVEAU (0 til 100)")]
    [Tooltip("Troværdighed for Det akademiske niveau")]
    [Range(0f, 100f)] [SerializeField] private float vidensniveauAkademisk = 100f;
    [Tooltip("Troværdighed for Det faglige niveau")]
    [Range(0f, 100f)] [SerializeField] private float vidensniveauFagligt = 70f;
    [Tooltip("Troværdighed for Det formidlende niveau")]
    [Range(0f, 100f)] [SerializeField] private float vidensniveauFormidlende = 50f;

    // Last calculated trust rating
    public float LastCalculatedTrustRating { get; private set; }

    // Event invoked when a rating is calculated, useful for UI displays or score meters
    public event Action<float, ArticleData> OnTrustRatingCalculated;

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
    /// Calculates the average trustworthiness rating (0 to 100) for the given ArticleData.
    /// If any category contains AI (Afsender.AI or Aegthed.AiBilledeTekst) or fake time (Aegthed.TidPasserIkke),
    /// the rating is immediately set to 0.0.
    /// </summary>
    public float CalculateTrustRating(ArticleData article)
    {
        if (article == null)
        {
            Debug.LogWarning("SourceCalculator: Kan ikke beregne troværdighed, da article er null.");
            return 0f;
        }

        float rating = CalculateTrustRatingInternal(article.name, article.Afsender, article.Aegthed, article.Tendens, article.Tid, article.Afhaengighed, article.Vidensniveau);
        OnTrustRatingCalculated?.Invoke(rating, article);
        return rating;
    }

    private float CalculateTrustRatingInternal(string sourceName, Afsender afsender, Aegthed aegthed, Tendens tendens, Tid tid, Afhaengighed afhaengighed, Vidensniveau vidensniveau)
    {
        // Check if any category disqualifies the source (AI or time mismatch)
        bool isAIRelated = afsender == Afsender.AI || aegthed == Aegthed.AiBilledeTekst;
        bool isTimeMismatch = aegthed == Aegthed.TidPasserIkke;

        if (isAIRelated || isTimeMismatch)
        {
            LastCalculatedTrustRating = 0f;

            string reason = (isAIRelated && isTimeMismatch)
                ? "AI opdaget og tid passer ikke (kilden er uægte)"
                : (isAIRelated ? "AI opdaget (AI-kilder er ikke akademisk pålidelige)" : "Tid passer ikke (kilden er uægte)");

            Debug.Log($"[Troværdighedsvurdering for: {sourceName}]\n" +
                      $"• Diskvalificeret: {reason}!\n" +
                      $"==> Troværdighed (Trust Rating): 0.0 / 100");

            return 0f;
        }

        float rAfsender = GetAfsenderRating(afsender);
        float rAegthed = GetAegthedRating(aegthed);
        float rTendens = GetTendensRating(tendens);
        float rTid = GetTidRating(tid);
        float rAfhaengighed = GetAfhaengighedRating(afhaengighed);
        float rVidensniveau = GetVidensniveauRating(vidensniveau);

        float total = rAfsender + rAegthed + rTendens + rTid + rAfhaengighed + rVidensniveau;
        float average = Mathf.Clamp(total / 6f, 0f, 100f);

        LastCalculatedTrustRating = average;

        Debug.Log($"[Troværdighedsvurdering for: {sourceName}]\n" +
                  $"• Afsender ({afsender}): {rAfsender:F1}\n" +
                  $"• Ægthed ({aegthed}): {rAegthed:F1}\n" +
                  $"• Tendens ({tendens}): {rTendens:F1}\n" +
                  $"• Tid ({tid}): {rTid:F1}\n" +
                  $"• Afhængighed ({afhaengighed}): {rAfhaengighed:F1}\n" +
                  $"• Vidensniveau ({vidensniveau}): {rVidensniveau:F1}\n" +
                  $"==> Gennemsnitlig troværdighed (Trust Rating): {average:F1} / 100");

        return average;
    }
}

