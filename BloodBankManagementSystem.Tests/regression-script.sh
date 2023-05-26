#!/bin/bash

# Set the email configuration
EMAIL_RECIPIENT="mahirbathija@gmail.com"
EMAIL_SUBJECT="Test Results"

# Restore all nuget packages
dotnet restore

# Build the project
dotnet build

# Run the tests and store the output in a temporary file
TEST_OUTPUT=$(dotnet test --no-build --logger "trx;LogFileName=test-results.xml" --results-directory TestResults)
TEST_EXIT_CODE=$?

# Check if the tests ran successfully
if [ $TEST_EXIT_CODE -eq 0 ]; then
    TEST_STATUS="Passed"
else
    TEST_STATUS="Failed"
fi

# Send an email with the test results
echo "Test Status: $TEST_STATUS" | mail -s "$EMAIL_SUBJECT" "$EMAIL_RECIPIENT" -A "$TEST_OUTPUT/TestResults/test-results.xml"

# Clean up the temporary test output file
rm -rf "$TEST_OUTPUT"