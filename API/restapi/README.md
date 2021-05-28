# REST API example

This is the REST API example code that we went through in class. Getting this running on your own is pretty straightforward. You'll need to make sure you have the [latest .NET core](https://dotnet.microsoft.com/download) and an [editor of choice](https://code.visualstudio.com/Download).

## To build and run the application locally

```bash
dotnet restore
dotnet build

ASPNETCORE_ENVIRONMENT=Development dotnet run
```

Now, it's up to you to extend this. As we talked about in class it's missing some functionality and that's where you come in. Assignment 2 is a 5-point coding exercise, mainly to make sure you can get everything compiled and running like we did in class, and to get you a taste of adding this functionality for the individual and team projects.

- Add support (state management, semantics, etc.) for:
  - Remove (DELETE) a draft or cancelled timecard
  - Replace (POST) a complete line item
  - Update (PATCH) a line item
  - Verify that timecard resource is consistent with the calling actor (you need to know the caller)
  - Verify that timecard approver is not timecard resource (you need to know the caller)
  - To do the above two operations will require that you implemement an Employee API that can accept GET for single and collection employees, and can accept a POST to register an employee
  - Add support to root document for creating a timesheet
  - Complete support in root document for employee list and registration
