# Documentación del Esquema de Autenticación (`general`)

Este documento detalla las tablas creadas en el esquema `general` de PostgreSQL para manejar la autenticación multi-empresa.

## Diagrama de Relación

```mermaid
erDiagram
    USUARIOS ||--o{ USUARIO_EMPRESA :"tiene acceso"
    EMPRESAS ||--o{ USUARIO_EMPRESA :"pertenece a"
    ROLES ||--o{ USUARIO_EMPRESA :"tiene rol"

    USUARIOS {
        string user_id PK "Identificador único (Login)"
        string user_email "Correo electrónico"
        string user_password "Hash BCrypt"
        string user_nombre "Nombre completo"
        int user_activo "1=Activo, 0=Inactivo"
    }

    EMPRESAS {
        string emp_schema PK "Schema de BD (ej: vidagong)"
        string emp_nombre "Nombre comercial"
        int emp_predet "1=Default"
    }

    ROLES {
        int rol_id PK "ID Numérico"
        string rol_descripcion "Nombre (Admin, Cajero)"
    }

    USUARIO_EMPRESA {
        string user_id FK,PK
        string emp_schema FK,PK
        int rol_id FK
        int ue_activo "1=Permitido"
        int ue_predeterminado "Empresa por defecto"
    }
```

## Detalle de Tablas

### 1. `general.usuarios` (Usuarios Globales)
Almacena la identidad de la persona, independientemente de la empresa.
- **Propósito**: Permitir que un usuario inicie sesión en el sistema una sola vez.
- **Seguridad**: El campo `user_password` guarda un Hash Seguro (BCrypt), nunca texto plano.

### 2. `general.empresas` (Catálogo de Empresas)
Define qué empresas (tenants) existen en el sistema.
- **Propósito**: Mapear el nombre comercial con su esquema técnico en PostgreSQL (campo `emp_schema`).
- **Ejemplo**: `Vida Gong Catering` -> Schema `vidagong`.

### 3. `general.roles` (Roles del Sistema)
Catálogo de niveles de acceso disponibles.
- **Ejemplos**:
  - `1`: Administrador (Acceso total)
  - `2`: Usuario (Acceso limitado)

### 4. `general.usuario_empresa` (Relación Acceso)
Esta es la tabla clave del sistema multi-empresa.
- **Propósito**: Define **QUÉ** usuario puede entrar a **QUÉ** empresa y con **QUÉ** rol.
- **Flexibilidad**:
  - Un usuario (`juan`) puede ser **Administrador** en la empresa A.
  - El mismo usuario (`juan`) puede ser solo **Cajero** en la empresa B.
  - Y no tener acceso a la empresa C.

## Flujo de Login
Cuando alguien intenta loguearse:
1. El sistema verifica usuario y contraseña en `general.usuarios`.
2. Si es correcto, consulta `general.usuario_empresa` para ver a dónde puede entrar.
3. Retorna un Token que contiene el ID del usuario y la lista de empresas permitidas.
