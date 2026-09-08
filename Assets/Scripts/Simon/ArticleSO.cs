using UnityEngine;

#region Kildekritik kategorier
public enum Afsender
{
    Journalist,
    Ekspert,
    AlmenPerson,
    AI,
    Influencer
}

public enum Aegthed
{
    AiBilledeTekst,
    TidPasserIkke,
    Aegte
}

public enum Tendens
{
    Bias,
    Objektiv,
    Subjektiv
}

public enum Tid
{
    Samtidskilde,
    Efterhaandskilde
}

public enum Afhaengighed
{
    Foerstehaandskilde,
    Andenhaandskilde
}

public enum Vidensniveau
{
    DetAkademiskeNiveau,
    DetFagligeNiveau,
    DetFormidlendeNiveau
}
#endregion

[CreateAssetMenu(fileName = "New Article", menuName = "Source/Article")]
public class ArticleSO : ScriptableObject
{
    [Header("Article elements")]
    [Header("Texts")]
    public string articleName; // The header of the article
    public string authorAndDate; // Author and date (top left)
    public string rightSideText; // The text on the right side of the top image
    public string articleText; // The text below the top image

    [Header("Images")]
    public Sprite topImage; // The image at the top
    public Sprite bottomImage; // The image at the top

    [Header("Kildekritik Indstillinger")]
    // Serialized variabler, som kan ændres i Inpector,
    // men da de skal læses et andet sted fra, så laver vi en 'Property' længere nede,
    // som returner værdien af den private variabel
    [SerializeField] private Afsender afsender;
    [SerializeField] private Aegthed aegthed;
    [SerializeField] private Tendens tendens;
    [SerializeField] private Tid tid;
    [SerializeField] private Afhaengighed afhaengighed;
    [SerializeField] private Vidensniveau vidensniveau;

    // Det her er properties. 
    public Afsender Afsender => afsender;
    public Aegthed Aegthed => aegthed;
    public Tendens Tendens => tendens;
    public Tid Tid => tid;
    public Afhaengighed Afhaengighed => afhaengighed;
    public Vidensniveau Vidensniveau => vidensniveau;
}