.. _setup:


Setup
=====

This section walks you through adding Stormpath to a new ASP.NET Core project. By the end
of this page you'll have working login and registration features for your application!

Create a Stormpath Account
--------------------------

If you haven't already, the first thing you'll want to do is `create a new Stormpath account <https://api.stormpath.com/register>`_.

Create an API Key Pair
----------------------

Once you've created a new account you need to create an API key pair. A new
API key pair is easily created by logging into your dashboard and clicking the
"Create an API Key" button. This will generate a new API key for you, and
prompt you to download your key pair.

.. note::
    Please keep the API key file safe!  This key and secret
    allow you to make Stormpath API requests, and should be properly protected.

Once you've downloaded your ``apiKey.properties`` file, make a new hidden folder in your home directory
called ``.stormpath`` and place the file in the new folder.

In the Windows Command prompt, run these commands from your Downloads folder:

.. code-block:: sh

    mkdir %homedrive%%homepath%\.stormpath
    move apiKey.properties %homedrive%%homepath%\.stormpath

In PowerShell or bash, the commands are similar:

.. code-block:: sh

    mkdir ~/.stormpath
    mv apiKey.properties ~/.stormpath

For the rest of this tutorial, we'll assume that you've placed this file in that
location. If you prefer to provide the API credentials with environment variables or
configuration options, you can do that too! Please see the :ref:`configuration`
section for other options.

Find Your Stormpath Application
-------------------------------

All new Stormpath Tenants will have a Stormpath Application, called
"My Application". You'll generally want one application per project, and we can
use this default application to get started.

An application has an **href**, and it looks like this:

.. code-block:: sh

    https://api.stormpath.com/v1/applications/l0ngr4nd0mstr1ngh3r3

From inside the `Admin Console`_, you can find the href by navigating to the
Application in the Applications list.

To learn more about Stormpath Applications, please see the
`Application Resource`_ and
`Setting up Development and Production Environments`_

.. note::
    Your default Application will also have a directory mapped to it. The
    Directory is where Stormpath stores accounts. To learn more, please see
    `Directory Resource`_ and `Modeling Your User Base`_.


Now that you've created an Application, you're ready to plug Stormpath
into your project!

Create a New Project
--------------------

.. note:: If you are adding Stormpath to an existing application, skip to the next section.

First, create a new project using the ASP.NET Core template in Visual Studio.

1. Click on **File - New Project**.
2. Under **Visual C# - Web**, pick the **ASP.NET Web Application** template.
3. In the New ASP.NET Project dialog, pick **Web Application** from **ASP.NET 5 Templates**.
4. Click **Change Authentication** and pick **No Authentication**. (You'll be adding it yourself!)

If you prefer the command line, you can use the [ASP.NET Yeoman Generator](https://github.com/OmniSharp/generator-aspnet) to scaffold a new project instead.
1. Run ``yo aspnet``.
2. Pick the **Web Application Basic [without Membership and Authorization]** template. Done!


Install the Package
-------------------

Now that you've got a project and a Stormpath account all set up and ready to go, all that's
left to do before we can dive into the code is install the `Stormpath.AspNetCore`_
package from NuGet.

This can be done with the NuGet Package Manager GUI, or using the Package Manager Console:

.. code-block:: none

    PM> install-package Stormpath.AspNetCore


Initialize the Middleware
----------------------------

Once the package is installed, you can add it to your application in ``Startup.cs``.

First, add the required services in ``ConfigureServices()``:

.. literalinclude:: code/csharp/setup/configure_services.cs
    :language: csharp

Next, add Stormpath to your middleware pipeline in ``Configure()``:

.. literalinclude:: code/csharp/setup/configure.cs
    :language: csharp

With this minimal configuration, the library will do the following:

- Look for your ``apiKey.properties`` file in the ``.stormpath`` folder.

- Fetch your Stormpath Application and all the data about its configuration and
  account stores.

- Attach the :ref:`default_features` to your application, such as the
  login page and registration page.

That's it, you're ready to go! Compile and run your project, and try navigating to these URLs:

- http://localhost:5000/login
- http://localhost:5000/register

.. note::
  If you are running on IIS or IIS Express, check your project's configuration to find the port assigned to the project.

You should be able to register for an account and log in. The newly created
account will be placed in the directory that is mapped to "My Application".

.. note::

    By default, we don't require email verification for new accounts, but we
    highly recommend you use this workflow. You can enable email verification
    by logging into the `Admin Console`_ and going to the the Workflows tab
    for the directory of your Stormpath Application.

There are many more features than login and registration. Continue to the
next section to learn more!


Example Applications
--------------------

Looking for some example applications?  We provide the following examples
applications to get you up and running quickly.  They show you how to setup
Stormpath, and implement a profile page for the logged-in user:

- `ASP.NET Core MVC6 Example Project`_

.. _Admin Console: https://api.stormpath.com/login
.. _Application Resource: https://docs.stormpath.com/rest/product-guide/latest/reference.html#application
.. _Directory Resource: https://docs.stormpath.com/rest/product-guide/latest/reference.html#directory
.. _Stormpath.AspNetCore: https://www.nuget.org/packages/Stormpath.AspNetCore
.. _Modeling Your User Base: https://docs.stormpath.com/rest/product-guide/latest/accnt_mgmt.html#modeling-your-user-base
.. _Setting up Development and Production Environments: https://docs.stormpath.com/guides/dev-test-prod-environments/
.. _ASP.NET Core MVC6 Example Project: https://github.com/stormpath/stormpath-aspnetcore-example
