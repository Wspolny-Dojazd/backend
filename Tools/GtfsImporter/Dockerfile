# Use alpine image as the base image for small image size
FROM --platform=arm64 ubuntu:latest

# Set the working directory inside the container
WORKDIR /app

# Copy the entire source code into the container
COPY artifacts/publish .

# Install required lib in order to support globalization
RUN apt-get update && apt-get install -y libicu-dev

# Run the application
CMD ./GtfsImporter/release/GtfsImporter