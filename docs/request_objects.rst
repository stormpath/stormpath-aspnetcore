.. _stormpath_objects:

Request Objects
=================

.. only:: aspnetcore

  When the Stormpath middleware is added to your ASP.NET Core request pipeline, these types will be available via dependency injection for each request:

  * Stormpath Client (``IClient``)
  * Stormpath Application (``IApplication``)
  * Current user's Stormpath Account (``IAccount`` or ``Lazy<IAccount>``)

  You can request any of these objects from the container using the ``[FromServices]`` attribute:

  .. literalinclude:: code/request_objects/aspnetcore/controller_fromservices.cs
      :language: csharp

  Or, if you prefer, you can use constructor injection:

  .. literalinclude:: code/request_objects/aspnetcore/controller_injection.cs
      :language: csharp

.. only:: aspnet

  .. todo::
    Add detail about extension methods here.

.. only:: nancy

  .. todo::
    Add detail here.


Stormpath Client
----------------

The ``IClient`` type is the starting point of the `Stormpath .NET SDK`_. You can use it to perform any action against the Stormpath API.

This type is available on every request.

.. only:: aspnet

  .. todo::

    Add detail about extension methods here.

.. only:: nancy

  .. todo::

    Add detail here.

.. note::
  For more information about the Client object, see the `Stormpath .NET API documentation`_.


Stormpath Application
---------------------

The ``IApplication`` type is a .NET representation of the Stormpath Application associated with your |framework| application. You can use this object to perform actions like creating and searching for user accounts programmatically.

This type is available on every request.

.. only:: aspnet

  .. todo::
    Add detail about extension methods here.

.. only:: nancy

  .. todo::
    Add detail here.


Current User Account
--------------------

The Stormpath middleware automatically checks incoming requests for authentication information, and resolves the user's identity to a Stormpath Account if the information is valid. This happens on **every** request, not just routes that require authentication.

.. tip::
  If you want to *require* authentication for a route or action, see the :ref:`authentication` section.

.. only:: aspnetcore

  A subset of the user's Stormpath Account details are automatically placed in the ``ClaimsPrincipal`` object for the request. This makes it possible to quickly do things like update a view if the user is logged in:

  .. literalinclude:: code/request_objects/aspnetcore/user_iprincipal.cshtml
      :language: html

  The full list of claims populated in ``Context.User`` are:

  * ``ClaimTypes.NameIdentifier`` (Stormpath Account href)
  * ``ClaimTypes.Email``
  * ``ClaimTypes.Name`` (Stormpath username, usually the same as email)
  * ``ClaimTypes.GivenName``
  * ``ClaimTypes.Surname``
  * ``"FullName"``

  .. only:: aspnetcore

    If you want full access to the Stormpath ``IAccount`` object, inject a ``Lazy<IAccount>`` in your controller:

    .. literalinclude:: code/request_objects/aspnetcore/injecting_user.cs
        :language: csharp

    If the request is unauthenticated, the lazy value will resolve to ``null``. If the request represents a valid user, you'll get an ``IAccount`` instance representing the user's Stormpath Account.

    .. tip::
      If your controller or action will *always* be authenticated (see the :ref:`authentication` section), you can drop the wrapper and inject ``IAccount`` directly. Don't do this on routes that can be accessed anonymously!

    You can also use the ``@inject`` directive to do the same injection directly in your views:

    .. literalinclude:: code/request_objects/aspnetcore/injecting_user_view.cshtml
        :language: html

.. only:: aspnet

  .. todo::
    Add detail here.

.. only:: nancy

  .. todo::
    Is this relevant?

If you want to require authentication on certain controllers or routes, jump to the :ref:`authentication` section.


Working with the Stormpath API
------------------------------

By accessing one of the available types (``IClient``, ``IApplication``, or ``IAccount``), you can use the `Stormpath .NET SDK`_ to interact with the Stormpath API.

To update the user's password, for example:

.. only:: aspnetcore

  .. literalinclude:: code/request_objects/aspnetcore/update_user_password.cs
      :language: csharp

.. only:: aspnet

  .. todo::
    Add code

.. only:: nancy

  .. todo::
    Add code

There are many more things you can do with the .NET SDK. Check out the `Stormpath .NET API documentation`_ to learn more!

.. _Stormpath .NET SDK: https://github.com/stormpath/stormpath-sdk-dotnet
.. _Stormpath .NET API documentation: http://docs.stormpath.com/dotnet/api/
