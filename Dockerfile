# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim-arm64v8 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the entire source code into the container
COPY . .

ENV DOTNET_NUGET_SIGNATURE_VERIFICATION=false
ENV NUGET_CERT_REVOCATION_MODE=offline

# Create build for current runtime
RUN dotnet publish --ucr --artifacts-path artifacts

# Generate certificates required for https functionality
RUN dotnet dev-certs https --trust

# Run the application
CMD ["./artifacts/publish/API/release/API", "--urls", "http://*:5121;https://*:7242"]