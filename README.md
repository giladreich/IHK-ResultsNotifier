# IHK Results Notifier

Simple tool to notify the user whenever the results of the IHK exams has been update on official website.


### Login Interface

![Login Interface](/pictures/login_window.png)
![Login Interface](/pictures/login_window_typing.png)

### Main Window Interface

![Main Window Interface](/pictures/main_window.png)









# The Cookies Trick For Authenticating User

## Steps to reproduce 

#### Description:

Before posting the login form, we need 2 cookies that the login pages automatically creates us.
If the credentials we send as post request to the server are correct, the server will respond with a new session key
that will replace the session key we got from the BB_auszubildende.jsp.
The server will identify that we are logged in with new session key that we got as response from the login and from the favicon.ico.
The following steps shows how it works:


// We send the first request to get the first cookie:
```sh
curl -I "https://apps.ihk-berlin.de/tibrosBB/BB_auszubildende.jsp"
```

// We send another request to get the second cookie from server:
```sh
curl -I "https://apps.ihk-berlin.de/favicon.ico"
```

// Send a post request with the session keys we get from step 1 and 2(where replaceme delete and add your new session key):
```sh
curl -v -X POST "https://apps.ihk-berlin.de/tibrosBB/azubiHome.jsp" -H "Cookie: TSESSIONID=replacemeSession1; TSESSIONID=replacemeSession2" -d "login=replacemeUser&pass=replacemePass&anmelden="
```

// We must first visit this website before jumping to step 5 link, as it will kick our session out if we do:
```sh
curl "https://apps.ihk-berlin.de/tibrosBB/azubiPruef.jsp" -H "Cookie: TSESSIONID=replacemeSession3; TSESSIONID=replacemeSession2"
```

// Get the last link with the exam results:
// NOTE: Jumping directly to this link won't work, you must first visit the link before with the same session keys.
```sh
curl "https://apps.ihk-berlin.de/tibrosBB/azubiErgebnisse.jsp?id=1" -H "Cookie: TSESSIONID=replacemeSession3; TSESSIONID=replacemeSession2"
```

#### Full requests detailed

// Gets the first cookie
```sh
curl -I "https://apps.ihk-berlin.de/tibrosBB/BB_auszubildende.jsp" -H "Connection: keep-alive" -H "Upgrade-Insecure-Requests: 1" -H "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36" -H "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8" -H "Accept-Encoding: gzip, deflate, br" -H "Accept-Language: en,en-US;q=0.9,de-DE;q=0.8,de;q=0.7,he;q=0.6" 
```

// Gets the second cookie
```sh
curl -I "https://apps.ihk-berlin.de/favicon.ico" -H "Accept: image/webp,image/apng,image/*,*/*;q=0.8" -H "Connection: keep-alive" -H "Accept-Encoding: gzip, deflate, br" -H "Referer: https://apps.ihk-berlin.de/tibrosBB/BB_auszubildende.jsp" -H "Accept-Language: en,en-US;q=0.9,de-DE;q=0.8,de;q=0.7,he;q=0.6" -H "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36" 
```

// Posts the login data with the 2 cookies created from before:
```sh
curl -v -X POST "https://apps.ihk-berlin.de/tibrosBB/azubiHome.jsp" -H "Connection: keep-alive" -H "Cache-Control: max-age=0" -H "Origin: https://apps.ihk-berlin.de" -H "Upgrade-Insecure-Requests: 1" -H "Content-Type: application/x-www-form-urlencoded" -H "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36" -H "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8" -H "Referer: https://apps.ihk-berlin.de/tibrosBB/BB_auszubildende.jsp" -H "Accept-Encoding: gzip, deflate, br" -H "Accept-Language: en,en-US;q=0.9,de-DE;q=0.8,de;q=0.7,he;q=0.6" -H "Cookie: TSESSIONID=replacemeSession1; TSESSIONID=replacemeSession2" --data "login=replacemeUser&pass=replacemePass&anmelden="
```

// Using the login authentication:
```sh
curl -v "https://apps.ihk-berlin.de/tibrosBB/azubiPruef.jsp" -H "Connection: keep-alive" -H "Upgrade-Insecure-Requests: 1" -H "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36" -H "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8" -H "Referer: https://apps.ihk-berlin.de/tibrosBB/azubiHome.jsp" -H "Accept-Encoding: gzip, deflate, br" -H "Accept-Language: en,en-US;q=0.9,de-DE;q=0.8,de;q=0.7,he;q=0.6" -H "Cookie: TSESSIONID=replacemeSession3; TSESSIONID=replacemeSession2"
```

// Results website
```sh
curl -v "https://apps.ihk-berlin.de/tibrosBB/azubiErgebnisse.jsp?id=1" -H "Connection: keep-alive" -H "Upgrade-Insecure-Requests: 1" -H "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36" -H "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8" -H "Referer: https://apps.ihk-berlin.de/tibrosBB/azubiPruef.jsp" -H "Accept-Encoding: gzip, deflate, br" -H "Accept-Language: en,en-US;q=0.9,de-DE;q=0.8,de;q=0.7,he;q=0.6" -H "Cookie: TSESSIONID=replacemeSession3; TSESSIONID=replacemeSession2"
```



