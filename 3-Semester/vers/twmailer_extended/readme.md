docker build --pull --rm -f "Dockerfile.server" -t twmailer_server:latest "."
docker build --pull --rm -f "Dockerfile.client" -t twmailer_client:latest "."

docker-compose up
