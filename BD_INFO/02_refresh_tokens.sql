-- Tabla para Refresh Tokens
CREATE TABLE IF NOT EXISTS general.refresh_tokens (
    id SERIAL PRIMARY KEY,
    user_id VARCHAR(50) NOT NULL,
    token VARCHAR(255) NOT NULL UNIQUE,
    expires TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    created TIMESTAMP WITHOUT TIME ZONE DEFAULT NOW(),
    revoked TIMESTAMP WITHOUT TIME ZONE NULL,
    CONSTRAINT fk_rt_usuario FOREIGN KEY (user_id) REFERENCES general.usuarios(user_id) ON DELETE CASCADE
);

-- Indice para búsquedas rápidas por token
CREATE INDEX IF NOT EXISTS idx_refresh_token ON general.refresh_tokens(token);
