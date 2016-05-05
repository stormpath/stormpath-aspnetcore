.. _me_api:


Current User API
=====================

If you are building a front-end or mobile application, you'll need a way to get the user's details after they log in.

By default, this library will provide a JSON-only ``/me`` route that can be used to get the details of a logged-in user. You can change this URI, or disable the feature entirely if you wish.


Configuration Options
---------------------

This feature supports several options that you can configure using code or markup (see the :ref:`Configuration` section):

* **enabled**: Whether the feature is enabled. (Default: ``true``)
* **uri**: The path for this feature. (Default: ``/me``)
* **expand**: Whether to expand Account resource links; see :ref:`me_expansion`.

.. note::
  Any unchanged options will retain their default values. See the :ref:`me_api_default_configuration` section to view the defaults.

Configuration Example
.....................

You can easily change the path to this route by changing the ``uri`` option (or disable it entirely by setting ``enabled = false``), as shown in YAML below:

.. code-block:: yaml

  stormpath:
    web:
      me:
        uri: "/userDetails"

You could also set this configuration via code:

.. only:: aspnetcore

  .. literalinclude:: code/me_api/aspnetcore/configure_uri.cs
    :language: csharp

.. only:: aspnet

  .. todo::
    Add code

.. only:: nancy

  .. todo::
    Add code

See the :ref:`configuration` section for more details on how configuration works, or :ref:`me_api_default_configuration` to see the default values.


Using the Endpoint
------------------

Given a valid access token value or cookie (see :ref:`authentication`), you can make a GET request to ``/me`` to get a JSON representation of the account that is currently logged in:

.. code-block:: http

  GET /me
  Accept: application/json
  Authorization: Bearer <access_token>
  -or- Cookie: access_token=<value>

The response from the endpoint looks like this:

  .. code-block:: json

   {
     "account": {
       "href": "https://api.stormpath.com/v1/accounts/4WvCtY0oCRDzQdYH3Q0qjz",
       "username": "foobar",
       "email": "foo@example.com",
       "givenName": "Foo",
       "middleName": null,
       "surname": "Bar",
       "fullName": "Foo Bar",
       "status": "ENABLED",
       "createdAt": "2015-10-13T20:54:22.215Z",
       "modifiedAt": "2016-03-17T16:40:17.631Z"
     }
   }

If the user is not logged in (the access token is not valid), the endpoint will return ``401 Unauthorized``.


.. _me_expansion:

Link Expansion
--------------

You can opt-in to including additional data in the JSON response by enabling expansion of one of the Stormpath Account linked resources. The linked resources that can be expanded are:

* `apiKeys`_
* `applications`_
* `customData`_
* `directory`_
* `groupMemberships`_
* `providerData`_
* `tenant`_

.. note::
  For more information about how link expansion works in the Stormpath API, see the `Links section`_ in the REST API documentation.

The following YAML configuration will opt-in to expanding the Account's Custom Data and Groups automatically:

.. code-block:: yaml

  stormpath:
    web:
      me:
        expand:
          customData: true
          groups: true


.. _me_api_default_configuration:

Default Configuration
---------------------

Options that are not overridden by explicit configuration (see :ref:`configuration`) will retain their default values.

For reference, the full default configuration for this route is shown as YAML below:

.. code-block:: yaml

  stormpath:
    web:
      me:
        enabled: true
        uri: "/me"
        expand:
          apiKeys: false
          applications: false
          customData: false
          directory: false
          groupMemberships: false
          groups: false
          providerData: false
          tenant: false

.. tip::
  You can also refer to the `Example Stormpath configuration`_ to see the entire default library configuration.


.. _Example Stormpath configuration: https://github.com/stormpath/stormpath-framework-spec/blob/master/example-config.yaml
.. _apiKeys: https://docs.stormpath.com/rest/product-guide/latest/reference.html#account-api-keys
.. _applications: https://docs.stormpath.com/rest/product-guide/latest/reference.html#application
.. _customData: https://docs.stormpath.com/rest/product-guide/latest/reference.html#custom-data
.. _directory: https://docs.stormpath.com/rest/product-guide/latest/reference.html#directory
.. _groupMemberships: https://docs.stormpath.com/rest/product-guide/latest/reference.html#group-membership
.. _providerData: https://docs.stormpath.com/rest/product-guide/latest/reference.html#provider
.. _tenant: https://docs.stormpath.com/rest/product-guide/latest/reference.html#tenant
.. _Links section: https://docs.stormpath.com/rest/product-guide/latest/reference.html#links
