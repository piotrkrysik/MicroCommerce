# Etap 1: Budowanie aplikacji
FROM node:20-alpine AS build
WORKDIR /app

# Kopiujemy pliki zależności
COPY package*.json ./
RUN npm install

# Kopiujemy resztę plików i budujemy projekt
COPY . .
RUN npm run build

# Etap 2: Serwowanie plików przez Nginx
FROM nginx:stable-alpine
# Vite domyślnie buduje do folderu 'dist'
COPY --from=build /app/dist /usr/share/nginx/html

# Kopiujemy własną konfigurację Nginx (opcjonalne, ale zalecane dla SPA)
# Jeśli nie masz pliku nginx.conf, Nginx użyje domyślnego
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]