1. Publish new Worker version
2. Build Worker: docker-compose build worker
3. Start RabbitMQ and several Workers: docker-compose up -d --scale worker=4