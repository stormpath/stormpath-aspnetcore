.. _password_reset:

Password Reset
==============

Stormpath provides a self-service password reset flow for your users, allowing them to request a link that resets their password. This library adds this functionality to your |framework| application automatically if it is enabled on your Stormpath Directory.

By default, this library will serve a Forgot Password form at ``/forgot`` and a callback handler at ``/change``, *if* the Password Reset Email workflow is enabled in Stormpath. You can change these URIs, or disable the feature entirely if you wish (see :ref:`password_reset_configuration`).


Enabling the Workflow
---------------------

To use the password reset workflow, you need to enable it on the Stormpath Directory
that your application is using. Follow these steps:

1. Use the `Stormpath Admin Console`_ to find the Stormpath Directory linked to your Application
2. Navigate to the Workflows section and enable the **Password Reset Email** workflow
3. Modify the **Link Base URL** to point to your application:

.. code-block:: sh

    http://yourapplication.com/change

.. note::
  The ``/change`` route is automatically provided by this library. If you want to change the route URI, see :ref:`password_reset_configuration`.


How it Works
------------

After enabling the workflow in Stormpath, restart your |framework| application.  You can now complete a self-service password reset:

* The login form at ``/login`` will show a "Forgot Password?" link.
* Clicking that link will take you to ``/forgot``, where you can enter your email address.
* After you receive the email, clicking on the link will take you to ``/change``.
* You'll see a form that allows you to enter a new password.
* After changing your password, you are taken to the login form.


.. _password_reset_configuration:

Configuration Options
---------------------

There are several options you can configure using code or markup (see the :ref:`Configuration` section). These options are split between the two steps of the workflow.

.. note::
  The **enabled** properties for these routes can be set to ``true``, ``false``, or ``null``. A value of ``null`` means that the route will automatically be enabled if the default Account Store for the current Stormpath application has the Password Reset Email workflow enabled.

Forgot Route
............

The first step in the password reset flow displays a form where the user can enter their email address. The configuration options for this route are in the section ``stormpath.web.forgotPassword``:

* **enabled**: Whether the feature is enabled. (Default: ``null``, see note above)
* **uri**: The path for this feature. (Default: ``/forgot``)
* **nextUri**: Where to send the user after they request a reset email. (Default: ``/login?status=forgot``)
* **view**: The view to render; see :ref:`templates`. (Default: ``forgot-password``)

Change Route
............

The second step in the password reset flow displays a form where the user can enter their new password, given a valid link. The configuration options for this route are in the section ``stormpath.web.changePassword``:

* **enabled**: Whether the feature is enabled. (Default: ``null``, see below)
* **autoLogin**: Whether the user is automatically logged in after changing their password. (Default: ``false``)
* **uri**: The path for this feature. (Default: ``/change``)
* **nextUri**: Where to send the user after changing their password. (Default: ``/login?status=reset``)
* **view**: The view to render; see :ref:`templates`. (Default: ``change-password``)
* **errorUri**: Where to send the user if the link is invalid. (Default: ``/forgot?status=invalid_sptoken``)

.. todo::
  Update autoLogin stuff.

Configuration Example
.....................

You could, for example, change the route paths for both endpoints by setting this configuration (shown as YAML):

.. code-block:: yaml

  stormpath:
    web:
      forgotPassword:
        uri: "/forgot-password"
      changePassword:
        uri: "/change-password"

.. tip::
  It's also possible to set this configuration via code. See the :ref:`configuration` section.

.. note::
  Any unchanged options will retain their default values. See the :ref:`password_reset_default_configuration` section to view the defaults.


Auto Login
----------

Our library implements the most secure workflow by default: the user must
request a password reset link, then login again after changing their password.

We recommend these settings for security purposes, but if you wish to automatically
log the user in after they reset their password, you can set this configuration:

.. code-block:: yaml

  stormpath:
    web:
      changePassword:
        autoLogin: true

.. todo::
  Update to new autoLogin spec.


Mobile/JSON API
---------------

If you are using this library from a mobile application, or a client framework like Angular or React, you'll interact with this endpoint via GET and POST requests.

To start the password reset flow, send a POST request to ``/forgot``:

.. code-block:: http

  POST /forgot
  Accept: application/json
  Content-Type: application/json

  {
    "email": "foo@bar.com"
  }

The ``/forgot`` endpoint will always respond with ``200 OK``, regardless of whether the email address is valid.

If you have a valid ``sptoken``, you can finish the password reset flow by sending a POST to ``/change`` with the user's new password:

.. code-block:: http

  POST /change
  Accept: application/json
  Content-Type: application/json

  {
    "sptoken": "the sent token",
    "password": "new password"
  }


.. _password_reset_default_configuration:

Default Configuration
---------------------

Options that are not overridden by explicit configuration (see :ref:`configuration`) will retain their default values.

For reference, the full default configuration for these routes is shown as YAML below:

.. code-block:: yaml

  stormpath:
    web:
      forgotPassword:
        enabled: null
        uri: "/forgot"
        view: "forgot-password"
        nextUri: "/login?status=forgot"

      changePassword:
        enabled: null
        autoLogin: false
        uri: "/change"
        nextUri: "/login?status=reset"
        view: "change-password"
        errorUri: "/forgot?status=invalid_sptoken"

.. todo::
  Update to new autoLogin spec.

.. tip::
  You can also refer to the `Example Stormpath configuration`_ to see the entire default library configuration.


.. _Example Stormpath configuration: https://github.com/stormpath/stormpath-framework-spec/blob/master/example-config.yaml
.. _Stormpath Admin Console: https://api.stormpath.com
.. _pre-built view templates: https://github.com/stormpath/stormpath-dotnet-owin-middleware/tree/master/src/Stormpath.Owin.Views
