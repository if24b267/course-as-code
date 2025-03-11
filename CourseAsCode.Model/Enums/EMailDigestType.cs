namespace CourseAsCode.Model.Enums;

public enum EMailDigestType
{
	NoDigest = 0, // user receives one e-mail per forum post
	DigestComplete = 1, // user receives one e-mail per day containing the complete contents of each forum posts 
	DigestSubjects = 2 // user receives one e-mail per day containing just the subjects of each forum post
}