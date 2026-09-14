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
public class ArticleData : ScriptableObject
{
    public Sprite ArticleTexture;
    public int ArticleID;

    [Header("Kildekritik Indstillinger")]
    // Public data. Is set for each source and is read elsewhere
    public Afsender Afsender;
    public Aegthed Aegthed;
    public Tendens Tendens;
    public Tid Tid;
    public Afhaengighed Afhaengighed;
    public Vidensniveau Vidensniveau;
}