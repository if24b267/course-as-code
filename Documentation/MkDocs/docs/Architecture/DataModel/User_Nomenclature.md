# User-specific Nomenclature
Information about a user enrolled in the course is stored in `CourseAsCode.Model/User.cs`. Just as with all other classes represented in the internal data model, an effort has been made to explain all necessary concepts in the code using Data Annotations. However, some of the properties, especially those defined using enums, may not be immediately understandable without further background knowledge. This section of the documentation is intended to provide developers with this background knowledge to work with the User model by explaining the most relevant terms in more detail:

### EnrolmentSuspended
Suspended users are no longer able to access any of their courses on Moodle but their data is preserved, so it is possible for them to return to their course at a later time.

| Value in Data Model | Value in Moodle XML | Description                 |
|---------------------|---------------------|-----------------------------|
| false               | 0                   | Enrolment Status: Active    |
| true                | 1                   | Enrolment Status: Suspended |

See [moodle docs](https://docs.moodle.org/405/en/Unenrolment#Suspended_or_Unenrolled?) for more information

### Timezone
The time zone is used to help convert the raw timestamps in the database to human readable dates and times, for the user who is requesting the page. 

| Value in Data Model | Value in Moodle XML | Description                                                     |
|--------------------|---------------------|-----------------------------------------------------------------|
| ServerDefault      | 99                  | set by the server administrator, default timezone for all users |
| EuropeVienna       | Europe/Vienna       | Central European Time                                           |
| EuropeBerlin       | Europe/Berlin       | Central European Time                                           |
| EuropeZurich       | Europe/Zurich       | Central European Time                                           |

See [moodle docs](https://docs.moodle.org/405/en/Location) for more information

### EMailVisibility
This setting determines who can view the user's e-mail address

| Value in Data Model     | Value in Moodle XML | Description                                                                                 |
|-------------------------|---------------------|---------------------------------------------------------------------------------------------|
| Hidden                  | 0                   | Only users with appropriate permissions such as teachers can view the user's e-mail address |
| VisibleToEveryone       | 1                   | All users on the site can view the user's e-mail address                                    |
| VisibleToParticipants   | 2                   | Only users who are enrolled in the same course as the user can view the user's e-mail address                                                                      |

### EMailDigestType
This setting defines the default notification settings of a user for forums. The digest type determines how often the user receives e-mail notifications from Moodle.

!!! tip
    In addition to this standard setting, the user can also change the E-Mail digest type for each forum individually.

| Value in Data Model | Value in Moodle XML | Description                                                                           |
|---------------------|---------------------|---------------------------------------------------------------------------------------|
| NoDigest            | 0                   | user receives one e-mail per forum post                                               |
| DigestComplete      | 1                   | user receives one e-mail per day containing the complete contents of each forum posts |
| DigestSubjects      | 2                   | user receives one e-mail per day containing just the subjects of each forum post      |

### AuthMethod (AuthenticationMethod)
Authentication is the process of allowing a user to log in to a Moodle site with a username and password. 

Authentication options are managed by plugins in Moodle. Some of these are integrated into Moodle Core, others can be installed manually.

| Value in Data Model      | Value in Moodle XML | Description                                                                                                                                         |
|--------------------------|---------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------|
| ManualCreation           | manual              | accounts are created manually by the administrator                                                                                                  |
| NoLogin                  | nologin             | suspends the user's account                                                                                                                         |
| EMailSelfRegistration    | email               | users can create their own accounts                                                                                                                 |
| CAS                      | cas                 | account details are located on an external CAS server                                                                                               |
| ExternalDatabase         | db                  | account details are located on an external database                                                                                                 |
| LDAP                     | ldap                | account details are located on an external LDAP server                                                                                              |
| LTI                      | lti                 | works with the Publish as LTI tool enrolment method to connect courses and activities                                                               |
| MNet                     | mnet                | Moodle network allows a Moodle administrator to establish a link with another Moodle site and to share some resources with the users of that Moodle |
| OAuth2                   | oauth2              | authenticate with an OAuth 2 service                                                                                                                |
| Shibboleth               | shibboleth          | account details are located on an external Shibboleth server                                                                                        |
| Webservice               | webservice          | for a dedicated account which integrates with your site via web services                                                                            |
| NoAuthentication         | none                | for testing purposes or if the Moodle site is not available on the Internet. Do NOT use on public servers!                                          |

!!! warning
    If an authentication method is selected for which the corresponding plugin is not installed, the user will not be able to log in  
    See [moodle docs](https://docs.moodle.org/405/en/Authentication) for more information

### Country
The country of the user. This field is optional.

| Value in Data Model | Value in Moodle XML | Description |
|---------------------|---------------------|-------------|
| Austria             | AT                  | Austria     |
| Germany             | DE                  | Germany     |
| Switzerland         | CH                  | Switzerland |

See [moodle source code](https://github.com/moodle/moodle/blob/main/lang/en/countries.php) for more information