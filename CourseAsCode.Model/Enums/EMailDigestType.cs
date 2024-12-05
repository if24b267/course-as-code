namespace CourseAsCode.Model.Enums;

public enum EMailDigestType
{
    NoDigest, // user receives one e-mail per forum post
    DigestComplete, // user receives one e-mail per day containing the complete contents of each forum posts 
    DigestSubjects // user receives one e-mail per day containing just the subjects of each forum post
}