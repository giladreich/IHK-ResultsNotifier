# IHK Results Notifier

A simple tool to notify you whenever the results on the official website of the IHK exams has been updated.
When the new results are available, the tool will make a sound and will bring it on top of all windows so you'll notice the new results.


### Login Interface

![Login Interface](/pictures/login_window.png)
![Login Interface](/pictures/login_window_typing.png)

### Main Window Interface

#### No Results Example:

![Main Window Interface1](/pictures/main_window1.png)

#### Listening for Results Example:

![Main Window Interface2](/pictures/main_window2.png)

#### Found Results Example:

![Main Window Interface3](/pictures/main_window3.png)





# The Cookies Trick For Authenticating A User

## Steps To Reproduce 

#### Description:

Before posting the login form, we need 2 cookies that the login pages automatically creates for us.
If the credentials we send to the server as a POST request are correct or accepted by the server, the server will identify us and respond with a new session key
that will replace the session key that we got from the `BB_auszubildende.jsp` page.
With the new cookie/session-key + the session-key from the `favicon.ico`, we can access user-specific/private pages.

The following steps will show how it works using curl:

```sh
// We send the first request to get the first cookie:
curl -I "https://apps.ihk-berlin.de/tibrosBB/BB_auszubildende.jsp"

// We send another request to get the second cookie from server:
curl -I "https://apps.ihk-berlin.de/favicon.ico"

// Send a post request with the session keys we get from step 1 and 2(where replaceme delete and add your new session key):
curl -v -X POST "https://apps.ihk-berlin.de/tibrosBB/azubiHome.jsp" -H "Cookie: TSESSIONID=replacemeSession1; TSESSIONID=replacemeSession2" -d "login=replacemeUser&pass=replacemePass&anmelden="

// We must first visit this website before jumping to step 5 link, as it will kick our session out if we do:
curl "https://apps.ihk-berlin.de/tibrosBB/azubiPruef.jsp" -H "Cookie: TSESSIONID=replacemeSession3; TSESSIONID=replacemeSession2"

// IMPORTANT NOTE: Jumping directly to this link won't work, you must first visit the link before with the same session keys.
// Get the last link with the exam results:
curl "https://apps.ihk-berlin.de/tibrosBB/azubiErgebnisse.jsp?id=1" -H "Cookie: TSESSIONID=replacemeSession3; TSESSIONID=replacemeSession2"
```

Note that this can be easily done by creating a cookie jar: `curl --cookie-jar` or `curl -c`, but to have a better understanding I show the requests commands the manual way.

