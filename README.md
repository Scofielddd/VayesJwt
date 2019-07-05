# VayesJwt
 .Net Core 2.2 auth service for vayes mobile application


# Check the following so that the mobile app can access
--  VayesJwt/.vs/VayesJwt/config/applicationhost.config
     ->  @Line164 if you haven't this binding line you need to add and change this port number which port the server(core) is running on.
     <br/>
     binding protocol="http" bindingInformation="*:50567:127.0.0.1" 
