.. _configuration:


Configuration
=============

The library provides several options that allow you to customize the authentication
features of your ASP.NET application. We will cover the major options in this
section, and more specific options in later sections of this guide.

There are a few ways you can set these configuration options:

* Environment variables
* Markup file (YAML or JSON)
* API Credentials file (``apiKey.properties``)
* Inline options (supplied to the middleware constructor)

If you would like a list of all available options, please refer to the
`Web Configuration Defaults`_.

Minimum Required Configuration
------------------------------

At the very least, these configuration options **must** be set in order for
the Stormpath middleware to initialize properly:

* ``stormpath.client.apiKey.id``
* ``stormpath.client.apiKey.secret``

Additionally, if you have more than one Application resource in your Stormpath tenant, either ``stormpath.application.name`` or ``stormpath.application.href`` must be set.

Any configuration option that is omitted will fall back to the `Web Configuration Defaults`_.

Environment Variables
---------------------

Configuration options can be set in environment variables by formatting the key name with underscores.
For example, ``stormpath.client.apiKey.id`` becomes ``STORMPATH_CLIENT_APIKEY_ID``.

.. tip::
  It is a best practice to store confidential information in environment
  variables, to avoid hard-coding it into your application. We recommend you store your
  confidential Stormpath information (API credentials and application URL) in this way.

In a bash-like shell, you can set environment variables by running these commands:

.. code-block:: bash

    export STORMPATH_CLIENT_APIKEY_ID=YOUR_ID_HERE
    export STORMPATH_CLIENT_APIKEY_SECRET=YOUR_SECRET_HERE
    export STORMPATH_APPLICATION_HREF=YOUR_APP_HREF

On Windows, the commands are:

.. code-block:: powershell

    setx STORMPATH_CLIENT_APIKEY_ID YOUR_ID_HERE
    setx STORMPATH_CLIENT_APIKEY_SECRET YOUR_SECRET_HERE
    setx STORMPATH_APPLICATION_HREF YOUR_APP_HREF

Any configuration option can be set using environment variables. The above are just examples!


Markup File (YAML or JSON)
--------------------------

Configuration options can also be set by placing a file called ``stormpath.yaml`` or ``stormpath.json`` in one of these locations:

* ``~/.stormpath`` (where ``~`` represents the user's home directory)
* The application's base directory

For example, this YAML configuration will disable the default ``/register`` and ``/login`` routes:

.. code-block:: yaml

    stormpath:
      web:
        register:
          enabled: false
        login:
          enabled: false


API Credentials File
--------------------------

The API Key ID (``stormpath.client.apiKey.id``) and API Key Secret (``stormpath.client.apiKey.secret``) can be set by placing the ``apiKey.properties`` file in one of these locations:

* ``~/.stormpath`` (where ``~`` represents the user's home directory)
* The application's base directory

If you don't opt to store the Stormpath API credentials in environment variables, this functionality makes it easy to download the ``apiKey.properties`` file from Stormpath and place it directly in one of these locations.


Inline Options
----------------

If you wish to define your configuration in code, you
can do so in the ``services.AddStormpath()`` call, like this:

.. literalinclude:: code/csharp/configuration/inline_config.cs
    :language: csharp

You can also use an anonymous object with the same (case-insensitive) names:

.. literalinclude:: code/csharp/configuration/anonymous_inline_config.cs
    :language: csharp

Both of these examples will use the Stormpath Application called "My Application" and disable the default ``/register`` route.

.. note::
  You'll need to add ``using Stormpath.Configuration.Abstractions;`` to your ``Startup.cs`` file in order to use the type-safe ``StormpathConfiguration`` model.

Like all the other methods of supplying configuration, any omitted options will fall back to the `Web Configuration Defaults`_.

.. tip::
  The most flexible way of providing configuration in a production environment is with YAML/JSON markup or environment variables. Inline options are useful during development.

Default Features
----------------

When you add Stormpath to your application using ``app.UseStormpath()``,
the library will automatically add the following routes to your application:

+--------------+-------------------------------------------------------------+---------------------------+
| URI          | Purpose                                                     | Documentation             |
+==============+=============================================================+===========================+
| /forgot      | Request a password reset link.                              | :ref:`password_reset`     |
+--------------+-------------------------------------------------------------+---------------------------+
| /change      | Reset your password (second step)                           | :ref:`password_reset`     |
+--------------+-------------------------------------------------------------+---------------------------+
| /login       | Login to your application with username and password.       | :ref:`login`              |
+--------------+-------------------------------------------------------------+---------------------------+
| /logout      | Accepts a POST request, and destroys the login session.     | :ref:`logout`             |
+--------------+-------------------------------------------------------------+---------------------------+
| /me          | Returns a JSON representation of the current user.          | :ref:`me_api`             |
+--------------+-------------------------------------------------------------+---------------------------+
| /oauth/token | Issue OAuth2 access and refresh tokens.                     | :ref:`oauth2`             |
+--------------+-------------------------------------------------------------+---------------------------+
| /register    | Create an account within your application.                  | :ref:`registration`       |
+--------------+-------------------------------------------------------------+---------------------------+
| /reset       | Reset an account password, from a password reset link.      | :ref:`password_reset`     |
+--------------+-------------------------------------------------------------+---------------------------+
| /verify      | Verify a new account, from a email verification link.       | :ref:`email_verification` |
+--------------+-------------------------------------------------------------+---------------------------+

Each feature has its own configurable options, which are explained in the feature's documentation. If you want to disable specific features, continue to the next section.


Disabling Features
------------------

The library enables many features by default, but you might not want to use all of them.
For example, if you wanted to disable all the default features, you could use
this configuration (or the equivalent YAML/JSON or environment variables):

.. literalinclude:: code/csharp/configuration/disable_default_features.cs
    :language: csharp

Disabling Content Types
-----------------------

By default, the routes provided by this library can handle requests from both browsers and clients such as Single Page Applications and mobile apps. Incoming requests are inspected for an Accept header of either ``text/html`` (browsers) or ``application/json`` (SPA/mobile).

It's possible to disable either of these modes by changing the values in ``stormpath.web.produces``. For example, if you want to build a pure API that will never send HTML responses, use this configuration:

.. literalinclude:: code/csharp/configuration/disable_html_produces.cs
    :language: csharp

Stormpath Client Options
------------------------

When you initialize this library, it creates an instance of a Stormpath Client.
The Stormpath client is responsible for communicating with the Stormpath REST
API and is provided by the `Stormpath .NET SDK`_.

Any options you set when initializing the Stormpath middleware library are also passed down to the SDK Client.

For example, to hardcode the Stormpath API credentials (not recommended!), you could use this configuration:

.. literalinclude:: code/csharp/configuration/api_credentials.cs
    :language: csharp

The Stormpath Client constructor ignores the ``stormpath.web`` node of the configuration. For more information about setting options on the SDK Client object, please see the
`.NET SDK documentation <https://docs.stormpath.com/dotnet/api/html/M_Stormpath_SDK_Client_IClientBuilder_SetConfiguration.htm>`_.


.. todo::
  Add logging when it's added: https://github.com/stormpath/stormpath-aspnetcore/issues/2

.. _Web Configuration Defaults: https://github.com/stormpath/stormpath-dotnet-config/blob/master/src/Stormpath.Configuration.Abstractions/Default.cs
.. _Stormpath .NET SDK: https://github.com/stormpath/stormpath-sdk-dotnet
