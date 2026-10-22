
pipeline {
    agent any

    options {
        timestamps()
        disableConcurrentBuilds()
    }

    parameters {
        string(
            name: 'BRANCH_NAME',
            defaultValue: 'main',
            description: 'Git branch to test'
        )
    }

    environment {
        PROJECT_FILE = 'WebBanHang/WebBanHang.csproj'
        TEST_PROJECT = 'WebBanHang.Tests/WebBanHang.Tests.csproj'
        PUBLISH_DIR = 'D:\\CICD\\jenkins-publish'
        IIS_DIR = 'D:\\CICD\\publish\\WebBanHang'
        IIS_SITE = 'WebBanHang'
        IIS_URL = 'http://localhost:8081'
    }

    stages {
        stage('Checkout') {
            steps {
                deleteDir()
                git branch: "${params.BRANCH_NAME}",
                    url: 'https://github.com/quanghieu1605/WebBanHang.git'
            }
        }

        stage('Restore') {
            steps {
                bat 'dotnet restore "%PROJECT_FILE%"'
                bat 'dotnet restore "%TEST_PROJECT%"'
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build "%PROJECT_FILE%" -c Release --no-restore'
            }
        }

        stage('Test') {
            steps {
                bat 'dotnet test "%TEST_PROJECT%" -c Release'
            }
        }

        stage('Publish') {
            when {
                expression { params.BRANCH_NAME == 'main' }
            }
            steps {
                bat 'dotnet publish "%PROJECT_FILE%" -c Release -o "%PUBLISH_DIR%"'
            }
        }

        stage('Deploy') {
            when {
                expression { params.BRANCH_NAME == 'main' }
            }
            steps {
                bat '''
                    echo ===== STOP IIS SITE =====
                    %windir%\\system32\\inetsrv\\appcmd.exe stop site /site.name:"%IIS_SITE%"

                    if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

                    echo ===== ENABLE APP OFFLINE =====
                    echo Offline > "%IIS_DIR%\\app_offline.htm"

                    powershell -NoProfile -Command "Start-Sleep -Seconds 3"

                    echo ===== COPY PUBLISHED FILES =====
                    robocopy "%PUBLISH_DIR%" "%IIS_DIR%" /E /PURGE /R:3 /W:5

                    if %ERRORLEVEL% GEQ 8 (
                        del "%IIS_DIR%\\app_offline.htm"
                        %windir%\\system32\\inetsrv\\appcmd.exe start site /site.name:"%IIS_SITE%"
                        exit /b 1
                    )

                    if exist "%IIS_DIR%\\app_offline.htm" del "%IIS_DIR%\\app_offline.htm"

                    echo ===== START IIS SITE =====
                    %windir%\\system32\\inetsrv\\appcmd.exe start site /site.name:"%IIS_SITE%"

                    if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%
                '''
            }
        }

        stage('Verify') {
            when {
                expression { params.BRANCH_NAME == 'main' }
            }
            steps {
                powershell '''
                    $url = $env:IIS_URL
                    $success = $false

                    for ($i = 1; $i -le 10; $i++) {
                        try {
                            $response = Invoke-WebRequest -Uri $url -UseBasicParsing -TimeoutSec 5
                            if ($response.StatusCode -eq 200) {
                                $success = $true
                                break
                            }
                        } catch {
                            Start-Sleep -Seconds 3
                        }
                    }

                    if (-not $success) {
                        throw "Website verification failed: $url"
                    }

                    Write-Output "Website verification succeeded: $url"
                '''
            }
        }
    }

    post {
        success {
            echo 'CI/CD completed successfully.'
        }
        failure {
            echo 'CI/CD failed. Check the stage logs.'
        }
        always {
            echo 'Pipeline finished.'
        }
    }
}
