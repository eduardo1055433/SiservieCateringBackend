# Referencia de Endpoints de API

Esta documentación detalla los endpoints disponibles en el Backend de SiservieCatering.

## Base URL
- **Local**: `http://localhost:5137`
- **Producción**: `https://api.siservie.com` (Ejemplo)

---

## 1. Autenticación (General)

### POST `/api/general/Auth/login`
Inicia sesión y devuelve los tokens de acceso.

**Cuerpo de la Petición (JSON):**
```json
{
  "userId": "admin",
  "password": "su_password"
}
```

**Respuesta Exitosa (200 OK):**
```json
{
  "userId": "admin",
  "nombre": "Administrador Sistema",
  "token": "JWT_ACCESS_TOKEN",
  "refreshToken": "RANDOM_REFRESH_TOKEN",
  "empresas": [
    {
      "schema": "vidagong",
      "nombreEmpresa": "VidaGong Catering",
      "rol": "Administrador",
      "predeterminado": true
    }
  ]
}
```

---

### POST `/api/general/Auth/refresh-token`
Renueva el Access Token (JWT) usando un Refresh Token válido.

**Cuerpo de la Petición (JSON):**
```json
{
  "token": "JWT_EXPIRADO_U_OPCIONAL",
  "refreshToken": "RANDOM_REFRESH_TOKEN_ACTUAL"
}
```

**Respuesta** (Igual que Login, con nuevos tokens).

---

## 2. Tablas

### GET `/api/tablas/VentasCab/{schema}`
Obtiene la lista de cabeceras de ventas para un esquema específico.
**Requiere:** `Authorization: Bearer <token>`

**Parámetros:**
- `schema` (Path): Nombre del esquema (ej: `vidagong`).

---

## 3. Funciones (Reportes)

### GET `/api/funciones/VentasFacturar`
Ejecuta la función de reporte de facturación.
**Requiere:** `Authorization: Bearer <token>`

**Parámetros (Query String):**
- `schema`: (default: `vidagong`)
- `ticket`: (default: `TODOS`)
- `fechaInicio`: (default: `2024-08-01`)
- `fechaFin`: (default: `2025-08-31`)
- `tipoProd`: (default: `NOPROD`)
- *Otros filtros del 5 al 13 (default: `TODOS`)*

---

## 4. Catálogos

### GET `/api/catalogo/tipo-cliente`
Obtiene la lista de TIPOS DE CLIENTE disponibles para filtros.
**Requiere:** `Authorization: Bearer <token>`

**Respuesta Exitosa (200 OK):**
```json
[
  { "label": "TODOS", "value": "TODOS" },
  { "label": "TRABAJADORES REGISTRADOS", "value": "TRAB" },
  { "label": "VISITA", "value": "Visita" }
]
```

### GET `/api/catalogo/tipo-venta`
Obtiene la lista de TIPOS DE VENTA disponibles para filtros.
**Respuesta Exitosa (200 OK):**
```json
[
  { "label": "TODOS", "value": "TODOS" },
  { "label": "BODEGA", "value": "B" },
  { "label": "RAPIDO", "value": "R" }
]
```

### GET `/api/catalogo/pos?schema={schema}`
Obtiene la lista de Puntos de Venta (POS) del esquema indicado.
**Parámetros:** `schema` (default: `vidagong`).
**Respuesta Exitosa (200 OK):**
```json
[
  { "label": "POS Principal", "value": "001-001" },
  { "label": "Caja 2", "value": "001-002" }
]
```

### GET `/api/catalogo/servicios?schema={schema}`
Obtiene la lista de Servicios del esquema indicado.
**Parámetros:** `schema` (default: `vidagong`).
**Respuesta Exitosa (200 OK):**
```json
[
  { "label": "ALMUERZO", "value": "ALMUERZO" },
  { "label": "CENA", "value": "CENA" }
]
```

## Seguridad
Todos los endpoints (excepto Login y RefreshToken) requieren un header de autorización:
`Authorization: Bearer <su_token_jwt>`
