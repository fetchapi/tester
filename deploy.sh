#!/bin/bash -v

set -e

if [ -z $1 ]; then
    echo "Argument cannot be empty."
    exit
fi

APP_NAME=$(echo $1 | awk '{print tolower($0)}') 
APP_DIR=/usr/share/training_projects/loanvt/$APP_NAME
ZIP=myapp-$APP_NAME.zip

# create temp folder for preparing zip
mkdir -p ~/training_projects/loanvt/$APP_NAME

# move published output from build stage to folder
mv $CI_PROJECT_DIR/MyApp.$1/bin/Release/netcoreapp2.0/publish/* ~/myapp/$APP_NAME

# navigate to folder to set root for zip
cd ~/training_projects/loanvt

# zip folder
zip -r ~/$ZIP $APP_NAME

# copy zip to server
scp -qr ~/$ZIP myserver:~/

# ssh to server and unzip within server to temp folder
ssh myserver "unzip -o $ZIP -d ~/myapp"

# ssh to server and remove app folder and content
ssh myserver "sudo rm -rf $APP_DIR/*"

# ssh to server and copy binaries from temp folder to app folder
ssh myserver"sudo cp -r ~/ek/$APP_NAME/* $APP_DIR"

# ssh to server and set user and group to user used by nginx and systemd
ssh myserver "sudo chown -R www-data:www-data $APP_DIR/*"

# ssh to server and restart systemd unit
ssh myserver "sudo systemctl restart myapp-$APP_NAME"