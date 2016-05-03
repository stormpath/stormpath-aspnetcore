.. _authentication:

Authentication
==============

This library offers several options for securing your application and
authenticating your users.  Which strategy should you use?  The answer depends
on your use case, and we'll discuss each one in detail.  But at a high level,
your choices look like this:

  * If you are building a traditional web app or single page application, you
    should use **Cookie Authentication**.

  * If you are building a mobile application, you should use the **OAuth2 Password Grant**.

.. todo::
  * If you are building an API service, you can use
    **HTTP Basic Authentication** or **OAuth2 Client Credentials**.


.. _cookie_authentication:

Cookie Authentication
---------------------

If you are building a web application that serves traditional HTML pages, or a
Single Page Application (such as Angular/React), this library will handle cookie authentication for you, out of the box!

To use cookie authentication, simply use the ``[Authorize]`` attribute on MVC or Web API routes:

.. literalinclude:: code/csharp/authentication/protected_route.cs
    :language: csharp

If the user is not logged in, they will be redirected to the built-in login route (``/login`` by default) to log in or register. After authenticating, they will be redirected back to the original route automatically.

Behind the scenes, the Stormpath middleware creates OAuth2 Access and Refresh tokens for the user (via the Stormpath API), and stores them in secure, HTTP-only cookies. After the user has logged in, these cookies will be supplied on every request. The Stormpath middleware will assert that the Access Token is valid.  If the Access Token is expired, it will attempt to refresh it with the Refresh Token.

.. note::
    By default, the cookie names are ``access_token`` and ``refresh_token``. See :ref:`configuring_cookie_flags` if you want to change the defaults.


.. _setting_token_expiration_time:

Setting Token Expiration Time
.............................

If you need to change the expiration time of the Access Token or Refresh Token,
please login to the `Stormpath Admin Console`_ and navigate to the OAuth Policy of
your Stormpath Application. You can configure the time-to-live (TTL) for both Access and Refresh Tokens.


.. _configuring_cookie_flags:

Configuring Cookie Flags
........................

This library creates two cookies, one for the Access Token and one for the
Refresh Token. The cookie details are configurable via code or markup (see the :ref:`Configuration` section).

The default cookie configuration (in YAML) is:

.. code-block:: yaml

  stormpath:
    web:
      accessTokenCookie:
        name: "access_token"
        domain: null
        httpOnly: true
        path: null
        secure: null

      refreshTokenCookie:
        name: "refresh_token"
        domain: null
        httpOnly: true
        path: null
        secure: null

The flags behave as follows:

+-------------+----------+----------------------------------------------------------+
| Cookie Flag | Default  | Description                                              |
+=============+==========+==========================================================+
| domain      | ``null`` | Set if needed, e.g. "subdomain.mydomain.com".            |
+-------------+----------+----------------------------------------------------------+
| httpOnly    | ``true`` | ``true`` by default, do not disable without good reason  |
|             |          | (exposes tokens to XSS  attacks).                        |
+-------------+----------+----------------------------------------------------------+
| path        | ``null`` | Set if needed, e.g. "/newapp". ``null`` equals ``/``.    |
+-------------+----------+----------------------------------------------------------+
| secure      | ``null`` | Will be ``true`` in HTTPS environments (as detected      |
|             |          | by the request URI), unless explicitly set to            |
|             |          | ``false`` (not recommended!).                            |
+-------------+----------+----------------------------------------------------------+


.. _token_validation_strategy:

Token Validation Strategy
.........................

When a request comes in to your server, the Stormpath middleware will use the Access Token
and Refresh Token cookies to make an authentication decision. The first step is to validate the Access Token to make sure it hasn't been tampered with.

There are two validation strategies: local validation (default) and Stormpath validation. Local validation does **not** make a network request to the Stormpath API, while Stormpath validation does make a network request and supports token revocation.

Both validation strategies follow the same pattern:

- If the Access Token is valid, accept the request.

- If the Access Token is expired or invalid, attempt to get a new one from the Stormpath API by using the Refresh Token.

- Deny the request if no new Access Token can be obtained.

With the ``local`` strategy, the middleware only checks the signature and expiration of
the Access Token to determine whether it is valid.  It does not check whether the token has been revoked.

If you want the ability to revoke Access Tokens, you'll need to update your configuration to opt-in to the ``stormpath`` validation strategy. This will make a network call to the Stormpath API on every incoming request. If the Access Token has been revoked, or the Stormpath Account has been disabled or deleted, the Access Token will not be considered valid.

The validation strategy can be changed via :ref:`Configuration`. The default configuration (in YAML) is:

.. code-block:: yaml

  stormpath:
    web:
      oauth2:
        password:
          validationStrategy: "local"

.. warning::

  When using local validation, your server will not be aware of token revocation
  or any changes to the associated Stormpath account.  **This is a security
  trade-off that optimizes for performance.**  If you prefer extra security, use
  the ``stormpath`` validation option.

  If you prefer local validation for the performance benefit, you can add a little more
  security by using a short expiration time for your Access Tokens (such as five minutes or
  less).  This will limit the amount of time that the Access Token can be used
  for validation.


.. todo::

  Issuing API Keys
  ----------------

  If you are building an API service, you will need to distribute API keys to your
  developers.  They will then use these keys to authenticate with your API, either
  via HTTP Basic Auth or OAuth2 Access tokens.  We'll cover those strategies in
  the next sections, but we need to provision API keys for your developers first.

  While your service may be an API service, you will still need to provide a
  basic website that developers can use to obtain their keys.  Here is an example
  of how you can create an API Key for the currently logged in user::

      app.post('/apiKeys', stormpath.loginRequired, function (req, res) {
        req.user.createApiKey(function (err, apiKey) {
          if (err) {
            res.status(400).end('Oops!  There was an error: ' + err.userMessage);
          }else{
            res.json(apiKey);
          }
        });
      });

  This is a naive example which simply prints out the API Keys for the user, but
  once they have the keys they will be able to authenticate with your API.

  For more information on API Keys, please see
  `Using Stormpath for API Authentication`_

.. todo::

  HTTP Basic Authentication
  -------------------------

  This strategy makes sense if you are building a simple API service that does
  not have complex needs around authorization and resource control.  This strategy
  is simple because the developer simply supplies their API keys on every request
  to your server.

  Once the developer has their API keys, they will use them to authenticate with your
  API.  For each request they will set the ``Authorization`` header, like this::

      Authorization: Basic <Base64UrlSafe(apiKeyId:apiKeySecret)>

  How this is done will depend on what tool or library they are using.  For example,
  if using curl:

  .. code-block:: sh

    curl -v --user apiKeyId:apiKeySecret http://localhost:3000/secret

  Or if you're using the ``request`` library:

  .. code-block:: javascript

    var request = require('request');

    request({
      url: 'http://localhost:3000/secret',
      auth: {
        user: 'apiKeyId',
        pass: 'apiKeySecret'
      }
    }, function (err, res){
      console.log(res.body);
    });

  You will need to tell your application that you want to secure this endpoint and
  allow basic authentication.  This is done with the ``apiAuthenticationRequired``
  middleware::

      app.get('/secret', stormpath.apiAuthenticationRequired, function (req, res) {
        res.json({
          message: "Hello, " + req.user.fullname
        });
      });


.. todo::

  OAuth2 Client Credentials
  -------------------------

  If you are building an API service and you have complex needs around
  authorization and security, this strategy should be used.  In this situation
  the developer does a one-time exchange of their API Keys for an Access Token.
  This Access Token is time limited and must be periodically refreshed.  This adds a
  layer of security, at the cost of being less simple than HTTP Basic
  Authentication.

  If you're not sure which strategy to use, it's best to start with HTTP Basic
  Authentication. You can always switch to OAuth2 at a later time.

  Once a developer has an API Key pair (see above, *Issuing API Keys*), they will
  need to use the OAuth2 Token Endpoint to obtain an Access Token.  In simple
  HTTP terms, that request looks like this::


      POST /oauth/token HTTP/1.1
      Host: myapi.com
      Content-Type: application/x-www-form-urlencoded
      Authorization: Basic <Base64UrlSafe(apiKeyId:apiKeySecret)>

      grant_type=client_credentials

  How you construct this request will depend on your library or tool, but the key
  parts you need to know are:

    * The request must be a POST request.
    * The content type must be form encoded, and the body must contain
      ``grant_type=client_credentials``.
    * The Authorization header must be Basic and contain the Base64 Url-Encoded
      values of the Api Key Pair.

  If you were doing this request with curl, it would look like this:

  .. code-block:: sh

    curl -X POST --user api_key_id:api_key_secret http://localhost:3000/oauth/token -d grant_type=client_credentials

  Or if using the ``request`` library:

  .. code-block:: javascript

    request({
      url: 'http://localhost:3000/oauth/token',
      method: 'POST',
      auth: {
        user: '1BWQHHJCOW90HI7HFQ5LTD6O0',
        pass: 'zzeu+NwmicjtJ9yDJ2KlRguC+8uTjKVm3AMs80ah6hw'
      },
      form: {
        'grant_type': 'client_credentials'
      }
    },function (err,res) {
      console.log(res.body);
    });

  If the credentials are valid, you will get an Access Token response that looks
  like this::

      {
        "access_token": "eyJ0eXAiOiJKV1QiL...",
        "token_type": "bearer",
        "expires_in": 3600
      }

  The response is a JSON object which contains:

  - ``access_token`` - Your OAuth Access Token.  This can be used to authenticate
    on future requests.
  - ``token_type`` - This will always be ``"bearer"``.
  - ``expires_in`` - This is the amount of seconds (*as an integer*) for which
    this token is valid.

  With this token you can now make requests to your API.  This request is simpler,
  as only thing you need to supply is ``Authorization`` header with the Access
  Token as a bearer token.  If you are using curl, that request looks like this:

  .. code-block:: sh

    curl -v -H "Authorization: Bearer eyJ0eXAiOiJKV1QiL..." http://localhost:3000/secret

  Or if using the ``request`` library:

  .. code-block:: javascript

    request({
      url: 'http://localhost:3000/secret',
      auth: {
        'bearer': 'eyJ0eXAiOiJKV1QiL...'
      }
    }, function (err, res){
      console.log(res.body);
    });

  In order to protect your API endpoint and allow this form of authenetication,
  you need to use the ``apiAuthenticationRequired`` middleware::

      app.get('/secret', stormpath.apiAuthenticationRequired, function (req, res) {
        res.json({
          message: "Hello, " + req.user.fullname
        });
      });

  By default the Access Tokens are valid for one hour.  If you want to change
  the expiration of these tokens you will need to configure it in the server
  configuration, like this::


      app.use(stormpath.init(app, {
        web: {
          oauth2: {
            client_credentials: {
              accessToken: {
                ttl: 3600 // your custom TTL, in seconds, goes here
              }
            }
          }
        }
      }));


OAuth2 Password Grant
---------------------

This is the authentication strategy that you will want to use for mobile clients.

In this situation the end-user supplies their username and password to your
mobile application.  The mobile application sends that username and password to
your ASP.NET server, which then verifies the password with Stormpath.

If the account is valid and the password is correct, Stormpath will generate
an Access and Refresh Token for the user.  Your server gets these tokens from Stormpath
and then sends them down to your mobile application.

The mobile application then stores the tokens in a secure location, and
uses them for future requests to your ASP.NET Web API application.

When a user wants to login to your mobile application, the mobile application
should make this request to your ASP.NET Web API application::

    POST /oauth/token HTTP/1.1
    Host: myapi.com
    Content-Type: application/x-www-form-urlencoded

    grant_type=password
    &username=user@example.com
    &password=theirPassword

If the authentication is successful, your server will return a token response to your mobile application.  The response will look like this::

    {
      "refresh_token": "eyJraWQiOiI2...",
      "stormpath_access_token_href": "https://api.stormpath.com/v1/accessTokens/3bBAHmSuTJ64DM574awVen",
      "token_type": "Bearer",
      "access_token": "eyJraWQiOiI2Nl...",
      "expires_in": 3600
    }

Your mobile application should store the tokens in a secure location.

.. note::
  By default the Access Token is valid for 1 hour and the Refresh Token is valid for 60 days. You can configure this in the Stormpath Admin Console; see :ref:`setting_token_expiration_time`.

Each subsequent request the mobile application makes to your ASP.NET Web API should include the Access Token as a ``Bearer`` header::

    GET /profile HTTP/1.1
    Host: myapi.com
    Authorization: Bearer eyJraWQiOiI2Nl...
    ...

When the Access Token expires, you can use the Refresh Token to obtain a new Access Token::

    POST /oauth/token HTTP/1.1
    Host: myapi.com
    Content-Type: application/x-www-form-urlencoded

    grant_type=refresh_token
    &refresh_token=eyJraWQiOiI2...

The response will contain a new Access Token.  Once the Refresh Token expires,
the user will have to re-authenticate with a username and password.

For full documentation on our OAuth2 Access Token features, please see
`Using Stormpath for OAuth 2.0 and Access/Refresh Token Management`_

.. _Stormpath Admin Console: https://api.stormpath.com/login
.. _Using Stormpath for API Authentication: https://docs.stormpath.com/guides/api-key-management/
.. _Using Stormpath for OAuth 2.0 and Access/Refresh Token Management: http://docs.stormpath.com/guides/token-management/
