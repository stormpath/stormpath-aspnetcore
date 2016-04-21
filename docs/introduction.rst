.. _introduction:


Introduction
============

Not sure if Express-Stormpath is for you?  Read along, and I'll help you decide
whether or not Express-Stormpath is a good fit for your project!


What does this library do?
--------------------------

This library will add a user database and authentication system to your application.
It will provide a way for users to create accounts and login to your application
with a username and password.  Your users can also create API Keys and
OAuth tokens (useful if you are creating an API service).

This library uses `Stormpath`_ as a service, and you will need a `Stormpath`_
account to continue.

This library is built on top of the `Stormpath .NET SDK`_, and uses the ASP.NET middleware pipeline to add security and built-in functionality to your application.
It handles a wide range of use cases, but if you need to build custom functionality or want to dig deeper into the `Stormpath API`_, you can use the `Stormpath .NET SDK`_ directly.
The SDK provides a low-level convenience wrapper around the Stormpath API.


What is Stormpath?
------------------

`Stormpath`_ is a hosted API service for creating and managing user accounts.
Stormpath manages the following tasks for your application:

- Create and edit user accounts and user data.
- Organize users with groups and roles.
- Store data on user objects.
- Securely store all user data.
- Customize group permissions (groups, roles, etc).
- Handle complex authentication and authorization patterns.
- Log users in via social login with `Facebook`_ and `Google`_.
- Generate and authenticate OAuth2 tokens.

Stormpath is used as a simple REST API, over HTTP. This means that we can
operate in almost any software environment. You can interact with the Stormpath API directly with HTTP requests,
or by using one of our SDK libraries (such as the `Stormpath .NET SDK`_).


Who Should Use Stormpath
------------------------

Stormpath is a great service, but it's not for everyone!

You might want to use Stormpath if:

- You want to make user creation, management, and security as simple as possible
  (you can get started in ASP.NET with only two lines of code)!
- User security is a top priority.  We're known for our security.
- Scaling your userbase is a potential problem (Stormpath handles scaling your
  users transparently).
- You need to store custom user data along with your user's basic information
  (email, password).
- You would like to have automatic email verification for new user accounts.
- You would like to configure and customize password strength rules.
- You'd like to keep your user data separate from your other applications to
  increase platform stability / availability.
- You are building a service oriented application, in which multiple
  independent services need access to the same user data.

**Basically, Stormpath is a great match for applications of any size where
security, speed, and simplicity are top priorities.**

You might **NOT** want to use Stormpath if:

- You are building an application that does not need user accounts.
- Your application is meant for internal-only usage.
- You aren't worried about user data / security much.
- You aren't worried about application availability / redundancy.
- You want to roll your own custom user authentication.

Want to use Stormpath?  OK, great!  Let's get started!


.. _Stormpath .NET SDK: https://github.com/stormpath/stormpath-sdk-dotnet
.. _Stormpath API: https://docs.stormpath.com/rest/product-guide/
.. _Stormpath: https://stormpath.com/
.. _Facebook: https://www.facebook.com/
.. _Google: https://www.google.com/
