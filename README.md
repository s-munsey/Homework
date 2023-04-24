# shaunMunsey/AxonHomework

# The task:

### Summary
 We have provided you the code below for a proof of concept (PoC) console application that satisfies the following requirements:
 - Reads random users from an API endpoint 5 times. 
 - All responses from the API should be written to a file.
 - All successful responses should be represented as a list of users with the following properties
  - last name
  - first name
  - age
  - city
  - email
and be written to file as valid JSON.

### There are now new requirements for this application, and we should like for you to update this console application to incorporate the following new requirements while satisfing the original requirements:
- Update this code so it follows .Net best practices and principles. The code should be extensible, reusable, and easy to test using Unit Tests.
- Add Unit tests.
- Make the the output file names configurable.
- Make the number of API calls configurable instead of 5.
- Add logging
</summary>

## Solution

The app follows an n-tier architecture structure, I find this works well for small to medium sized projects,
and a repository-service pattern abstracting out key functionality allowing for easy-to-test code.

### Setup

- you can configure the number of calls and the output file name in the appsettings.json file.
- if it does fail to run make sure the file location you are writing to exists
- main entry point is in App.cs