pipeline {
    agent any

    environment {
        DEPLOY_DIR = "/var/www/vinheria"
    }

    stages {

        stage('Checkout') {
            steps {
                sshagent(credentials: ['898ee6b8-de51-47d8-aabb-c1d9a280f30b']) {
                    git branch: 'main', url: 'git@github.com:caiolucasb/vinheria-agnello-fase-7.git'
                }
            }
        }

        stage('Build') {
            steps {
                echo "===== BUILD DA VINHERIA ====="

                sh '''
                    echo "Iniciando build..."
                    mkdir -p build
                    echo "Deploy da Vinheria" > build/build.log
                    echo "Build finalizado!" 
                '''
            }
        }

        stage('Deploy') {
            steps {
                echo "===== DEPLOY ====="

                sh '''
                    # Garante que a pasta de deploy existe
                    mkdir -p $DEPLOY_DIR

                    echo "Copiando arquivos para a pasta de publicação..."
                    cp -r ./* $DEPLOY_DIR/

                    echo "Deploy concluído!"
                '''
            }
        }
    }
}
