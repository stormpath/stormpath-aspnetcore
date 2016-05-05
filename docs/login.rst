.. _login:


Login
=====

The login feature of this library allows you to accept login attempts from any active Stormpath account in the Stormpath directory linked to your application. If the attempt is successful, cookies are created automatically for the user (see :ref:`cookie_authentication`).

.. todo::
  See social_login if you want to accept logins from providers such as Facebook or Google.

By default, this library will serve an HTML login page at ``/login``.  You can change this URI, or disable the feature entirely if you wish.


Configuration Options
---------------------

This feature supports several options that you can configure using code or markup (see the :ref:`Configuration` section):

* **enabled**: Whether the feature is enabled. (Default: ``true``)
* **uri**: The path for this feature. (Default: ``/login``)
* **nextUri**: The location to send the user after logging in. (Default: ``/``)
* **view**: The view to render; see :ref:`templates`. (Default: ``login``)
* **form**: The fields that will be displayed on the form; see :ref:`login_customizing_form`.

.. note::
  Any unchanged options will retain their default values. See the :ref:`login_default_configuration` section to view the defaults.

Configuration Example
.....................

You could, for example, change the endpoint path by setting this configuration (shown as YAML):

.. code-block:: yaml

  stormpath:
    web:
      login:
        uri: "/logMeIn"

You could also set this configuration via code:

.. only:: aspnetcore

  .. literalinclude:: code/login/aspnetcore/configure_uri.cs
    :language: csharp

.. only:: aspnet

  .. todo::
    Add code

.. only:: nancy

  .. todo::
    Add code


See the :ref:`configuration` section for more details on how configuration works, or :ref:`login_default_configuration` to see the default values.


.. _login_customizing_form:

Customizing the Form
--------------------

The login form will render these required fields by default:

* Username or Email
* Password

You can change the label and placeholder text that is displayed by changing the configuration, as shown in YAML below:

.. code-block:: yaml

  stormpath:
    web:
      login:
        form:
          fields:
            login:
              label: "Email"
              placeholder: "you@yourdomain.com"
            password:
              placeholder: "Tip: Use a strong password!"

Or, through code:

.. only:: aspnetcore

  .. literalinclude:: code/login/aspnetcore/configure_labels.cs
    :language: csharp

.. only:: aspnet

  .. todo::
    Add code

.. only:: nancy

  .. todo::
    Add code


Next URI
--------

If the login attempt is successful, the user will be redirected to ``/`` by default. If you want to change this, set the ``nextUri`` option:

.. code-block:: yaml

  stormpath:
    web:
      login:
        nextUri: "/dashboard"


.. todo::
  .. _pre_login_handler:

  Pre Login Handler
  .. -----------------

  Want to validate or modify the form data before it's handled by us? Then this is
  the handler that you want to use!

  To use a ``preLoginHandler`` you need to define your handler function in the
  Stormpath config::

      app.use(stormpath.init(app, {
        preLoginHandler: function (formData, req, res, next) {
          console.log('Got login request', formData);
          next();
        }
      }));

  As you can see in the example above, the ``preLoginHandler`` function
  takes in four parameters:

  - ``formData``: The data submitted in the form.
  - ``req``: The Express request object.  This can be used to modify the incoming
    request directly.
  - ``res``: The Express response object.  This can be used to modify the HTTP
    response directly.
  - ``next``: The callback to call after you have done your custom work.  If you
    call this with an error then we immediately return this error to the user and
    form processing stops.  But if you call it without an error, then our library
    will continue to process the form and respond with the default behavior.

  In the example below, we'll use the ``preLoginHandler`` to validate that
  the user doesn't enter an email domain that is restricted::

      app.use(stormpath.init(app, {
        preLoginHandler: function (formData, req, res, next) {
          if (formData.login.indexOf('@some-domain.com') !== -1) {
            return next(new Error('You\'re not allowed to login with \'@some-domain.com\'.'));
          }

          next();
        }
      }));

  .. _post_login_handler:

  Post Login Handler
  .. ------------------

  Want to run some custom code after a user logs into your site?  By defining a ``postLoginHandler`` you're able achieve tasks like:

  - Refresh a user's third-party services.
  - Calculate the last login time of a user.
  - Prompt a user to complete their profile, or setup billing.
  - etc.

  To use a ``postLoginHandler``, you need to define your handler function
  in the Stormpath config::

      app.use(stormpath.init(app, {
        postLoginHandler: function (account, req, res, next) {
          console.log('User:', account.email, 'just logged in!');
          next();
        }
      }));

  As you can see in the example above, the ``postLoginHandler`` function
  takes in four parameters:

  - ``account``: The new, successfully logged in, user account.
  - ``req``: The Express request object.  This can be used to modify the incoming
    request directly.
  - ``res``: The Express response object.  This can be used to modify the HTTP
    response directly.
  - ``next``: The callback to call when you're done doing whatever it is you want
    to do.  If you call this, execution will continue on normally.  If you don't
    call this, you're responsible for handling the response.

  In the example below, we'll use the ``postLoginHandler`` to redirect the
  user to a special page (*instead of the normal login flow*)::

      app.use(stormpath.init(app, {
        postLoginHandler: function (account, req, res, next) {
          res.redirect(302, '/secretpage').end();
        }
      }));


.. _json_login_api:

Mobile/JSON API
---------------

If you are using this library from a mobile application, or a client framework like Angular or React, you'll interact with this endpoint via GET and POST requests.


Making a Login Attempt
......................

Simply POST to the ``/login`` endpoint with the user's credentials:

.. code-block:: http

    POST /login
    Accept: application/json
    Content-Type: application/json

    {
      "login": "foo@bar.com",
      "password": "myPassword"
    }

If the login attempt is successful, you will receive a 200 OK response and the
session cookies will be set on the response. (See :ref:`cookie_authentication`)

If an error occurs, you'll get an error response that looks like this:

.. code-block:: json

  {
    "status": 400,
    "message": "Invalid username or password."
  }


Getting the Form View Model
...........................

By making a GET request to the login endpoint with the ``Accept:
application/json`` set, you can retreive a JSON view model that describes the login
form and the social account stores that are mapped to your Stormpath
Application.

Here's an example view model that shows you an application that has the default login form, and a mapped Google directory:

.. code-block:: javascript

  {
    "accountStores": [
      {
        "name": "express-stormpath google",
        "href": "https://api.stormpath.com/v1/directories/gc0Ty90yXXk8ifd2QPwt",
        "provider": {
          "providerId": "google",
          "href": "https://api.stormpath.com/v1/directories/gc0Ty90yXXk8ifd2QPwt/provider",
          "clientId": "422132428-9auxxujR9uku8I5au.apps.googleusercontent.com",
          "scope": "email profile"
        }
      }
    ],
    "form": {
      "fields": [
        {
          "label": "Username or Email",
          "placeholder": "Username or Email",
          "required": true,
          "type": "text",
          "name": "login"
        },
        {
          "label": "Password",
          "placeholder": "Password",
          "required": true,
          "type": "password",
          "name": "password"
        }
      ]
    }
  }

.. note::

  You may have to explicitly tell your client library that you want a JSON
  response from the server. Not all libraries do this automatically. If the
  library does not set the ``Accept: application/json`` header on the request,
  you'll get back the HTML login form instead of the JSON response that you
  expect!


.. _login_default_configuration:

Default Configuration
---------------------

Options that are not overridden by explicit configuration (see :ref:`configuration`) will retain their default values.

For reference, the full default configuration for this route is shown as YAML below:

.. code-block:: yaml

  stormpath:
    web:
      login:
        enabled: true
        uri: "/login"
        nextUri: "/"
        view: "login"
        form:
          fields:
            login:
              enabled: true
              visible: true
              label: "Username or Email"
              placeholder: "Username or Email"
              required: true
              type: "text"
            password:
              enabled: true
              visible: true
              label: "Password"
              placeholder: "Password"
              required: true
              type: "password"
          fieldOrder:
            - "login"
            - "password"

.. tip::
  You can also refer to the `Example Stormpath configuration`_ to see the entire default library configuration.

.. _Stormpath Admin Console: https://api.stormpath.com
.. _Example Stormpath configuration: https://github.com/stormpath/stormpath-framework-spec/blob/master/example-config.yaml
