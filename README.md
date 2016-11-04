# UmbHttpsRedirect
Event handler plugin for restricting parts of an Umbraco website to an Intranet.

## What's inside

This project includes a DLL that will register as an event handler with Umbraco. Depending on set page properties or config settings, required pages will be redirected to HTTPS.

## System requirements
1. NET Framework 4.5
2. Umbraco 7.3.7+ (should work with older versions but not tested)

## NuGet availability
This project is available on [NuGet](https://www.nuget.org/packages/UmbHttpsRedirect/).

## Usage instructions
### Getting started
1. Add **UmbHttpsRedirect.dll** as a reference in your project or place it in the **\bin** folder.
2. Insert the following `<appSettings>` keys in **web.config**:
  - `HttpsRedirect:DocTypes` - comma-separated list of document type aliases that should use HTTPS
  - `HttpsRedirect:PageIds` - comma-separate list of page IDs that should use HTTPS
  - `HttpsRedirect:Templates` - comma separated list of templates that should use HTTPS
  - `HttpsRedirect:ForceHttp` - true/false (default false) to redirect pages that aren't required to be HTTPS to HTTP
  - `HttpsRedirect:HttpPort` - optional port to use when redirecting to HTTP
  - `HttpsRedirect:HttpsPort` - optional port to use when redirecting to HTTPS
  - `HttpsRedirect:UseTemporaryRedirects` - true/false (default false) if to use 302 Temporary redirects rather than 301 Permanent redirects
3. To restrict access to specific pages, add a true/false-type document property in Umbraco:
  - `umbHttpsRedirect`
