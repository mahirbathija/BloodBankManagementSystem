#!usr/bin/env bash
# rgggunoqshakkufe

read -p "Enter recipient email : " receiver

echo "restoring packages"

# Restore all nuget packages
dotnet restore
dotnet restore BloodBankManagementSystem
dotnet restore BloodBankManagementSystem.Tests

echo "building project"

# Run the build command
dotnet build

# Check the exit code of the build command
if [ $? -eq 0 ]; then
    build_succeeded=true
else
    build_succeeded=false
fi

# Run the tests and store the output in a temporary file
TEST_OUTPUT=$(dotnet test --no-build --logger "trx;LogFileName=test-results.xml" --results-directory TestResults)
TEST_EXIT_CODE=$?

# Check if the tests ran successfully
if [ $TEST_EXIT_CODE -eq 0 ]; then
    TEST_STATUS=true
else
    TEST_STATUS=false
fi

sender="mahirbathija@gmail.com"

gapp="rgggunoqshakkufe"

body="Build Succeeded = $build_succeeded \n Tests Succeeded = $TEST_STATUS"          # storing the content of file in a variable



    file="TestResults/test-results.xml"           # set file as the 1st positional parameter
    
    # MIME type for multiple type of input file extensions
    
    MIMEType=`file --mime-type "$file" | sed 's/.*: //'`
    curl -s --url 'smtps://smtp.gmail.com:465' --ssl-reqd \
    --mail-from $sender \
    --mail-rcpt $receiver\
    --user $sender:$gapp \
     -H "Subject: Test Results" -H "From: $sender" -H "To: $receiver" -F \
    '=(;type=multipart/mixed' -F "=$body;type=text/plain" -F \
      "file=@$file;type=$MIMEType;encoder=base64" -F '=)'
     