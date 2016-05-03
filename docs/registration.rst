.. _registration:


Registration
============

The registration feature of this library allows you to use Stormpath to create
new accounts in a Stormpath directory.  You can create traditional password-based accounts, or gather account data from other providers such as Facebook and Google.

By default this library will serve an HTML registration page at ``/register``.
You can change this URI, or disable the feature entirely if you wish.


Configuration Options
---------------------

This feature supports several options that you can configure using code or markup (see the :ref:`Configuration` section):

* **enabled**: Whether the feature is enabled. (Default: ``true``)
* **uri**: The path for this feature. (Default: ``/register``)
* **autoLogin**: Whether the user should be logged in after registering. (Default: ``false``)
* **nextUri**: The location to send the user after registering, if **autoLogin** is on. (Default: ``/``)
* **view**: The view to render for HTML requests; see :ref:`register_custom_view`. (Default: ``register``)
* **form**: The fields that will be displayed on the form; see :ref:`register_customizing_form`.

.. note::
  Any unchanged options will retain their default values. See the :ref:`register_default_configuration` section to view the defaults.

.. _register_customizing_form:

Customizing the Form
--------------------

The registration form will render these fields by default, and they will be required:

* First Name (given name)
* Last Name (surname)
* Email
* Password

You can customize the form by simply changing the configuration. For example, while email and password will always be required, you could make first and last name optional. Or, you can ask the user for both an email address and a username. You can also specify your own custom fields!

Each field item in ``web.register.form.fields`` has these configurable properties:

* **enabled**: Whether the field exists on the form is and accepted in POST requests.
* **visible**: Whether the field is visible on the form.
* **required**: Whether an error will be shown if the field is missing.
* **label**: The label text rendered for the control.
* **placeholder**: The placeholder text in the control.
* **type**: The HTML input type of the control.


Making Fields Optional
......................

If you would like to show a field, but not require it, set the ``required`` property to ``false``, as shown in YAML below:

.. code-block:: yaml

  stormpath:
    web:
      register:
        form:
          fields:
            givenName:
              required: false
            surname:
              required: false

.. tip::
  It's also possible to set this configuration via code. See the :ref:`configuration` section.

If you would like to remove a field from the form entirely, set the ``visible`` or ``enabled`` properties to ``false``. The behavior is slightly different:

* If a field is ``visible = false`` but is still enabled, it will not be rendered on the form, but POST requests can still supply data for the field.
* If a field is ``enabled = false``, POST requests containing the field will return an error.

.. note::
  Because the Stormpath API requires a first and last name, the library will auto-fill these fields with ``UNKNOWN`` if you make them optional and the user does not provide them.


.. _register_custom_fields:

Adding Custom Fields
....................

You can add your own custom fields to the form.  The values will be
automatically added to the user's `Custom Data`_ object when they register
successfully.  You can create a custom field by defining a new field configuration:

.. code-block:: yaml

  stormpath:
    web:
      register:
        form:
          fields:
            // Other fields
            // ...
            favoriteColor:
              enabled: true
              label: "Favorite Color"
              name: "favoriteColor"
              placeholder: "e.g. red, blue"
              required: true
              type: "text"


Changing Field Order
....................

If you want to change the order of the fields, you can do so by specifying the
``fieldOrder`` array:

.. code-block:: yaml

  stormpath:
    web:
      register:
        form:
          fieldOrder:
            - "surname"
            - "givenName"
            - "email"
            - "password"

Any visible fields that are omitted from the `fieldOrder` array will be placed at the end of the form.

.. _register_custom_view:

Using a Custom View
-------------------

By default, this route will use a pre-built view created by Stormpath.

If you want to customize the look and feel of the view, you can set the ``view`` option to the name of (or path to) a Razor view available in your project:

.. code-block:: yaml

  stormpath:
    web:
      register:
        view: "~/Views/Register/Register.cshtml"

.. note::
  The view you specify will be passed a model of type ``Stormpath.Owin.Abstractions.ViewModel.ExtendedRegisterViewModel``.

Feel free to copy and modify the `pre-built view templates`_ and use them as a starting point!

.. todo::
  Update this section when it's possible to simply update the included Razor files.


Password Strength Requirements
------------------------------

Stormpath supports complex password strength rules, such as the number of letters
or special characters required.  These settings are controlled on a per-Directory
basis.

If you want to modify the password strength rules for your application, use the `Stormpath Admin Console`_ to find the directory that is mapped to your application, and modify the associated password policy.

For more information, see `Account Password Strength Policy`_.



Email Verification
------------------

We **highly** recommend that you use email verification, as it adds an additional layer
of security to your site (it makes it harder for bots to create spam accounts).

When the Stormpath email verification workflow is enabled on the directory, we will send new accounts an email with a link they must click in order to verify their account.

To enable email verification, you need to configure the Stormpath Directory. Follow these steps:

1. Use the `Stormpath Admin Console`_ to find the Stormpath Directory linked to your Application
2. Navigate to the Workflows section and enable the **Verification Email** workflow
3. Modify the **Link Base URL** to point to your application:

.. code-block:: sh

    http://yourapplication.com/verify

.. note::
  The ``/verify`` route is automatically handled by the Stormpath middleware; see the :ref:`email_verification` section.


Auto Login
----------

If you are *not* using email verification, you may log users in
automatically when they register.  This can be achieved with this configuration:

.. code-block:: yaml

  stormpath:
    web:
      register:
        autoLogin: true
        nextUri: "/"

By default the ``nextUri`` is to the ``/`` root page, but you can modify this to whatever destination you want.


.. todo::
  .. _pre_registration_handler:

  Pre Registration Handler
  ------------------------

  Want to validate or modify the form data before it's handled by Stormpath? Then this is
  the handler that you want to use!

  To use a ``preRegistrationHandler`` you need to define your handler function in
  the Stormpath middleware setup::

      app.use(stormpath.init(app, {
        preRegistrationHandler: function (formData, req, res, next) {
          console.log('Got registration request', formData);
          next();
        }
      }));

  As you can see in the example above, the ``preRegistrationHandler`` function
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

  In the example below, we'll use the ``preRegistrationHandler`` to validate that
  the user doesn't enter an email domain that is restricted::

      app.use(stormpath.init(app, {
        preRegistrationHandler: function (formData, req, res, next) {
          if (formData.email.indexOf('@some-domain.com') !== -1) {
            return next(new Error('You\'re not allowed to register with \'@some-domain.com\'.'));
          }

          next();
        }
      }));

  .. _post_registration_handler:

  Post Registration Handler
  -------------------------

  Want to run some custom code after a user registers for your site?  If so, this
  is the event you want to handle!

  By defining a ``postRegistrationHandler`` you're able to do stuff like:

  - Send a new user a welcome email.
  - Generate API keys for all new users.
  - Setup Stripe billing.
  - etc.

  To use a ``postRegistrationHandler``, you need to define your handler function
  in the Stormpath middleware setup::

      app.use(stormpath.init(app, {
        postRegistrationHandler: function (account, req, res, next) {
          console.log('User:', account.email, 'just registered!');
          next();
        }
      }));

  As you can see in the example above, the ``postRegistrationHandler`` function
  takes in four parameters:

  - ``account``: The new, successfully created, user account.
  - ``req``: The Express request object.  This can be used to modify the incoming
    request directly.
  - ``res``: The Express response object.  This can be used to modify the HTTP
    response directly.
  - ``next``: The callback to call when you're done doing whatever it is you want
    to do.  If you call this, execution will continue on normally.  If you don't
    call this, you're responsible for handling the response.

  In the example below, we'll use the ``postRegistrationHandler`` to redirect the
  user to a special page (*instead of the normal registration flow*)::

      app.use(stormpath.init(app, {
        postRegistrationHandler: function (account, req, res, next) {
          res.redirect(302, '/secretpage').end();
        }
      }));

.. _json_registration_api:

JSON Registration API
---------------------

If you are using this library from a client framework like Angular or React, you will interact with the registration endpoint via GET and POST requests, instead of letting the middleware render an HTML view.


Getting the Form View Model
...........................

By making a GET request to the registration endpoint with the ``Accept: application/json`` header set, you can retrieve a JSON view model that describes the registration form and any external account stores that are mapped to your Stormpath Application.

Here's an example view model that represents an application that has the default registration form, and a mapped Google social directory:

.. code-block:: javascript

  {
    "accountStores": [
      {
        "name": "Google social directory",
        "href": "https://api.stormpath.com/v1/directories/gc0Ty90yXXk8ifd2QPwt",
        "provider": {
          "providerId": "google",
          "clientId": "441084632428-9au0gijbo5icagep9u79qtf7ic7cc5au.apps.googleusercontent.com",
          "scope": "email profile",
          "href": "https://api.stormpath.com/v1/directories/gc0Ty90yXXk8ifd2QPwt/provider"
        }
      }
    ],
    "form": {
      "fields": [
        {
          "label": "First Name",
          "placeholder": "First Name",
          "required": true,
          "type": "text",
          "name": "givenName"
        },
        {
          "label": "Last Name",
          "placeholder": "Last Name",
          "required": true,
          "type": "text",
          "name": "surname"
        },
        {
          "label": "Email",
          "placeholder": "Email",
          "required": true,
          "type": "email",
          "name": "email"
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

.. todo::
  Update form with new visible flag and enabled value

.. note::

  You may have to explicitly tell your client library that you want a JSON
  response from the server. Not all libraries do this automatically. If the
  library does not set the ``Accept: application/json`` header on the request,
  you'll get back the HTML registration form instead of the JSON response that you
  expect!


Registering a User
..................

Simply post an object to ``/register`` and supply the fields that you wish to
populate for the user:

.. code-block:: json

    {
        "email": "foo@bar.com",
        "password": "mySuper3ecretPAssw0rd",
        "surname": "bar",
        "givenName": "foo"
    }

If the user is created successfully, you'll get a ``200 OK`` response. The body of the response will contain the account object that was created:

.. code-block:: json

  {
    "account": {
      "href": "https://api.stormpath.com/v1/accounts/xxx",
      "username": "foo@bar.com",
      "modifiedAt": "2016-01-26T20:50:03.931Z",
      "status": "ENABLED",
      "createdAt": "2015-10-13T20:54:22.215Z",
      "email": "foo@bar.com",
      "middleName": null,
      "surname": "bar",
      "givenName": "foo",
      "fullName": "foo bar"
    }
  }

If an error occurs, you'll get an error object that looks like this:

.. code-block:: json

  {
    "status": 400,
    "message": "Invalid username or password."
  }

Supplying Custom Fields
.......................

If any custom fields exist on the form (see :ref:`register_custom_fields`), you can supply them either as a root property, or a child of a property called ``customData``:

.. code-block:: json

  {
      "email": "foo@bar.com",
      "password": "mySuper3ecretPAssw0rd",
      "surname": "bar",
      "givenName": "foo",
      "customValue": "custom value can be on root object or in customData object",
      "customData": {
        "favoriteColor": "beige"
      }
  }


.. _register_default_configuration:

Default Configuration
---------------------

Options that are not overridden by explicit configuration (see :ref:`configuration`) will retain their default values.

For reference, the full default configuration is shown in YAML below:

.. code-block:: yaml

  stormpath:
    web:
      register:
        enabled: true
        uri: "/register"
        autoLogin: false
        nextUri: "/"
        view: "register"
        form:
          fields:
            givenName:
              enabled: true
              label: "First Name"
              placeholder: "First Name"
              required: true
              type: "text"
            middleName:
              enabled: false
              label: "Middle Name"
              placeholder: "Middle Name"
              required: true
              type: "text"
            surname:
              enabled: true
              label: "Last Name"
              placeholder: "Last Name"
              required: true
              type: "text"
            username:
              enabled: false
              label: "Username"
              placeholder: "Username"
              required: true
              type: "text"
            email:
              enabled: true
              label: "Email"
              placeholder: "Email"
              required: true
              type: "email"
            password:
              enabled: true
              label: "Password"
              placeholder: "Password"
              required: true
              type: "password"
            confirmPassword:
              enabled: false
              label: "Confirm Password"
              placeholder: "Confirm Password"
              required: true
              type: "password"
          fieldOrder:
            - "username"
            - "givenName"
            - "middleName"
            - "surname"
            - "email"
            - "password"
            - "confirmPassword"

.. _Custom Data: http://docs.stormpath.com/rest/product-guide/latest/accnt_mgmt.html#how-to-store-additional-user-information-as-custom-data
.. _pre-built view templates: https://github.com/stormpath/stormpath-dotnet-owin-middleware/tree/master/src/Stormpath.Owin.Views

.. _Stormpath Admin Console: https://api.stormpath.com
.. _Account Password Strength Policy: https://docs.stormpath.com/rest/product-guide/latest/accnt_mgmt.html#manage-password-policies
