# Use alpine image as the base image for small image size
FROM --platform=arm64 ubuntu:latest

# Set the working directory inside the container
WORKDIR /app

# Make folder for publish
RUN mkdir ./publish

# Copy the publish into the container
COPY ./artifacts/publish ./publish

# Copy the DatabaseContext bundle into the container
COPY ./DatabaseContextBundle .

# Copy the PTSDbContext bundle into the container
COPY ./PTSDbContextBundle .

# Install required lib in order to support globalization
RUN apt-get update && apt-get install -y libicu-dev

# Run the application
CMD ./DatabaseContextBundle && ./PTSDbContextBundle && ./publish/API/release/API --urls http://*:80