.. _templates:


View Templates
==============

This library has includes built-in views that are rendered automatically for HTML requests. You can use these views as-is, or you can customize them to your liking.


Default Views
-------------

By default this library will use pre-built views for the following routes:

* Login Page (``/login``)
* Registration Page (``/register``)
* Forgot Password Page (``/forgot``)
* Change Password Page (``/change``)
* Email Verifiation Page (``/verify``)

If you like the way the default views look, great! They work perfectly out of the box.


.. _templates_custom_views:

Custom Views
------------

If you want to customize the look and feel of the views, you can set a route's ``view`` configuration option to the name of (or the path to) a Razor view available in your project.

For example, to use a custom view for the login page, use this configuration (shown as YAML):

.. code-block:: yaml

  stormpath:
    web:
      login:
        view: "~/Views/Login/Login.cshtml"

.. tip::
  It's also possible to set this configuration via code. See the :ref:`configuration` section.

Feel free to copy and modify our `pre-built view templates`_, and use them as a starting point!

.. todo::
  Update this section when it's possible to simply update the included Razor files.


View Models
...........

Each route has a view model class defined in the ``Stormpath.Owin.Abstractions.ViewModel`` namespace. If you are supplying your own views, use these models:

+--------------------------------------------+---------------------------------------------------------------------+
| **Route**                                  | **View Model**                                                      |
+--------------------------------------------+---------------------------------------------------------------------+
| :ref:`login`                               | ``Stormpath.Owin.Abstractions.ViewModel.ExtendedLoginViewModel``    |
+--------------------------------------------+---------------------------------------------------------------------+
| :ref:`registration`                        | ``Stormpath.Owin.Abstractions.ViewModel.ExtendedRegisterViewModel`` |
+--------------------------------------------+---------------------------------------------------------------------+
| :ref:`Forgot Password <password_reset>`    | ``Stormpath.Owin.Abstractions.ViewModel.ForgotPasswordViewModel``   |
+--------------------------------------------+---------------------------------------------------------------------+
| :ref:`Change Password <password_reset>`    | ``Stormpath.Owin.Abstractions.ViewModel.ChangePasswordViewModel``   |
+--------------------------------------------+---------------------------------------------------------------------+
| :ref:`email_verification`                  | ``Stormpath.Owin.Abstractions.ViewModel.VerifyEmailViewModel``      |
+--------------------------------------------+---------------------------------------------------------------------+

.. _pre-built view templates: https://github.com/stormpath/stormpath-dotnet-owin-middleware/tree/master/src/Stormpath.Owin.Views
