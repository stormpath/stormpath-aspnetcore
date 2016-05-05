.. _logout:

Logout
======

If you are using browser-based sessions, you'll need a way for the user to log out and destroy their session cookies. Similarly, in a mobile application, you'll need a way to destroy the user's tokens if they want to log out. The logout feature does both.

By default, this library will provide a POST route at ``/logout``.
Simply make a POST request to this URI and the user's tokens and cookies will be destroyed. You can change this URI, or disable the feature entirely if you wish.

Configuration Options
---------------------

This feature supports several options that you can configure using code or markup (see the :ref:`Configuration` section):

* **enabled**: Whether the feature is enabled. (Default: ``true``)
* **uri**: The path for this feature. (Default: ``/logout``)
* **nextUri**: The location to send the user after logging out. (Default: ``/``)

.. note::
  Any unchanged options will retain their default values. See the :ref:`logout_default_configuration` section to view the defaults.


Configuration Example
.....................

You can easily change the logout route URI and post-logout location by changing the configuration, as shown in YAML below:

.. code-block:: yaml

  stormpath:
    web:
      logout:
        uri: "/logMeOut"
        nextUri: "/goodbye"

You could also set this configuration via code:

.. only:: aspnetcore

  .. literalinclude:: code/logout/aspnetcore/configure_uris.cs
    :language: csharp

.. only:: aspnet

  .. todo::
    Add code

.. only:: nancy

  .. todo::
    Add code

See the :ref:`configuration` section for more details on how configuration works, or :ref:`logout_default_configuration` to see the default values.


.. todo::

  .. _post_logout_handler:

  Post Logout Handler
  .. -------------------

  Want to run some custom code after a user has logged out of your site?
  By defining a ``postLogoutHandler`` you're able to do just that!

  To use a ``postLogoutHandler``, you need to define your handler function
  in the Stormpath config::

      app.use(stormpath.init(app, {
        postLogoutHandler: function (account, req, res, next) {
          console.log('User', account.email, 'just logged out!');
          next();
        }
      }));

  As you can see in the example above, the ``postLogoutHandler`` function
  takes four parameters:

  - ``account``: The successfully logged out user account.
  - ``req``: The Express request object.  This can be used to modify the incoming
    request directly.
  - ``res``: The Express response object.  This can be used to modify the HTTP
    response directly.
  - ``next``: The callback to call when you're done doing whatever it is you want
    to do.  If you call this, execution will continue on normally.  If you don't
    call this, you're responsible for handling the response.

  In the example below, we'll use the ``postLogoutHandler`` to redirect the
  user to a special page (*instead of the normal logout flow*)::

      app.use(stormpath.init(app, {
        postLogoutHandler: function (account, req, res, next) {
          res.redirect(302, '/farewell').end();
        }
      }));


.. _logout_default_configuration:

Default Configuration
---------------------

Options that are not overridden by explicit configuration (see :ref:`configuration`) will retain their default values.

For reference, the full default configuration for this route is shown as YAML below:

.. code-block:: yaml

  stormpath:
    web:
      logout:
        enabled: true
        uri: "/logout"
        nextUri: "/"

.. tip::
  You can also refer to the `Example Stormpath configuration`_ to see the entire default library configuration.


.. _Example Stormpath configuration: https://github.com/stormpath/stormpath-framework-spec/blob/master/example-config.yaml
