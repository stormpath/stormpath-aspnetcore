.. _oauth2:

OAuth 2.0 API
=============

If you are building a mobile application, and don't want to (or can't) use cookie-based sessions, you'll need to generate access tokens using the OAuth 2.0 API provided by this library.

By default, this library will provide a POST route at ``/oauth/token``. Simply make a POST request to this URI with the user's credentials to generate tokens for the user. You can change this URI, or disable the feature entirely if you wish.

This topic describes how to configure the OAuth 2.0 API endpoint. For details on how to use it, see the :ref:`authentication` section.


Configuration Options
---------------------

This feature supports several options that you can configure using code or markup (see the :ref:`Configuration` section):

* **enabled**: Whether the feature is enabled. (Default: ``true``)
* **uri**: The path for this feature. (Default: ``/oauth/token``)

Additionally, there are specific options for each supported OAuth 2.0 grant type:

Client Credentials Grant Options
................................

.. todo::
  Link to the client_credentials docs over on authentication.

* **enabled**: Whether the Client Credentials Grant flow is enabled. (Default: ``true``)
* **accessToken.ttl**: The time-to-live (in seconds) of the generated access token. (Default: 3600)

Password Grant Options
......................

See the :ref:`oauth2_password_grant` topic in the Authentication section for a detailed description of how the Password Grant flow works.

* **enabled**: Whether the Password Grant flow is enabled. (Default: ``true``)
* **validationStrategy**: Whether to validate the token locally or via the Stormpath API. (Default: local, see :ref:`token_validation_strategy`)

.. note::
  Any unchanged options will retain their default values. See the :ref:`oauth2_default_configuration` section to view the defaults.

Configuration Example
.....................

To change the token endpoint URI and use remote (Stormpath API) validation of the access token, use this configuration (shown in YAML):

.. code-block:: yaml

  stormpath:
    web:
      oauth2:
        uri: "/api/token"
        password:
          validationStrategy: "stormpath"

.. tip::
  It's also possible to set this configuration via code. See the :ref:`configuration` section.


.. _oauth2_default_configuration:

Default Configuration
---------------------

Options that are not overridden by explicit configuration (see :ref:`configuration`) will retain their default values.

For reference, the full default configuration for this route is shown as YAML below:

.. code-block:: yaml

  stormpath:
    web:
      oauth2:
        enabled: true
        uri: "/oauth/token"
        client_credentials:
          enabled: true
          accessToken:
            ttl: 3600
        password:
          enabled: true
          validationStrategy: "local"
