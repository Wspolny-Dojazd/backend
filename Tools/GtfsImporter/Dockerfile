# Use alpine image as the base image for small image size
FROM --platform=arm64 ubuntu:latest

# Set the working directory inside the container
WORKDIR /app

# Copy the entire source code into the container
COPY artifacts/publish .

# Install required lib in order to support globalization
# Install required lib in order to support HTTPS GET requests
RUN apt-get update && apt-get install -y libicu-dev && apt-get install -y ca-certificates && update-ca-certificates

# Run the application
CMD ./GtfsImporter/release/GtfsImporter