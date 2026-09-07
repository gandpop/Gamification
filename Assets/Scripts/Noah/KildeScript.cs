using UnityEngine;

#region Kildekritik Enums

public enum Afsender
{
    [InspectorName("Journalist")]
    Journalist,
    [InspectorName("Ekspert")]
    Ekspert,
    [InspectorName("Almen person")]
    AlmenPerson,
    [InspectorName("AI")]
    AI,
    [InspectorName("Influencer")]
    Influencer
}

public enum Aegthed
{
    [InspectorName("AI billede/tekst")]
    AiBilledeTekst,
    [InspectorName("Tid passer ikke")]
    TidPasserIkke,
    [InspectorName("Ægte")]
    Aegte
}

public enum Tendens
{
    [InspectorName("Bias")]
    Bias,
    [InspectorName("Objektiv")]
    Objektiv,
    [InspectorName("Subjektiv")]
    Subjektiv
}

public enum Tid
{
    [InspectorName("Samtidskilde")]
    Samtidskilde,
    [InspectorName("Efterhåndskilde")]
    Efterhaandskilde
}

public enum Afhaengighed
{
    [InspectorName("Førstehåndskilde")]
    Foerstehaandskilde,
    [InspectorName("Andenhåndskilde")]
    Andenhaandskilde
}

public enum Vidensniveau
{
    [InspectorName("Det akademiske niveau")]
    DetAkademiskeNiveau,
    [InspectorName("Det faglige niveau")]
    DetFagligeNiveau,
    [InspectorName("Det formidlende niveau")]
    DetFormidlendeNiveau
}

#endregion

public class KildeScript : MonoBehaviour
{
    [Header("Kildekritik Indstillinger")]
    [SerializeField] private Afsender afsender;
    [SerializeField] private Aegthed aegthed;
    [SerializeField] private Tendens tendens;
    [SerializeField] private Tid tid;
    [SerializeField] private Afhaengighed afhaengighed;
    [SerializeField] private Vidensniveau vidensniveau;

    // Public properties to allow access from other scripts (e.g., GameManager, UI, scoring)
    public Afsender Afsender => afsender;
    public Aegthed Aegthed => aegthed;
    public Tendens Tendens => tendens;
    public Tid Tid => tid;
    public Afhaengighed Afhaengighed => afhaengighed;
    public Vidensniveau Vidensniveau => vidensniveau;
}

