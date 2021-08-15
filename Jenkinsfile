pipeline {
   
  agent any
  
   environment{
       scannerHome = tool 'sonar_scanner_dotnet'
       username = 'bhavinibatra'
       registry = 'bhavinibatra/pracmaster'
    }

  stages {
    stage('Checkout') {
      steps {
        checkout([$class: 'GitSCM', branches: [
          [name: '*/master']
        ], userRemoteConfigs: [
          [credentialsId: 'GitCreds', url: 'https://github.com/BhaviniB/devops_practice']
        ]])
      }
    }
    stage('Nuget Restore') {
      steps {
        bat "dotnet restore WebApplication4\\WebApplication4.csproj"
      }
    }
      stage('Start Sonarqube Analysis') {
      steps {
        withSonarQubeEnv('Test_Sonar'){
            bat "${scannerHome}\\SonarScanner.MSBuild.exe begin /k:sonar-bhavinibatra-prac /n:sonar-bhavinibatra-prac /v:1.0"
        }
        
      }
    }

    stage('Code Build') {
      steps {
        bat 'dotnet clean WebApplication4\\WebApplication4.csproj'

        bat 'dotnet build WebApplication4\\WebApplication4.csproj -c Release -o "WebApplication4/app/build"'

      }
    }
        
         stage('Stop Sonarqube Analysis') {
      steps {
        withSonarQubeEnv('Test_Sonar'){
            bat "${scannerHome}\\SonarScanner.MSBuild.exe end"
        }
        
      }
    }
    
      stage('Docker Image') {
      steps {
             bat "dotnet publish WebApplication4\\WebApplication4.csproj"
             
            bat "docker build -t i-${username}-master-p1 --no-cache ."
        
      }
    }
     
    stage('Containers'){
      parallel{
  stage('PreContainerCheck') {
                    environment {
                        CONTAINER_ID = "${bat(script:'docker ps -aqf name="^c-bhavinibatra-master-p1$"', returnStdout: true).trim().readLines().drop(1).join("")}"
                    }
                    steps {
                        echo "Running pre container check"
                        script {
                        if(env.CONTAINER_ID != null) {
                            echo "Removing container: ${env.CONTAINER_ID}"
                            bat "docker rm -f ${env.CONTAINER_ID}"       
                        }
                        else{
                            echo "Container removal not needed."

                        }
                   }
                                  
                    }
                }  
      stage('Push to DockerHub') {
      steps {
             bat "docker tag i-${username}-master-p1 ${registry}:${BUILD_NUMBER}"
              bat "docker tag i-${username}-master-p1 ${registry}:latest"
             withDockerRegistry([credentialsId: 'DockerHub', url:""]){
             bat "docker push ${registry}:${BUILD_NUMBER}"
             bat "docker push ${registry}:latest"
             }
        
      }
    }
      }
    }
     stage('Docker Deployment') {
      steps {
             bat "docker run --name c-${username}-master-p1 -d -p 1200:80 ${registry}:${BUILD_NUMBER}"
            
      }
    }
    stage('Kubernetes Deployment'){
      steps{
        bat "kubectl apply -f deployment.yaml"
      }
    }

  }

}