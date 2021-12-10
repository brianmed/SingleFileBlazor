#! /bin/bash

set -e

if [ -e "/etc/os-release" ]; then
    DIR="$(dirname "$(readlink -f "$0")")"
else
    GREADLINK=$(which greadlink)

    if [ -x "$GREADLINK" ]; then
        DIR="$(dirname "$("$GREADLINK" -f "$0")")"
    else
        echo 'greadlink not found'
        exit 1
    fi
fi

cd "$DIR"

cd ../Client 
dotnet publish -c Release -r linux-x64 --output ../Server/clientWwwroot

cd ../Server
dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true
