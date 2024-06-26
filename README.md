LinkedinAutomation.ProfilePictureLoader

Application for linkedin profile picture loading.

## Prerequisites

List of any prerequisites or requirements needed before installing or using the project.

- .NET
- Ngrok (to provide localhost as a public IP address for 3-legged OAuth redirection)

## Setup

Step-by-step instructions on how to setup the project.

1. Clone project
2. Configure Your Application in Linkedin (More info: https://learn.microsoft.com/en-us/linkedin/shared/authentication/authorization-code-flow?context=linkedin%2Fcontext&tabs=HTTPS1)
    - Create and select your application in the LinkedIn Developer Portal
    - Add sign in using OpenId product ![image](https://github.com/9novikoff/LinkedinAutomation.ProfilePictureLoader/assets/93492723/8aa6a5e0-23f0-4730-a9d2-ae976ee11675)
3. Using ngrok expose localhost port used in api (8080 by default) 
4. Add this url to redirect url's in Linkedin
5. Fill empty string values in appsettings.json (except AccessToken - it will be set automatically)
