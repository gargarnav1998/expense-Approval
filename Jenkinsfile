

pipeline{
try {
     agent any
    stages{
		stage("Checkout")
		{
		   
    		steps{
    			echo "Checkout latest committed code"
			checkout([$class: 'GitSCM', branches: [[name: '*/feat/CI']], doGenerateSubmoduleConfigurations: false, extensions: [[$class: 'CheckoutOption', timeout: 2000]], submoduleCfg: [], userRemoteConfigs: [[credentialsId: '129b1034-2565-448c-a5dc-0dac0fb0fae0', url: 'https://github.com/optimusinfo/Expense-Approval-Backend.git']]])    		
    			
                	}
		}
		

		stage("Build")
		{
		    
        	steps{
        		echo "Build the solution"
        		bat "\"${tool 'MSBuild'}\" ExpenseApproval/ExpenseApproval.sln /p:Configuration=Release /p:Platform=\"Any CPU\" /p:ProductVersion=15.1.0.0"
        		}
		}  
 
		stage("Deploy")
		{
        			steps{
        			echo "Deploying the sln"
        			bat "\"${tool 'MSBuild'}\" ExpenseApproval/ExpenseApproval.sln /p:Configuration=Release /p:Platform=\"Any CPU\" /p:DeployOnBuild=true /p:PublishProfile=\"WebAppArnav - Web Deploy\" /p:Password=lwLeR7t7tpYkCMQrRWdq5o75sCE25tQt2ElSromGtiWXoh2C1G2qf9QDrg9m /p:AllowUntrustedCertificate=true /p:ProductVersion=15.1.0.0"
        				}
		}
    }
    post {
        
        success {
            echo 'The Pipeline successful :)'
		    emailext body: 'CI/CD successful', subject: 'Pipeline success Jenkins - Project_Name', to: 'arnav.garg@optimusinfo.com'
        }
        failure {
            echo 'The Pipeline failed :('
			emailext body: 'CI/CD failed', subject: 'Pipeline failed Jenkins - Project_Name', to: 'arnav.garg@optimusinfo.com'
        }
    }
}
catch(e)
{
   // If there was an exception thrown, the build failed
   currentBuild.result = "FAILED"
   throw e
 } finally {
   // Success or failure, always send notifications
   notifyBuild(currentBuild.result)
 }
}


def notifyBuild(String buildStatus = 'STARTED') {
  // build status of null means successful
  buildStatus =  buildStatus ?: 'SUCCESSFUL'

  // Default values
  def colorName = 'RED'
  def colorCode = '#FF0000'
  def subject = "${buildStatus}: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]'"
  def summary = "${subject} (${env.BUILD_URL})"

  // Override default values based on build status
  if (buildStatus == 'STARTED') {
    color = 'YELLOW'
    colorCode = '#FFFF00'
  } else if (buildStatus == 'SUCCESSFUL') {
    color = 'GREEN'
    colorCode = '#00FF00'
  } else {
    color = 'RED'
    colorCode = '#FF0000'
  }

  // Send notifications
  slackSend (color: colorCode, message: summary)
}