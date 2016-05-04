.. _introduction:


Introduction
============

Welcome! This page will give you an overview of Stormpath and the Stormpath ASP.NET library, to help you decide if it's right for your project.

If you're already using Stormpath, feel free to dive into any of the sections on the left for help with specific features.


What does this library do?
--------------------------

With a few lines of code, you can add a user database and authentication system to your ASP.NET application.
Out of the box, this library will provide a way for users to create accounts and log in to your application
with a username and password.  Your users can also create API Keys and
OAuth tokens (useful if you are creating an API service).

This library uses `Stormpath`_ as a service, and you will need a `free account <https://api.stormpath.com/register>`_ to continue.

.. note::
  This library is built on top of the `Stormpath .NET SDK`_, It handles a wide range of use cases out of the box, but if you need to build custom functionality or want to dig deeper into the `Stormpath API`_, you can use the `Stormpath .NET SDK`_ directly. The SDK provides a low-level convenience wrapper around the Stormpath API.


What is Stormpath?
------------------

`Stormpath`_ is a hosted API service for creating and managing user accounts.
Stormpath manages the following tasks for your application:

- Creating and editing user accounts and user data.
- Organizing users with groups and roles.
- Securely storing profile or other data on user objects.
- Logging users in via social login providers like Facebook and Google.
- Customizing group permissions (groups, roles, etc).
- Handling complex authentication and authorization patterns.
- Generating and validating OAuth 2.0 tokens.

Stormpath is used as a simple REST API over HTTP. This means that Stormpath can be used in almost any software environment. You can interact with the Stormpath API directly with HTTP requests,
or by using one of our rich SDK libraries (such as the `Stormpath .NET SDK`_).


Who Should Use Stormpath
------------------------

Stormpath is a great service, but it's not for everyone!

You might want to use Stormpath if:

- You want to make user creation, management, and security as simple as possible.
  (You can get started in ASP.NET with only a couple of lines of code!)
- User security is a top priority. We're security experts so you don't have to be one.
- Scaling your user base is a concern (Stormpath handles scaling transparently).
- You need to store custom user data along with your user's basic information
  (email, password), but you don't want a separate database.
- You would like to have automatic email verification for new user accounts.
- You would like to configure and customize password strength rules.
- You'd like to keep your user data separate from your other applications to
  increase platform stability/availability.
- You are building a service-oriented application, in which multiple
  independent services need access to the same user data.

**Tl;dr** - Stormpath is a great match for applications of any size where
security, speed, and simplicity are top priorities!

You might *not* want to use Stormpath if:

- You are building an application that does not need user accounts.
- Security and availability aren't high priorities.
- You want to roll your own custom user authentication.

Want to use Stormpath?  Okay, great!  Let's get started!


.. _Stormpath .NET SDK: https://github.com/stormpath/stormpath-sdk-dotnet
.. _Stormpath API: https://docs.stormpath.com/rest/product-guide/
.. _Stormpath: https://stormpath.com/
