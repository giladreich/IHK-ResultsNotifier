# IHK Results Notifier

A simple tool to notify you whenever the results on the official website of the IHK exams has been updated.
When the new results are available, the tool will make a sound and will bring it on top of all windows so you'll notice the new results.


## The Cookies Trick For Authenticating a User

In order to get the idea, I'll be using curl to demonstrate the authentication process. <br/><br/>

When we visit the login page(`BB_auszubildende.jsp`), the server creates for us 2 cookies that will be used later in order to check that we are not some kind of a robot and that we actually visited the website. <br/>
So before we posting the login form, we need to simulate a website visit to the following pages: <br/>
```sh
curl -I "https://apps.ihk-berlin.de/tibrosBB/BB_auszubildende.jsp"

curl -I "https://apps.ihk-berlin.de/favicon.ico"
```

At this point, the server has sent us 2 cookies that we'll to include in the header of the POST request when we posting the login form(Note that the order of the cookies is important):
```sh
// Where REPLACEME_*, add the appropriate information:
curl -v -X POST "https://apps.ihk-berlin.de/tibrosBB/azubiHome.jsp" -H "Cookie: TSESSIONID=REPLACEME_SESSION1; TSESSIONID=REPLACEME_SESSION2" -d "login=REPLACEME_USER&pass=REPLACEME_PASS&anmelden="
```

NOTE: If the server has accepted our POST request and successfully identified us, it will send us back a new cookie as response, otherwise, we get no cookie back. <br/>
We'll treat the new cookie from the response as `REPLACEME_SESSION3`(you'll see later why). <br/><br/>

Now that we're logged in, the next step is to try and access some of the user's pages that requires authentication and see if we get OK from the server.

For our purpose, we need to retrieve the exam results page, which is `azubiErgebnisse.jsp?id=1`, but here it's get a little bit tricky, as if we try directly visiting 
the results page, without first visiting the `azubiErgebnisse.jsp`, then the server will destroy our session and log us out automatically.
The way to access the results page, we use the following commands:
```sh
// Simulate a visit in order to let the server confirm that we visited this page before we go any further:
curl "https://apps.ihk-berlin.de/tibrosBB/azubiPruef.jsp" -H "Cookie: TSESSIONID=REPLACEME_SESSION3; TSESSIONID=REPLACEME_SESSION2"

// Now we are safe to visit here and get what we wanted:
curl "https://apps.ihk-berlin.de/tibrosBB/azubiErgebnisse.jsp?id=1" -H "Cookie: TSESSIONID=REPLACEME_SESSION3; TSESSIONID=REPLACEME_SESSION2"
```

Note that now every request we send, we always including the 2 cookies in the request-header for any type of request, because that's how the server identifies our session and the user that is currently authenticated.<br/>

Also note that this can be easily done by creating a cookie jar: `curl --cookie-jar` or `curl -c`, but to have a better understanding I show the request commands the manual way.

In C# we simply bind a `CookieContainer` to an `HttpClientHandler` that will be used in `HttpClient` and it saves us the trouble :)



## Some Pictures

### Login Interface

![Login Interface](/pictures/login_window.png)
![Login Interface](/pictures/login_window_typing.png)

### Main Window Interface

![Main Window Interface1](/pictures/main_window1.png)

#### Listening for Results:

![Main Window Interface2](/pictures/main_window2.png)

#### Found Results:

![Main Window Interface3](/pictures/main_window3.png)