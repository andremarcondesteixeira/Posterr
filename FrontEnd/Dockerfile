FROM node:22.3 AS base
WORKDIR /app
COPY ./Core/package.json ./Core/
COPY ./Core/package-lock.json ./Core/
COPY ./React/NextJS/package.json ./React/NextJS/
COPY ./React/NextJS/package-lock.json ./React/NextJS/
WORKDIR /app/Core
RUN npm install
WORKDIR /app/React/NextJS
RUN npm install
WORKDIR /app
COPY . .
WORKDIR /app/React/NextJS
RUN npm run build
ENTRYPOINT [ "npm", "start" ]
