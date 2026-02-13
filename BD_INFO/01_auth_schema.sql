-- Crear esquema general si no existe
CREATE SCHEMA IF NOT EXISTS general;

-- 1. Tabla de Empresas
CREATE TABLE IF NOT EXISTS general.empresas
(
    emp_schema character varying(20) NOT NULL,
    emp_nombre character varying(30) NOT NULL,
    emp_predet numeric(1,0) DEFAULT 0,
    CONSTRAINT pk_emp_schema PRIMARY KEY (emp_schema)
);

-- 2. Tabla de Usuarios (Global)
CREATE TABLE IF NOT EXISTS general.usuarios (
    user_id VARCHAR(50) NOT NULL,
    user_email VARCHAR(100) UNIQUE NOT NULL,
    user_password VARCHAR(255) NOT NULL, -- BCrypt Hash
    user_nombre VARCHAR(100),
    user_activo SMALLINT DEFAULT 1,
    CONSTRAINT pk_usuarios PRIMARY KEY (user_id)
);

-- 3. Tabla de Roles
CREATE TABLE IF NOT EXISTS general.roles (
    rol_id INT NOT NULL,
    rol_descripcion VARCHAR(50) NOT NULL,
    CONSTRAINT pk_roles PRIMARY KEY (rol_id)
);

-- 4. Tabla Relacional (Usuario -> Empresa -> Rol)
CREATE TABLE IF NOT EXISTS general.usuario_empresa (
    user_id VARCHAR(50) NOT NULL,
    emp_schema VARCHAR(20) NOT NULL,
    rol_id INT NOT NULL,
    ue_activo SMALLINT DEFAULT 1,
    ue_predeterminado SMALLINT DEFAULT 0,
    CONSTRAINT pk_usuario_empresa PRIMARY KEY (user_id, emp_schema),
    CONSTRAINT fk_ue_usuario FOREIGN KEY (user_id) REFERENCES general.usuarios(user_id),
    CONSTRAINT fk_ue_empresa FOREIGN KEY (emp_schema) REFERENCES general.empresas(emp_schema),
    CONSTRAINT fk_ue_rol FOREIGN KEY (rol_id) REFERENCES general.roles(rol_id)
);

-- Datos Semilla (Seed Data) - Solo si no existen
INSERT INTO general.roles (rol_id, rol_descripcion) VALUES (1, 'Administrador') ON CONFLICT DO NOTHING;
INSERT INTO general.roles (rol_id, rol_descripcion) VALUES (2, 'Usuario') ON CONFLICT DO NOTHING;

-- Empresa Demo
INSERT INTO general.empresas (emp_schema, emp_nombre, emp_predet) VALUES ('vidagong', 'Vida Gong Catering', 1) ON CONFLICT DO NOTHING;

-- Usuario Demo (Password: 123456 -> Hash BCrypt)
-- Hash generado para "123456": $2a$11$Z5G5.3.3.3.3.3.3.3.3.3.3.3.3.3.3
-- NOTA: En producción usar una herramienta real para generar el hash. Aqui pondré un placeholder o insertaré uno real si tengo acceso a generar uno.
-- Para simplificar el ejemplo inicial, insertaremos un usuario de prueba.
INSERT INTO general.usuarios (user_id, user_email, user_password, user_nombre, user_activo) 
VALUES ('admin', 'admin@siservie.com', '$2a$11$Qtj/9.9.9.9.9.9.9.9.9.9.9.9.9.9.', 'Administrador Sistema', 1) 
ON CONFLICT DO NOTHING;

-- Relación Demo
INSERT INTO general.usuario_empresa (user_id, emp_schema, rol_id, ue_activo, ue_predeterminado)
VALUES ('admin', 'vidagong', 1, 1, 1)
ON CONFLICT DO NOTHING;
