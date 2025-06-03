# Use alpine image as the base image for small image size
FROM --platform=arm64 ubuntu:latest

# Set the working directory inside the container
WORKDIR /app

# Copy the entire source code into the container
COPY ./artifacts .

# Copy the DatabaseContext bundle into the container
COPY ./DatabaseContextBundle .

# Copy the PTSDbContext bundle into the container
COPY ./PTSDbContextBundle .

# Install required lib in order to support globalization and install certificates in order to send requests to 3rd party api
RUN apt-get update && apt-get install -y libicu-dev && apt-get install -y ca-certificates

# Run the application
CMD ./DatabaseContextBundle && ./PTSDbContextBundle && ./publish/API/release/API --urls http://*:80
