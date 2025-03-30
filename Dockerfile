# Use alpine image as the base image for small image size
FROM alpine:latest

# Set the working directory inside the container
WORKDIR /app

# Copy the entire source code into the container
COPY ./artifacts .

# Copy the DatabaseContext bundle into the container
COPY ./DatabaseContextBundle .

# Copy the PTSDbContext bundle into the container
COPY ./PTSDbContextBundle .

# Run the application
CMD ["./DatabaseContextBundle;","./PTSDbContextBundle;","./artifacts/publish/API/release/API", "--urls", "http://*:80"]