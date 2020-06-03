# Sefe-Frameworks

My personal .Net Framework library that i used over the years. It includes many process and functions. You can find descriptions into readme file.

## 1-ApplicationSettings

This layer includes a class that provide getting config settings, query string and session values.

Example code;

```
using Sefe.ApplicationSettings;

int invoiceYearFromConfig = Settings.GetAppSetting("invoice_year", 2018); 

int invoiceYearFromQueryString = Settings.GetQueryString("invoice_year", 2018); 

int invoiceYearFromSession = Settings.GetSessionValue("invoice_year", 2018); 
```

## 2-Core

This layer is a return type for any process. Whole library uses this class for handle any process result.

Example code;

```
ProcessResult result = Serializing.Serialize(someObject);

if(!result.IsSuccess())

{

MessageBox.Show(result.ErrorMessage);

}
```

## 3-Caching

This layer does memory cache in Business layer. You can add values to cache and get values from cache.

## 4-Data

The Data layer includes many database operations. Data layer uses .Net Entity Framework for operations in repository pattern.

You can use it as Code first or Database first. When you use this layer to DB operations, layer makes every process in try and tell you is process success or not.

## 5-Emailing

This layer for email sending.

Example code;

```
Email email = new Email();

email.Send("OkMail", "We have your message", "example@example.com", null, "mail body", true,  true);

```

## 6-Logging

This layer writes log to event log.

## 7-Security

Security layer makes some encryptions like SHA256 hash.

## 8-Serializing

Serialize some classes or any data.